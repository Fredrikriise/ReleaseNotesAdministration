using Api.Controllers;
using AutoMapper;
using Moq;
using Newtonsoft.Json;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.IO;
using Xunit;

namespace test.Api.Controllers
{
    public class ProductControllerShould
    {
        [Fact]
        public void Call_Create_method()
        {
            var mockedProductController = new Mock<IProductsRepository>();
            mockedProductController.Setup(a => a.CreateProduct(It.IsAny<ProductDto>())).Verifiable();

            //var mockMapper = new Mock<IMapper>();

            //var controller = new ProductController(mockedProductController.Object, mockMapper.Object);

//            var input = ReturnJsonInput();

//            var result = controller.Create(input);

            //mockedProductController.Verify(a => a.CreateProduct(It.IsAny<ProductDto>()), Times.Once);
        }



        //private static ProductDto ReturnJsonInput()
        //{
        //    var goodFile = $"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\ValidInput\\ProcessTemplate.json";
        //    using (StreamReader r = new StreamReader(goodFile))
        //    {
        //        string json = r.ReadToEnd();
        //        var model = JsonConvert.DeserializeObject<Product>(json);
        //        return model;
        //    }
        //}
    }
}
