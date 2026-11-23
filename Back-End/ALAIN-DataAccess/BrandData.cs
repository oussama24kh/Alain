using Microsoft.Data.SqlClient;

namespace ALAIN_DataAccess
{
    public static class BrandData
    {
        public static List<string> GetAllBrands()
        {
            List<string> brands = new List<string>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select BrandName from Brand order by BrandName;";
                using (SqlCommand command = new SqlCommand(query, connection)) 
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                            brands.Add(reader.GetString(0));
                    }
                }
            }
            return brands;
        }
    }
}