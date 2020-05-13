using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Services;
using Services.Repository.Config;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test.ServicesTests.test_context;
using Xunit;

namespace test.ServicesTests.Repository
{
    public class ProductRepositoryTest
    {
        private readonly ProductsRepository _productsRepo;
        private readonly Mock<IMapper> _mapper;

        public ProductRepositoryTest()
        {
            DatabaseFixture database = new DatabaseFixture();

            _mapper = new Mock<IMapper>();
            _productsRepo = new ProductsRepository(Options.Create(new SqlDbConnection
            {
                ConnectionString = database.ConnectionString
            }), _mapper.Object);
        }

        [Fact]
        public async Task GetAllProducts_Should_ReturnMappedProduct()
        {
            // Arrange
            var repo = _productsRepo;

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

            // Act
            _mapper.Setup(x => x.Map<List<ProductDto>>(It.IsAny<List<Product>>())).Returns(testListProductDto);
            var result = await repo.GetAllProducts();

            // Assert
            var test = Assert.IsType<List<ProductDto>>(result);
            Assert.IsAssignableFrom<List<ProductDto>>(test);
        }

        [Fact]
        public async Task GetAllProducts_Should_Throw_Exception_Mapping_Fails()
        {
            // Arrange
            var repo = _productsRepo;

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.GetAllProducts());
        }

        [Fact]
        public async Task GetProductById_Should_ReturnMappedProduct()
        {
            // Arrange
            var repo = _productsRepo;

            var productId = 2;

            ProductDto testProductDto = new ProductDto()
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName 1",
                ProductImage = "testProductDto ProductImage 1"
            };

            // Act
            _mapper.Setup(x => x.Map<ProductDto>(It.IsAny<Product>())).Returns(testProductDto);
            var result = await repo.GetProductById(productId);

            // Assert
            var test = Assert.IsType<ProductDto>(result);
            Assert.IsAssignableFrom<ProductDto>(test);
        }

        [Fact]
        public async Task GetProductById_Should_Throw_Exception_Mapping_Fails()
        {
            // Arrange
            var repo = _productsRepo;
            var productId = 2;

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.GetProductById(productId));
        }

        [Fact]
        public async Task CreateProduct_Should_Return_Result()
        {
            // Arrange
            var repo = _productsRepo;

            ProductDto testProductDto = new ProductDto()
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName 1",
                ProductImage = "testProductDto ProductImage 1"
            };

            // Act
            var result = await repo.CreateProduct(testProductDto);

            // Assert
            var test = Assert.IsType<int>(result);
            Assert.IsAssignableFrom<int>(test);
        }

        [Fact]
        public async Task CreateProduct_Should_Throw_Exception_Product_Is_Null()
        {
            // Arrange
            var repo = _productsRepo;

            ProductDto testProductDto = new ProductDto()
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName 1",
                ProductImage = "testProductDto ProductImage 1"
            };
            testProductDto = null;

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.CreateProduct(testProductDto));
        }

        [Fact]
        public async Task UpdateProduct_Should_Execute_Async()
        {
            // Arrange
            var repo = _productsRepo;

            var productId = 1;

            Product testProduct = new Product()
            {
                ProductId = 1,
                ProductName = "testProduct ProductName 1",
                ProductImage = "testProduct ProductImage 1"
            };

            ProductDto testProductDto = new ProductDto()
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName 1",
                ProductImage = "testProductDto ProductImage 1"
            };

            // Act
            _mapper.Setup(x => x.Map<Product>(It.IsAny<ProductDto>())).Returns(testProduct);
            var result = await repo.UpdateProduct(productId, testProductDto);

            // Assert
            var test = Assert.IsType<ProductDto>(result);
            Assert.IsAssignableFrom<ProductDto>(test);
        }

        [Fact]
        public async Task UpdateProduct_Should_Throw_Exception_productMapped()
        {
            // Arrange
            var repo = _productsRepo;

            var productId = 1;

            ProductDto testProductDto = new ProductDto()
            {
                ProductId = 1,
                ProductName = "testProductDto ProductName 1",
                ProductImage = "testProductDto ProductImage 1"
            };

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.UpdateProduct(productId, testProductDto));
        }

        [Fact]
        public async Task DeleteProduct_Should_Execute_Async()
        {
            // Arrange
            var repo = _productsRepo;

            var productId = 1;

            // Act
            var result = await repo.DeleteProduct(productId);

            // Assert
            var test = Assert.IsType<bool>(result);
            Assert.IsAssignableFrom<bool>(test);
        }
    }
}
