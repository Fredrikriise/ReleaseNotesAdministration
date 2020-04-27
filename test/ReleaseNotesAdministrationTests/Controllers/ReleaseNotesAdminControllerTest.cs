using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Moq.Protected;
using ReleaseNotesAdministration.Controllers;
using ReleaseNotesAdministration.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace test.ReleaseNotesAdministrationTests.Controllers
{
    public class ReleaseNotesAdminControllerTest
    {
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private Mock<HttpClient> _mockHttpClient;
        private readonly ReleaseNotesAdminController _controller;

        public ReleaseNotesAdminControllerTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClient = new Mock<HttpClient>();
            _controller = new ReleaseNotesAdminController(_mockClientFactory.Object);
        }

        [Fact]
        public async Task ListAllReleaseNotes_Should_Return_View_With_List()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of OK (200) and Content of work items
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"title\":\"Release note 3.6 - Manager\",\"bodyText\":\"body text test\",\"id\":24,\"productId\":1,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":false,\"pickedWorkItems\":null}," +
                    "{\"title\":\"Release note 3.7 - Recruiter\",\"bodyText\":\"body text test\",\"id\":26,\"productId\":2,\"createdBy\":\"Fredrik\",\"createdDate\":\"2020-03-07T23:47:49\",\"lastUpdatedBy\":\"Felix\",\"lastUpdateDate\":\"2020-03-09T12:23:54\",\"isDraft\":true,\"pickedWorkItems\":null}]")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.GetAsync("/ReleaseNotes/");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ListAllReleaseNotes();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<ReleaseNoteAdminViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ListAllReleaseNotes_Should_Throw_Exception()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.GetAsync("/ReleaseNotes/");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ListAllReleaseNotes());
        }

        [Fact]
        public async Task CreateReleaseNote_NoParameters_Should_Return_View()
        {

        }

        [Fact]
        public async Task CreateReleaseNote_NoParameters_Should_Throw_Exception_Getting_Product()
        {

        }

        [Fact]
        public async Task CreateReleaseNote_NoParameters_Should_Throw_Exception_Getting_WorkItem()
        {

        }



        [Fact]
        public async Task DeleteReleaseNote_Should_Delete_WorkItem_And_RedirectToAction()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("DeleteRN", "Success"));

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work item
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"title\":\"Release note 3.7 - Recruiter\",\"bodyText\":\"body text test\",\"id\":26,\"productId\":2,\"createdBy\":\"Fredrik\",\"createdDate\":\"2020-03-07T23:47:49\",\"lastUpdatedBy\":\"Felix\",\"lastUpdateDate\":\"2020-03-09T12:23:54\",\"isDraft\":true,\"pickedWorkItems\":null}")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.DeleteAsync($"/ReleaseNotes/{Id}");

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.DeleteReleaseNote(Id);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.IsType<RedirectToActionResult>(viewResult);
        }

        [Fact]
        public async Task DeleteReleaseNote_Should_Throw_Exception()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msg);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResult = await httpClient.DeleteAsync($"/ReleaseNotes/{Id}");

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.DeleteReleaseNote(Id));
        }
    }
}
