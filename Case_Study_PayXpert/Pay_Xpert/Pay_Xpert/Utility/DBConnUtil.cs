using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
namespace Pay_Xpert.Utility
{
    internal class DBConnUtil
    {
        private static IConfiguration _configuration;

        static DBConnUtil()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _configuration = builder.Build();
        }

        public static string GetConnectionString()
        {
            return _configuration.GetConnectionString("LocalConnectionString");
        }

        public static SqlConnection GetConnection()
        {
            string connectionString = GetConnectionString();
            return new SqlConnection(connectionString);
        }
    }
}
