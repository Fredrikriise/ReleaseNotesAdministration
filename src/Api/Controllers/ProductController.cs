using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
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

            if(returnedProducts == null)
            {
                return NotFound();
            }

            var mappedProducts = _mapper.Map<List<Product>>(returnedProducts);

            if (mappedProducts == null)
            {
                return NotFound();
            }

            return Ok(mappedProducts);
        }

        [HttpGet]
        [Route("/Product/{productId}")]
        public async Task<IActionResult> GetProductById(int? productId)
        {
            var product = await _productRepo.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            var mappedProduct = _mapper.Map<Product>(product);

            if(mappedProduct == null)
            {
                return NotFound();
            }
            return Ok(mappedProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var mappedProduct = _mapper.Map<ProductDto>(product);
            Console.WriteLine(mappedProduct);
            if(mappedProduct == null)
            {
                return NotFound();
            }

            await _productRepo.CreateProduct(mappedProduct);
            return Created("", product);
        }

        [HttpPut]
        [Route("/Product/{productId}")]
        public async Task<IActionResult> UpdateProduct(int? productId, Product product)
        {
            var mappedProduct = _mapper.Map<ProductDto>(product);
            Console.WriteLine(mappedProduct);

            if (mappedProduct == null)
            {
                return NotFound();
            }

            await _productRepo.UpdateProduct(productId, mappedProduct);
            return Ok();
        }

        [HttpDelete]
        [Route("/Product/{ProductId}")]
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
    }
}