using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Services
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
        public async Task<List<ProductDto>> GetAllProducts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT *
                FROM [Products]";

                var product = await connection.QueryAsync<Product>(query);
                var productMapped = _mapper.Map<List<ProductDto>>(product);
                return productMapped;
            }
        }

        public async Task<ProductDto> GetProductById(int? productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM [Products]
                WHERE [ProductId] = @ProductId";

                var product = await connection.QueryFirstOrDefaultAsync<Product>(query, new Product { @ProductId = productId });
                var mappedProduct = _mapper.Map<ProductDto>(product);
                return mappedProduct;
            }
        }

        public async Task<int?> CreateProduct(ProductDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var insert = @"INSERT INTO [Products]
                                (
                                    [ProductName],
                                    [ProductImage],
                                )
                                VALUES
                                (
                                    @ProductName,
                                    @ProductImage,
                                )";
                    var returnResult = await connection.QueryFirstOrDefaultAsync<int?>(insert, new ProductDto
                    {
                        ProductName = productDto.ProductName,
                        ProductImage = productDto.ProductImage,
                    });
                    Console.WriteLine(returnResult);
                    return returnResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductDto> UpdateProduct(int? ProductId, ProductDto product)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var updateDb = @"UPDATE [Products]
                    SET
                        [ProductName] = @ProductName,
                        [ProductImage] = @ProductImage,
                    WHERE [ProductId] = @ProductId";
                    var productMapped = _mapper.Map<Product>(product);
                    productMapped.AddProductId(ProductId);

                    var result = await connection.ExecuteAsync(updateDb, productMapped);
                    return product;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteProduct(int? productId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var Delete = "DELETE FROM [Products] WHERE ProductId = @ProductId";
                    var returnedProduct = await connection.ExecuteAsync(Delete, new Product { @ProductId = productId });
                    bool success = returnedProduct > 0;
                    return success;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
