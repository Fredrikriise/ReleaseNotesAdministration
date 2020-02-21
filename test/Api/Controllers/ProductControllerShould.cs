using Api.Controllers;
using AutoMapper;
using Moq;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace test.Api.Controllers
{
    public class ProductControllerShould
    {
        [Fact]
        public void ListAllProducts()
        {
            var mockedProductController = new Mock<IProductsRepository>();
            mockedProductController.Setup(a => a.CreateProduct(It.IsAny<ProductDto>())).Verifiable();

            var mockMapper = new Mock<IMapper>();

            var controller = new ProductController(mockedProductController.Object, mockMapper.Object);

            //var input = ReturnJsonInput();
            
        }
    }
}
