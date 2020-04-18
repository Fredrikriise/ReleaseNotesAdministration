using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReleaseNotes.Controllers;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;
using Services.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace test.ReleaseNotes.Controllers
{
    public class ProductControllerTest
    {
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private Mock<HttpClient> _mockProductsClient;
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockProductsClient = new Mock<HttpClient>();
            _controller = new ProductController(_mockClientFactory.Object);
        }

        [Fact]
        public async Task Task_List_All_Products_Should_Return_View()
        {
            // Arrange
            var controller = _controller;
            

            // Act
            var result = await controller.ListAllProducts();
            var viewResult = Assert.IsType<ViewResult>(result);

            // Assert
            Assert.IsAssignableFrom<List<ProductViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Task_List_All_Products_Should_Not_Return_Exception()
        {
            // Arrange
            var controller = _controller;

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ListAllProducts());

            // Assert
            
        }
    }
}
