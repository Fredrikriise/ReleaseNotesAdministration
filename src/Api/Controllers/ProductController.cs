using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productRepo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductsRepository productsRepository, IMapper mapper)
        {
            _productRepo = productsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //if(!ProductId.HasValue)
            //{
            //    _logger.LogWarning($"The {nameof(ProductId)} : {ProductId} is not a valid parameter value");
            //}

            var returnedProducts = await _productRepo.GetAllProducts();
            var mappedProducts = _mapper.Map<List<Product>>(returnedProducts);
            return Ok(mappedProducts);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var mappedProduct = _mapper.Map<ProductDto>(product);
            await _productRepo.CreateProduct(mappedProduct);
            return Created("", product);
        }
        

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int? productId, Product product)
        {
            var mappedProduct = _mapper.Map<ProductDto>(product);
            await _productRepo.UpdateProduct(productId, mappedProduct);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int? productId)
        {
            var deletedProduct = await _productRepo.DeleteProduct(productId);

            if (deletedProduct)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("/product/{productId}")]
        public async Task<IActionResult> GetProductById(int? productId)
        {
            var product = await _productRepo.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            var mappedProduct = _mapper.Map<Product>(product);
            return Ok(mappedProduct);
        }
    }
}