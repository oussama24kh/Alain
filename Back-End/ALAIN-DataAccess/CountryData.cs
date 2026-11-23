using Microsoft.Data.SqlClient;
using System.Data;

namespace ALAIN_DataAccess
{
    public static class CountryData
    {
        public static List<string> GetAllCountries()
        {
            List<string>countries = new List<string>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"select Name from Country;", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                            countries.Add(reader.GetString("Name"));
                    }
                }
            }
            return countries;
        }
        public static List<string> GetActiveCountries()
        {
            List<string>countries = new List<string>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Name from Country where CountryID in(select distinct CountryID from Customers);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                            countries.Add(reader.GetString("Name"));
                    }
                }
            }

            return countries;
        }
    }
}