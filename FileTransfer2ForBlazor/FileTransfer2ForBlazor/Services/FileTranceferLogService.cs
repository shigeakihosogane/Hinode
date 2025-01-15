using FileTransfer2ForBlazor.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer2ForBlazor.Services
{
    public class FileTranceferLogService
    {
        private readonly string _connectionString;
        private readonly SharedService _sharedService;
        public FileTranceferLogService(DBConnection connection, SharedService sharedService)
        {
            _connectionString = connection.GetConnectionString();
            _sharedService = sharedService;
        }


        public async Task InsertFileTranceferLogAsync(FileTranceferLog fileTranceferLog)
        {
            var sql = @"INSERT INTO T_TF_D_FileTranceferLog (
Process, 
Result, 
FullPathBeforeTransfer, 
FullPathAfterTransfer, 
ProcessedDateTime
) VALUES (
@Process, 
@Result, 
@FullPathBeforeTransfer, 
@FullPathAfterTransfer, 
@ProcessedDateTime
)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                try
                {
                    await connection.OpenAsync();
                    command.Parameters.Add(new SqlParameter("@Process", fileTranceferLog.Process));
                    command.Parameters.Add(new SqlParameter("@Result", fileTranceferLog.Result));
                    command.Parameters.Add(new SqlParameter("@FullPathBeforeTransfer", fileTranceferLog.FullPathBeforeTransfer));
                    command.Parameters.Add(new SqlParameter("@FullPathAfterTransfer", fileTranceferLog.FullPathAfterTransfer));
                    command.Parameters.Add(new SqlParameter("@ProcessedDateTime", (object?)fileTranceferLog.ProcessedDateTime ?? DBNull.Value));
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("SQLエラー: " + ex.Message);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("InsertFileTranceferLogAsync: " + ex.Message);
                }
            }
            var external = new ExternalClass(_sharedService);
            await external.CallGetFileTranceferLogAsync();
        }


        public async Task<List<FileTranceferLog>> GetFileTranceferLogAsync()
        {
            var fileTranceferLogs = new List<FileTranceferLog>();
            var sql = @"SELECT                      dbo.T_TF_D_FileTranceferLog.*
FROM                         dbo.T_TF_D_FileTranceferLog";


            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                try
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {
                                var fileTranceferLog = new FileTranceferLog
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Process = Convert.ToString(reader["Process"]) ?? "",
                                    Result = Convert.ToString(reader["Result"]) ?? "",
                                    FullPathBeforeTransfer = Convert.ToString(reader["FullPathBeforeTransfer"]) ?? "",
                                    FullPathAfterTransfer = Convert.ToString(reader["FullPathAfterTransfer"]) ?? "",
                                    ProcessedDateTime = reader["ProcessedDateTime"] != DBNull.Value ? Convert.ToDateTime(reader["ProcessedDateTime"]) : null,
                                };
                                fileTranceferLogs.Add(fileTranceferLog);
                            }
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine("SQLエラー: " + ex.Message);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("GetFileTranceferLogAsync: " + ex.Message);
                }
            }
            return fileTranceferLogs;
        }









    }
}
