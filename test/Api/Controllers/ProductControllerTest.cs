using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.IO;
using Xunit;

namespace test.Api.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductsRepository> _mockRepo;
        private readonly ProductController _controller;
        private readonly Mock<IMapper> _mapper;

        public ProductControllerTest()
        {
            _mockRepo = new Mock<IProductsRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new ProductController(_mockRepo.Object, _mapper.Object);
        }

        [Fact]
        public async void Task_Get_All_Products_Should_Return_OkResult()
        {
            //Arrange
            var controller = _controller;

            //Act
            var data = await controller.Get();

            //Assert
            Assert.IsType<OkObjectResult>(data);

        } 

        [Fact]
        public async void Task_Get_Product_By_Id_Should_Return_OkResult()
        {
            //Arrange
            var controller = _controller;
            int? productId = 1;

            //Act
            var data = await controller.GetProductById(productId);

            //Assert
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_Update_Product_Should_Return_OkResult()
        {
            //Arrange
            var controller = _controller;
            int? productId = 2;

            Product testProduct = new Product
            {
                ProductId = 2,
                ProductImage = "test-image.png",
                ProductName = "Test product",
            };

            //Act
            var data = await controller.UpdateProduct(productId, testProduct);
            
            //Assert
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Task_Delete_Product_Should_Return_OkResult()
        {
            //Arrange
            var controller = _controller;
            int? ProductId = 2;

            //Act
            var data = await controller.DeleteProduct(ProductId);

            //Assert
            Assert.IsType<OkResult>(data);
        }
    }
}
