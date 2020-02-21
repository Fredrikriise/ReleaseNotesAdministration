using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IProductsRepository
    {
        Task CreateProduct(ProductDto productDto);
        Task<ProductDto> GetProductById(int? productId);
        Task<ProductDto> UpdateProduct(int? ProductId, ProductDto product);
        Task<bool> DeleteProduct(int? productId);
        Task<List<ProductDto>> GetAllProducts();
    }
}
