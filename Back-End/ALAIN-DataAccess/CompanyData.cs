using ALAIN_DataAccess.DTO;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ALAIN_DataAccess
{
    public static class CompanyData
    {
        public static bool SaveCompanySettings(Model.Company company)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"update CompanySettings set CompanyName=@Name,Email=@Email,Phone=@Phone,Address=@Address,
                                    City=@City where Id=1;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", company.Name);
                    command.Parameters.AddWithValue("@Email", company.Email);
                    command.Parameters.AddWithValue("@Phone", company.Phone);
                    command.Parameters.AddWithValue("@Address", company.Address);
                    command.Parameters.AddWithValue("@City", company.City);


                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static Model.Company? GetCompanyInfo()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(@"select * from CompanySettings;", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Model.Company(reader.GetInt32("Id"), reader.GetString("CompanyName"),
                                reader.GetString("Email"), reader.GetString("Phone"), reader.GetString("Address")
                                , reader.GetString("City"));
                        }
                        return null;
                    }
                }
            }
        }
    }
}