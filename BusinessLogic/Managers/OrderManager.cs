using BusinessLogic.Inputs;
using BusinessLogic.Responses;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Managers
{
    public class OrderManager : IOrderManager
    {
        DataContext _context;
        ILogger<OrderManager> _logger;
        //private readonly IConfiguration _configuration;

        public OrderManager(DataContext context, ILogger<OrderManager> logger)
        {
            _context = context;
            _logger = logger;
            //_configuration = configuration;
        }

        public async Task<DataQuery> SearchOrders()
        {
            var queryResult = new DataQuery(); 

            try
            {
                IQueryable<Order> QueryOrder = _context.Orders;
                var orderList = QueryOrder.ToList();

                if(orderList.Count == 0)
                {
                    queryResult.ResponseStatus = Constants.StatusOk;
                    queryResult.data = null;
                    queryResult.total = 0;
                    queryResult.ResponseDescription = Constants.OrderQueryEmptyResult;

                    return queryResult;
                }

                queryResult.ResponseStatus = Constants.StatusOk;
                queryResult.data = orderList;
                queryResult.total = orderList.Count;
                queryResult.ResponseDescription = Constants.OrderQuerySuccessResult;

                _logger.LogInformation("OrderList request found {0} orders.", orderList.Count);

                return queryResult;

            }
            catch (Exception ex)
            {
                _logger.LogError("Fail request for OrderList.");

                queryResult.data = null;
                queryResult.total = 0;
                queryResult.ResponseDescription = ex.Message;
                queryResult.ResponseStatus = Constants.StatusError;

                return queryResult;
            }
        }

        public async Task<DataQuery> SearchOrderByType(string type)
        {
            var queryResult = new DataQuery();

            try
            {
                if(IsOrderTypeValid(type))
                {
                    IQueryable<Order> QueryOrder = _context.Orders.Where(o => o.Type == type);
                    var orderList = QueryOrder.ToList();

                    if (orderList.Count == 0)
                    {
                        queryResult.ResponseStatus = Constants.StatusOk;
                        queryResult.data = null;
                        queryResult.total = 0;
                        queryResult.ResponseDescription = Constants.OrderQueryEmptyResult;

                        return queryResult;
                    }

                    _logger.LogInformation("OrderList request found {0} orders for type: {1}.", orderList.Count, type);

                    queryResult.ResponseStatus = Constants.StatusOk;
                    queryResult.data = orderList;
                    queryResult.total = orderList.Count;
                    queryResult.ResponseDescription = Constants.OrderQuerySuccessResult;

                    return queryResult;
                }
                else
                {
                    queryResult.ResponseStatus = Constants.StatusError;
                    queryResult.data = null;
                    queryResult.total = 0;
                    queryResult.ResponseDescription = Constants.OrderTypeIsInvalid;

                    return queryResult;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Fail request for OrderList with type: {0}.", type);

                queryResult.data = null;
                queryResult.total = 0;
                queryResult.ResponseDescription = ex.Message;
                queryResult.ResponseStatus = Constants.StatusError;

                return queryResult;
            }
        }

        public async Task<SingleQuery> SearchById(string id)
        {
            var singleQuery = new SingleQuery();
            var order = new Order();

            try
            {
                order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                {
                    _logger.LogInformation("Order doesnt exists.");

                    singleQuery.ResponseStatus = Constants.StatusError;
                    singleQuery.Order = null;
                    singleQuery.DescriptionResponse = Constants.OrderUpdatedErrorId;

                    return singleQuery;
                }

                _logger.LogInformation("Order with Id: {0} founded.", id);

                singleQuery.ResponseStatus = Constants.StatusOk;
                singleQuery.Order = order;
                singleQuery.DescriptionResponse = Constants.OrderFounded;

                return singleQuery;

            }
            catch (Exception ex)
            {
                _logger.LogError("Order with Id: {0} was not found.", id);

                singleQuery.ResponseStatus = Constants.StatusError;
                singleQuery.Order = null;
                singleQuery.DescriptionResponse = ex.Message;

                return singleQuery;
            }
        }

        public async Task<ResponseStatus> CreateOrder(OrderInput input)
        {
            var response = new ResponseStatus();
            var order = new Order();

            order.Id = Guid.NewGuid().ToString();
            order.Type = input.Type;
            order.CustomerName = input.CustomerName;
            order.CreatedByUsername = input.CreatedByUsername;
            order.CreatedDate = DateTime.UtcNow;

            try
            {
                if(IsOrderTypeValid(input.Type))
                {
                    await _context.Orders.AddAsync(order);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Order with Id: {0} was created at: {1}", order.Id, order.CreatedDate);

                    response.id = order.Id;
                    response.Status = Constants.StatusOk;
                    response.ResponseDescription = Constants.OrderCreated;

                    return response;
                }
                else
                {
                    _logger.LogInformation("Order was not created because ordertype: {0} is invalid.", order.Type);

                    response.id = string.Empty;
                    response.Status = Constants.StatusError;
                    response.ResponseDescription = Constants.OrderErrorType;

                    return response;
                }                
            }
            catch (Exception ex )
            {
                _logger.LogError("Order was not created.");

                response.id = string.Empty;
                response.ResponseDescription = ex.Message;
                response.Status = Constants.StatusError;

                return response;
            }
        }

        public async Task<ResponseStatus> UpdateOrder(string id, OrderInput input)
        {
            var responseStatus = new ResponseStatus();
            var order = new Order();

            try
            {
                order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

                if(order == null)
                {
                    responseStatus.Status = Constants.StatusError;
                    responseStatus.id = string.Empty;
                    responseStatus.ResponseDescription = Constants.OrderUpdatedErrorId;

                    return responseStatus;
                }

                order.Type = input.Type;
                order.CustomerName = input.CustomerName;
                order.CreatedDate = input.CreatedDate;
                order.CreatedByUsername = input.CreatedByUsername;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Order with Id: {0} was updated at: {1}", order.Id, DateTime.Now);

                responseStatus.Status = Constants.StatusOk;
                responseStatus.id = order.Id;
                responseStatus.ResponseDescription = Constants.OrderUpdated;

                return responseStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError("Order was not updated.");

                responseStatus.Status = Constants.StatusError;
                responseStatus.id = string.Empty;
                responseStatus.ResponseDescription = ex.Message;

                return responseStatus;
            }            
        }

        public async Task<ResponseStatus> DeleteOrder(string id)
        {
            var order = new Order();
            var responseStatus = new ResponseStatus();

            try
            {
                order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order == null)
                {
                    _logger.LogInformation("Order doesnt exists.");

                    responseStatus.Status = Constants.StatusError;
                    responseStatus.id = string.Empty;
                    responseStatus.ResponseDescription = Constants.OrderUpdatedErrorId;

                    return responseStatus;
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                responseStatus.Status = Constants.StatusOk;
                responseStatus.id = id;
                responseStatus.ResponseDescription = Constants.OrderRemoved;

                return responseStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError("Order was not removed.");

                responseStatus.Status = Constants.StatusError;
                responseStatus.id = string.Empty;
                responseStatus.ResponseDescription = ex.Message;

                return responseStatus;
            }
        }

        private bool IsOrderTypeValid(string type)
        {
            var listValidateTypes = new List<string>();
            listValidateTypes.Add("ReturnOrder");
            listValidateTypes.Add("PurchaseOrder");
            listValidateTypes.Add("Standard");
            listValidateTypes.Add("TransferOrder");
            listValidateTypes.Add("SaleOrder");

            if(listValidateTypes.Contains(type))
            {
                return true;
            }

            return false;
        }
    }
}
