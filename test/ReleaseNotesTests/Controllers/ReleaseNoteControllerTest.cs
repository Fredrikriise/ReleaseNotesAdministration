using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using ReleaseNotes.Controllers;
using ReleaseNotes.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace test.ReleaseNotesTests.Controllers
{
    public class ReleaseNoteControllerTest
    {
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly ReleaseNotesController _controller;

        public ReleaseNoteControllerTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClient = new Mock<HttpClient>();
            _controller = new ReleaseNotesController(_mockClientFactory.Object);
        }

        [Fact]
        public async Task ListAllReleaseNotes_Should_Return_View_With_List_Of_ReleaseNotes()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of OK (200) and Conent of release notes
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"title\":\"Release note 3.6 - Manager\",\"bodyText\":\"body text test\",\"id\":24,\"productId\":1,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":false,\"pickedWorkItems\":null}," +
                    "{\"title\":\"Release note 3.7 - Recruiter\",\"bodyText\":\"body text test\",\"id\":26,\"productId\":2,\"createdBy\":\"Fredrik\",\"createdDate\":\"2020-03-07T23:47:49\",\"lastUpdatedBy\":\"Felix\",\"lastUpdateDate\":\"2020-03-09T12:23:54\",\"isDraft\":false,\"pickedWorkItems\":null}]")
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
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient")).Returns(httpClient);

            var controller = new ReleaseNotesController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ListAllReleaseNotes();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(2, ((List<ReleaseNoteViewModel>)viewResult.ViewData.Model).Count);

            var releaseNotes = viewResult.ViewData.Model as IList<ReleaseNoteViewModel>;
            var createdDateObj1 = new DateTime(2020, 3, 7, 23, 47, 49);
            var lastUpdatedObj1 = new DateTime(2020, 3, 9, 12, 23, 54);
            Assert.Equal(26, releaseNotes[0].Id);
            Assert.Equal("Release note 3.7 - Recruiter", releaseNotes[0].Title);
            Assert.Equal("body text test", releaseNotes[0].BodyText);
            Assert.Equal(2, releaseNotes[0].ProductId);
            Assert.Equal("Fredrik", releaseNotes[0].CreatedBy);
            Assert.Equal(createdDateObj1, releaseNotes[0].CreatedDate);
            Assert.Equal("Felix", releaseNotes[0].LastUpdatedBy);
            Assert.Equal(lastUpdatedObj1, releaseNotes[0].LastUpdateDate);

            var createdDateObj2 = new DateTime(2020, 3, 5, 23, 47, 49);
            var lastUpdatedObj2 = new DateTime(2020, 3, 6, 18, 36, 24);
            Assert.Equal(24, releaseNotes[1].Id);
            Assert.Equal("Release note 3.6 - Manager", releaseNotes[1].Title);
            Assert.Equal("body text test", releaseNotes[1].BodyText);
            Assert.Equal(1, releaseNotes[1].ProductId);
            Assert.Equal("Felix", releaseNotes[1].CreatedBy);
            Assert.Equal(createdDateObj2, releaseNotes[1].CreatedDate);
            Assert.Equal("Fredrik", releaseNotes[1].LastUpdatedBy);
            Assert.Equal(lastUpdatedObj2, releaseNotes[1].LastUpdateDate);

            Assert.IsAssignableFrom<List<ReleaseNoteViewModel>>(viewResult.ViewData.Model);
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

            // mockHandler mocked httpclient
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
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient")).Returns(httpClient);

            var controller = new ReleaseNotesController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ListAllReleaseNotes());
        }

        [Fact]
        public async Task ListReleaseNotesForProduct_Should_Return_View_With_List()
        {
            // Arrange
            var productId = 2;

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of release notes
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"title\":\"Release note 3.8 - Recruiter\",\"bodyText\":\"body text test\",\"id\":24,\"productId\":2,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":false,\"pickedWorkItems\":null}," +
                    "{\"title\":\"Release note 3.7 - Recruiter\",\"bodyText\":\"body text test\",\"id\":26,\"productId\":2,\"createdBy\":\"Fredrik\",\"createdDate\":\"2020-03-07T23:47:49\",\"lastUpdatedBy\":\"Felix\",\"lastUpdateDate\":\"2020-03-09T12:23:54\",\"isDraft\":false,\"pickedWorkItems\":null}]")
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
            var httpClientResult = await httpClient.GetAsync($"/ReleaseNotes?productId={productId}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient")).Returns(httpClient);

            var controller = new ReleaseNotesController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ListReleaseNotesForProduct(productId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(2, ((List<ReleaseNoteViewModel>)viewResult.ViewData.Model).Count);

            var releaseNotes = viewResult.ViewData.Model as IList<ReleaseNoteViewModel>;
            var createdDateObj1 = new DateTime(2020, 3, 7, 23, 47, 49);
            var lastUpdatedObj1 = new DateTime(2020, 3, 9, 12, 23, 54);
            Assert.Equal(26, releaseNotes[0].Id);
            Assert.Equal("Release note 3.7 - Recruiter", releaseNotes[0].Title);
            Assert.Equal("body text test", releaseNotes[0].BodyText);
            Assert.Equal(2, releaseNotes[0].ProductId);
            Assert.Equal("Fredrik", releaseNotes[0].CreatedBy);
            Assert.Equal(createdDateObj1, releaseNotes[0].CreatedDate);
            Assert.Equal("Felix", releaseNotes[0].LastUpdatedBy);
            Assert.Equal(lastUpdatedObj1, releaseNotes[0].LastUpdateDate);

            var createdDateObj2 = new DateTime(2020, 3, 5, 23, 47, 49);
            var lastUpdatedObj2 = new DateTime(2020, 3, 6, 18, 36, 24);
            Assert.Equal(24, releaseNotes[1].Id);
            Assert.Equal("Release note 3.8 - Recruiter", releaseNotes[1].Title);
            Assert.Equal("body text test", releaseNotes[1].BodyText);
            Assert.Equal(2, releaseNotes[1].ProductId);
            Assert.Equal("Felix", releaseNotes[1].CreatedBy);
            Assert.Equal(createdDateObj2, releaseNotes[1].CreatedDate);
            Assert.Equal("Fredrik", releaseNotes[1].LastUpdatedBy);
            Assert.Equal(lastUpdatedObj2, releaseNotes[1].LastUpdateDate);

            Assert.IsAssignableFrom<List<ReleaseNoteViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ListReleaseNotesForProduct_Should_Throw_Exception()
        {
            // Arrange
            var productId = It.IsAny<int>();

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
            var httpClientResult = await httpClient.GetAsync($"/ReleaseNotes?productId={productId}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesApiClient")).Returns(httpClient);

            var controller = new ReleaseNotesController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ListReleaseNotesForProduct(productId));
        }
    }
}
