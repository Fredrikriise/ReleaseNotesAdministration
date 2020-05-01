using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Services;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using Services.Repository.Models.DataTransferObjects;
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

            // Act
            var result = await repo.GetAllProducts();

            // Assert
            var test = Assert.IsType<List<ProductDto>>(result);
            Assert.IsAssignableFrom<List<ProductDto>>(test);
        }
    }
}
