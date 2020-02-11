using Api.Models;
using AutoMapper;
using Services.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    interface IReleaseNotesRepository
    {
        // Metoder i repository
        Task<ReleaseNoteDto> GetReleaseNote(Guid Id, Guid ProductId);
    }
}
