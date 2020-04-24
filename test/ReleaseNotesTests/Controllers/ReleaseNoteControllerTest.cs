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
        private Mock<HttpClient> _mockHttpClient;
        private readonly ReleaseNotesController _controller;

        public ReleaseNoteControllerTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClient = new Mock<HttpClient>();
            _controller = new ReleaseNotesController(_mockClientFactory.Object);
        }

        [Fact]
        public async Task ListAllReleaseNotes_Should_Return_View()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of OK (200) and Conent of release notes
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"title\":\"Release note 3.6 - Manager\",\"bodyText\":\"Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis\",\"id\":23,\"productId\":2,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:26\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-07T18:31:54\",\"isDraft\":false,\"pickedWorkItems\":null},{\"title\":\"Release note 3.7 - Recruiter\",\"bodyText\":\"Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis\",\"id\":24,\"productId\":1,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":false,\"pickedWorkItems\":null},{\"title\":\"Release note 3.8 - Talmundo\",\"bodyText\":\"Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis\",\"id\":25,\"productId\":3,\"createdBy\":\"Fredrik\",\"createdDate\":\"2020-03-05T23:48:29\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-02T18:01:34\",\"isDraft\":false,\"pickedWorkItems\":null},{\"title\":\"Test draft \",\"bodyText\":\"Test\",\"id\":54,\"productId\":2,\"createdBy\":\"Fredrik Riise\",\"createdDate\":\"2020-04-02T21:19:09\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-02T21:43:31\",\"isDraft\":true,\"pickedWorkItems\":null},{\"title\":\"Test release note with work items selected\",\"bodyText\":\"Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quiss\",\"id\":81,\"productId\":1,\"createdBy\":\"Fredrik Riise\",\"createdDate\":\"2020-04-13T19:02:14\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-19T15:14:59\",\"isDraft\":false,\"pickedWorkItems\":null},{\"title\":\"Test\",\"bodyText\":\"Test\",\"id\":94,\"productId\":1,\"createdBy\":\"Fredrik Riise\",\"createdDate\":\"2020-04-19T17:39:16\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-19T17:51:03\",\"isDraft\":false,\"pickedWorkItems\":\"false\"},{\"title\":\"Test\",\"bodyText\":\"Test\",\"id\":95,\"productId\":1,\"createdBy\":\"Fredrik Riise\",\"createdDate\":\"2020-04-19T22:25:43\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-20T13:27:21\",\"isDraft\":false,\"pickedWorkItems\":\"21703 23909 23909 23909 23909 \"}]")
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

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<ReleaseNoteViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ListAllReleaseNotes_Should_Return_Exception()
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
            var productId = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Conent of release notes
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"title\":\"Release note 3.6 - Manager\",\"bodyText\":\"Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis\",\"id\":23,\"productId\":2,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:26\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-07T18:31:54\",\"isDraft\":false,\"pickedWorkItems\":null},{\"title\":\"Release note 3.7 - Recruiter\",\"bodyText\":\"Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis\",\"id\":24,\"productId\":1,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":false,\"pickedWorkItems\":null},{\"title\":\"Release note 3.8 - Talmundo\",\"bodyText\":\"Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis\",\"id\":25,\"productId\":3,\"createdBy\":\"Fredrik\",\"createdDate\":\"2020-03-05T23:48:29\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-02T18:01:34\",\"isDraft\":false,\"pickedWorkItems\":null},{\"title\":\"Test draft \",\"bodyText\":\"Test\",\"id\":54,\"productId\":2,\"createdBy\":\"Fredrik Riise\",\"createdDate\":\"2020-04-02T21:19:09\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-02T21:43:31\",\"isDraft\":true,\"pickedWorkItems\":null},{\"title\":\"Test release note with work items selected\",\"bodyText\":\"Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quiss\",\"id\":81,\"productId\":1,\"createdBy\":\"Fredrik Riise\",\"createdDate\":\"2020-04-13T19:02:14\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-19T15:14:59\",\"isDraft\":false,\"pickedWorkItems\":null},{\"title\":\"Test\",\"bodyText\":\"Test\",\"id\":94,\"productId\":1,\"createdBy\":\"Fredrik Riise\",\"createdDate\":\"2020-04-19T17:39:16\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-19T17:51:03\",\"isDraft\":false,\"pickedWorkItems\":\"false\"},{\"title\":\"Test\",\"bodyText\":\"Test\",\"id\":95,\"productId\":1,\"createdBy\":\"Fredrik Riise\",\"createdDate\":\"2020-04-19T22:25:43\",\"lastUpdatedBy\":\"Fredrik Riise\",\"lastUpdateDate\":\"2020-04-20T13:27:21\",\"isDraft\":false,\"pickedWorkItems\":\"21703 23909 23909 23909 23909 \"}]")
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
            Assert.IsAssignableFrom<List<ReleaseNoteViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ListReleaseNotesForProduct_Should_Return_Exception()
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
