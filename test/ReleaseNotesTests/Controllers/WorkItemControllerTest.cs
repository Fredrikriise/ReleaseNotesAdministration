using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using ReleaseNotes.Controllers;
using ReleaseNotes.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace test.ReleaseNotesTests.Controllers
{
    public class WorkItemControllerTest
    {
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private Mock<HttpClient> _mockHttpClient;
        private readonly WorkItemController _controller;

        public WorkItemControllerTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClient = new Mock<HttpClient>();
            _controller = new WorkItemController(_mockClientFactory.Object);
        }

        [Fact]
        public async Task ListWorkItem_Should_Return_View_With_WorkItem()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of release notes
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"id\":23909,\"title\":\"Test work item\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}")
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
            var httpClientResult = await httpClient.GetAsync($"/WorkItem/{Id}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient")).Returns(httpClient);

            var controller = new WorkItemController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ListWorkItem(Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<WorkItemViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ListWorkItem_Should_Return_Exception()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of release notes
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
            var httpClientResult = await httpClient.GetAsync($"/WorkItem/{Id}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient")).Returns(httpClient);

            var controller = new WorkItemController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ListWorkItem(Id));
        }
    }
}
