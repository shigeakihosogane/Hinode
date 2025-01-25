using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using FileSearchSystem.Components;
using System;
using FileSearchSystem.Models;
using System.ComponentModel;

namespace FileSearchSystem
{    
    public partial class Form2 : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<FileTransferHistory> Histories { get; set; }

        public Form2(IServiceProvider serviceProvider, List<FileTransferHistory> histories)
        {
            InitializeComponent();
            Histories = histories;



            blazorWebView1.HostPage = "wwwroot/index.html";
            blazorWebView1.Services = serviceProvider;
            blazorWebView1.RootComponents.Add<Sub>("#app", new Dictionary<string, object>
        {
            { "histories", Histories }
        });



        }
    }
}
