using AutoMapper;
using Microsoft.Extensions.Options;
using Services;
using Services.Repository.Config;
using System.Threading.Tasks;
using test.ServicesTests.test_context;
using Xunit;

namespace test.ServicesTests.Repository
{
    public class ProductRepositoryTest
    {
        private readonly ProductsRepository _productsRepo;
#pragma warning disable CS0649 // Field 'ProductRepositoryTest._mapper' is never assigned to, and will always have its default value null
        private readonly IMapper _mapper;
#pragma warning restore CS0649 // Field 'ProductRepositoryTest._mapper' is never assigned to, and will always have its default value null

        public ProductRepositoryTest(DatabaseFixture database)
        {
            _productsRepo = new ProductsRepository(Options.Create(new SqlDbConnection
            {
                ConnectionString = database.ConnectionString
            }), _mapper);
        }

        [Fact]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async Task GetAllProducts_Should_ReturnMappedProduct()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

        }
    }
}
