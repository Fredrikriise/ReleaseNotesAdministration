using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ProductList Get()
        {
            var productList = new List<ProductModel>
            {
                new ProductModel
                {
                    ProductId = 1,
                    ProductName = "Talent Recruiter",
                    ProductImage = "pic-recruiter.png",
                    ProductDescription = "TalentRecruiter"
                },
                new ProductModel
                {
                    ProductId = 2,
                    ProductName = "Talent Onboarding",
                    ProductImage = "pic-onboarding.png",
                    ProductDescription = "TalentOnboarding"
                },
                new ProductModel
                {
                    ProductId = 3,
                    ProductName = "Talent Manager",
                    ProductImage = "pic-manager.png",
                    ProductDescription = "TalentManager"
                }
            };

            return new ProductList()
            {
                Products = productList
            };
        }
    }
}