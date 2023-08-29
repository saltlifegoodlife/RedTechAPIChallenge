using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RedTechAPIChallenge.Controllers;
using RedTechAPIChallenge.Models.Repositories;
using RedTechAPIChallenge.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedTechAPIChallenge.Tests.RepositoryTests
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _orderController;

        private readonly Mock<IOrderRepository> _mock_orderRepository;

        public OrdersControllerTests()
        {
            _mock_orderRepository = new();
            _orderController = new OrdersController(_mock_orderRepository.Object);
        }

        [Fact]
        public void RemoveOrder_Given_ArrayOfOrderIDs_ShouldReturn_NoContentResponse()
        {
            var arr = new[] {"1","2","3","4"};
            _mock_orderRepository.Setup(_ => _.CheckOrder(arr)).Returns(new List<string>());
            _mock_orderRepository.Setup(_ => _.DeleteOrder(arr)).Returns(4);
            var actual = _orderController.RemoveOrder(arr); 

            Assert.IsType<NoContentResult>(actual);
        }

        [Fact]
        public void Update_GivenValidInput_ReturnsSuccess()
        {
            
            var orderInput = new OrderInput
            {
                orderType = "SomeType",
                customerName = "John Doe",
                createdBy = "Admin",
                orderID = "123"
            };


            _mock_orderRepository.Setup(repo => repo.UpdateOrder(It.IsAny<Models.Order>(), It.IsAny<int>()))
                          .Returns(true);

            var result = _orderController.Update(orderInput);

            
            Assert.NotNull(result);
            Assert.Equal("Success", result.status);
            Assert.Equal("123", result.recordId);
            Assert.Equal("Order updated!", result.message);
        }

        [Fact]
        public void Update_GivenInvalidInput_ReturnsFailed()
        {
      
            var orderInput = new OrderInput
            {
                orderType = "",
                customerName = "",
                createdBy = "",
                orderID = "456"
            };

            var result = _orderController.Update(orderInput);

            
            Assert.NotNull(result);
            Assert.Equal("Failed", result.status);
            Assert.Equal("Error during save.", result.message);
        }
    }
}
