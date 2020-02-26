using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using Services.Repository.Config;
using Services.Repository.Interfaces;
using Services.Repository.Models;
using Services.Repository.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

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

        public async Task<int?> CreateReleaseNote(ReleaseNoteDto releaseNoteDto)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var insert = @"INSERT INTO [ReleaseNotes]
                                (
                                    [Title],
                                    [BodyText],
                                    [ProductId],
                                    [CreatedBy],
                                    [CreatedDate],
                                    [LastUpdatedBy],
                                    [LastUpdateDate]
                                )
                                VALUES
                                (
                                    @Title,
                                    @BodyText,
                                    @ProductId,
                                    @CreatedBy,
                                    @CreatedDate,
                                    @LastUpdatedBy,
                                    @LastUpdateDate
                                )";
                    var returnResult = await connection.QueryFirstOrDefaultAsync<int?>(insert, new ReleaseNoteDto
                    {
                        Title = releaseNoteDto.Title,
                        BodyText = releaseNoteDto.BodyText,
                        ProductId = releaseNoteDto.ProductId,
                        CreatedBy = releaseNoteDto.CreatedBy,
                        CreatedDate = releaseNoteDto.CreatedDate,
                        LastUpdatedBy = releaseNoteDto.LastUpdatedBy,
                        LastUpdateDate = releaseNoteDto.LastUpdateDate
                    });
                    return returnResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReleaseNoteDto> GetReleaseNoteById(int? Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT *
                FROM [ReleaseNotes]
                WHERE [Id] = @Id";

                var releaseNote = await connection.QueryFirstOrDefaultAsync<ReleaseNote>(query, new ReleaseNote { @Id = Id });
                var mappedReleaseNote = _mapper.Map<ReleaseNoteDto>(releaseNote);
                return mappedReleaseNote;
            }
        }

        public async Task<ReleaseNoteDto> UpdateReleaseNote(int? Id, ReleaseNoteDto releaseNote)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var updateDb = @"UPDATE [ReleaseNotes]
                    SET
                        [Title] = @Title,
                        [BodyText] = @BodyText,
                        [ProductId] = @ProductId,
                        [CreatedBy] = @CreatedBy,
                        [CreatedDate] = @CreatedDate, 
                        [LastUpdatedBy] = @LastUpdatedBy,
                        [LastUpdateDate] = @LastUpdateDate
                    WHERE [Id] = @Id";
                    var releaseNoteMapped = _mapper.Map<ReleaseNote>(releaseNote);
                    releaseNoteMapped.AddReleaseNoteId(Id);

                    var result = await connection.ExecuteAsync(updateDb, releaseNoteMapped);
                    return releaseNote;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteReleaseNote(int? id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var Delete = "DELETE FROM [ReleaseNotes] WHERE Id = @Id";
                    var returnedReleaseNote = await connection.ExecuteAsync(Delete, new { @Id = id });
                    bool success = returnedReleaseNote > 0;
                    return success;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ReleaseNoteDto>> GetAllReleaseNotes()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT *
                FROM [ReleaseNotes]";

                var releaseNotes = await connection.QueryAsync<ReleaseNote>(query);
                var releaseNotesMapped = _mapper.Map<List<ReleaseNoteDto>>(releaseNotes);
                return releaseNotesMapped;
            }
        }
    }
}