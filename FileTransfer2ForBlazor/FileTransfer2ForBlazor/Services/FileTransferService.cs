﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FileTransfer2ForBlazor.Components.Pages;
using FileTransfer2ForBlazor.Models;
using Microsoft.VisualBasic.Logging;
using Index = FileTransfer2ForBlazor.Models.Index;


namespace FileTransfer2ForBlazor.Services
{
    public class FileTransferService
    {  
        private readonly SettingService _settingService;
        private readonly string _serialNumber;
        private readonly IndexService _indexService;
        private readonly NinusiInfoService _ninusiInfoService;
        private readonly FileTransferHistoryService _fileTransferHistoryService;
        private readonly FileTranceferLogService _fileTranceferLogService;

        public FileTransferService(SettingService settingService, IndexService indexService, NinusiInfoService ninusiInfoService, MotherboardIDService motherboardIDService,
            FileTransferHistoryService fileTransferHistoryService, FileTranceferLogService fileTranceferLogService)
        {
            _settingService = settingService;
            _serialNumber = motherboardIDService.GetMotherboardSerialNumber();
            _indexService = indexService;
            _ninusiInfoService = ninusiInfoService;
            _fileTransferHistoryService = fileTransferHistoryService;
            _fileTranceferLogService = fileTranceferLogService;
        }


