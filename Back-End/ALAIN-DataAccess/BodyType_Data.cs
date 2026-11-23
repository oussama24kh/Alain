using Microsoft.Data.SqlClient;
using System.Data;

namespace ALAIN_DataAccess
{
    public static class BodyType_Data
    {
        public static List<string> GetAllBodyTypes()
        {
            List<string> types = new List<string>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select BodyTypeName from BodyType where BodyTypeID 
                        in(select distinct BodyTypeID from Car) order by BodyTypeName;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            types.Add(reader.GetString("BodyTypeName"));
                        }
                    }
                }
            }
            

            return types;
        }
    }
}