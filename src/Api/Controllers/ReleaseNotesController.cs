﻿using System;
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
            var BodytextData = "Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis id duis, nisl nulla risus.";

            var releaseNotesList = new List<ReleaseNotesModel>
            {
                new ReleaseNotesModel
                {
                   Title = "Release note 0.1 - Onboarding",
                    BodyText = BodytextData,
                    Id = 1,
                    ProductId = 2,
                    CreatedBy = "Fredrik Svevad Riise",
                    CreatedDate = DateTime.ParseExact("27/01/2020", "dd/MM/yyyy", null),
                    LastUpdatedBy = null,
                    LastUpdatedDate = null
                },
                new ReleaseNotesModel
                {
                   Title = "Release note 0.93 - Manager",
                    BodyText = BodytextData,
                    Id = 2,
                    ProductId = 3,
                    CreatedBy = "Felix Thu Falkendal Nilsen",
                    CreatedDate = DateTime.ParseExact("28/01/2020", "dd/MM/yyyy", null),
                    LastUpdatedBy = "Felix Thu Falkendal Nilsen",
                    LastUpdatedDate = DateTime.ParseExact("31/01/2020", "dd/MM/yyyy", null),
                }
            };

            return new ReleaseNoteList()
            {
                ReleaseNotes = releaseNotesList
            };
        }
    }
}
