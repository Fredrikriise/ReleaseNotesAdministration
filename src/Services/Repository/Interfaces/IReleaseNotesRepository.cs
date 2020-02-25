using Services.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IReleaseNotesRepository
    {
        // Metoder i repository
        Task<ReleaseNoteDto> GetReleaseNoteById(int? Id);
        Task<int?> CreateReleaseNote(ReleaseNoteDto releaseNoteDto);
        Task<ReleaseNoteDto> UpdateReleaseNote(int? Id, ReleaseNoteDto releaseNote);
        Task<bool> DeleteReleaseNote(int? id);
        Task<List<ReleaseNoteDto>> GetAllReleaseNotes();

    }
}
