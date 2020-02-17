using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Services.Repository.Models;
using Services.Repository.Models.DataTransferObjects;

namespace Services.Repository.Interfaces
{
    public interface IProductsRepository
    {
        Task<int?> CreateProduct(ProductDto productDto);
        Task<ProductDto> GetProduct(int? productId);
        Task<ProductDto> UpdateProduct(int? ProductId, ProductDto product);
        Task<bool> DeleteProduct(int? productId);
        Task<List<ProductDto>> GetAllProducts();
    }
}
