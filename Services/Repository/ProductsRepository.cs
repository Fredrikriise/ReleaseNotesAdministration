using AutoMapper;
using Microsoft.Extensions.Options;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using System;
using System.Threading.Tasks;
using Services.Repository.Models;
using Services.Repository.Models.DataTransferObjects;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using Services.Repository.Models.DatabaseModels;

namespace Services.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        public readonly string _connectionString;
        private readonly IMapper _mapper;

        public ProductsRepository(IOptions<SqlDbConnection> sqlDbConnection, IMapper mapper)
        {

            
            _connectionString = sqlDbConnection.Value.ConnectionString;
            _mapper = mapper;
        }

        public async Task<int?> CreateProduct(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);

                using (var connection = new SqlConnection(_connectionString))
                {
                    var insert = @"INSERT INTO [ProductDb]
                                (
                                    [ProductId],
                                    [ProductName],
                                    [ProductImage],
                                    [ProductDescription]
                                )
                                VALUES
                                (
                                    @ProductId,
                                    @ProductName
                                    @ProductImage
                                    @ProductDescription
                                )
                                SELECT [Id] FROM [ReleaseNotesDb] WHERE [Id] = @Id AND [ProductId] = @ProductId";
                    var returnResult = await connection.QueryFirstAsync<int?>(insert, product);
                    return returnResult;
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }

        public async Task<ProductDto> GetProduct(int? productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM [ProductDb]
                WHERE [ProductId] = @ProductId";

                var product = await connection.QueryFirstOrDefaultAsync<Product>(query, new { @ProductId = productId });
                var mappedProduct = _mapper.Map<ProductDto>(product);
                return mappedProduct;
            }
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {
            Console.WriteLine(_connectionString);
            using (var connection = new SqlConnection(_connectionString))
            {
                Console.WriteLine(connection);
                var query = @"SELECT *
                FROM [ProductsDb]";

                var product = await connection.QueryAsync<Product>(query);
                Console.WriteLine(product);
                var productMapped = _mapper.Map<List<ProductDto>>(product);
                Console.WriteLine(productMapped);
                return productMapped;
            }
        }

        public async Task<ProductDto> UpdateProduct(int? ProductId, ProductDto product)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var updateDb = @"UPDATE [ProductDb]
                    SET
                        [ProductId] = @ProductId, 
                        [ProductName] = @ProductName,
                        [ProductImage] = @ProductImage,
                        [ProductDescription] = @ProductDescription
                    WHERE [ProductId] = @ProductId";
                    var productMapped = _mapper.Map<Product>(product);
                    productMapped.AddProductId(ProductId);

                    var result = await connection.ExecuteAsync(updateDb, productMapped);
                    return product;
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }

        public async Task<bool> DeleteProduct(int? productId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var Delete = "DELETE FROM ProductDb WHERE ProductId = @ProductId";
                    var returnedProduct = await connection.ExecuteAsync(Delete, new { @ProductId = productId });
                    bool success = returnedProduct > 0;
                    return success;
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }
    }
}
