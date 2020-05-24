using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Services;
using Services.Repository.Config;
using Services.Repository.Models;
using Services.Repository.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using test.ServicesTests.test_context;
using Xunit;

namespace test.ServicesTests.Repository
{
    public class ReleaseNotesRepositoryTest
    {
        private readonly ReleaseNotesRepository _releaseNotesRepo;
        private readonly Mock<IMapper> _mapper;

        public ReleaseNotesRepositoryTest()
        {
            DatabaseFixture database = new DatabaseFixture();

            _mapper = new Mock<IMapper>();
            _releaseNotesRepo = new ReleaseNotesRepository(Options.Create(new SqlDbConnection
            {
                ConnectionString = database.ConnectionString
            }), _mapper.Object);
        }

        [Fact]
        public async Task GetAllReleaseNotes_Should_Return_Mapped_ReleaseNotes()
        {
            // Arrange
            var repo = _releaseNotesRepo;

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

            // Act
            _mapper.Setup(x => x.Map<List<ReleaseNoteDto>>(It.IsAny<List<ReleaseNote>>())).Returns(testListReleaseNoteDto);
            var result = await repo.GetAllReleaseNotes();

            // Assert
            var test = Assert.IsType<List<ReleaseNoteDto>>(result);
            Assert.IsAssignableFrom<List<ReleaseNoteDto>>(test);
        }

        [Fact]
        public async Task GetAllReleaseNotes_Should_Throw_Exception_Mapping_Fails()
        {
            // Arrange
            var repo = _releaseNotesRepo;

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.GetAllReleaseNotes());
        }

        [Fact]
        public async Task GetReleaseNoteById_Should_Return_Mapped_ReleaseNote()
        {
            // Arrange
            var repo = _releaseNotesRepo;
            var Id = 1;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto()
            {
                Id = 1,
                Title = "Title 1 - ReleasenoteDto",
                BodyText = "Bodytext 1",
                ProductId = 2,
                CreatedBy = "CreatedBy 1",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 1",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "23563 20345"
            };

            // Act
            _mapper.Setup(x => x.Map<ReleaseNoteDto>(It.IsAny<ReleaseNote>())).Returns(testReleaseNoteDto);
            await repo.CreateReleaseNote(testReleaseNoteDto);
            var result = await repo.GetReleaseNoteById(Id);

            // Assert
            var test = Assert.IsType<ReleaseNoteDto>(result);
            Assert.IsAssignableFrom<ReleaseNoteDto>(test);
        }

        [Fact]
        public async Task GetReleaseNoteById_Should_Throw_Exception_Mapping_Fails()
        {
            // Arrange
            var repo = _releaseNotesRepo;
            var Id = 2;

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.GetReleaseNoteById(Id));
        }

        [Fact]
        public async Task CreateReleaseNote_Should_Return_Result()
        {
            // Arrange
            var repo = _releaseNotesRepo;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto()
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
            var result = await repo.CreateReleaseNote(testReleaseNoteDto);

            // Assert
            var test = Assert.IsType<int>(result);
            Assert.IsAssignableFrom<int>(test);
        }

        [Fact]
        public async Task CreateReleaseNote_Should_Throw_Exception_ReleaseNote_Is_Null()
        {
            // Arrange
            var repo = _releaseNotesRepo;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto()
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
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.CreateReleaseNote(testReleaseNoteDto));
        }

        [Fact]
        public async Task UpdateReleaseNote_Should_Execute_Async()
        {
            // Arrange
            var repo = _releaseNotesRepo;
            var Id = 1;

            ReleaseNote testReleaseNote = new ReleaseNote()
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

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto()
            {
                Id = 3,
                Title = "Title 3 - ReleasenoteDto",
                BodyText = "Bodytext 3",
                ProductId = 3,
                CreatedBy = "CreatedBy 3",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 3",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "34567 23452"
            };

            // Act
            _mapper.Setup(x => x.Map<ReleaseNote>(It.IsAny<ReleaseNoteDto>())).Returns(testReleaseNote);
            var result = await repo.UpdateReleaseNote(Id, testReleaseNoteDto);

            // Assert
            var test = Assert.IsType<ReleaseNoteDto>(result);
            Assert.IsAssignableFrom<ReleaseNoteDto>(test);
        }

        [Fact]
        public async Task UpdateReleaseNote_Should_Throw_Exception_Mapping_Fails()
        {
            // Arrange
            var repo = _releaseNotesRepo;
            var Id = 1;

            ReleaseNoteDto testReleaseNoteDto = new ReleaseNoteDto()
            {
                Id = 3,
                Title = "Title 3 - ReleasenoteDto",
                BodyText = "Bodytext 3",
                ProductId = 3,
                CreatedBy = "CreatedBy 3",
                CreatedDate = DateTime.Now,
                LastUpdatedBy = "LastUpdatedBy 3",
                LastUpdateDate = DateTime.Today,
                IsDraft = false,
                PickedWorkItems = "34567 23452"
            };

            // Act
            var ex = await Assert.ThrowsAsync<Exception>(() => repo.UpdateReleaseNote(Id, testReleaseNoteDto));
        }

        [Fact]
        public async Task DeleteReleaseNote_Should_Execute_Async()
        {
            // Arrange
            var repo = _releaseNotesRepo;
            var Id = 1;

            // Act
            var result = await repo.DeleteReleaseNote(Id);

            // Assert
            var test = Assert.IsType<bool>(result);
            Assert.IsAssignableFrom<bool>(test);
        }
    }
}
