using Microsoft.AspNetCore.Mvc;
using Moq;
using ReleaseNotes.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace test.ReleaseNotes.Controllers
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
        public void Task_List_All_Release_Notes_Should_Return_View()
        {
            // Arrange
            var controller = _controller;

            // Act
            var data = controller.ListAllReleaseNotes();

            // Assert
            Assert.IsAssignableFrom<Task<IActionResult>>(data);
        }

    }
}
