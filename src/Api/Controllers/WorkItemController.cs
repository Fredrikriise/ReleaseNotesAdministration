using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WorkItemController : Controller
    {
        private readonly ILogger<WorkItemController> _logger;
        private readonly IMapper _mapper;
        private readonly IWorkItemRepository _workItemRepo;

        public WorkItemController(ILogger<WorkItemController> logger, IWorkItemRepository workItemRepository, IMapper mapper)
        {
            _logger = logger;
            _workItemRepo = workItemRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var returnedWorkItems = await _workItemRepo.GetAllWorkItems();
            var mappedWorkItems = _mapper.Map<List<WorkItem>>(returnedWorkItems);
            return Ok(mappedWorkItems);
        }

        // Method for getting work item by Id
        [HttpGet]
        [Route("/WorkItem/{Id}")]
        public async Task<IActionResult> GetWorkItemById(int Id)
        {
            var workItem = await _workItemRepo.GetWorkItemById(Id);

            if (workItem == null)
            {
                return NotFound();
            }

            var mappedWorkItem = _mapper.Map<WorkItem>(workItem);
            return Ok(mappedWorkItem);
        }
    }
}