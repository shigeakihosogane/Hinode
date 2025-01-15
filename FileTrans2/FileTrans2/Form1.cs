using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MetroFramework.Forms;
using FileTrans2.SqlServer;
namespace FileTrans2
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        //public string GetConnectionString()
        //{
        //    return ConfigurationManager.ConnectionStrings["hinode-server"].ConnectionString;
        //}
        
        //接続テスト(datatable)
        private void metroButton1_Click(object sender, EventArgs e)
        {
            metroGrid1.DataSource = T_TF_D_index.GetDataTable();
        }

        //接続テスト(datatable)
        private void metroButton2_Click(object sender, EventArgs e)
        {
            metroGrid1.DataSource = T_TF_D_index.GetDataReader();
        }





        //ON-OFF切り替え
        private void OnOffSwich(bool flg)
        {
            if (flg)
            {


            }
            else
            {
                
            }
        }

        








        //タイマー








    }
}
