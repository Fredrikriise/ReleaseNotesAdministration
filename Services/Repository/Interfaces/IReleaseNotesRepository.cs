using Api.Models;
using AutoMapper;
using Services.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IReleaseNotesRepository
    {
        // Metoder i repository
        Task<ReleaseNoteDto> GetReleaseNote(int? Id, int ProductId);
        Task<int?> Create(int? Id, int ProductId, ReleaseNoteDto releaseNoteDto);
        Task<ReleaseNoteDto> UpdateReleaseNote(int? Id, ReleaseNoteDto releaseNote);
        Task<bool> Delete(int? id, int productId);

    }
}
