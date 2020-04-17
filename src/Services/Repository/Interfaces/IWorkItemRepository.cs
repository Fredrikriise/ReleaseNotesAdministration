using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IWorkItemRepository
    {
        Task<List<WorkItemDto>> GetAllWorkItems();
        Task<WorkItemDto> GetWorkItemById(int Id);
        Task<int?> CreateWorkItem(WorkItemDto workItemDto);
        Task<WorkItemDto> UpdateWorkItem(int Id, WorkItemDto workItem);
    }
}
