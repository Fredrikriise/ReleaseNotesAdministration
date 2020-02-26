using Services.Repository.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repository.Interfaces
{
    public interface IWorkItemRepository
    {
        Task<WorkItemDto> GetWorkItemById(int Id);
        Task<List<WorkItemDto>> GetAllWorkItems();
    }
}