        public async Task AtherFileFormat(string file)
        {
            var setting = await _settingService.GetSetting(_serialNumber);                           
            var fullPath = Path.GetFullPath(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var extensionName = Path.GetExtension(file);
            //以下全部一緒
            //this.txtAPPログ.Text += "\r\n" + file;
            //this.txtAPPログ.Text += "\r\n" + fullPath;
            //this.txtAPPログ.Text += "\r\n" + directoryName+ @"\" + fileName + extensionName;

            var fileTranceferLog = new FileTranceferLog
            {
                FullPathBeforeTransfer = fullPath,
                Process = "取込処理"
            };

            var directryTo = setting.Trans1Successful;
            string newFileName;                
            if (DateTime.TryParseExact(fileName, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                newFileName = "scan__" + dt.ToString("yyyyMMddHHmmss");
            }
            else
            {
                newFileName = "取込__" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            int i = 1;
            var newFullPath = directryTo + @"\" + newFileName + extensionName;
            while (File.Exists(newFullPath))
            {
                newFullPath = directryTo + @"\" + newFileName + "(" + i.ToString() + ")" + extensionName;
                i++;
            }
            try
            {
                var fileInfo = new FileInfo(fullPath);
                fileInfo.MoveTo(newFullPath);
                fileTranceferLog.Result = "正常";
                fileTranceferLog.ProcessedDateTime = DateTime.Now;
                fileTranceferLog.FullPathAfterTransfer = newFullPath;
                await _fileTranceferLogService.InsertFileTranceferLogAsync(fileTranceferLog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);                    
            }
            
        }





        public async Task GenerateFileNameWithIndex(string file)
        {
            var fullPath = Path.GetFullPath(file);
            //var directoryName = Path.GetDirectoryName(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var extensionName = Path.GetExtension(file);
            //以下全部一緒
            //this.txtAPPログ.Text += "\r\n" + file;
            //this.txtAPPログ.Text += "\r\n" + fullPath;
            //this.txtAPPログ.Text += "\r\n" + directoryName+ @"\" + fileName + extensionName;

            DateTime dt;
            var emsg = "";
            var fNameArray = fileName.Split('_');


            var fileNameElement = new FileNameElement
            {
                拡張子 = extensionName,
                元ファイルFullPath = fullPath
            };
            var fileTransferHistory = new FileTransferHistory
            {
                DefaultFullPath = fullPath,
                OrderAmount = 0
            };
            var fileTranceferLog = new FileTranceferLog
            {
                FullPathBeforeTransfer = fullPath
            };
            try
            {
                //pdf以外は戻す
                if (extensionName != ".pdf")
                {
                    //戻し転送処理
                    await this.MoveToErrorDirectory(fullPath, emsg, fileTranceferLog);                    
                    return;
                }

                if (decimal.TryParse(fNameArray[0], out decimal zyutyuuID))//
                {
                    var index = new Index();
                    index = await _indexService.GetIndexAsync(zyutyuuID);
                    fileTransferHistory.ZyutyuuID = zyutyuuID;                                      

                    //タイムスタンプ位置を取得
                    int len = fNameArray.Length;
                    int tsPosition;
                    for (tsPosition = 0; tsPosition < len - 1; tsPosition++)
                    {
                        var str = fNameArray[tsPosition];
                        if (str.Length >= 14)
                        {
                            if (DateTime.TryParseExact(fNameArray[tsPosition][..14], "yyyyMMddHHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dt))
                            {
                                break;
                            }
                        }
                    }

                    if (index.ID != 0)
                    {
                        //受注ファイル以外
                        if (zyutyuuID == 501 || zyutyuuID == 601 || zyutyuuID == 701 || zyutyuuID == 801 || zyutyuuID == 901)
                        {
                            if (len >= 5 && tsPosition == 4)//要素数5以上、タイムスタンプ位置4
                            {
                                var ninusi = fNameArray[2];
                                if (ninusi != "" && ninusi != "登録名称不明" && ninusi != "scan" && ninusi != "取込" && ninusi != "本社受信")
                                {
                                    var sdate = DateTime.Now.ToString("yyyyMMdd");
                                    var edate = DateTime.Now.ToString("yyyyMMdd");
                                    var comment = fNameArray[1];
                                    var commentArray = comment.Split('-');
                                    var datestr = commentArray[0];
                                    if (DateTime.TryParseExact(datestr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                                    {
                                        sdate = datestr;
                                        edate = datestr;
                                        if (commentArray.Length > 1)
                                        {
                                            comment = commentArray[1];
                                            if (commentArray.Length > 2)
                                            {
                                                for (int i = 2; i < commentArray.Length; i++)
                                                {
                                                    comment = comment + "-" + commentArray[i];
                                                }
                                            }
                                        }
                                    }
                                    var faxnumber = fNameArray[3];
                                    if (faxnumber == "" && index.FAX番号 != "")
                                    {
                                        faxnumber = index.FAX番号;
                                    }
                                    fileNameElement.荷主名 = ninusi;
                                    fileNameElement.FAX番号 = faxnumber;
                                    fileNameElement.タイムスタンプ = fNameArray[4][..14];
                                    fileNameElement.受注CD = fNameArray[0];
                                    fileNameElement.開始日 = sdate;
                                    fileNameElement.終了日 = edate;
                                    fileNameElement.担当部署 = index.担当部署;
                                    fileNameElement.備考 = comment;
                                    fileNameElement.ページ = "";

                                    if (DateTime.TryParseExact(edate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime defaultDate))
                                    {
                                        fileTransferHistory.TemporaryStorageLimit = defaultDate.AddDays(30);
                                    }
                                }
                                else
                                {
                                    emsg = "仕分先不明";
                                }
                            }
                            else//スキャンされたファイル等、整頓FAXでファイル名が生成されていないファイル、
                            {
                                emsg = "ファイル名構成不正";
                            }
                        }
                        else//通常受注ファイル
                        {
                            if (len >= 4 && tsPosition == 3)
                            {
                                var ninusi = fNameArray[1];
                                if (ninusi != "登録名称不明" && ninusi != "scan" && ninusi != "取込" && ninusi != "")
                                {
                                    if (index.名称_TF == "")
                                    {
                                        await this.UpdateNinusimeiFromDB(index.荷主ID, fNameArray[1]);
                                    }
                                }
                                var faxnumber = fNameArray[2];
                                if (faxnumber == "")
                                {
                                    if (index.FAX番号 != "")
                                    {
                                        faxnumber = index.FAX番号;
                                    }
                                }
                                else
                                {
                                    if (index.FAX番号 == "")
                                    {
                                        await this.UpdateFaxNumberFromDB(index.荷主ID, faxnumber);
                                    }
                                }
                                var page = "";
                                //if (len - 1 > tsPosition)
                                //{
                                //    for (var i = tsPosition + 1; i < len; i++)
                                //    {
                                //        page += fNameArray[i];
                                //    }
                                //}
                                fileNameElement.荷主名 = index.荷主名;
                                fileNameElement.FAX番号 = faxnumber;
                                fileNameElement.タイムスタンプ = fNameArray[3][..14];
                                fileNameElement.受注CD = fNameArray[0];
                                fileNameElement.開始日 = index.開始日;
                                fileNameElement.終了日 = index.終了日;
                                fileNameElement.担当部署 = index.担当部署;
                                fileNameElement.備考 = index.備考;
                                fileNameElement.ページ = page;

                                if (DateTime.TryParseExact(index.終了日, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime defaultDate))
                                {
                                    fileTransferHistory.TemporaryStorageLimit = await GetTemporaryStorageLimit(defaultDate, index.請求締日);
                                }

                            }
                            else
                            {
                                emsg = "ファイル名構成不正";
                            }
                        }
                    }
                    else
                    {
                        emsg = "受注データなし";
                    }
                }
                else
                {
                    emsg = "受注コード不正";
                }
            }
            catch
            {
                emsg = "例外発生";
            }

            if (emsg == "")
            {
                //正常転送処理
                await this.MoveToSuccessDirectory(fileNameElement, fileTransferHistory, fileTranceferLog);
            }
            else
            {
                //エラー戻し転送
                await this.MoveToErrorDirectory(fullPath, emsg, fileTranceferLog);
            }
        }


        private async Task MoveToSuccessDirectory(FileNameElement fileNameElement, FileTransferHistory fileTransferHistory, FileTranceferLog fileTranceferLog)
        {
            var setting = await _settingService.GetSetting(_serialNumber);//           
            var directoryTo = setting.Trans2Successful + @"\" + fileNameElement.荷主名 + @"\" + fileNameElement.担当部署;
            if (fileNameElement.担当部署 == "4.倉庫保管")
            {
                directoryTo += @"\" + fileNameElement.備考;
            }
            if (!Directory.Exists(directoryTo))
            {
                Directory.CreateDirectory(directoryTo);
            }
            var newFileName = fileNameElement.荷主名 + "_" + 
                                fileNameElement.FAX番号 + "_" + 
                                fileNameElement.タイムスタンプ + "_" +
                                fileNameElement.受注CD + "_" + 
                                fileNameElement.開始日 + "_" + 
                                fileNameElement.終了日 + "_" +
                                fileNameElement.担当部署 + "_" +
                                fileNameElement.備考 + "_" +
                                fileNameElement.ページ;
            var newPath = directoryTo + @"\" + newFileName + fileNameElement.拡張子;
            int i = 1;
            while (File.Exists(newPath))
            {
                newPath = directoryTo + @"\" + newFileName + "(" + i.ToString() + ")" + fileNameElement.拡張子;
                i++;
            }

            //Tf
            var newTfPath = setting.TfUploader + @"\" + newFileName + fileNameElement.拡張子;
            i = 1;
            while (File.Exists(newTfPath))
            {
                newTfPath = setting.TfUploader + @"\" + newFileName + "(" + i.ToString() + ")" + fileNameElement.拡張子;
                i++;
            }
            //

            try
            {
                var fileInfo = new FileInfo(fileNameElement.元ファイルFullPath);
                fileInfo.CopyTo(newTfPath);
                fileInfo.MoveTo(newPath);
                var dt = DateTime.Now;
                fileTranceferLog.Result = "正常";
                fileTranceferLog.ProcessedDateTime = dt;
                //======================================================================================Therefore用転送
                fileTranceferLog.Process = "Therefore";//===============================================
                fileTranceferLog.FullPathAfterTransfer = newTfPath;//===================================
                await _fileTranceferLogService.InsertFileTranceferLogAsync(fileTranceferLog);//=========
                //======================================================================================
                fileTranceferLog.Process = "一時保管";
                fileTranceferLog.FullPathAfterTransfer = newPath;
                await _fileTranceferLogService.InsertFileTranceferLogAsync(fileTranceferLog);

                fileTransferHistory.TemporaryStorageFullPath = newPath;
                fileTransferHistory.TemporaryStorageTime = dt;
                await _fileTransferHistoryService.InsertFileTransferHistoryAsync(fileTransferHistory);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);                
            }



        }
        private async Task MoveToErrorDirectory(string fullPath, string emsg, FileTranceferLog fileTranceferLog)
        {
            var setting = await _settingService.GetSetting(_serialNumber);//
            var fileName = Path.GetFileNameWithoutExtension(fullPath);
            var extensionName = Path.GetExtension(fullPath);
            //this.txtAPPログ.Text += "\r\n" + file;
            //this.txtAPPログ.Text += "\r\n" + fullPath;
            //this.txtAPPログ.Text += "\r\n" + directoryName+ @"\" + fileName + extensionName;

            var fNameArray = fileName.Split('_');
            var newFileName = "";
            var zyutyuuID = Convert.ToInt32(fNameArray[0]);
            var sp = 1;
            if (zyutyuuID == 501 || zyutyuuID == 601 || zyutyuuID == 701 || zyutyuuID == 801 || zyutyuuID == 901)
            {
                sp = 2;
            }
            for (var n = sp; n < fNameArray.Length; n++)
            {
                if (n > sp)
                {
                    newFileName += "_";
                }
                newFileName += fNameArray[n];
            }
            var newPath = setting.Trans2Error + @"\" + emsg + newFileName + extensionName;
            int i = 1;
            while (File.Exists(newPath))
            {
                newPath = setting.Trans2Error + @"\" + emsg + newFileName + "(" + i.ToString() + ")" + extensionName;
            }
            try
            {
                var fileInfo = new FileInfo(fullPath);
                fileInfo.MoveTo(newPath);
                fileTranceferLog.Result = emsg;
                fileTranceferLog.ProcessedDateTime = DateTime.Now;
                await _fileTranceferLogService.InsertFileTranceferLogAsync(fileTranceferLog);                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private async Task UpdateNinusimeiFromDB(int ninusiID, string ninusimei)
        {
            await _ninusiInfoService.UpdateNinusimeiAsync(ninusiID, ninusimei);
        }
        private async Task UpdateFaxNumberFromDB(int ninusiID, string faxnumber)
        {
            await _ninusiInfoService.UpdateFaxNumberAsync(ninusiID, faxnumber);
        }


        private async Task<DateTime> GetTemporaryStorageLimit(DateTime defaultDate, int cutOfDate)
        {
            var setting = await _settingService.GetSetting(_serialNumber);
            var transferQueueTime = setting.TransferQueueTime;
            if (cutOfDate == 31 || cutOfDate == 0)//末締め
            {
                defaultDate = defaultDate.AddMonths(1);
                defaultDate = new DateTime(defaultDate.Year, defaultDate.Month, 1, 0, 0, 0);
                defaultDate = defaultDate.AddDays(-1); 
            }
            else//末締め以外
            {
                if (defaultDate.Day > cutOfDate)//締日過ぎ
                {
                    defaultDate = defaultDate.AddMonths(1);                    
                }
                defaultDate = new DateTime(defaultDate.Year, defaultDate.Month, cutOfDate , 0, 0, 0);
            }            
            return defaultDate.AddDays(transferQueueTime);
        }





    }
}
