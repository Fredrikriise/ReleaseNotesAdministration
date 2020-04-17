using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System;
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

        public async Task<int?> CreateWorkItem(WorkItemDto workItemDto)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var insert = @"INSERT INTO [WorkItems]
                                (
                                    [Id],
                                    [Title],
                                    [AssignedTo],
                                    [State]
                                )
                                VALUES
                                (
                                    @Id,
                                    @Title,
                                    @AssignedTo,
                                    @State
                                )";
                    var returnResult = await connection.QueryFirstOrDefaultAsync<int?>(insert, new WorkItemDto
                    {
                        Id = workItemDto.Id,
                        Title = workItemDto.Title,
                        AssignedTo = workItemDto.AssignedTo,
                        State = workItemDto.State
                    });
                    return returnResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WorkItemDto> UpdateWorkItem(int Id, WorkItemDto workItem)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var updateDb = @"UPDATE [WorkItems]
                    SET
                        [Id] = @Id,
                        [Title] = @Title,
                        [AssignedTo] = @AssignedTo,
                        [State] = @State
                    WHERE [Id] = @Id";
                    var workItemMapped = _mapper.Map<WorkItem>(workItem);
                    workItemMapped.AddWorkItemId(Id);

                    var result = await connection.ExecuteAsync(updateDb, workItemMapped);
                    return workItem;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
