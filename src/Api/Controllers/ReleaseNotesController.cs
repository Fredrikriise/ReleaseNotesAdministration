using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Repository.Interfaces;
using Services.Repository.Models;
using Services.Repository.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ReleaseNotesController : ControllerBase
    {
        private readonly ILogger<ReleaseNotesController> _logger;
        private readonly IMapper _mapper;
        private readonly IReleaseNotesRepository _releaseNoteRepo;

        public ReleaseNotesController(ILogger<ReleaseNotesController> logger, IReleaseNotesRepository releaseNoteRepository, IMapper mapper)
        {
            _logger = logger;
            _releaseNoteRepo = releaseNoteRepository;
            _mapper = mapper;
        }

        // Method for getting all release notes
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //if(!ProductId.HasValue)
            //{
            //    _logger.LogWarning($"The {nameof(ProductId)} : {ProductId} is not a valid parameter value");
            //}

            var returnedReleaseNotes = await _releaseNoteRepo.GetAllReleaseNotes();
            var mappedReleaseNotes = _mapper.Map<List<ReleaseNote>>(returnedReleaseNotes);
            return Ok(mappedReleaseNotes);
        }

        // Method for creating Release Note
        [HttpPost]
        public async Task<IActionResult> Create(ReleaseNote releaseNote)
        {
            var mappedReleaseNote = _mapper.Map<ReleaseNoteDto>(releaseNote);
            await _releaseNoteRepo.CreateReleaseNote(mappedReleaseNote);
            return Created("", releaseNote);
        }

        // Method for editing/updating Release note with Id
        [HttpPut]
        [Route("/ReleaseNotes/{Id}")]
        public async Task<IActionResult> UpdateReleaseNote(int? Id, ReleaseNote releaseNote)
        {
            var mappedReleaseNote = _mapper.Map<ReleaseNoteDto>(releaseNote);
            await _releaseNoteRepo.UpdateReleaseNote(Id, mappedReleaseNote);
            return Ok();
        }

        // Method for deleting relese 
        [HttpDelete]
        [Route("/ReleaseNotes/{Id}")]
        public async Task<IActionResult> DeleteReleaseNote(int? Id)
        {
            var deletedReleaseNote = await _releaseNoteRepo.DeleteReleaseNote(Id);

            if (deletedReleaseNote)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // Method for getting Release Note with Id
        [HttpGet]
        [Route("/ReleaseNotes/{Id}")]
        public async Task<IActionResult> GetReleaseNoteById(int? Id)
        {
            var releaseNote = await _releaseNoteRepo.GetReleaseNoteById(Id);

            if (releaseNote == null)
            {
                return NotFound();
            }

            var mappedReleaseNote = _mapper.Map<ReleaseNote>(releaseNote);
            return Ok(mappedReleaseNote);
        }
    }
}
