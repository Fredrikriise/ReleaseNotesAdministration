using Api.Controllers;
using AutoMapper;
using Moq;
using Services.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace test.ApiTests.Controllers
{
    class WorkItemControllerApiTest
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


    }
}
