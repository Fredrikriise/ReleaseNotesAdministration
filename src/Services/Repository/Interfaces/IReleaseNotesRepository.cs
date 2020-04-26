using Services.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IReleaseNotesRepository
    {
        // Methods in repository
        Task<List<ReleaseNoteDto>> GetAllReleaseNotes();
        Task<ReleaseNoteDto> GetReleaseNoteById(int Id);
        Task<int> CreateReleaseNote(ReleaseNoteDto releaseNoteDto);
        Task<ReleaseNoteDto> UpdateReleaseNote(int Id, ReleaseNoteDto releaseNote);
        Task<bool> DeleteReleaseNote(int id);
    }
}
