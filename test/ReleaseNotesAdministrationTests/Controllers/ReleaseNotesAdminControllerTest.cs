using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Moq.Protected;
using ReleaseNotesAdministration.Controllers;
using ReleaseNotesAdministration.Models;
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
            // HttpResponseMessage with a StatusCode of OK (200) and Content of release notes
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
            Assert.Equal(2, ((List<ReleaseNoteAdminViewModel>)viewResult.ViewData.Model).Count);

            var releaseNotes = viewResult.ViewData.Model as IList<ReleaseNoteAdminViewModel>;
            var createdDateObj1 = new DateTime(2020, 3, 5, 23, 47, 49);
            var lastUpdatedObj1 = new DateTime(2020, 3, 6, 18, 36, 24);

            Assert.Equal(24, releaseNotes[0].Id);
            Assert.Equal("Release note 3.6 - Manager", releaseNotes[0].Title);
            Assert.Equal("body text test", releaseNotes[0].BodyText);
            Assert.Equal(1, releaseNotes[0].ProductId);
            Assert.Equal("Felix", releaseNotes[0].CreatedBy);
            Assert.Equal(createdDateObj1, releaseNotes[0].CreatedDate);
            Assert.Equal("Fredrik", releaseNotes[0].LastUpdatedBy);
            Assert.Equal(lastUpdatedObj1, releaseNotes[0].LastUpdateDate);

            var createdDateObj2 = new DateTime(2020, 3, 7, 23, 47, 49);
            var lastUpdatedObj2 = new DateTime(2020, 3, 9, 12, 23, 54);

            Assert.Equal(26, releaseNotes[1].Id);
            Assert.Equal("Release note 3.7 - Recruiter", releaseNotes[1].Title);
            Assert.Equal("body text test", releaseNotes[1].BodyText);
            Assert.Equal(2, releaseNotes[1].ProductId);
            Assert.Equal("Fredrik", releaseNotes[1].CreatedBy);
            Assert.Equal(createdDateObj2, releaseNotes[1].CreatedDate);
            Assert.Equal("Felix", releaseNotes[1].LastUpdatedBy);
            Assert.Equal(lastUpdatedObj2, releaseNotes[1].LastUpdateDate);
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
            // Arrange
            // HttpResponseMessage with a StatusCode of OK (200) and Content of products
            HttpResponseMessage msgProduct = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"productId\":1,\"productName\":\"Talent Recruiter\",\"productImage\":\"pic-recruiter.png\"},{\"productId\":2,\"productName\":\"Talent Manager\",\"productImage\":\"pic-manager.png\"},{\"productId\":3,\"productName\":\"Talmundo\",\"productImage\":\"logo_talmundo.png\"},{\"productId\":10,\"productName\":\"ReachMee\",\"productImage\":\"reachmeelogo.png\"},{\"productId\":12,\"productName\":\"Webrecruiter\",\"productImage\":\"webrecruiter-logo.png\"}]")
            };

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work items
            HttpResponseMessage msgWorkItem = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"id\":21625,\"title\":\"Adding the styling to correct file (User module)\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"},{\"id\":21680,\"title\":\"Make listing of 'All Release Notes' descending based of publish-date\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}]")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msgWorkItem);

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msgProduct);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResultProduct = await httpClient.GetAsync("/Product/");
            var contentProduct = await httpClientResultProduct.Content.ReadAsStringAsync();

            var httpClientResultWorkItem = await httpClient.GetAsync("/WorkItem/");
            var contentWorkItem = await httpClientResultWorkItem.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.Create();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task CreateReleaseNote_NoParameters_Should_Throw_Exception_Getting_Product()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msgProduct = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work items
            HttpResponseMessage msgWorkItem = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"id\":21625,\"title\":\"Adding the styling to correct file (User module)\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"},{\"id\":21680,\"title\":\"Make listing of 'All Release Notes' descending based of publish-date\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}]")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msgWorkItem);

            mockHandler.Protected()
                      .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                      ItExpr.IsAny<CancellationToken>())
                      .ReturnsAsync(msgProduct);

            // mocking client 
            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResultProduct = await httpClient.GetAsync("/Product/");
            var contentProduct = await httpClientResultProduct.Content.ReadAsStringAsync();

            var httpClientResultWorkItem = await httpClient.GetAsync("/WorkItem/");
            var contentWorkItem = await httpClientResultWorkItem.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.Create());
        }

        [Fact]
        public async Task CreateReleaseNote_NoParameters_Should_Throw_Exception_Getting_WorkItem()
        {
            // Arrange

            // HttpResponseMessage with a StatusCode of OK (200) and Content of products
            HttpResponseMessage msgProduct = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"productId\":1,\"productName\":\"Talent Recruiter\",\"productImage\":\"pic-recruiter.png\"}," +
                    "{\"productId\":2,\"productName\":\"Talent Manager\",\"productImage\":\"pic-manager.png\"}," +
                    "{\"productId\":3,\"productName\":\"Talmundo\",\"productImage\":\"logo_talmundo.png\"}," +
                    "{\"productId\":10,\"productName\":\"ReachMee\",\"productImage\":\"reachmeelogo.png\"}," +
                    "{\"productId\":12,\"productName\":\"Webrecruiter\",\"productImage\":\"webrecruiter-logo.png\"}]")
            };

            // HttpResponseMessage with a StatusCode of NotFound and Content of an empty string
            HttpResponseMessage msgWorkItem = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };

            // mockHandler and mocked httpclient
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msgProduct);

            mockHandler.Protected()
                       .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                       ItExpr.IsAny<CancellationToken>())
                       .ReturnsAsync(msgWorkItem);

            var httpClient = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:44324/")
            };
            var httpClientResultProduct = await httpClient.GetAsync("/Product/");
            var contentProduct = await httpClientResultProduct.Content.ReadAsStringAsync();

            var httpClientResultWorkItem = await httpClient.GetAsync("/WorkItem/");
            var contentWorkItem = await httpClientResultWorkItem.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.Create());
        }

        [Fact]
        public async Task CreateReleaseNote_With_Parameters_Should_Create_ReleaseNote()
        {
            // Arrange
            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("CreateRN", "Success"));

            // HttpResponseMessage with a StatusCode of OK (200) and Content of release note
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"title\":\"Release note 3.6 - Manager\",\"bodyText\":\"body text test\",\"id\":24,\"productId\":1,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":true,\"pickedWorkItems\": \"21345 12342 23345 \"}")
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
            var httpClientResult = await httpClient.PostAsync("/ReleaseNotes/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            ReleaseNoteAdminApiModel testReleaseNote = new ReleaseNoteAdminApiModel
            {
                Id = 1,
                Title = "Release note 3.6 - Manager",
                BodyText = "body text test",
                ProductId = 2,
                CreatedBy = "Felix",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "Fredrik",
                LastUpdateDate = DateTime.Now,
                IsDraft = true,
                PickedWorkItems = "21345 23453"
            };

            var submitButton = "Save as draft";

            string[] PickedWorkItems = { "21345", "12342", "23345" };

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.CreateReleaseNote(testReleaseNote, submitButton, PickedWorkItems);

            // Assert
            Assert.Matches(@"^[a-zA-Z0-9, _ - ! ?. ""-]{3,100}$", testReleaseNote.Title);
            Assert.Matches(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", testReleaseNote.CreatedBy);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListAllReleaseNotes", viewResult.ActionName);
        }

        [Fact]
        public async Task CreateReleaseNote_With_Parameters_Should_Return_View_Regex_Fails()
        {
            // Arrange
            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("CreateRN", "Failed"));

            // HttpResponseMessage with a StatusCode of OK (200) and Content of release note
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"title\":\"@£@£$$£€ff\",\"bodyText\":\"body text test\",\"id\":24,\"productId\":1,\"createdBy\":\"@£@€$£€dfshh\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":true,\"pickedWorkItems\": \"21345 12342 23345 \"}")
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
            var httpClientResult = await httpClient.PostAsync("/ReleaseNotes/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            ReleaseNoteAdminApiModel testReleaseNote = new ReleaseNoteAdminApiModel
            {
                Id = 1,
                Title = "@£@£$$£€ff",
                BodyText = "body text test",
                ProductId = 2,
                CreatedBy = "@£@€$£€dfshh",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "Fredrik",
                LastUpdateDate = DateTime.Now,
                IsDraft = true,
                PickedWorkItems = "21345 23453"
            };

            var submitButton = "Save as draft";

            string[] PickedWorkItems = { "21345", "12342", "23345" };

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.CreateReleaseNote(testReleaseNote, submitButton, PickedWorkItems);

            // Assert
            Assert.DoesNotMatch(@"^[a-zA-Z0-9, _ - ! ?. ""-]{3,100}$", testReleaseNote.Title);
            Assert.DoesNotMatch(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", testReleaseNote.CreatedBy);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task CreateReleaseNote_With_Parameters_Should_Throw_Exception()
        {
            // Arrange
            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("CreateRN", "Success"));

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
            var httpClientResult = await httpClient.PostAsync("/ReleaseNotes/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            ReleaseNoteAdminApiModel testReleaseNote = new ReleaseNoteAdminApiModel
            {
                Id = 1,
                Title = "Release note 3.6 - Manager",
                BodyText = "body text test",
                ProductId = 2,
                CreatedBy = "Felix",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "Fredrik",
                LastUpdateDate = DateTime.Now,
                IsDraft = true,
                PickedWorkItems = "21345 23453"
            };

            var submitButton = "Save as draft";

            string[] PickedWorkItems = { "21345", "12342", "23345" };

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.CreateReleaseNote(testReleaseNote, submitButton, PickedWorkItems));
        }

        [Fact]
        public async Task EditReleaseNote_Post_Should_Edit_ReleaseNote_And_Redirect_To_View()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("EditRN", "Success"));

            // HttpResponseMessage with a StatusCode of OK (200) and Content of release note
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"title\":\"Release note 3.6 - Manager\",\"bodyText\":\"body text test\",\"id\":24,\"productId\":1,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":true,\"pickedWorkItems\": \"21345 12342 23345 \"}")
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
            var httpClientResult = await httpClient.PutAsync($"/ReleaseNotes/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            ReleaseNoteAdminViewModel testReleaseNote = new ReleaseNoteAdminViewModel
            {
                Id = 1,
                Title = "Release note 3.6 - Manager",
                BodyText = "body text test",
                ProductId = 2,
                CreatedBy = "Felix",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "Fredrik",
                LastUpdateDate = DateTime.Now,
                IsDraft = false,
                PickedWorkItems = "21345 23453"
            };

            var submitButton = "Save and publish";

            string[] PickedWorkItems = { "21345", "12342", "23345" };

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.EditReleaseNote(Id, testReleaseNote, submitButton, PickedWorkItems);

            // Assert
            Assert.Matches(@"^[a-zA-Z0-9, _ - ! ?. ""-]{3,100}$", testReleaseNote.Title);
            Assert.Matches(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", testReleaseNote.CreatedBy);
            Assert.Matches(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", testReleaseNote.LastUpdatedBy);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ViewReleaseNote", viewResult.ActionName);
        }

        [Fact]
        public async Task EditReleaseNote_Post_Should_Return_View_Regex_Fails()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("EditRN", "Failed"));

            // HttpResponseMessage with a StatusCode of OK (200) and Content of release note
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"title\":\"Release note 3.6 - Manager\",\"bodyText\":\"body text test\",\"id\":24,\"productId\":1,\"createdBy\":\"Felix\",\"createdDate\":\"2020-03-05T23:47:49\",\"lastUpdatedBy\":\"Fredrik\",\"lastUpdateDate\":\"2020-03-06T18:36:24\",\"isDraft\":true,\"pickedWorkItems\": \"21345 12342 23345 \"}")
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
            var httpClientResult = await httpClient.PutAsync($"/ReleaseNotes/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            ReleaseNoteAdminViewModel testReleaseNote = new ReleaseNoteAdminViewModel
            {
                Id = 1,
                Title = "@@@e34#¤%",
                BodyText = "body text test",
                ProductId = 2,
                CreatedBy = "@£@$€£$€$",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "£@£€$£}$$~~df",
                LastUpdateDate = DateTime.Now,
                IsDraft = false,
                PickedWorkItems = "21345 23453"
            };

            var submitButton = "Save and publish";

            string[] PickedWorkItems = { "21345", "12342", "23345" };

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.EditReleaseNote(Id, testReleaseNote, submitButton, PickedWorkItems);

            // Assert
            Assert.DoesNotMatch(@"^[a-zA-Z0-9, _ - ! ?. ""-]{3,100}$", testReleaseNote.Title);
            Assert.DoesNotMatch(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", testReleaseNote.CreatedBy);
            Assert.DoesNotMatch(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", testReleaseNote.LastUpdatedBy);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task EditReleaseNote_Post_Should_Throw_Exception()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("EditRN", "Success"));

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
            var httpClientResult = await httpClient.PutAsync($"/ReleaseNotes/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new ReleaseNotesAdminController(httpClientFactoryMock.Object);

            ReleaseNoteAdminViewModel testReleaseNote = new ReleaseNoteAdminViewModel
            {
                Id = 1,
                Title = "Release note 3.6 - Manager",
                BodyText = "body text test",
                ProductId = 2,
                CreatedBy = "Felix",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "Fredrik",
                LastUpdateDate = DateTime.Now,
                IsDraft = false,
                PickedWorkItems = "21345 23453"
            };

            var submitButton = "Save and publish";

            string[] PickedWorkItems = { "21345", "12342", "23345" };

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.EditReleaseNote(Id, testReleaseNote, submitButton, PickedWorkItems));
        }

        [Fact]
        public async Task DeleteReleaseNote_Post_Should_Delete_WorkItem_And_RedirectToAction()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("DeleteRN", "Success"));

            // HttpResponseMessage with a StatusCode of OK (200) and Content of release note
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
            Assert.IsAssignableFrom<RedirectToActionResult>(result);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListAllReleaseNotes", viewResult.ActionName);
        }

        [Fact]
        public async Task DeleteReleaseNote_Post_Should_Throw_Exception()
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
