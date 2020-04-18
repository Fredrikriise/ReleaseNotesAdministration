using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class WorkItemController : Controller
    {
        //private readonly ILogger<WorkItemController> _logger;
        private readonly IMapper _mapper;
        private readonly IWorkItemRepository _workItemRepo;

        public WorkItemController(IWorkItemRepository workItemRepository, IMapper mapper)
        {
            _workItemRepo = workItemRepository;
            _mapper = mapper;
        }

        //Method to get all work item as a list
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

        // Method for creating work item
        [HttpPost]
        public async Task<IActionResult> Create(WorkItem workitem)
        {
            var mappedWorkItem = _mapper.Map<WorkItemDto>(workitem);
            await _workItemRepo.CreateWorkItem(mappedWorkItem);
            return Created("", workitem);
        }

        //Method for updating work item
        [HttpPut]
        [Route("/WorkItem/{Id}")]
        public async Task<IActionResult> UpdateWorkItem(int Id, WorkItem workItem)
        {
            var mappedWorkItem = _mapper.Map<WorkItemDto>(workItem);
            await _workItemRepo.UpdateWorkItem(Id, mappedWorkItem);
            return Ok();
        }

        //Method for deleting work item
        [HttpDelete]
        [Route("/WorkItem/{Id}")]
        public async Task<IActionResult> DeleteWorkItem(int Id)
        {
            var deletedWorkItem = await _workItemRepo.DeleteWorkItem(Id);

            if (deletedWorkItem)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}