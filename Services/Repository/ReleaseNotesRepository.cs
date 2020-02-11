using AutoMapper;
using Microsoft.Extensions.Options;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using Services.Repository.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Services
{
    public class ReleaseNotesRepository : IReleaseNotesRepository
    {
        public readonly string _connectionString;
        private readonly IMapper _mapper;

        public ReleaseNotesRepository(IOptions<SqlConnection> sqlConnection, IMapper mapper)
        {
            _connectionString = sqlConnection.Value.ConnectionString;
            _mapper = mapper;
        }

        public async Task<ReleaseNoteDto> GetReleaseNote(Guid Id, Guid ProductId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM [ReleaseNotesDB]
                WHERE [Id] = @Id AND [ProductId] = @ProductId";

                var process = await connection.QueryFirstOrDefaultAsync<
            }
        }
    }
}
