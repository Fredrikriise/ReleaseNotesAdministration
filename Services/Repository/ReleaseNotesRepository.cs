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
using Services.Repository.Models.DatabaseModels;
using Dapper;

namespace Services
{
    public class ReleaseNotesRepository : IReleaseNotesRepository
    {
        public readonly string _connectionString;
        private readonly IMapper _mapper;

        public ReleaseNotesRepository(IOptions<SqlDbConnection> sqlDbConnection, IMapper mapper)
        {
            _connectionString = sqlDbConnection.Value.ConnectionString;
            _mapper = mapper;
        }

        public async Task<int?> Create(int? Id, int ProductId, ReleaseNoteDto releaseNoteDto)
        {
            try
            {
                var releaseNote = _mapper.Map<ReleaseNote>(releaseNoteDto);
                releaseNote.Id = Id;
                releaseNote.ProductId = ProductId;

                using (var connection = new SqlConnection(_connectionString))
                {
                    var insert = @"INSERT INTO [ReleaseNotesDb]
                                (
                                    [Title],
                                    [BodyText],
                                    [Id],
                                    [ProductId],
                                    [CreatedBy],
                                    [CreatedDate],
                                    [LastUpdatedBy],
                                    [LastUpdatedDate]
                                )
                                VALUES
                                (
                                    @Title,
                                    @BodyText
                                    @Id
                                    @ProductId
                                    @CreatedBy
                                    @CreatedDate
                                    @LastUpdatedBy
                                    @LastUpdatedDate
                                )
                                SELECT [Id] FROM [ReleaseNotesDb] WHERE [Id] = @Id AND [ProductId] = @ProductId";
                    var returnResult = await connection.QueryFirstAsync<int?>(insert, releaseNote);
                    return returnResult;
                }
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }

        public async Task<ReleaseNoteDto> GetReleaseNote(int? Id, int ProductId)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM [ReleaseNotesDb]
                WHERE [Id] = @Id AND [ProductId] = @ProductId";

                var releaseNote = await connection.QueryFirstOrDefaultAsync<ReleaseNote>(query, new { @Id = Id, @ProductId = ProductId });
                var mappedReleaseNote = _mapper.Map<ReleaseNoteDto>(releaseNote);
                return mappedReleaseNote;
            }
        }

        public async Task<ReleaseNoteDto> UpdateReleaseNote(int? Id, ReleaseNoteDto releaseNote)
        {
            try
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    var updateDb = @"UPDATE [ReleaseNotesDb]
                    SET
                        [Title] = @Title,
                        [BodyText] = @BodyText,
                        [Id] = @Id,
                        [ProductId] = @ProductId,
                        [CreatedBy] = @CreatedBy,
                        [CreatedDate] = @CreatedDate, 
                        [LastUpdatedBy] = @LastUpdatedBy,
                        [LastUpdatedDate] = @LastUpdatedDate
                    WHERE [Id] = @Id AND [ProductId] = @ProductId";
                    var releaseNoteMapped = _mapper.Map<ReleaseNote>(releaseNote);
                    releaseNoteMapped.AddReleaseNoteId(Id);

                    var result = await connection.ExecuteAsync(updateDb, releaseNoteMapped);
                    return releaseNote;
                }
            } catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }

        public async Task<bool> Delete(int? id, int productId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var Delete = "DELETE FROM ReleaseNotesDb WHERE Id = @Id and ProductId = @ProductId";
                    var returnedReleaseNote = await connection.ExecuteAsync(Delete, new { @Id = id, @ProductId = productId });
                    bool success = returnedReleaseNote > 0;
                    return success;
                }
            } catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }
    }
}