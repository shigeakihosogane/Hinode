using Microsoft.Data.SqlClient;

namespace FileTransfer2ForBlazor.Services
{
    public class DBConnection
    {
        private String _connectionString;

        public DBConnection()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"hinode-server.database.windows.net";
            builder.InitialCatalog = "HINODEDB";
            builder.UserID = "hinode";
            builder.Password = "Hkanri8739";
            builder.TrustServerCertificate = true;
            _connectionString = builder.ToString();
        }

        public string GetConnectionString()
        {
            return _connectionString; 
        }




    }
}
