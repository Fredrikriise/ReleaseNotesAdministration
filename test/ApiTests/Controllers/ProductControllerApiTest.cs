using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
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

            List<ProductDto> testList = new List<ProductDto>()
            {
                new ProductDto
                {
                ProductId = 1,
                ProductName = "test",
                ProductImage = "test"
                },
                new ProductDto
                {
                ProductId = 2,
                ProductName = "test",
                ProductImage = "test"
                }
            };

            // Oppretter liste for to produkter for å kunne returnere i mapping 
            List<Product> testListProducts = new List<Product>()
            {
                new Product
                {
                    ProductId = 1,
                    ProductName = "test",
                    ProductImage = "test"
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "test",
                    ProductImage = "test"
                }
            };
            
            //Act
            mockRepo.Setup(x => x.GetAllProducts()).ReturnsAsync(testList);
            mapper.Setup(x => x.Map<List<Product>>(testList)).Returns(testListProducts);
            var result = await sut.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Task_Get_All_Products_Should_Return_NotFoundResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            List<ProductDto> testList = new List<ProductDto>()
            {
                new ProductDto
                {
                ProductId = 1,
                ProductName = "test",
                ProductImage = "test"
                },
                new ProductDto
                {
                ProductId = 2,
                ProductName = "test",
                ProductImage = "test"
                }
            };
            testList = null;

            //Act
            mockRepo.Setup(x => x.GetAllProducts()).ReturnsAsync(testList);
            var data = await sut.Get();
            mockRepo.Verify(x => x.GetAllProducts());

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetProductById_Should_Return_OkObjectResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            var productId = 1;
            ProductDto testProduct = new ProductDto
            {
                ProductId = 1,
                ProductName = "test",
                ProductImage = "test"
            };

            Product product = new Product
            {
                ProductId = 1,
                ProductName = "test updated",
                ProductImage = "test updated"
            };

            // Act
            mockRepo.Setup(x => x.GetProductById(productId)).ReturnsAsync(testProduct);
            mapper.Setup(x => x.Map<Product>(testProduct)).Returns(product);
            var result = await sut.GetProductById(productId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void Task_Get_Product_By_Id_Should_Return_NotFoundResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            var productId = 0;
            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "test",
                ProductImage = "test"
            };
            testProductDto = null;

            // Act
            mockRepo.Setup(x => x.GetProductById(productId)).ReturnsAsync(testProductDto);
            var result = await sut.GetProductById(productId);
            mockRepo.Verify(x => x.GetProductById(productId), Times.Once);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Task_Create_Should_Return_CreatedResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "test",
                ProductImage = "test"
            };

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductImage = "test-image.png",
                ProductName = "Test product",
            };

            //Act
            mapper.Setup(x => x.Map<ProductDto>(testProduct));
            mockRepo.Setup(x => x.CreateProduct(testProductDto)).ReturnsAsync(1);
            var data = await sut.Create(testProduct);
            Console.WriteLine(data);

            //Assert
            Assert.IsType<CreatedResult>(data);
        }

        [Fact]
        public async void Task_Update_Product_Should_Return_OkResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            int? productId = 1;

            Product updatedProduct = new Product
            {
                ProductName = "Test product",
                ProductImage = "test-image.png"
            };

            //Act
            //var mappedProduct = mapper.Setup(x => x.Map<ProductDto>(updatedProduct));
            mockRepo.Setup(x => x.UpdateProduct(It.IsAny<int?>(), It.IsAny<ProductDto>())).ReturnsAsync(It.IsAny<ProductDto>());
            var result = await sut.UpdateProduct(productId, updatedProduct);
            mockRepo.Verify(x => x.UpdateProduct(It.IsAny<int?>(), It.IsAny<ProductDto>()), Times.Once);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        // Denne returnerer NotFoundResult uansett
        [Fact]
        public async void Task_Update_Product_Should_Return_NotFoundResult()
        {
            //Arrange
            Mock<IProductsRepository> mockRepo = new Mock<IProductsRepository>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var sut = new ProductController(mockRepo.Object, mapper.Object);

            int? productId = 1;

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductImage = "test-image.png",
                ProductName = "Test product",
            };
            testProduct = null;

            //Act
            var data = await sut.UpdateProduct(productId, testProduct);

            //Assert
            Assert.IsType<NotFoundResult>(data);
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
            var result = await sut.DeleteProduct(productId);
            //mockRepo.Verify(x => x.DeleteProduct(It.IsAny<int?>()), Times.Once());
            Console.WriteLine(result);

            //Assert
            Assert.IsType<OkResult>(result);
        }

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

        
    }
}
