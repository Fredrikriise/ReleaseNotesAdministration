using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using ReleaseNotes.Models;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace test.Api.Controllers
{
    public class ProductControllerApiTest
    {
        private readonly Mock<IProductsRepository> _mockRepo;
        private readonly ProductController _controller;
        private readonly Mock<IMapper> _mapper;

        public ProductControllerApiTest()
        {
            _mockRepo = new Mock<IProductsRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new ProductController(_mockRepo.Object, _mapper.Object);
        }

        [Fact]
        public async void Task_Get_All_Products_Should_Return_OkObjectResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            //Act
            //var _ = mockRepo.Setup(x => x.GetAllProducts()).ReturnsAsync();
            var data = await sut.Get();
            mockRepo.Verify(x => x.GetAllProducts());

            //Assert
            Assert.IsType<OkObjectResult>(data);

        }

        [Fact]
        public async void Task_Get_Product_By_Id_Should_Return_OkObjectResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            var productId = 1;

            // Act
            //mockRepo.Setup(x => x.GetProductById(It.IsAny<int?>())).ReturnsAsync();
            var ex = await sut.GetProductById(productId);


            //Assert
            //Assert.IsType<OkObjectResult>(ex);
        }

        [Fact]
        public async void Task_Get_Product_By_Id_Should_Return_NotFoundResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            var productId = 0;

            // Act
            //var mockMethod = mockRepo.Setup(x => x.GetProductById(productId)).Returns();
            var ex = await sut.GetProductById(productId);

            //Assert
            //Assert.IsType<OkObjectResult>(ex);
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
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            int? productId = 1;

            //Act
            mockRepo.Setup(x => x.DeleteProduct(It.IsAny<int?>())).ReturnsAsync(true);
            var data = await sut.DeleteProduct(productId);
            //mockRepo.Verify(x => x.DeleteProduct(It.IsAny<int?>()), Times.Once());
            
            //Assert
            Assert.IsType<OkResult>(data);
        }

        // 
        [Fact]
        public async void Task_Delete_Product_Should_Return_NotFoundResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            int? productId = 0;

            //Act
            mockRepo.Setup(x => x.DeleteProduct(productId)).ReturnsAsync(false);
            var data = await sut.DeleteProduct(productId);
            mockRepo.Verify(x => x.DeleteProduct(It.IsAny<int?>()), Times.Once());

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }


        [Fact]
        public async void Task_Create_Should_Return_CreatedResult()
        {
            //Arrange
            var controller = _controller;

            Product testProduct = new Product
            {
                ProductId = 2,
                ProductImage = "test-image.png",
                ProductName = "Test product",
            };

            //Act
            var data = await controller.Create(testProduct);
            Console.WriteLine(data);
            //Assert
            Assert.IsType<CreatedResult>(data);
        }
    }
}
