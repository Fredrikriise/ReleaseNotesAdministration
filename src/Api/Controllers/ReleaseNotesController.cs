using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                new ReleaseNotesModel 
                { 
                    Title = "Release Note 0.93"
                },
                new ReleaseNotesModel
                {
                    Title= "Release note 0.90"
                }
            };
        }
    }
}
