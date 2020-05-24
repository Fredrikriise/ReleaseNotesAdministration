using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Services.Repository;
using Services.Repository.Config;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test.ServicesTests.test_context;
using Xunit;

namespace test.ServicesTests.Repository
{
    public class WorkItemRepositoryTest
    {
        private readonly WorkItemRepository _workItemRepo;
        private readonly Mock<IMapper> _mapper;

        public WorkItemRepositoryTest()
        {
            DatabaseFixture database = new DatabaseFixture();
            _mapper = new Mock<IMapper>();
            _workItemRepo = new WorkItemRepository(Options.Create(new SqlDbConnection
            {
                ConnectionString = database.ConnectionString
            }), _mapper.Object);
        }

        [Fact]
        public async Task GetAllWorkItems_Should_ReturnMappedProduct()
        {
            // Arrange
            var repo = _workItemRepo;

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

            // Act
            _mapper.Setup(x => x.Map<List<WorkItemDto>>(It.IsAny<List<WorkItem>>())).Returns(testListWorkItemDto);
            var result = await repo.GetAllWorkItems();

            // Assert
            var test = Assert.IsType<List<WorkItemDto>>(result);
            Assert.IsAssignableFrom<List<WorkItemDto>>(test);
        }

        [Fact]
        public async Task GetAllWorkItems_Should_Throw_Exception_Mapping_Fails()
        {
            // Arrange
            var repo = _workItemRepo;

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.GetAllWorkItems());
        }

        [Fact]
        public async Task GetWorkItemById_Should_ReturnMappedProduct()
        {
            // Arrange
            var repo = _workItemRepo;
            var Id = 1;

            WorkItemDto testWorkItemDto = new WorkItemDto()
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            // Act
            _mapper.Setup(x => x.Map<WorkItemDto>(It.IsAny<WorkItem>())).Returns(testWorkItemDto);
            await repo.CreateWorkItem(testWorkItemDto);
            var result = await repo.GetWorkItemById(Id);

            // Assert
            var test = Assert.IsType<WorkItemDto>(result);
            Assert.IsAssignableFrom<WorkItemDto>(test);
        }

        [Fact]
        public async Task GetWorkItemById_Should_Throw_Exception_Mapping_Fails()
        {
            // Arrange
            var repo = _workItemRepo;
            var Id = 2;

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.GetWorkItemById(Id));
        }

        [Fact]
        public async Task CreateWorkItem_Should_Return_Result()
        {
            // Arrange
            var repo = _workItemRepo;

            WorkItemDto testWorkItemDto = new WorkItemDto()
            {
                Id = 21674,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            // Act
            var result = await repo.CreateWorkItem(testWorkItemDto);

            // Assert
            var test = Assert.IsType<int>(result);
            Assert.IsAssignableFrom<int>(test);
        }

        [Fact]
        public async Task CreateWorkItem_Should_Throw_Exception_WorkItem_Is_Null()
        {
            // Arrange
            var repo = _workItemRepo;

            WorkItemDto testWorkItemDto = new WorkItemDto()
            {
                Id = 1,
                Title = "21674 - WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };
            testWorkItemDto = null;

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.CreateWorkItem(testWorkItemDto));
        }

        [Fact]
        public async Task UpdateWorkItem_Should_Execute_Async()
        {
            // Arrange
            var repo = _workItemRepo;
            var Id = 21542;

            WorkItem testWorkItem = new WorkItem()
            {
                Id = 21674,
                Title = "WorkItemDto",
                AssignedTo = "Felix",
                State = "Active"
            };

            WorkItemDto testWorkItemDto = new WorkItemDto()
            {
                Id = 21542,
                Title = "WorkItemDto",
                AssignedTo = "Fredrik",
                State = "New"
            };

            // Act
            _mapper.Setup(x => x.Map<WorkItem>(It.IsAny<WorkItemDto>())).Returns(testWorkItem);
            var result = await repo.UpdateWorkItem(Id, testWorkItemDto);

            // Assert
            var test = Assert.IsType<WorkItemDto>(result);
            Assert.IsAssignableFrom<WorkItemDto>(test);
        }

        [Fact]
        public async Task UpdateWorkItem_Should_Throw_Exception_Mapping_Fails()
        {
            // Arrange
            var repo = _workItemRepo;
            var Id = 1;

            WorkItemDto testWorkItemDto = new WorkItemDto()
            {
                Id = 21542,
                Title = "WorkItemDto",
                AssignedTo = "Fredrik",
                State = "New"
            };

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.UpdateWorkItem(Id, testWorkItemDto));
        }

        [Fact]
        public async Task DeleteWorkItem_Should_Execute_Async()
        {
            // Arrange
            var repo = _workItemRepo;
            var Id = 21432;

            // Act
            var result = await repo.DeleteWorkItem(Id);

            // Assert
            var test = Assert.IsType<bool>(result);
            Assert.IsAssignableFrom<bool>(test);
        }
    }
}