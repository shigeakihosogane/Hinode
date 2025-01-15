
namespace FileTransfer2ForBlazor.Services
{
    public class FolderPickerService
    {
        public async Task<string> GetFolderPathAsync()
        {
            return await Task.Run(() =>
            {
                string result = string.Empty;

                // UIスレッドで動作させる
                Form? form = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;

                form?.Invoke((Action)(() =>
                    {
                        using var fbd = new FolderBrowserDialog();
                        fbd.Description = "フォルダを指定してください。";
                        fbd.SelectedPath = @"C:\Windows";
                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            result = fbd.SelectedPath;
                        }
                    }));
                return result;
            });
        }
    }
}
