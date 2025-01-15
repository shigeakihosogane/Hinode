using FileTransfer2ForBlazor.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer2ForBlazor.Services
{
    public class ArchiveService
    {
        private readonly string _connectionString;
        private readonly SettingService _settingService;
        private readonly FileTransferHistoryService _fileTransferHistoryService;
        private readonly FileTranceferLogService _fileTranceferLogService;
        public ArchiveService(DBConnection connection, SettingService settingService,
            FileTransferHistoryService fileTransferHistoryService, FileTranceferLogService fileTranceferLogService)
        {
            _connectionString = connection.GetConnectionString();
            _settingService = settingService;            
            _fileTransferHistoryService = fileTransferHistoryService;
            _fileTranceferLogService = fileTranceferLogService;
        }


        public async Task<int> ArchiveProcessedFile()//処理完了ファイル
        {            
            var result = 0;            
            var sql = @"SELECT                      dbo.T_TF_D_FileTransferHistory.*
FROM                         dbo.T_TF_D_FileTransferHistory
WHERE                       (TemporaryStorageLimit IS NOT NULL) AND (TemporaryStorageLimit < @DT) AND (ArchiveFullPath = N'')";


            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                try
                {
                    await connection.OpenAsync();
                    command.Parameters.Add(new SqlParameter("@DT", DateTime.Now));
                    using var reader = await command.ExecuteReaderAsync();
                    try
                    {
                        while (await reader.ReadAsync())
                        {
                            var fileTransferHistory = new FileTransferHistory
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ZyutyuuID = Convert.ToDecimal(reader["ZyutyuuID"]),
                                DefaultFullPath = Convert.ToString(reader["DefaultFullPath"]) ?? "",
                                TemporaryStorageFullPath = Convert.ToString(reader["TemporaryStorageFullPath"]) ?? "",
                                TemporaryStorageTime = reader["TemporaryStorageTime"] != DBNull.Value ? Convert.ToDateTime(reader["TemporaryStorageTime"]) : null,
                                TemporaryStorageLimit = reader["TemporaryStorageLimit"] != DBNull.Value ? Convert.ToDateTime(reader["TemporaryStorageLimit"]) : null,
                                ArchiveFullPath = Convert.ToString(reader["ArchiveFullPath"]) ?? "",
                                ArchiveTime = reader["ArchiveTime"] != DBNull.Value ? Convert.ToDateTime(reader["ArchiveTime"]) : null,
                                OrderAmount = Convert.ToDecimal(reader["OrderAmount"])
                            };
                            result++;

                            var fileTranceferLog = new FileTranceferLog
                            {
                                Process = "アーカイブ",
                                FullPathBeforeTransfer = fileTransferHistory.TemporaryStorageFullPath
                            };

                            await this.MoveToArchive(fileTransferHistory, fileTranceferLog);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("ArchiveProcessedFile: " + ex.Message);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("ArchiveProcessedFile: " + ex.Message);
                }
            }
            return result;
               
        }
       

        public async Task<int> ArchiveExpiredFile()//期限切れファイル
        {
            await Task.Delay(1000);








            return 0;
        }



        private async Task MoveToArchive(FileTransferHistory fileTransferHistory, FileTranceferLog fileTranceferLog)
        {
            string result;
            string newPath;

            if (!File.Exists(fileTransferHistory.TemporaryStorageFullPath))
            {
                result = "失敗";
                newPath = "一時保管ファイルが見つかりません。";
            }
            else
            {                
                var setting = await _settingService.GetSetting();
                var directoryTo = setting.ArchiveFolder;
                var fullPath = Path.GetFullPath(fileTransferHistory.TemporaryStorageFullPath);
                var fileName = Path.GetFileNameWithoutExtension(fullPath);
                var extensionName = Path.GetExtension(fullPath);

                var fileNameArray = fileName.Split('_');
                directoryTo += @"\" + fileNameArray[0];
                directoryTo += @"\" + fileNameArray[6];
                if (fileNameArray[6] == "4.倉庫保管")
                {
                    directoryTo += @"\" + fileNameArray[7]; ;
                }
                if (fileNameArray[4].Length == 8)
                {
                    directoryTo += string.Concat(@"\", fileNameArray[4].AsSpan(0, 6));                    
                }
                if (!Directory.Exists(directoryTo))
                {
                    Directory.CreateDirectory(directoryTo);
                }
                var newFileName = "";
                for (var n = 0; n < 8; n++)
                {
                    newFileName += fileNameArray[n] + "_"; 
                }
                newFileName += fileTransferHistory.OrderAmount.ToString() + "_";
                newPath = directoryTo + @"\" + newFileName + extensionName;
                int i = 1;
                while (File.Exists(newPath))
                {
                    newPath = directoryTo + @"\" + newFileName + "(" + i.ToString() + ")" + extensionName;
                    i++;
                }

                try//転送、History更新
                {
                    var fileInfo = new FileInfo(fullPath);                    
                    fileInfo.MoveTo(newPath);
                    result = "正常";
                }
                catch(Exception ex)
                {
                    result = "例外";
                    newPath = ex.Message;
                }
            }

            try
            {
                var ts = DateTime.Now;
                fileTranceferLog.Result = result;                
                fileTranceferLog.FullPathAfterTransfer = newPath;
                fileTranceferLog.ProcessedDateTime = ts;
                await _fileTranceferLogService.InsertFileTranceferLogAsync(fileTranceferLog);

                fileTransferHistory.ArchiveFullPath = newPath;
                fileTransferHistory.ArchiveTime = ts;
                await _fileTransferHistoryService.UpdateFileTransferHistoryAsync(fileTransferHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine("MoveToArchive" + ex.Message);
            }
            
            
        }




    }
}
