using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;
using Xunit;

namespace test.ApiTests.Controllers
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
        public async void GetAllProducts_Should_Return_OkObjectResult()
        {
            //Arrange
            var sut = _controller;

            List<ProductDto> testListProductDto = new List<ProductDto>()
            {
                new ProductDto
                {
                ProductId = 1,
                ProductName = "testProductDto ProductName 1",
                ProductImage = "testProductDto ProductImage 1"
                },
                new ProductDto
                {
                ProductId = 2,
                ProductName = "testProductDto ProductName 2",
                ProductImage = "testProductDto ProductImage 2"
                }
            };

            List<Product> testListProducts = new List<Product>()
            {
                new Product
                {
                    ProductId = 1,
                    ProductName = "testProduct ProductName 1",
                    ProductImage = "testProduct ProductImage 1"
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "testProduct ProductName 2",
                    ProductImage = "testProduct ProductImage 2"
                }
            };

            //Act
            _mockRepo.Setup(x => x.GetAllProducts()).ReturnsAsync(testListProductDto);
            _mapper.Setup(x => x.Map<List<Product>>(testListProductDto)).Returns(testListProducts);
            var result = await sut.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllProducts_Should_Return_NotFoundResult_returnedProducts()
        {
            //Arrange
            var sut = _controller;

            List<ProductDto> testListProductDto = new List<ProductDto>()
            {
                new ProductDto
                {
                ProductId = 1,
                ProductName = "testProductDto ProductName 1",
                ProductImage = "testProductDto ProductImage 1"
                },
                new ProductDto
                {
                ProductId = 2,
                ProductName = "testProductDto ProductName 2",
                ProductImage = "testProductDto ProductImage 2"
                }
            };
            testListProductDto = null;

            //Act
            _mockRepo.Setup(x => x.GetAllProducts()).ReturnsAsync(testListProductDto);
            var result = await sut.Get();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetAllProducts_Should_Return_NotFoundResult_mappedProducts()
        {
            //Arrange
            var sut = _controller;

            List<ProductDto> testListProductDto = new List<ProductDto>()
            {
                new ProductDto
                {
                ProductId = 1,
                ProductName = "testProductDto ProductName 1",
                ProductImage = "testProductDto ProductImage 1"
                },
                new ProductDto
                {
                ProductId = 2,
                ProductName = "testProductDto ProductName 2",
                ProductImage = "testProductDto ProductImage 2"
                }
            };

            //Act
            _mockRepo.Setup(x => x.GetAllProducts()).ReturnsAsync(testListProductDto);
            var result = await sut.Get();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetProductById_Should_Return_OkObjectResult()
        {
            //Arrange
            var sut = _controller;

            var productId = 1;

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName",
                ProductImage = "testProductDto ProductImage"
            };

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductName = "testProduct ProductName",
                ProductImage = "testProduct ProductImage"
            };

            // Act
            _mockRepo.Setup(x => x.GetProductById(productId)).ReturnsAsync(testProductDto);
            _mapper.Setup(x => x.Map<Product>(testProductDto)).Returns(testProduct);
            var result = await sut.GetProductById(productId);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetProductById_Should_Return_NotFoundResult_product()
        {
            //Arrange
            var sut = _controller;

            var productId = 0;

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName",
                ProductImage = "testProductDto ProductImage"
            };
            testProductDto = null;

            // Act
            _mockRepo.Setup(x => x.GetProductById(productId)).ReturnsAsync(testProductDto);
            var result = await sut.GetProductById(productId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetProductById_Should_Return_NotFoundResult_mappedProduct()
        {
            //Arrange
            var sut = _controller;

            var productId = 1;

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName",
                ProductImage = "testProductDto ProductImage"
            };

            // Act
            _mockRepo.Setup(x => x.GetProductById(productId)).ReturnsAsync(testProductDto);
            var result = await sut.GetProductById(productId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void CreateProduct_Should_Return_CreatedResult()
        {
            //Arrange
            var sut = _controller;

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName",
                ProductImage = "testProductDto ProductImage"
            };

            ProductDto testProductDtoResult = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDtoResult ProductName",
                ProductImage = "testProductDtoResult ProductImage"
            };

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductName = "testProduct ProductName",
                ProductImage = "testProduct ProductImage"
            };

            //Act
            _mapper.Setup(x => x.Map<ProductDto>(testProduct)).Returns(testProductDtoResult);
            _mockRepo.Setup(x => x.CreateProduct(testProductDto));
            var result = await sut.Create(testProduct);

            //Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async void CreateProduct_Should_Return_NotFound_mappedProduct()
        {
            //Arrange
            var sut = _controller;

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName",
                ProductImage = "testProductDto ProductImage"
            };

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductName = "testProduct ProductName",
                ProductImage = "testProduct ProductImage"
            };

            //Act
            _mockRepo.Setup(x => x.CreateProduct(testProductDto));
            var result = await sut.Create(testProduct);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void CreateProduct_Should_Return_NotFound_product()
        {
            //Arrange
            var sut = _controller;

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName",
                ProductImage = "testProductDto ProductImage"
            };

            ProductDto testProductDtoResult = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDtoResult ProductName",
                ProductImage = "testProductDtoResult ProductImage"
            };

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductName = "testProduct ProductName",
                ProductImage = "testProduct ProductImage"
            };
            testProduct = null;

            //Act
            _mapper.Setup(x => x.Map<ProductDto>(testProduct)).Returns(testProductDtoResult);
            _mockRepo.Setup(x => x.CreateProduct(testProductDto));
            var result = await sut.Create(testProduct);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void UpdateProduct_Should_Return_OkResult()
        {
            //Arrange
            var sut = _controller;

            int? productId = 1;

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductName = "testProduct ProductName",
                ProductImage = "testProduct ProductImage"
            };

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName mapped",
                ProductImage = "testProductDto ProductImage mapped"
            };

            ProductDto testProductDtoResult = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDtoResult ProductName",
                ProductImage = "testProductDtoResult ProductImage"
            };

            //Act
            _mapper.Setup(x => x.Map<ProductDto>(testProduct)).Returns(testProductDto);
            _mockRepo.Setup(x => x.UpdateProduct(productId, testProductDto)).ReturnsAsync(testProductDtoResult);
            var result = await sut.UpdateProduct(productId, testProduct);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateProduct_Should_Return_NotFound_mappedProduct()
        {
            //Arrange
            var sut = _controller;

            int? productId = 1;

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName",
                ProductImage = "testProductDto ProductImage"
            };

            ProductDto testProductDtoResult = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDtoResult ProductName",
                ProductImage = "testProductDtoResult ProductImage"
            };

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductName = "testProduct ProductName",
                ProductImage = "testProduct ProductImage"
            };

            //Act
            _mockRepo.Setup(x => x.UpdateProduct(productId, testProductDto)).ReturnsAsync(testProductDtoResult);
            var result = await sut.UpdateProduct(productId, testProduct);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void UpdateProduct_Should_Return_NotFound_updatedProduct()
        {
            //Arrange
            var sut = _controller;

            int? productId = 1;

            ProductDto testProductDto = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName",
                ProductImage = "testProductDto ProductImage"
            };

            ProductDto testProductDtoResult = new ProductDto
            {
                ProductId = 1,
                ProductName = "testProductDtoResult ProductName",
                ProductImage = "testProductDtoResult ProductImage"
            };
            testProductDtoResult = null;

            Product testProduct = new Product
            {
                ProductId = 1,
                ProductName = "testProduct ProductName",
                ProductImage = "testProduct ProductImage"
            };

            //Act
            _mapper.Setup(x => x.Map<ProductDto>(testProduct)).Returns(testProductDto);
            _mockRepo.Setup(x => x.UpdateProduct(productId, testProductDto)).ReturnsAsync(testProductDtoResult);
            var result = await sut.UpdateProduct(productId, testProduct);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeleteProduct_Should_Return_OkResult()
        {
            //Arrange
            var sut = _controller;

            int? productId = 1;

            //Act
            _mockRepo.Setup(x => x.DeleteProduct(productId)).ReturnsAsync(true);
            var result = await sut.DeleteProduct(productId);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DeleteProduct_Should_Return_NotFoundResult_deletedProduct()
        {
            //Arrange
            var sut = _controller;

            int? productId = 0;

            //Act
            _mockRepo.Setup(x => x.DeleteProduct(productId)).ReturnsAsync(false);
            var result = await sut.DeleteProduct(productId);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
