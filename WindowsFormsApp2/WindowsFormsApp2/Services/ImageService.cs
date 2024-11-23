using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Models;

namespace WindowsFormsApp2.Services
{
    public class ImageService
    {
        private static String _connectionString;

        static ImageService()
        {
            _connectionString = DBConnection.GetConnectionString();
        }


        public static async Task<List<ImageInfo>> GetImageDataFromDBAsync(ImageInfo image)
        {
            var imageInfos = new List<ImageInfo>();
            //WHERE句作成
            var whereStr = "";
            if (image.受注ID != 0)
            {
                whereStr += " AND 受注ID = @ZID";
            }
            else
            {
                if (image.荷主名 != "") whereStr += " AND 荷主名 = @NME";
                if (image.担当部署 != "") whereStr += " AND 担当部署 = @TBS";
                if (image.備考 != "") whereStr += " AND 備考 = @BKU";
                if (image.開始日 != null && image.終了日 != null) whereStr += " AND 開始日 <= @ED AND 終了日 >= @SD";
            }
            if(whereStr != "")
            {
                whereStr = " WHERE " + whereStr.Substring(5);
            }
            var sql = @"SELECT dbo.T_TF_D_Image.* FROM dbo.T_TF_D_Image " + whereStr;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                try
                {
                    await connection.OpenAsync();
                    if(image.受注ID != 0) command.Parameters.Add(new SqlParameter("@ZID", image.受注ID));
                    if (image.荷主名 != "") command.Parameters.Add(new SqlParameter("@NME", image.荷主名));
                    if (image.担当部署 != "") command.Parameters.Add(new SqlParameter("@TBS", image.担当部署));
                    if (image.備考 != "") command.Parameters.Add(new SqlParameter("@BKU", image.備考));
                    if (image.開始日 != null) command.Parameters.Add(new SqlParameter("@SD", image.開始日));
                    if (image.終了日 != null) command.Parameters.Add(new SqlParameter("@ED", image.終了日));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {
                                var imageInfo = new ImageInfo();
                                imageInfo.Id = reader["Id"].ToString();
                                imageInfo.CreateDate = reader["CreateDate"] as DateTime?;
                                imageInfo.ImageData = reader["ImageData"] as byte[];
                                imageInfo.ImageName = reader["ImageName"].ToString();
                                imageInfo.IdDelete = (bool)reader["IsDelete"];
                                imageInfo.受注ID = (decimal)reader["受注ID"];
                                imageInfo.荷主ID = (int)reader["荷主ID"];
                                imageInfo.荷主名 = reader["荷主名"].ToString();
                                imageInfo.担当部署 = reader["担当部署"].ToString();
                                imageInfo.備考 = reader["備考"].ToString();
                                imageInfo.開始日 = reader["開始日"] as DateTime?;
                                imageInfo.終了日 = reader["終了日"] as DateTime?;
                                //imageinfo.Id = Convert.ToString(reader["Id"]);
                                //imageinfo.CreateDate = Convert.ToDateTime(reader["CreateDate"]);
                                //imageinfo.ImageData = (byte[])reader["ImageData"];
                                //imageinfo.ImageName = Convert.ToString(reader["Id"]);
                                //imageinfo.IdDelete = Convert.ToBoolean(reader["IsDelete"]);
                                //imageinfo.受注ID = Convert.ToDecimal(reader["受注ID"]);
                                //imageinfo.荷主ID = Convert.ToInt32(reader["荷主ID"]);
                                //imageinfo.荷主名 = Convert.ToString(reader["荷主名"]);
                                //imageinfo.担当部署 = Convert.ToString(reader["担当部署"]);
                                //imageinfo.備考 = Convert.ToString(reader["備考"]);
                                //imageinfo.開始日 = Convert.ToDateTime(reader["開始日"]);
                                //imageinfo.終了日 = Convert.ToDateTime(reader["終了日"]);
                                imageInfos.Add(imageInfo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("SQLエラー: " + ex.Message);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("データベース接続エラー: " + ex.Message);
                }
            }
            
            return imageInfos;
        }

        public static async Task<ImageInfo> GetImageDataFromDBByIdAsync(string Id)
        {
            var imageInfo = new ImageInfo();
            var sql = @"SELECT dbo.T_TF_D_Image.* FROM dbo.T_TF_D_Image WHERE Id = @ID";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                try
                {
                    await connection.OpenAsync();
                    command.Parameters.Add(new SqlParameter("@ID", Id));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        try
                        {
                            while (await reader.ReadAsync())
                            {
                                imageInfo.Id = reader["Id"].ToString();
                                imageInfo.CreateDate = reader["CreateDate"] as DateTime?;
                                imageInfo.ImageData = reader["ImageData"] as byte[];
                                imageInfo.ImageName = reader["ImageName"].ToString();
                                imageInfo.IdDelete = (bool)reader["IsDelete"];
                                imageInfo.受注ID = (decimal)reader["受注ID"];
                                imageInfo.荷主ID = (int)reader["荷主ID"];
                                imageInfo.荷主名 = reader["荷主名"].ToString();
                                imageInfo.担当部署 = reader["担当部署"].ToString();
                                imageInfo.備考 = reader["備考"].ToString();
                                imageInfo.開始日 = reader["開始日"] as DateTime?;
                                imageInfo.終了日 = reader["終了日"] as DateTime?;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("SQLエラー: " + ex.Message);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("データベース接続エラー: " + ex.Message);
                }
            }

            return imageInfo;
        }







        public static async Task AddImageDataToDBAsync(ImageInfo image)
        {
            var sql = @"INSERT INTO T_TF_D_Image (
ImageData,
ImageName,
IsDelete,
受注ID,
荷主ID,
荷主名,
担当部署,
備考,
開始日,
終了日
) VALUES (
@ImageData,
@ImageName,
@IsDelete,
@受注ID,
@荷主ID,
@荷主名,
@担当部署,
@備考,
@開始日,
@終了日)";


            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                try
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@ImageData", image.ImageData);
                    command.Parameters.AddWithValue("@ImageName", image.ImageName);
                    command.Parameters.AddWithValue("@IsDelete", image.IdDelete);
                    command.Parameters.AddWithValue("@受注ID", image.受注ID);
                    command.Parameters.AddWithValue("@荷主ID", image.荷主ID);
                    command.Parameters.AddWithValue("@荷主名", image.荷主名);
                    command.Parameters.AddWithValue("@担当部署", image.担当部署);
                    command.Parameters.AddWithValue("@備考", image.備考);
                    command.Parameters.AddWithValue("@開始日", image.開始日);
                    command.Parameters.AddWithValue("@終了日", image.終了日);

                    //command.Parameters.Add(new SqlParameter("@ImageData", image.ImageData));
                    //command.Parameters.Add(new SqlParameter("@ImageName", image.ImageName));
                    //command.Parameters.Add(new SqlParameter("@IsDelete", image.IdDelete));
                    //command.Parameters.Add(new SqlParameter("@受注ID", image.受注ID));
                    //command.Parameters.Add(new SqlParameter("@荷主ID", image.荷主ID));
                    //command.Parameters.Add(new SqlParameter("@荷主名", image.荷主名));
                    //command.Parameters.Add(new SqlParameter("@担当部署", image.担当部署));
                    //command.Parameters.Add(new SqlParameter("@備考", image.備考));
                    //command.Parameters.Add(new SqlParameter("@開始日", image.開始日));
                    //command.Parameters.Add(new SqlParameter("@終了日", image.終了日));
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SQLエラー: " + ex.Message);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("データベース接続エラー: " + ex.Message);
                }
            }   
        }




    }
}
