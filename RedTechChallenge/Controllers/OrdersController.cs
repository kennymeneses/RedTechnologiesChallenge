using BusinessLogic.Inputs;
using BusinessLogic.Managers;
using Microsoft.AspNetCore.Mvc;

namespace RedTechChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        ILogger<OrdersController> _logger;
        IOrderManager _orderManager;

        public OrdersController(ILogger<OrdersController> logger, IOrderManager orderManager)
        {
            _logger = logger;
            _orderManager = orderManager;
        }

        [HttpGet]
        [Route("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {            
            try
            {
                var response = await _orderManager.SearchOrders();

                if (response.ResponseStatus == "Error")
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetOrderByType/{type}")]
        public async Task<IActionResult> GetOrderByType(string type)
        {
            try
            {
                var response = await _orderManager.SearchOrderByType(type);

                if (response.ResponseStatus == "Error")
                {
                    return NotFound(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetOrder/{id}")]
        //[Route("GetOrders")]
        public async Task<IActionResult> GetOrder(string id)
        {
            try
            {
                var singleQuery = await _orderManager.SearchById(id);

                if(singleQuery.ResponseStatus == "Error")
                {
                    return NotFound(singleQuery);
                }

                return Ok(singleQuery);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> Post([FromBody] OrderInput order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var response = await _orderManager.CreateOrder(order);

                if(response.Status == "Error")
                {
                    return StatusCode(503, response);
                }

                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Post method: {0} ", ex.Message);
                throw;
            }
        }
        [HttpPut("{id}")]
        //[Route("UpdateOrder")]
        public async Task<IActionResult> Put(string id, [FromBody] OrderInput order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var response = await _orderManager.UpdateOrder(id, order);

                if(response.Status.Equals("Error"))
                {
                    return StatusCode(422, response);
                }

                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Update method: {0} ", ex.Message);
                throw;
            }


        }
        [HttpDelete("{id}")]
        //[Route("DeleteOrder")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var response = await _orderManager.DeleteOrder(id);
                if(response.Status.Equals("Error"))
                {
                    return StatusCode(500, response);
                }

                return StatusCode(202, response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Delete method: {0} ", ex.Message);

                throw;
            }
        }
    }
}
