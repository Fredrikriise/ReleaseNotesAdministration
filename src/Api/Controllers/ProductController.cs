﻿using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Logic.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductLogic _productRepo;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductLogic productsRepository, IMapper mapper)
        {
            _logger = logger;
            _productRepo = productsRepository;
            _mapper = mapper;
        }
        
        //[HttpGet]
        //public ProductList Get()
        //{
        //    var productList = new List<ProductModel>
        //    {
        //        new ProductModel
        //        {
        //            ProductId = 1,
        //            ProductName = "Talent Recruiter",
        //            ProductImage = "pic-recruiter.png",
        //            ProductDescription = "TalentRecruiter"
        //        },
        //        new ProductModel
        //        {
        //            ProductId = 2,
        //            ProductName = "Talent Onboarding",
        //            ProductImage = "pic-onboarding.png",
        //            ProductDescription = "TalentOnboarding"
        //        },
        //        new ProductModel
        //        {
        //            ProductId = 3,
        //            ProductName = "Talent Manager",
        //            ProductImage = "pic-manager.png",
        //            ProductDescription = "TalentManager"
        //        }
        //    };
        //
        //    return new ProductList()
        //    {
        //        Products = productList
        //    };
        //}
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //if(!ProductId.HasValue)
            //{
            //    _logger.LogWarning($"The {nameof(ProductId)} : {ProductId} is not a valid parameter value");
            //}

            var returnedProducts = await _productRepo.GetAllProducts();

            var mappedProducts = _mapper.Map<List<ProductList>>(returnedProducts);
            Console.WriteLine(mappedProducts);
            return Ok(mappedProducts);

        } 
    }
}