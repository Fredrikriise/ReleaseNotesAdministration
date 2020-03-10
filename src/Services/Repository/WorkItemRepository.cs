using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Services.Repository
{
    public class WorkItemRepository : IWorkItemRepository
    {
        public readonly string _connectionString;
        private readonly IMapper _mapper;

        public WorkItemRepository(IOptions<SqlDbConnection> sqlDbConnection, IMapper mapper)
        {
            _connectionString = sqlDbConnection.Value.ConnectionString;
            _mapper = mapper;
        }

        public async Task<List<WorkItemDto>> GetAllWorkItems()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT *
                FROM [WorkItems]";

                var workItems = await connection.QueryAsync<WorkItem>(query);
                var workItemsMapped = _mapper.Map<List<WorkItemDto>>(workItems);
                return workItemsMapped;
            }
        }

        public async Task<WorkItemDto> GetWorkItemById(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @" SELECT * 
                FROM [WorkItems]
                WHERE [Id] = @Id";

                var workItem = await connection.QueryFirstOrDefaultAsync<WorkItem>(query, new WorkItem { Id = Id });
                var mappedWorkItem = _mapper.Map<WorkItemDto>(workItem);
                return mappedWorkItem;
            }
        }
    }
}
