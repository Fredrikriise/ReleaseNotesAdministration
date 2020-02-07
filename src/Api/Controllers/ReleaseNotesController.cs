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
        public ReleaseNoteList Get()
        {
            var list = new List<ReleaseNotesModel>
            {
                new ReleaseNotesModel
                {
                    Title = "Release Note 0.93",
                    BodyText = "Dette er bodytext",
                    Id = 1,
                    ProductId = 2,
                    CreatedBy = "Rotta fredrik",
                    CreatedDate = DateTime.ParseExact("27/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = "Felix",
                    lasteUpdatedDate = DateTime.ParseExact("28/01/2020", "dd/MM/yyyy", null)
                },
                new ReleaseNotesModel
                {
                    Title = "Release Note 1",
                    BodyText = "Dette er bodytext",
                    Id = 2,
                    ProductId = 3,
                    CreatedBy = "Rotta Ronny",
                    CreatedDate = DateTime.ParseExact("24/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = "Fredrik",
                    lasteUpdatedDate = DateTime.ParseExact("25/01/2020", "dd/MM/yyyy", null)
                }
            };

            return new ReleaseNoteList()
            {
                ReleaseNotes = list
            };
        }
    }
}
