using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System;
using Microsoft.Extensions.Options;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;

namespace Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductsRepository _productRepo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductsRepository productsRepository, IMapper mapper)
        {
            _logger = logger;
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
            var mappedProducts = _mapper.Map <List<Product>> (returnedProducts);
            return Ok(mappedProducts);
        } 
    }
}