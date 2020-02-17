using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Services.Logic.Models;

namespace Services.Logic.Interfaces
{
    public interface IProductLogic
    {
        Task<int?> CreateProduct(Product productDto);
        Task<Product> GetProduct(int? productId);
        Task<Product> UpdateProduct(int? ProductId, Product productDto);
        Task<bool> DeleteProduct(int? productId);
        Task<Product> GetAllProducts();
    }
}
