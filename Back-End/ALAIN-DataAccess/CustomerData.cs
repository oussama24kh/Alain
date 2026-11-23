using ALAIN_DataAccess.Model;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace ALAIN_DataAccess
{
    public static class CustomerData
    {
        public static List<Model.Customer> GetAllCutomers()
        {
            List<Model.Customer> customers = new List<Model.Customer>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT C.CustomerID,C.NationalNumber,C.FirstName,C.LastName,C.Phone,C.Email,
                               C.Address,C.DateOfBirth,CO.Name AS Nationality,
                               (
                                   SELECT COUNT(*)
                                   FROM Rental R
                                   WHERE R.CustomerID = C.CustomerID
                               ) AS TotalRentals
                               FROM Customers C INNER JOIN Country CO ON C.CountryID = CO.CountryID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Model.Customer(reader.GetInt32("CustomerID"), reader.GetString("NationalNumber"),
                                            reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Phone"),
                                            reader.GetString("Email"), reader.IsDBNull("Address") ? null : reader.GetString("Address"),
                                            reader.GetDateTime("DateOfBirth"),reader.GetString("Nationality"),reader.GetInt32("TotalRentals")));
                        }
                    }
                }
            }
            return customers;
        }
        public static List<Model.Customer> FilterCustomersByNationality(string Nationality)
        {
            List<Model.Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT C.CustomerID,C.NationalNumber,C.FirstName,C.LastName,C.Phone,C.Email,
                               C.Address,C.DateOfBirth,CO.Name AS Nationality,
                               (
                                   SELECT COUNT(*)
                                   FROM Rental R
                                   WHERE R.CustomerID = C.CustomerID
                               ) AS TotalRentals
                               FROM Customers C INNER JOIN Country CO ON C.CountryID = CO.CountryID
                               where CO.Name = @Country;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Country", Nationality);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Model.Customer(reader.GetInt32("CustomerID"), reader.GetString("NationalNumber"),
                                            reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Phone"),
                                            reader.GetString("Email"), reader.IsDBNull("Address") ? null : reader.GetString("Address"),
                                            reader.GetDateTime("DateOfBirth"), reader.GetString("Nationality"), reader.GetInt32("TotalRentals")));
                        }
                    }
                }
            }
            return customers;
        }
        public static List<Model.Customer> FilterCustomersByCustomerName(string Name)
        {
            List<Model.Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT C.CustomerID,C.NationalNumber,C.FirstName,C.LastName,C.Phone,C.Email,
                               C.Address,C.DateOfBirth,CO.Name AS Nationality,
                               (
                                   SELECT COUNT(*)
                                   FROM Rental R
                                   WHERE R.CustomerID = C.CustomerID
                               ) AS TotalRentals
                               FROM Customers C INNER JOIN Country CO ON C.CountryID = CO.CountryID
                               where C.FirstName+' '+C.LastName like @Name;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", Name + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Model.Customer(reader.GetInt32("CustomerID"), reader.GetString("NationalNumber"),
                                            reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Phone"),
                                            reader.GetString("Email"), reader.IsDBNull("Address") ? null : reader.GetString("Address"),
                                            reader.GetDateTime("DateOfBirth"), reader.GetString("Nationality"), reader.GetInt32("TotalRentals")));
                        }
                    }
                }
            }
            return customers;
        }
        public static List<Model.Customer> FilterCustomersByNationalNumber(string NationalNumber)
        {
            List<Model.Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT C.CustomerID,C.NationalNumber,C.FirstName,C.LastName,C.Phone,C.Email,
                               C.Address,C.DateOfBirth,CO.Name AS Nationality,
                               (
                                   SELECT COUNT(*)
                                   FROM Rental R
                                   WHERE R.CustomerID = C.CustomerID
                               ) AS TotalRentals
                               FROM Customers C INNER JOIN Country CO ON C.CountryID = CO.CountryID
                               where C.NationalNumber like @NationalNumber;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNumber", NationalNumber + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Model.Customer(reader.GetInt32("CustomerID"), reader.GetString("NationalNumber"),
                                            reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Phone"),
                                            reader.GetString("Email"), reader.IsDBNull("Address") ? null : reader.GetString("Address"),
                                            reader.GetDateTime("DateOfBirth"), reader.GetString("Nationality"), reader.GetInt32("TotalRentals")));
                        }
                    }
                }
            }
            return customers;
        }
        public static List<Model.Customer> FilterCustomersByPhoneNumber(string Phone)
        {
            List<Model.Customer> customers = new List<Customer>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT C.CustomerID,C.NationalNumber,C.FirstName,C.LastName,C.Phone,C.Email,
                               C.Address,C.DateOfBirth,CO.Name AS Nationality,
                               (
                                   SELECT COUNT(*)
                                   FROM Rental R
                                   WHERE R.CustomerID = C.CustomerID
                               ) AS TotalRentals
                               FROM Customers C INNER JOIN Country CO ON C.CountryID = CO.CountryID
                               where C.Phone like @Phone;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Phone", Phone + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Model.Customer(reader.GetInt32("CustomerID"), reader.GetString("NationalNumber"),
                                            reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Phone"),
                                            reader.GetString("Email"), reader.IsDBNull("Address") ? null : reader.GetString("Address"),
                                            reader.GetDateTime("DateOfBirth"), reader.GetString("Nationality"), reader.GetInt32("TotalRentals")));
                        }
                    }
                }
            }
            return customers;
        }
        public static Model.Customer? GetCustomerInfoByNationalNumber(string NationalNumber)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT C.CustomerID,C.NationalNumber,C.FirstName,C.LastName,C.Phone,C.Email,
                               C.Address,C.DateOfBirth,CO.Name AS Nationality,
                               (
                                   SELECT COUNT(*)
                                   FROM Rental R
                                   WHERE R.CustomerID = C.CustomerID
                               ) AS TotalRentals
                               FROM Customers C INNER JOIN Country CO ON C.CountryID = CO.CountryID
                               where C.NationalNumber=@NationalNo;";
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNumber);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Model.Customer(reader.GetInt32("CustomerID"), reader.GetString("NationalNumber"),
                                            reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Phone"),
                                            reader.GetString("Email"), reader.IsDBNull("Address") ? null : reader.GetString("Address"),
                                            reader.GetDateTime("DateOfBirth"), reader.GetString("Nationality"), reader.GetInt32("TotalRentals"));
                        }
                        return null;
                    }
                }
            }
        }
        public static Model.Customer? GetCustomerInfoByCustomerID(int CustomerID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT C.CustomerID,C.NationalNumber,C.FirstName,C.LastName,C.Phone,C.Email,
                               C.Address,C.DateOfBirth,CO.Name AS Nationality,
                               (
                                   SELECT COUNT(*)
                                   FROM Rental R
                                   WHERE R.CustomerID = C.CustomerID
                               ) AS TotalRentals
                               FROM Customers C INNER JOIN Country CO ON C.CountryID = CO.CountryID
                               where C.CustomerID=@CustomerID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", CustomerID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Model.Customer(reader.GetInt32("CustomerID"), reader.GetString("NationalNumber"),
                                            reader.GetString("FirstName"), reader.GetString("LastName"), reader.GetString("Phone"),
                                            reader.GetString("Email"), reader.IsDBNull("Address") ? null : reader.GetString("Address"),
                                            reader.GetDateTime("DateOfBirth"), reader.GetString("Nationality"), reader.GetInt32("TotalRentals"));
                        }
                        return null;
                    }
                }
            }
        }
        public static bool CheckIfNationalNumberAlreadyExist(string NationalNo)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select iif(exists(select 1 from Customers where NationalNumber=@NationalNo),1,0);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    return Convert.ToBoolean(command.ExecuteScalar());
                }
            }
        }
        public static int CountCustomerNew(int Year, int Month)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) AS TotalCustomers FROM Customers
                        WHERE YEAR(CreatedAt) = @Year AND MONTH(CreatedAt) = @Month;";
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@Year", Year);
                    command.Parameters.AddWithValue("@Month", Month);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static int CountTotalCustomers()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select count(*) from Customers;", connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static int? AddNewCustomer(Model.Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"insert into Customers(NationalNumber,FirstName,LastName,Phone,Email,Address,DateOfBirth,
                                CountryID)values (@NationalNo,@FirstName,@LastName,@Phone,@Email,@Address,@DateOfBirth,
                                (select CountryID from Country where Name=@Nationality));select SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo",customer.NationalNo);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Phone", customer.Phone);
                    command.Parameters.AddWithValue("@Email", customer.Email);

                    if (customer.Address == "")
                        command.Parameters.AddWithValue("@Address", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Address", customer.Address);

                    command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                    command.Parameters.AddWithValue("@Nationality", customer.Nationality);

                    int? Id = Convert.ToInt32(command.ExecuteScalar());
                    if (Id != null && Id > 0)
                    {
                        customer.Id = Id.Value;
                        return Id;
                    }
                    return null;
                }
            }
        }
        public static bool EditCustomer(Model.Customer customer)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"update Customers set NationalNumber=@NationalNo,FirstName=@FirstName,LastName=@LastName
                                ,Phone=@Phone,Email=@Email,Address=@Address,DateOfBirth=@DateOfBirth,
                                CountryID=(select CountryID from Country where Name=@Nationality)
                                where CustomerID = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", customer.Id);
                    command.Parameters.AddWithValue("@NationalNo", customer.NationalNo);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@Phone", customer.Phone);
                    command.Parameters.AddWithValue("@Email", customer.Email);

                    if (customer.Address == "")
                        command.Parameters.AddWithValue("@Address", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Address", customer.Address);

                    command.Parameters.AddWithValue("@DateOfBirth", customer.DateOfBirth);
                    command.Parameters.AddWithValue("@Nationality", customer.Nationality);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static int DeleteCustomer(string NationalNo)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"delete from Customers where NationalNumber=@NationalNo;", connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    if(command.ExecuteNonQuery() > 0)
                    {
                        command.CommandText = @"select count(*) from Customers;";
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    return -1;
                }
            }
        }
    }
}