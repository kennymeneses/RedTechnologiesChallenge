using BusinessLogic.Inputs;
using BusinessLogic.Managers;
using DataAccess;
using DataAccess.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests
{
    public class OrderManagerTests
    {
        string GuidExample = string.Empty;
        string GuidExample2 = string.Empty; 

        private readonly DataContext context = DataContextInitializer.GetContext();
        private readonly Mock<ILogger<OrderManager>> mock_manager_logger = new Mock<ILogger<OrderManager>>();

        OrderManager _orderManager => new OrderManager(context, mock_manager_logger.Object);

        public OrderManagerTests()
        {
            GuidExample = Guid.NewGuid().ToString();
            GuidExample2 = Guid.NewGuid().ToString();

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
                Id = GuidExample2,
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
        public async Task It_ShouldReturnAllOrders()
        {
            var response = await _orderManager.SearchOrders();
            Assert.Equal(4, response.total);
        }

        [Fact]
        public async Task It_ShouldNotCreateAOrder()
        {
            var orderInput = new OrderInput();
            orderInput.Type = "SomeType";
            orderInput.CustomerName = "Kenny";
            orderInput.CreatedByUsername = "Admin";
            orderInput.CreatedDate = DateTime.Now;

            var response = await _orderManager.CreateOrder(orderInput);
            Assert.NotEqual("Ok", response.Status);
        }

        [Fact]
        public async Task It_ShouldCreateAOrder()
        {
            var orderInput = new OrderInput();
            orderInput.Type = "Standard";
            orderInput.CustomerName = "Kenny";
            orderInput.CreatedByUsername = "Admin";
            orderInput.CreatedDate = DateTime.Now;

            var response = await _orderManager.CreateOrder(orderInput);
            Assert.Equal("Ok", response.Status);
        }
        [Fact]
        public async Task It_ShouldGetOrderTypeList()
        {
            string type = "SaleOrder";

            var response = await _orderManager.SearchOrderByType(type);

            Assert.True(response.total > 0);
        }

        [Fact]
        public async Task It_ShouldNotGetOrderTypeList()
        {
            string type = "SomeType";

            var response = await _orderManager.SearchOrderByType(type);

            Assert.NotEqual("Ok", response.ResponseStatus);
        }

        [Fact]
        public async Task It_ShouldReturnAnSpecifiedOrder()
        {
            var response = await _orderManager.SearchById(GuidExample);

            Assert.IsType<Order>(response.Order);
        }

        //[Fact]
        //public async Task It_ShouldRemoveOrder()
        //{
        //    var response = await _orderManager.DeleteOrder(GuidExample2);

        //    Assert.Equal("Ok", response.Status);
        //}
    }
}
