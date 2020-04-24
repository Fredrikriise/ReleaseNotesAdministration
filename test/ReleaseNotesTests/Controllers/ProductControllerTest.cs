using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using ReleaseNotes.Controllers;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;
using Services.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
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
        public async void Task_List_All_Products_Should_Return_View()
        {
            //var handlerMock = new Mock<HttpMessageHandler>();

            //handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            //    .ReturnsAsync(new HttpResponseMessage()
            //    {
            //        StatusCode = HttpStatusCode.OK
            //    }).Verifiable();

            //// use real http client with mocked handler here
            //var httpClient = new HttpClient(handlerMock.Object)
            //{
            //    BaseAddress = new Uri("https://localhost:44324/")
            //};

            //// create the mock client factory mock
            //var httpClientFactoryMock = _mockClientFactory;
            //var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient")).Returns(httpClient);

            //var controller = new ProductController(httpClientFactoryMock.Object);

            //-------------------------------------------------------------------------------------------------
            // Arrange
            var controller = _controller;

            // 1. Set up json-data (which originally should have come from /Product/
            var testJson = "[{\"productId\":1,\"productName\":\"Talent Recruiter\",\"productImage\":\"pic-recruiter.png\"},{\"productId\":2,\"productName\":\"Talent Manager\",\"productImage\":\"pic-manager.png\"},{\"productId\":3,\"productName\":\"Talmundo\",\"productImage\":\"logo_talmundo.png\"},{\"productId\":10,\"productName\":\"ReachMee\",\"productImage\":\"reachmeelogo.png\"},{\"productId\":12,\"productName\":\"Webrecruiter\",\"productImage\":\"webrecruiter-logo.png\"}]";

            // Act
            // 2. dezerialize json data to List<ProductApiModel> 
            var converted = JsonConvert.DeserializeObject<List<ProductApiModel>>(testJson);

            // 3. "map" this to ProductViewModel objects
            var mappedJson = converted.Select(x => new ProductViewModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage
            }).ToList();

            var result = await controller.ListAllProducts();
            var viewResult = Assert.IsType<ViewResult>(result);

            // Assert
            // 4. Check if returned data is type 
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
