using BusinessLogic.Inputs;
using BusinessLogic.Managers;
using BusinessLogic.Responses;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RedTechChallenge.Controllers;

namespace Tests
{
    public class OrderControllerTests
    {
        string GuidExample = string.Empty;

        private readonly DataContext context = DataContextInitializer.GetContext();
        private readonly Mock<ILogger<OrderManager>> mock_manager_logger = new Mock<ILogger<OrderManager>>();
        OrderManager orderManager => new OrderManager(context, mock_manager_logger.Object);

        private readonly Mock<ILogger<OrdersController>> mock_logger = new Mock<ILogger<OrdersController>>();
        private readonly Mock<OrderManager> mock_manager = new Mock<OrderManager>();

        OrdersController orderController => new OrdersController(mock_logger.Object, orderManager);

        public OrderControllerTests()
        {
            GuidExample = Guid.NewGuid().ToString();

            context.Orders.Add(new Order()
            {
                Id = GuidExample,
                Type = "ReturnOrder",
                CustomerName = "Test",
                CreatedByUsername = "Admin",
                CreatedDate = DateTime.Now,
            });

            context.Orders.Add(new Order()
            {
                Id = Guid.NewGuid().ToString(),
                Type = "SaleOrder",
                CustomerName = "Test",
                CreatedByUsername = "Admin",
                CreatedDate = DateTime.Now,
            });

            context.Orders.Add(new Order()
            {
                Id = Guid.NewGuid().ToString(),
                Type = "SaleOrder",
                CustomerName = "Test",
                CreatedByUsername = "Admin",
                CreatedDate = DateTime.Now,
            });

            context.Orders.Add(new Order()
            {
                Id = Guid.NewGuid().ToString(),
                Type = "PurchaseOrder",
                CustomerName = "Test",
                CreatedByUsername = "Admin",
                CreatedDate = DateTime.Now,
            });

            context.SaveChanges();
        }

        [Fact]
        public async Task It_ShouldSearchOrders()
        {
            var response = await orderController.GetOrders();
            var okResponse = response as OkObjectResult;

            Assert.IsType<DataQuery>(okResponse.Value);

            Assert.NotNull(response);
        }




        [Fact]
        public async Task It_ShouldFindSpecifiedOrder()
        {
            var actionResult = await orderController.GetOrder(GuidExample);

            var okObjectResult = actionResult as OkObjectResult;

            //Assert.NotNull(okObjectResult.Value);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
        }



        [Fact]
        public async Task It_ShouldCreateOrderSuccessfully()
        {
            var orderInput = new OrderInput();
            orderInput.Type = "Standard";
            orderInput.CustomerName = "Kenny";
            orderInput.CreatedByUsername = "Admin";
            orderInput.CreatedDate = DateTime.Now;

            var response = await orderController.Post(orderInput);

            var ObjectResult = response as ObjectResult;

            Assert.Equal(201, ObjectResult.StatusCode);
        }

        [Fact]
        public async Task It_ShouldNotCreateAnInvalidOrder()
        {
            var orderInput = new OrderInput();
            orderInput.Type = "SomeType";
            orderInput.CustomerName = "Kenny";
            orderInput.CreatedByUsername = "Admin";
            orderInput.CreatedDate = DateTime.Now;

            var response = await orderController.Post(orderInput);

            var ObjectResult = response as ObjectResult;

            Assert.NotEqual(201, ObjectResult.StatusCode);
        }

        [Fact]
        public async Task It_ShouldUpdateOrderSuccessfully()
        {
            var orderInput = new OrderInput();

            orderInput.Type = "ReturnOrder";
            orderInput.CustomerName = "Kenny";
            orderInput.CreatedByUsername = "Admin";
            orderInput.CreatedDate = DateTime.Now;

            var response = await orderController.Put(GuidExample, orderInput);
            //....
        }

        [Fact]
        public async Task It_ShouldRemovedOrderSuccessfully()
        {
            var response = await orderController.Delete(GuidExample);
            //..
        }
    }
}
