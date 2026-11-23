using System.Configuration;

namespace ALAIN_DataAccess
{
    internal static class DataAccessSettings
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        /*string connectionString =
    Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")
    ?? throw new InvalidOperationException(
        "Set the SQL_CONNECTION_STRING environment variable.");*/
    }
}