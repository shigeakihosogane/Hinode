using FileTransfer2ForBlazor.Components.Pages;
using FileTransfer2ForBlazor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Setting = FileTransfer2ForBlazor.Models.Setting;

namespace FileTransfer2ForBlazor.Services
{
    public class SettingService
    {

        public async Task<Setting> GetSetting()
        {
            var setting = new Setting();
            var now = DateTime.Now; 
            await Task.Run(() =>
            {
                setting.Trans1Monitoring = @"C:\Users\Hinode24\Documents\Test\Scan";
                setting.Trans1Successful = @"C:\Users\Hinode24\Documents\Test\Fax受信\FAX受信トレイ";
                setting.Trans2Monitoring = @"C:\Users\Hinode24\Documents\Test\Fax受信\FAX受信トレイ\Fax転送";
                setting.Trans2Successful = @"C:\Users\Hinode24\Documents\Test\Fax受信\FAX一時保管";
                setting.Trans2Error = @"C:\Users\Hinode24\Documents\Test\Fax受信\FAX受信トレイ";
                setting.TfUploader = @"C:\Users\Hinode24\Documents\Test\TfUpload";
                setting.ArchiveFolder = @"C:\Users\Hinode24\Documents\Test\Fax受信\FAX倉庫";
                setting.MonitoringInterval = 1000;//監視インターバル
                setting.ScheduledExecutionTime = new DateTime(now.Year, now.Month, now.Day,2,0,0);
                setting.TransferQueueTime = 10;
            });
            return setting;
        }








    }
}
