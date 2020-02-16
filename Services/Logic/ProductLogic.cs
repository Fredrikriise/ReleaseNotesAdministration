using AutoMapper;
using Services.Logic.Interfaces;
using Services.Logic.Models;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Services.Repository.Interfaces;


namespace Services.Logic
{
    public class ProductLogic : IProductLogic
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productRepo;

        public ProductLogic(IMapper mapper, IProductsRepository productRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _productRepo.GetAllProducts();
            var mappedProducts = _mapper.Map<List<Product>>(products);
            return mappedProducts;
        }

        public async Task<int?> CreateProduct(Product productDto)
        {
            var mappedProduct = _mapper.Map<Repository.Models.DataTransferObjects.ProductDto>(productDto);
            await _productRepo.CreateProduct(mappedProduct);
            return mappedProduct.ProductId;
        }

        public async Task<Product> GetProduct(int? productId)
        {
            var product = await _productRepo.GetProduct(productId);
            var mappedProduct = _mapper.Map<Product>(product);
            return mappedProduct;
        }

        public async Task<Product> UpdateProduct(int? ProductId, Product productDto)
        {
            var mappedInput = _mapper.Map<Repository.Models.DataTransferObjects.ProductDto>(productDto);
            var result = await _productRepo.UpdateProduct(ProductId, mappedInput);
            var mappedResult = _mapper.Map<Product>(result);
            return mappedResult;
        }

        public async Task<bool> DeleteProduct(int? productId)
        {
            var result = await _productRepo.DeleteProduct(productId);
            return result;
        }
    }
}
