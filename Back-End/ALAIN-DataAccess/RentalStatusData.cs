using Microsoft.Data.SqlClient;
using System.Data;

namespace ALAIN_DataAccess
{
    public static class RentalStatusData
    {
        public static List<string> GetActiveStatus()
        {
            List<string> RentalStatus = new List<string>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select StatusName as Status from RentalStatus 
                    where RentalStatusID in(select distinct RentalStatusID);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            RentalStatus.Add(reader.GetString("Status"));
                    }
                }
            }
            return RentalStatus;
        }
    }
}