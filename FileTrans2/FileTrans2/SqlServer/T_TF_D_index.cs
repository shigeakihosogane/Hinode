using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace FileTrans2.SqlServer
{
    public static class T_TF_D_index
    {
        private static string _connectionString;
        static T_TF_D_index()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "hinode-server.database.windows.net";
            builder.InitialCatalog = "HINODEDB";
            builder.UserID = "hinode";
            builder.Password = "Hkanri8739";
            _connectionString = builder.ToString();

        }

        public static DataTable GetDataTable()
        {
            var sql = @"SELECT TOP (10000) ID, 受注ID, 開始日, 終了日, 担当部署, 備考, 荷主ID, 荷主名, 入力日時
                        FROM dbo.T_TF_D_Index
                        WHERE (開始日 IS NOT NULL) AND (終了日 IS NOT NULL)";

            DataTable dt = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            using (var adapter = new SqlDataAdapter(sql, connection))
            {
                connection.Open();
                adapter.Fill(dt);
            }
            return dt;
        }

        public static List<T_TF_D_indexEntity> GetDataReader()
        {
            var sql = @"SELECT TOP (10000) ID, 受注ID, 開始日, 終了日, 担当部署, 備考, 荷主ID, 荷主名, 入力日時
                        FROM dbo.T_TF_D_Index
                        WHERE (開始日 IS NOT NULL) AND (終了日 IS NOT NULL)";

            var result = new List<T_TF_D_indexEntity>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                { 
                    while(reader.Read())
                    {
                        result.Add(new T_TF_D_indexEntity(
                            Convert.ToInt32(reader["ID"]),
                            Convert.ToDecimal(reader["受注ID"]),
                            Convert.ToDateTime(reader["開始日"]),
                            Convert.ToDateTime(reader["終了日"]),
                            Convert.ToString(reader["担当部署"]),
                            Convert.ToString(reader["備考"]),
                            Convert.ToInt32(reader["荷主ID"]),
                            Convert.ToString(reader["荷主名"]),
                            Convert.ToDateTime(reader["入力日時"])));
                    }
                }
            }
            return result;
        }







    }
}
