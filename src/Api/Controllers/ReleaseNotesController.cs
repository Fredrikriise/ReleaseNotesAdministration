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

        [HttpPost]
        public async Task<IActionResult> Create(ReleaseNote releaseNote)
        {
            var mappedReleaseNote = _mapper.Map<ReleaseNoteDto>(releaseNote);
            await _releaseNoteRepo.CreateReleaseNote(mappedReleaseNote);
            return Created("", releaseNote);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateReleaseNote(int? Id, ReleaseNote releaseNote)
        {
            var mappedReleaseNote = _mapper.Map<ReleaseNoteDto>(releaseNote);
            await _releaseNoteRepo.UpdateReleaseNote(Id, mappedReleaseNote);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReleaseNote(int? Id, int productId)
        {
            var deletedReleaseNote = await _releaseNoteRepo.DeleteReleaseNote(Id, productId);

            if (deletedReleaseNote)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("/releasenotes/{Id}")]
        public async Task<IActionResult> GetReleaseNoteById(int? Id)
        {
            var releaseNote = await _releaseNoteRepo.GetReleaseNoteById(Id);
            Console.WriteLine(releaseNote);

            if (releaseNote == null)
            {
                return NotFound();
            }

            var mappedReleaseNote = _mapper.Map<ReleaseNote>(releaseNote);
            return Ok(mappedReleaseNote);
        }
    }
}
