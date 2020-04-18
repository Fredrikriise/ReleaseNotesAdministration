using Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Repository.Interfaces;
using Services.Repository.Models;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using Xunit;

namespace test.ApiTests.Controllers
{
    public class ReleaseNotesControllerApiTest
    {
        private readonly Mock<IReleaseNotesRepository> _mockRepo;
        private readonly ReleaseNotesController _controller;
        private readonly Mock<IMapper> _mapper;

        public ReleaseNotesControllerApiTest()
        {
            _mockRepo = new Mock<IReleaseNotesRepository>();
            _mapper = new Mock<IMapper>();
            _controller = new ReleaseNotesController(_mockRepo.Object, _mapper.Object);
        }

        [Fact]
        public async void GetAllReleaseNotes_Should_Return_OkObjectResult()
        {
            //Arrange
            var sut = _controller;

            List<ReleaseNoteDto> testListReleaseNoteDto = new List<ReleaseNoteDto>()
            {
                new ReleaseNoteDto
                {
                    Id = 1,
                    Title = "Title 1 - ReleasenoteDto",
                    BodyText = "Bodytext 1",
                    ProductId = 1,
                    CreatedBy = "CreatedBy 1",
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "LastUpdatedBy 1",
                    LastUpdateDate = DateTime.Today,
                    IsDraft = true,
                    PickedWorkItems = "23563 20345"
                },
                new ReleaseNoteDto
                {
                    Id = 2,
                    Title = "Title 2 - ReleasenoteDto",
                    BodyText = "BodyText 2",
                    ProductId = 2,
                    CreatedBy = "CreatedBy 2",
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "LastUpdatedBy 2",
                    LastUpdateDate = DateTime.Today,
                    IsDraft = false,
                    PickedWorkItems = "21345 23453"
                }
            };

            List<ReleaseNote> testListReleaseNote = new List<ReleaseNote>()
            {
                new ReleaseNote
                {
                    Id = 1,
                    Title = "Title 1 - Releasenote",
                    BodyText = "Bodytext 1",
                    ProductId = 1,
                    CreatedBy = "CreatedBy 1",
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "LastUpdatedBy 1",
                    LastUpdateDate = DateTime.Today,
                    IsDraft = true,
                    PickedWorkItems = "23563 20345"
                },
                new ReleaseNote
                {
                    Id = 2,
                    Title = "Title 2 - Releasenote",
                    BodyText = "BodyText 2",
                    ProductId = 2,
                    CreatedBy = "CreatedBy 2",
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "LastUpdatedBy 2",
                    LastUpdateDate = DateTime.Today,
                    IsDraft = false,
                    PickedWorkItems = "21345 23453"
                }
            };

            //Act
            _mockRepo.Setup(x => x.GetAllReleaseNotes()).ReturnsAsync(testListReleaseNoteDto);
            _mapper.Setup(x => x.Map<List<ReleaseNote>>(testListReleaseNoteDto)).Returns(testListReleaseNote);
            var result = await sut.Get();

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetAllReleaseNotes_Should_Return_NotFoundResult_returnedReleaseNotes()
        {
            //Arrange
            var sut = _controller;

            List<ReleaseNoteDto> testListReleaseNoteDto = new List<ReleaseNoteDto>()
            {
                new ReleaseNoteDto
                {
                    Id = 1,
                    Title = "Title 1 - ReleasenoteDto",
                    BodyText = "Bodytext 1",
                    ProductId = 1,
                    CreatedBy = "CreatedBy 1",
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "LastUpdatedBy 1",
                    LastUpdateDate = DateTime.Today,
                    IsDraft = true,
                    PickedWorkItems = "23563 20345"
                },
                new ReleaseNoteDto
                {
                    Id = 2,
                    Title = "Title 2 - ReleasenoteDto",
                    BodyText = "BodyText 2",
                    ProductId = 2,
                    CreatedBy = "CreatedBy 2",
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "LastUpdatedBy 2",
                    LastUpdateDate = DateTime.Today,
                    IsDraft = false,
                    PickedWorkItems = "21345 23453"
                }
            };
            testListReleaseNoteDto = null;

            //Act
            _mockRepo.Setup(x => x.GetAllReleaseNotes()).ReturnsAsync(testListReleaseNoteDto);
            var result = await sut.Get();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetAllReleaseNotes_Should_Return_NotFoundResult_mappedReleaseNotes()
        {
            //Arrange
            var sut = _controller;

            List<ReleaseNoteDto> testListReleaseNoteDto = new List<ReleaseNoteDto>()
            {
                new ReleaseNoteDto
                {
                    Id = 1,
                    Title = "Title 1 - ReleasenoteDto",
                    BodyText = "Bodytext 1",
                    ProductId = 1,
                    CreatedBy = "CreatedBy 1",
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "LastUpdatedBy 1",
                    LastUpdateDate = DateTime.Today,
                    IsDraft = true,
                    PickedWorkItems = "23563 20345"
                },
                new ReleaseNoteDto
                {
                    Id = 2,
                    Title = "Title 2 - ReleasenoteDto",
                    BodyText = "BodyText 2",
                    ProductId = 2,
                    CreatedBy = "CreatedBy 2",
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = "LastUpdatedBy 2",
                    LastUpdateDate = DateTime.Today,
                    IsDraft = false,
                    PickedWorkItems = "21345 23453"
                }
            };

            //Act
            _mockRepo.Setup(x => x.GetAllReleaseNotes()).ReturnsAsync(testListReleaseNoteDto);
            var result = await sut.Get();

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetReleaseNoteById_Should_Return_OkObjectResult()
        {
            //Arrange
            var sut = _controller;

            var Id = 1;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleasenoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNote testReleaseNote = new ReleaseNote
            {
                Id = 1,
                Title = "Title 1 - Releasenote",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            // Act
            _mockRepo.Setup(x => x.GetReleaseNoteById(Id)).ReturnsAsync(testReleaseNoteDto);
            _mapper.Setup(x => x.Map<ReleaseNote>(testReleaseNoteDto)).Returns(testReleaseNote);
            var result = await sut.GetReleaseNoteById(Id);

            //Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetReleaseNoteById_Should_Return_NotFoundResult_releaseNote()
        {
            //Arrange
            var sut = _controller;

            var Id = 0;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleasenoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };
            testReleaseNoteDto = null;

            // Act
            _mockRepo.Setup(x => x.GetReleaseNoteById(Id)).ReturnsAsync(testReleaseNoteDto);
            var result = await sut.GetReleaseNoteById(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetReleaseNoteById_Should_Return_NotFoundResult_mappedReleaseNote()
        {
            //Arrange
            var sut = _controller;

            var Id = 1;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleasenoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            // Act
            _mockRepo.Setup(x => x.GetReleaseNoteById(Id)).ReturnsAsync(testReleaseNoteDto);
            var result = await sut.GetReleaseNoteById(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void CreateReleaseNote_Should_Return_CreatedResult()
        {
            //Arrange
            var sut = _controller;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNoteDto testReleaseNoteDtoResult = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDtoResult",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNote testReleaseNote = new ReleaseNote
            {
                Id = 1,
                Title = "Title 1 - ReleaseNote",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            //Act
            _mapper.Setup(x => x.Map<ReleaseNoteDto>(testReleaseNote)).Returns(testReleaseNoteDtoResult);
            _mockRepo.Setup(x => x.CreateReleaseNote(testReleaseNoteDto));
            var result = await sut.Create(testReleaseNote);

            //Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async void CreateReleaseNote_Should_Return_NotFound_mappedReleaseNote()
        {
            //Arrange
            var sut = _controller;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNote testReleaseNote = new ReleaseNote
            {
                Id = 1,
                Title = "Title 1 - ReleaseNote",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            //Act
            _mockRepo.Setup(x => x.CreateReleaseNote(testReleaseNoteDto));
            var result = await sut.Create(testReleaseNote);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void CreateReleaseNote_Should_Return_NotFound_releaseNote()
        {
            //Arrange
            var sut = _controller;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNoteDto testReleaseNoteDtoResult = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDtoResult",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNote testReleaseNote = new ReleaseNote
            {
                Id = 1,
                Title = "Title 1 - ReleaseNote",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };
            testReleaseNote = null;

            //Act
            _mapper.Setup(x => x.Map<ReleaseNoteDto>(testReleaseNote)).Returns(testReleaseNoteDtoResult);
            _mockRepo.Setup(x => x.CreateReleaseNote(testReleaseNoteDto));
            var result = await sut.Create(testReleaseNote);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void UpdateReleaseNote_Should_Return_OkResult()
        {
            //Arrange
            var sut = _controller;

            int? Id = 1;

            ReleaseNote testReleaseNote = new ReleaseNote
            {
                Id = 1,
                Title = "Title 1 - ReleaseNote",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNoteDto testReleaseNoteDtoResult = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDtoResult",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            //Act
            _mapper.Setup(x => x.Map<ReleaseNoteDto>(testReleaseNote)).Returns(testReleaseNoteDto);
            _mockRepo.Setup(x => x.UpdateReleaseNote(Id, testReleaseNoteDto)).ReturnsAsync(testReleaseNoteDtoResult);
            var result = await sut.UpdateReleaseNote(Id, testReleaseNote);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void UpdateReleaseNote_Should_Return_NotFound_mappedReleaseNote()
        {
            //Arrange
            var sut = _controller;

            int? Id = 1;

            ReleaseNote testReleaseNote = new ReleaseNote
            {
                Id = 1,
                Title = "Title 1 - ReleaseNote",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNoteDto testReleaseNoteDtoResult = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDtoResult",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            //Act
            _mockRepo.Setup(x => x.UpdateReleaseNote(Id, testReleaseNoteDto)).ReturnsAsync(testReleaseNoteDtoResult);
            var result = await sut.UpdateReleaseNote(Id, testReleaseNote);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void UpdateReleaseNote_Should_Return_NotFound_updatedReleaseNote()
        {
            //Arrange
            var sut = _controller;

            int? Id = 1;

            ReleaseNote testReleaseNote = new ReleaseNote
            {
                Id = 1,
                Title = "Title 1 - ReleaseNote",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDto",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = true,
                PickedWorkItems = "23563 20345"
            };

            ReleaseNoteDto testReleaseNoteDtoResult = new ReleaseNoteDto
            {
                Id = 1,
                Title = "Title 1 - ReleaseNoteDtoResult",
                BodyText = "Bodytext 1",
                ProductId = 1,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };
            testReleaseNoteDtoResult = null;

            //Act
            _mapper.Setup(x => x.Map<ReleaseNoteDto>(testReleaseNote)).Returns(testReleaseNoteDto);
            _mockRepo.Setup(x => x.UpdateReleaseNote(Id, testReleaseNoteDto)).ReturnsAsync(testReleaseNoteDtoResult);
            var result = await sut.UpdateReleaseNote(Id, testReleaseNote);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void DeleteReleaseNote_Should_Return_OkResult()
        {
            //Arrange
            var sut = _controller;

            int? Id = 1;

            //Act
            _mockRepo.Setup(x => x.DeleteReleaseNote(Id)).ReturnsAsync(true);
            var result = await sut.DeleteReleaseNote(Id);

            //Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async void DeleteReleaseNote_Should_Return_NotFoundResult_deletedReleaseNote()
        {
            //Arrange
            var sut = _controller;

            int? Id = 0;

            //Act
            _mockRepo.Setup(x => x.DeleteReleaseNote(Id)).ReturnsAsync(false);
            var result = await sut.DeleteReleaseNote(Id);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
