using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ReleaseNotesController : ControllerBase
    {
        private static readonly string[] ReleaseNotes = new[]
        {
            "RN1", "RN2", "RN3", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ReleaseNotesController> _logger;

        public ReleaseNotesController(ILogger<ReleaseNotesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<ReleaseNotesModel> Get()
        {
            return new List<ReleaseNotesModel>
            {
                new ReleaseNotesModel { Title = "Hei" },
                new ReleaseNotesModel { Title= "Release note 2"},
                new ReleaseNotesModel { },
                new ReleaseNotesModel { },
                new ReleaseNotesModel { }
            };
        }

    }
}
