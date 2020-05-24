using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;
using Xunit;

namespace test.ApiTests.Controllers
{
    public class WorkItemControllerApiTest
    {
        private readonly Mock<IWorkItemRepository> _mockRepo;
        private readonly WorkItemController _controller;
        private readonly Mock<IMapper> _mapper;

        public WorkItemControllerApiTest()
        {
            _mockRepo = new Mock<IWorkItemRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new WorkItemController(_mockRepo.Object, _mapper.Object);
        }

        [Fact]
        public async void GetAllWorkItems_Should_Return_OkObjectResult()
        {
            //Arrange
            var sut = _controller;

            List<WorkItemDto> testListWorkItemDto = new List<WorkItemDto>()
            {
                new WorkItemDto
                {
                    Id = 1,
                    Title = "21674 - WorkItemDto",
                    AssignedTo = "Felix",
                    State = "Active"
                },
                new WorkItemDto
                {
                    Id = 2,
                    Title = "25373 - WorkItemDto",
                    AssignedTo = "Fredrik",
                    State = "Closed"
                }
            };

            List<WorkItem> testListWorkItems = new List<WorkItem>()
            {
                 new WorkItem
                {
                    Id = 1,
                    Title = "22342 - WorkItem",
                    AssignedTo = "Felix",
                    State = "Active"
                },
                new WorkItem
                {
                    Id = 2,
                    Title = "20764 - WorkItem",
                    AssignedTo = "Fredrik",
                    State = "Closed"
                }
            };

            //Act
            _mockRepo.Setup(x => x.GetAllWorkItems()).ReturnsAsync(testListWorkItemDto);
            _mapper.Setup(x => x.Map<List<WorkItem>>(testListWorkItemDto)).Returns(testListWorkItems);
            var result = await sut.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllWorkItems_Should_Return_NotFoundResult_returnedWorkItems()
        {
            //Arrange
            var sut = _controller;

            List<WorkItemDto> testListWorkItemDto = new List<WorkItemDto>()
            {
                new WorkItemDto
                {
                    Id = 1,
                    Title = "21674 - WorkItemDto",
                    AssignedTo = "Felix",
                    State = "Active"
                },
                new WorkItemDto
                {
                    Id = 2,
                    Title = "25373 - WorkItemDto",
                    AssignedTo = "Fredrik",
                    State = "Closed"
                }
            };
            testListWorkItemDto = null;

            //Act
            _mockRepo.Setup(x => x.GetAllWorkItems()).ReturnsAsync(testListWorkItemDto);
            var result = await sut.Get();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetAllWorkItems_Should_Return_NotFoundResult_mappedWorkItems()
        {
            //Arrange
            var sut = _controller;

            List<WorkItemDto> testListWorkItemDto = new List<WorkItemDto>()
            {
                new WorkItemDto
                {
                    Id = 1,
                    Title = "21674 - WorkItemDto",
                    AssignedTo = "Felix",
                    State = "Active"
                },
                new WorkItemDto
                {
                    Id = 2,
                    Title = "25373 - WorkItemDto",
                    AssignedTo = "Fredrik",
                    State = "Closed"
                }
            };

            //Act
            _mockRepo.Setup(x => x.GetAllWorkItems()).ReturnsAsync(testListWorkItemDto);
            var result = await sut.Get();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetWorkItemById_Should_Return_OkObjectResult()
        {
            //Arrange
            var sut = _controller;
            var Id = 1;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            WorkItem testWorkItem = new WorkItem
            {
                Id = 2,
                Title = "20764 - WorkItem",
                AssignedTo = "Fredrik",
                State = "Closed"
            };

            // Act
            _mockRepo.Setup(x => x.GetWorkItemById(It.IsAny<int>())).ReturnsAsync(testWorkItemDto);
            _mapper.Setup(x => x.Map<WorkItem>(testWorkItemDto)).Returns(testWorkItem);
            var result = await sut.GetWorkItemById(Id);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetWorkItemById_Should_Return_NotFoundResult_workItem()
        {
            //Arrange
            var sut = _controller;
            var Id = 0;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };
            testWorkItemDto = null;

            // Act
            _mockRepo.Setup(x => x.GetWorkItemById(It.IsAny<int>())).ReturnsAsync(testWorkItemDto);
            var result = await sut.GetWorkItemById(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetWorkItemById_Should_Return_NotFoundResult_mappedWorkItem()
        {
            //Arrange
            var sut = _controller;
            var Id = 1;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            // Act
            _mockRepo.Setup(x => x.GetWorkItemById(It.IsAny<int>())).ReturnsAsync(testWorkItemDto);
            var result = await sut.GetWorkItemById(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void CreateWorkItem_Should_Return_CreatedResult()
        {
            //Arrange
            var sut = _controller;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            WorkItemDto testWorkItemDtoResult = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDtoResult",
                AssignedTo = "Fredrik",
                State = "New"
            };

            WorkItem testWorkItem = new WorkItem
            {
                Id = 2,
                Title = "20764 - WorkItem",
                AssignedTo = "Fredrik",
                State = "Closed"
            };

            //Act
            _mapper.Setup(x => x.Map<WorkItemDto>(testWorkItem)).Returns(testWorkItemDtoResult);
            _mockRepo.Setup(x => x.CreateWorkItem(testWorkItemDto));
            var result = await sut.Create(testWorkItem);

            //Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async void CreateWorkItem_Should_Return_NotFound_mappedWorkItem()
        {
            //Arrange
            var sut = _controller;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            WorkItem testWorkItem = new WorkItem
            {
                Id = 2,
                Title = "20764 - WorkItem",
                AssignedTo = "Fredrik",
                State = "Closed"
            };

            //Act
            _mockRepo.Setup(x => x.CreateWorkItem(testWorkItemDto));
            var result = await sut.Create(testWorkItem);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void CreateWorkItem_Should_Return_NotFound_workitem()
        {
            //Arrange
            var sut = _controller;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            WorkItemDto testWorkItemDtoResult = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDtoResult",
                AssignedTo = "Fredrik",
                State = "New"
            };

            WorkItem testWorkItem = new WorkItem
            {
                Id = 2,
                Title = "20764 - WorkItem",
                AssignedTo = "Fredrik",
                State = "Closed"
            };
            testWorkItem = null;

            //Act
            _mapper.Setup(x => x.Map<WorkItemDto>(testWorkItem)).Returns(testWorkItemDtoResult);
            _mockRepo.Setup(x => x.CreateWorkItem(testWorkItemDto));
            var result = await sut.Create(testWorkItem);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void UpdateWorkItem_Should_Return_OkResult()
        {
            //Arrange
            var sut = _controller;
            var Id = 1;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            WorkItemDto testWorkItemDtoResult = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDtoResult",
                AssignedTo = "Fredrik",
                State = "New"
            };

            WorkItem testWorkItem = new WorkItem
            {
                Id = 2,
                Title = "20764 - WorkItem",
                AssignedTo = "Fredrik",
                State = "Closed"
            };

            //Act
            _mapper.Setup(x => x.Map<WorkItemDto>(testWorkItem)).Returns(testWorkItemDto);
            _mockRepo.Setup(x => x.UpdateWorkItem(It.IsAny<int>(), testWorkItemDto)).ReturnsAsync(testWorkItemDtoResult);
            var result = await sut.UpdateWorkItem(Id, testWorkItem);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateWorkItem_Should_Return_NotFound_mappedWorkItem()
        {
            //Arrange
            var sut = _controller;
            var Id = 1;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            WorkItemDto testWorkItemDtoResult = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDtoResult",
                AssignedTo = "Fredrik",
                State = "New"
            };

            WorkItem testWorkItem = new WorkItem
            {
                Id = 2,
                Title = "20764 - WorkItem",
                AssignedTo = "Fredrik",
                State = "Closed"
            };

            //Act
            _mockRepo.Setup(x => x.UpdateWorkItem(It.IsAny<int>(), testWorkItemDto)).ReturnsAsync(testWorkItemDtoResult);
            var result = await sut.UpdateWorkItem(Id, testWorkItem);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void UpdateWorkItem_Should_Return_NotFound_updatedWorkItem()
        {
            //Arrange
            var sut = _controller;
            var Id = 1;

            WorkItemDto testWorkItemDto = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            WorkItemDto testWorkItemDtoResult = new WorkItemDto
            {
                Id = 1,
                Title = "21674 - WorkItemDtoResult",
                AssignedTo = "Fredrik",
                State = "New"
            };
            testWorkItemDtoResult = null;

            WorkItem testWorkItem = new WorkItem
            {
                Id = 2,
                Title = "20764 - WorkItem",
                AssignedTo = "Fredrik",
                State = "Closed"
            };

            //Act
            _mapper.Setup(x => x.Map<WorkItemDto>(testWorkItem)).Returns(testWorkItemDto);
            _mockRepo.Setup(x => x.UpdateWorkItem(It.IsAny<int>(), testWorkItemDto)).ReturnsAsync(testWorkItemDtoResult);
            var result = await sut.UpdateWorkItem(Id, testWorkItem);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeleteWorkItem_Should_Return_OkResult()
        {
            //Arrange
            var sut = _controller;
            var Id = 1;

            //Act
            _mockRepo.Setup(x => x.DeleteWorkItem(It.IsAny<int>())).ReturnsAsync(true);
            var result = await sut.DeleteWorkItem(Id);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DeleteWorkItem_Should_Return_NotFoundResult_deletedWorkItem()
        {
            //Arrange
            var sut = _controller;
            var Id = 0;

            //Act
            _mockRepo.Setup(x => x.DeleteWorkItem(It.IsAny<int>())).ReturnsAsync(false);
            var result = await sut.DeleteWorkItem(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
