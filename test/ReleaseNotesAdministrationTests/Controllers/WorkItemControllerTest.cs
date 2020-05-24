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
    public class WorkItemControllerTest
    {
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private Mock<HttpClient> _mockHttpClient;
        private readonly WorkItemAdminController _controller;

        public WorkItemControllerTest()
        {
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _mockHttpClient = new Mock<HttpClient>();
            _controller = new WorkItemAdminController(_mockClientFactory.Object);
        }

        [Fact]
        public async Task ListAllWorkItems_Should_Return_View_With_List()
        {
            // Arrange
            // HttpResponseMessage with a StatusCode of OK (200) and Content of work items
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "[{\"id\":21625,\"title\":\"Adding the styling to correct file (User module)\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}," +
                    "{\"id\":21680,\"title\":\"Make listing of 'All Release Notes' descending based of publish-date\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}]")
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
            var httpClientResult = await httpClient.GetAsync("/WorkItem/");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ListAllWorkItems();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(2, ((List<WorkItemViewModel>)viewResult.ViewData.Model).Count);

            var workitems = viewResult.ViewData.Model as IList<WorkItemViewModel>;
            Assert.Equal(21625, workitems[0].Id);
            Assert.Equal("Adding the styling to correct file (User module)", workitems[0].Title);
            Assert.Equal("Fredrik Riise", workitems[0].AssignedTo);
            Assert.Equal("New", workitems[0].State);

            Assert.Equal(21680, workitems[1].Id);
            Assert.Equal("Make listing of 'All Release Notes' descending based of publish-date", workitems[1].Title);
            Assert.Equal("Fredrik Riise", workitems[1].AssignedTo);
            Assert.Equal("New", workitems[1].State);

            Assert.IsAssignableFrom<List<WorkItemViewModel>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ListAllWorkItems_Should_Throw_Exception()
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
            var httpClientResult = await httpClient.GetAsync("/WorkItem/");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ListAllWorkItems());
        }

        [Fact]
        public async Task ViewWorkItem_Should_Return_View_With_workitem()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work item
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"id\":21625,\"title\":\"Adding the styling to correct file (User module)\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}")
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
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.ViewWorkItem(Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(21625, ((WorkItemViewModel)viewResult.ViewData.Model).Id);
            Assert.Equal("Adding the styling to correct file (User module)", ((WorkItemViewModel)viewResult.ViewData.Model).Title);
            Assert.Equal("Fredrik Riise", ((WorkItemViewModel)viewResult.ViewData.Model).AssignedTo);
            Assert.Equal("New", ((WorkItemViewModel)viewResult.ViewData.Model).State);

            Assert.IsAssignableFrom<WorkItemViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task ViewWorkItem_Should_Throw_Exception()
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
            var httpClientResult = await httpClient.GetAsync($"/WorkItem/{Id}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient"))
                                    .Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.ViewWorkItem(Id));
        }

        [Fact]
        public void Create_Should_Return_View()
        {
            //Arrange
            var controller = _controller;

            // Act
            var result = controller.Create();

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task CreateWorkItem_Should_Create_WorkItem()
        {
            // Arrange
            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("CreateWorkItem", "Success"));

            // testWorkItem for creation of work item
            WorkItemApiModel testWorkItem = new WorkItemApiModel
            {
                Id = 23423,
                Title = "Work item test",
                AssignedTo = "Felix",
                State = "New"
            };

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work item
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"id\":21625,\"title\":\"Adding the styling to correct file (User module)\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}")
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
            var httpClientResult = await httpClient.PostAsync("/WorkItem/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.CreateWorkItem(testWorkItem);

            // Assert
            Assert.Matches(@"^[0-9]{1,99}$",
                testWorkItem.Id.ToString());
            Assert.Matches(@"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$",
                testWorkItem.Title);
            Assert.Matches(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$",
                testWorkItem.AssignedTo);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListAllWorkItems", viewResult.ActionName);
            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task CreateWorkItem_Should_Return_Create_View_Regex_Fails()
        {
            // Arrange
            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("CreateWorkItem", "Failed"));

            // testWorkItem for creation of work item
            WorkItemApiModel testWorkItem = new WorkItemApiModel
            {
                Id = -1,
                Title = "£2@@fgdjgi@",
                AssignedTo = "23425€34€$£@",
                State = "$£€.-.!!#"
            };

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work item
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"id\":0,\"title\":\"£2@@fgdjgi@\",\"assignedTo\":\"23425€34€$£@\",\"state\":\"$£€.-.!!#\"}")
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
            var httpClientResult = await httpClient.PostAsync("/WorkItem/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.CreateWorkItem(testWorkItem);

            // Assert
            Assert.DoesNotMatch(@"^[0-9]{1,99}$",
                testWorkItem.Id.ToString());
            Assert.DoesNotMatch(@"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$",
                testWorkItem.Title);
            Assert.DoesNotMatch(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$",
                testWorkItem.AssignedTo);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task CreateWorkItem_Should_Throw_Exception()
        {
            // Arrange
            // testWorkItem for creation of work item
            WorkItemApiModel testWorkItem = new WorkItemApiModel
            {
                Id = 23423,
                Title = "Work item test",
                AssignedTo = "Felix",
                State = "New"
            };

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
            var httpClientResult = await httpClient.PostAsync("/WorkItem/", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.CreateWorkItem(testWorkItem));
        }

        [Fact]
        public async Task EditWorkItem_With_Only_IdAsParameter_Should_Return_View_With_Updated_WorkItem()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work item
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"id\":21625,\"title\":\"Adding the styling to correct file (User module)\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}")
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
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var result = await controller.EditWorkItem(Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Adding the styling to correct file (User module)", ((WorkItemViewModel)viewResult.ViewData.Model).Title);
            Assert.Equal("Fredrik Riise", ((WorkItemViewModel)viewResult.ViewData.Model).AssignedTo);
            Assert.Equal("New", ((WorkItemViewModel)viewResult.ViewData.Model).State);

            Assert.IsAssignableFrom<WorkItemViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task EditWorkIten_With_Only_IdAsParameter_Should_Throw_Exception()
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
            var httpClientResult = await httpClient.GetAsync($"/WorkItem/{Id}");
            var content = await httpClientResult.Content.ReadAsStringAsync();

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.EditWorkItem(Id));
        }

        [Fact]
        public async Task EditWorkItem_Should_Redirect_To_ViewWorkItem()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("EditWorkItem", "Success"));

            // testWorkItem for editing work item
            WorkItemViewModel testWorkItem = new WorkItemViewModel
            {
                Id = 21123,
                Title = "Work item test",
                AssignedTo = "Fredrik",
                State = "Active"
            };

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work item
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"id\":21625,\"title\":\"Adding the styling to correct file (User module)\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}")
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
            var httpClientResult = await httpClient.PutAsync($"/WorkItem/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.EditWorkItem(Id, testWorkItem);

            // Assert
            Assert.Matches(@"^[0-9]{1,99}$",
                testWorkItem.Id.ToString());
            Assert.Matches(@"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$",
                testWorkItem.Title);
            Assert.Matches(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$",
                testWorkItem.AssignedTo);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ViewWorkItem", viewResult.ActionName);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task EditWorkItem_Should_Return_Edit_View_Regex_Fails()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("EditWorkItem", "Failed"));

            // testWorkItem for creation of work item
            WorkItemViewModel testWorkItem = new WorkItemViewModel
            {
                Id = -1,
                Title = "£2@@fgdjgi@",
                AssignedTo = "23425€34€$£@",
                State = "$£€.-.!!#"
            };

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work item
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"id\":-1,\"title\":\"£2@@fgdjgi@\",\"assignedTo\":\"23425€34€$£@\",\"state\":\"$£€.-.!!#\"}")
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
            var httpClientResult = await httpClient.PutAsync($"/WorkItem/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.EditWorkItem(Id, testWorkItem);

            // Assert
            Assert.DoesNotMatch(@"^[0-9]{1,99}$",
                testWorkItem.Id.ToString());
            Assert.DoesNotMatch(@"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$",
                testWorkItem.Title);
            Assert.DoesNotMatch(@"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$",
                testWorkItem.AssignedTo);

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task EditWorkItem_Should_Throw_Exception()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // testProduct 
            WorkItemViewModel testWorkItem = new WorkItemViewModel
            {
                Id = 21123,
                Title = "Work item test",
                AssignedTo = "Fredrik",
                State = "Active"
            };

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
            var httpClientResult = await httpClient.PutAsync($"/WorkItem/{Id}", msg.Content);

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.EditWorkItem(Id, testWorkItem));
        }

        [Fact]
        public async Task DeleteWorkItem_Should_Delete_WorkItem_And_RedirectToAction()
        {
            // Arrange
            var Id = It.IsAny<int>();

            // mocking TempData 
            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x.Add("DeleteWorkItem", "Success"));

            // HttpResponseMessage with a StatusCode of OK (200) and Content of work item
            HttpResponseMessage msg = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    "{\"id\":21625,\"title\":\"Adding the styling to correct file (User module)\",\"assignedTo\":\"Fredrik Riise\",\"state\":\"New\"}")
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
            var httpClientResult = await httpClient.DeleteAsync($"/WorkItem/{Id}");

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            controller.TempData = tempDataMock.Object;
            var result = await controller.DeleteWorkItem(Id);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListAllWorkItems", viewResult.ActionName);

            Assert.IsAssignableFrom<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task DeleteWorkItem_Should_Throw_Exception()
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
            var httpClientResult = await httpClient.DeleteAsync($"/WorkItem/{Id}");

            var httpClientFactoryMock = _mockClientFactory;
            var client = httpClientFactoryMock.Setup(x => x.CreateClient("ReleaseNotesAdminApiClient")).Returns(httpClient);

            var controller = new WorkItemAdminController(httpClientFactoryMock.Object);

            // Act
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() => controller.DeleteWorkItem(Id));
        }
    }
}
