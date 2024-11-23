﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WindowsFormsApp2.Models;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2.Services
{    
    public static class IndexService
    {
        private static String _connectionString;

        static IndexService()
        {
            _connectionString = DBConnection.GetConnectionString();
        }

        public async static Task<Index> GetIndexAsync(Decimal zID)
        {
            Index index = new Index();            
            var sql = @"SELECT                      dbo.T_TF_D_Index.*, dbo.T_TF_M_NinusiInfo.*
FROM                         dbo.T_TF_D_Index LEFT OUTER JOIN
                                      dbo.T_TF_M_NinusiInfo ON dbo.T_TF_D_Index.荷主ID = dbo.T_TF_M_NinusiInfo.荷主ID
WHERE                       (dbo.T_TF_D_Index.受注ID = @ZID)";

            await Task.Run(() =>
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        command.Parameters.Add(new SqlParameter("@ZID", zID));
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                try
                                {
                                    while (reader.Read())
                                    {
                                        index.ID = Convert.ToInt32(reader["ID"]);
                                        index.受注ID = Convert.ToDecimal(reader["受注ID"]);
                                        if (!(reader["開始日"] is DBNull))
                                        {
                                            var dt = Convert.ToDateTime(reader["開始日"]);
                                            index.開始日 = dt.ToString("yyyyMMdd");
                                        }
                                        if (!(reader["終了日"] is DBNull))
                                        {
                                            var dt = Convert.ToDateTime(reader["終了日"]);
                                            index.終了日 = dt.ToString("yyyyMMdd");
                                        }
                                        index.担当部署 = Convert.ToString(reader["担当部署"]);
                                        index.備考 = Convert.ToString(reader["備考"]);
                                        index.荷主ID = Convert.ToInt32(reader["荷主ID"]);
                                        index.荷主名 = Convert.ToString(reader["荷主名"]);
                                        index.名称_HND = Convert.ToString(reader["名称_HND"]);
                                        index.名称_TF = Convert.ToString(reader["名称_TF"]);
                                        index.FAX番号 = Convert.ToString(reader["FAX番号"]);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"エラーの詳細: {ex.Message}");
                                }
                            }
                            else
                            {
                                index = null;
                            }
                        }
                    }
                    catch(SqlException ex)
                    {
                        Console.WriteLine("データベース接続エラー: " + ex.Message);
                    }
                }                
            });
            return index;
        }










        
    }
}
