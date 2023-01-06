using BusinessLogic.Inputs;
using BusinessLogic.Managers;
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
        }

        [Fact]
        public async Task It_ShouldSearchOrders()
        {
            var response = await orderController.GetOrders();
            Assert.NotNull(response);
        }

        [Fact]
        public async Task It_ShouldFindSpecifiedOrder()
        {
            var actionResult = await orderController.GetOrder(GuidExample);

            var okObjectResult = actionResult as ObjectResult;

            Assert.NotNull(okObjectResult.Value);
        }



        //[Fact]
        //public async Task It_ShouldReturnSomething()
        //{
        //    var orderInput = new OrderInput();
        //    orderInput.Type = "Standard";
        //    orderInput.CustomerName = "Kenny";
        //    orderInput.CreatedByUsername = "Admin";
        //    orderInput.CreatedDate = DateTime.Now;

        //    var response = await orderController.Post(orderInput);
        //    var a = response.ExecuteResultAsync(context);

        //    Assert.NotNull(Ok() , );

        //    Assert.
        //}

        //[Fact]
        //public async Task It_ShouldNotCreateAnInvalidOrder()
        //{
        //    var orderInput = new OrderInput();
        //    orderInput.Type = "SomeType";
        //    orderInput.CustomerName = "Kenny";
        //    orderInput.CreatedByUsername = "Admin";
        //    orderInput.CreatedDate = DateTime.Now;

        //    var response = await orderController.Post(orderInput);            

        //    Assert.Equal();
        //}
    }
}
