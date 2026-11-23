using ALAIN_DataAccess.DTO;
using ALAIN_DataAccess.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ALAIN_DataAccess
{
    public static class RentalData
    {
        public static List<Rental_DTO> GetAllRentals()
        {
            List<Rental_DTO> rentals = new List<Rental_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Brand.BrandName+' '+Car.Model as CarFullName,Car.LicensePlate as CarPlate,
                                Customers.FirstName+' '+Customers.LastName as CustomerName, Customers.NationalNumber
                                as NationalNo, Rental.StartDate, iif(ActualEndDate is not null,Rental.ActualEndDate,
                                Rental.ExpectedEndDate) as EndDate,CASE when RentalStatus.RentalStatusID=2 and 
                                cast(Rental.ExpectedEndDate as date) < cast(getdate() as date) then 'EN RETARD'
                                else RentalStatus.StatusName end as Status
                                from Rental join Car on Rental.CarID=Car.CarID join Customers on 
                                Rental.CustomerID=Customers.CustomerID join RentalStatus on 
                                Rental.RentalStatusID = RentalStatus.RentalStatusID join Brand on Brand.BrandID=Car.BrandID
                                order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rentals.Add(new Rental_DTO(reader.GetString("RentalCode"), reader.GetString("CarFullName"),
                                reader.GetString("CarPlate"), reader.GetString("CustomerName"), reader.GetString("NationalNo"),
                                reader.GetDateTime("StartDate"), reader.GetDateTime("EndDate"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return rentals;
        }
        public static List<Rental_DTO> FilterRentalsByRentalStatus(string RentalStatus)
        {
            List<Rental_DTO> rentals = new List<Rental_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Brand.BrandName+' '+Car.Model as CarFullName,Car.LicensePlate as CarPlate,
                                Customers.FirstName+' '+Customers.LastName as CustomerName, Customers.NationalNumber
                                as NationalNo, Rental.StartDate, iif(ActualEndDate is not null,Rental.ActualEndDate,
                                Rental.ExpectedEndDate) as EndDate, CASE when RentalStatus.RentalStatusID=2 and 
                                cast(Rental.ExpectedEndDate as date) < cast(getdate() as date) then 'EN RETARD'
                                else RentalStatus.StatusName end as Status
                                from Rental join Car on Rental.CarID=Car.CarID join Customers on 
                                Rental.CustomerID=Customers.CustomerID join RentalStatus on 
                                Rental.RentalStatusID = RentalStatus.RentalStatusID join Brand on Brand.BrandID=Car.BrandID
                                where Rental.RentalStatusID=(select RentalStatusID from RentalStatus where StatusName=@Status) 
                                order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", RentalStatus);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rentals.Add(new Rental_DTO(reader.GetString("RentalCode"), reader.GetString("CarFullName"),
                                reader.GetString("CarPlate"), reader.GetString("CustomerName"), reader.GetString("NationalNo"),
                                reader.GetDateTime("StartDate"), reader.GetDateTime("EndDate"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return rentals;
        }
        public static List<Rental_DTO> FilterRentalsByRentalCode(string RentalCode)
        {
            List<Rental_DTO> rentals = new List<Rental_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select * from(
                                  select Rental.RentalID,Rental.RentalCode,Brand.BrandName+' '+Car.Model as CarFullName,
                                  Car.LicensePlate as CarPlate,Customers.FirstName+' '+Customers.LastName as CustomerName,
                                  Customers.NationalNumber as NationalNo, Rental.StartDate, iif(ActualEndDate is not null,
                                  Rental.ActualEndDate,
                                  Rental.ExpectedEndDate) as EndDate, CASE when RentalStatus.RentalStatusID=2 and 
                                  cast(Rental.ExpectedEndDate as date) < cast(getdate() as date) then 'EN RETARD'
                                  else RentalStatus.StatusName end as Status,
                                  RIGHT(Rental.RentalCode, LEN(Rental.RentalCode) - CHARINDEX('-', Rental.RentalCode)) as SmallCode
                                  from Rental join Car on Rental.CarID=Car.CarID join Customers on 
                                  Rental.CustomerID=Customers.CustomerID join RentalStatus on 
                                  Rental.RentalStatusID = RentalStatus.RentalStatusID join Brand on Brand.BrandID=Car.BrandID
                                 )x where x.SmallCode like @Code order by x.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", RentalCode+"%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rentals.Add(new Rental_DTO(reader.GetString("RentalCode"), reader.GetString("CarFullName"),
                                reader.GetString("CarPlate"), reader.GetString("CustomerName"), reader.GetString("NationalNo"),
                                reader.GetDateTime("StartDate"), reader.GetDateTime("EndDate"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return rentals;
        }
        public static List<Rental_DTO> FilterRentalsByPlateNumber(string PlateNumber)
        {
            List<Rental_DTO> rentals = new List<Rental_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Brand.BrandName+' '+Car.Model as CarFullName,Car.LicensePlate as CarPlate,
                                Customers.FirstName+' '+Customers.LastName as CustomerName, Customers.NationalNumber
                                as NationalNo, Rental.StartDate, iif(ActualEndDate is not null,Rental.ActualEndDate,
                                Rental.ExpectedEndDate) as EndDate, CASE when RentalStatus.RentalStatusID=2 and 
                                cast(Rental.ExpectedEndDate as date) < cast(getdate() as date) then 'EN RETARD'
                                else RentalStatus.StatusName end as Status
                                from Rental join Car on Rental.CarID=Car.CarID join Customers on 
                                Rental.CustomerID=Customers.CustomerID join RentalStatus on 
                                Rental.RentalStatusID = RentalStatus.RentalStatusID join Brand on Brand.BrandID=Car.BrandID
                                where Car.LicensePlate like @PlateNumber order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlateNumber", PlateNumber+"%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rentals.Add(new Rental_DTO(reader.GetString("RentalCode"), reader.GetString("CarFullName"),
                                reader.GetString("CarPlate"), reader.GetString("CustomerName"), reader.GetString("NationalNo"),
                                reader.GetDateTime("StartDate"), reader.GetDateTime("EndDate"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return rentals;
        }
        public static List<Rental_DTO> FilterRentalsByNationalNumber(string NationalNumber)
        {
            List<Rental_DTO> rentals = new List<Rental_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Brand.BrandName+' '+Car.Model as CarFullName,Car.LicensePlate as CarPlate,
                                Customers.FirstName+' '+Customers.LastName as CustomerName, Customers.NationalNumber
                                as NationalNo, Rental.StartDate, iif(ActualEndDate is not null,Rental.ActualEndDate,
                                Rental.ExpectedEndDate) as EndDate, CASE when RentalStatus.RentalStatusID=2 and 
                                cast(Rental.ExpectedEndDate as date) < cast(getdate() as date) then 'EN RETARD'
                                else RentalStatus.StatusName end as Status
                                from Rental join Car on Rental.CarID=Car.CarID join Customers on 
                                Rental.CustomerID=Customers.CustomerID join RentalStatus on 
                                Rental.RentalStatusID = RentalStatus.RentalStatusID join Brand on Brand.BrandID=Car.BrandID
                                where Customers.NationalNumber like @NationalNumber order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNumber", NationalNumber + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rentals.Add(new Rental_DTO(reader.GetString("RentalCode"), reader.GetString("CarFullName"),
                                reader.GetString("CarPlate"), reader.GetString("CustomerName"), reader.GetString("NationalNo"),
                                reader.GetDateTime("StartDate"), reader.GetDateTime("EndDate"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return rentals;
        }
        public static List<Rental_DTO> GetLatestRentals(int NumberOfRentals)
        {
            List<Rental_DTO> rentals = new List<Rental_DTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select top (@NoRentals) Rental.RentalCode,Brand.BrandName+' '+Car.Model as CarFullName,Car.LicensePlate as CarPlate,
                                Customers.FirstName+' '+Customers.LastName as CustomerName, Customers.NationalNumber
                                as NationalNo, Rental.StartDate, iif(ActualEndDate is not null,Rental.ActualEndDate,
                                Rental.ExpectedEndDate) as EndDate, CASE when RentalStatus.RentalStatusID=2 and 
                                cast(Rental.ExpectedEndDate as date) < cast(getdate() as date) then 'EN RETARD'
                                else RentalStatus.StatusName end as Status
                                from Rental join Car on Rental.CarID=Car.CarID join Customers on 
                                Rental.CustomerID=Customers.CustomerID join RentalStatus on 
                                Rental.RentalStatusID = RentalStatus.RentalStatusID join Brand on Brand.BrandID=Car.BrandID
                                where Rental.RentalStatusID=2 order by Rental.CreatedAt desc;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NoRentals", NumberOfRentals);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rentals.Add(new Rental_DTO(reader.GetString("RentalCode"), reader.GetString("CarFullName"),
                                reader.GetString("CarPlate"), reader.GetString("CustomerName"), reader.GetString("NationalNo"),
                                reader.GetDateTime("StartDate"), reader.GetDateTime("EndDate"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return rentals;
        }
        public static int CountRentalsByStatus(string Status)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select count(*) from Rental where RentalStatusID=
                        (select RentalStatusID from RentalStatus where StatusName=@Status);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", Status);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static int CountRentalsByStatus(string Status, int Year, int Month)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) AS TotalRentals FROM Rental WHERE YEAR(CreatedAt)=@Year and MONTH(CreatedAt)=@Month
                                    and Rental.RentalStatusID=(select RentalStatusID from RentalStatus where StatusName=@Status);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.AddWithValue("@Year", Year);
                    command.Parameters.AddWithValue("@Month", Month);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static int CountCompletedRentals(int Year,int Month)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) AS TotalRentals FROM Rental WHERE ActualEndDate is not null and 
                                YEAR(ActualEndDate)=@Year and MONTH(ActualEndDate)=@Month and
                                Rental.RentalStatusID=3;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", Year);
                    command.Parameters.AddWithValue("@Month", Month);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static int CountActiveRentals()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select count(*) from Rental where RentalStatusID in(2,5);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static Model.Rental? AddNewRental(Model.Rental rental)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"insert into Rental(CustomerID,CarID,StartDate,ExpectedEndDate,ActualEndDate,StartMileage,EndMileage,
			                    DailyPrice,Notes,RentalCode) values (@CustomerID,@CarID,@StartDate,@ExpectedEndDate,
			                    @ActualEndDate,@StartMileage,@EndMileage,@DailyPrice,@Notes,dbo.GetNextContractReference());
			                    select SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", rental.CustomerID);
                    command.Parameters.AddWithValue("@CarID", rental.CarID);
                    command.Parameters.AddWithValue("@StartDate", rental.StartDate);
                    command.Parameters.AddWithValue("@ExpectedEndDate", rental.ExpectedEndDate);
                    command.Parameters.AddWithValue("@ActualEndDate", DBNull.Value);
                    command.Parameters.AddWithValue("@StartMileage", rental.StartMileage);
                    command.Parameters.AddWithValue("@EndMileage", DBNull.Value);
                    command.Parameters.AddWithValue("@DailyPrice", rental.DailyPrice);

                    if (string.IsNullOrWhiteSpace(rental.Notes))
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Notes", rental.Notes);

                    object? result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        rental.RentalID = Convert.ToInt32(result);

                        command.CommandText = @"select RentalCode from Rental where RentalID=@ID;";
                        command.Parameters.AddWithValue("@ID", rental.RentalID);

                        rental.RentalCode = Convert.ToString(command.ExecuteScalar());

                        return rental;
                    }
                    return null;
                }
            }
        }
        public static bool EditRental(Model.Rental rental)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"update Rental set CustomerID=@CustomerID,CarID=@CarID,StartDate=@StartDate,ExpectedEndDate=@ExpectedEndDate,
	                            ActualEndDate=@ActualEndDate,StartMileage=@StartMileage,EndMileage=@EndMileage,
	                            RentalStatusID=(select RentalStatusID from RentalStatus where StatusName=@Status),
	                            DailyPrice=@DailyPrice,Notes=@Notes where RentalID=@RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RentalID", rental.RentalID);
                    command.Parameters.AddWithValue("@CustomerID", rental.CustomerID);
                    command.Parameters.AddWithValue("@CarID", rental.CarID);
                    
                    command.Parameters.AddWithValue("@StartDate", rental.StartDate);
                    command.Parameters.AddWithValue("@ExpectedEndDate", rental.ExpectedEndDate);
                    if (rental.EndDate == null)
                        command.Parameters.AddWithValue("@ActualEndDate", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@ActualEndDate", rental.EndDate);

                    command.Parameters.AddWithValue("@StartMileage", rental.StartMileage);
                    if (rental.EndMileage == null)
                        command.Parameters.AddWithValue("@EndMileage", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@EndMileage", rental.EndMileage.Value);

                    command.Parameters.AddWithValue("@Status", rental.Status.ToString().Replace("_", " "));

                    command.Parameters.AddWithValue("@DailyPrice", rental.DailyPrice);

                    if (string.IsNullOrWhiteSpace(rental.Notes))
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Notes", rental.Notes);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static Model.Rental? GetRentalInfoByRentalID(int RentalID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"select Rental.RentalID,Rental.RentalCode,Rental.CustomerID,Rental.CarID,Rental.StartDate,
                                Rental.ExpectedEndDate,Rental.ActualEndDate,Rental.StartMileage,Rental.EndMileage,
                                Rental.RentalStatusID as Status,Rental.DailyPrice,Rental.Notes 
                                from Rental where Rental.RentalID=@RentalID;";
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RentalID", RentalID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return new Model.Rental(reader.GetInt32("RentalID"), reader.GetString("RentalCode"),
                                reader.GetInt32("CustomerID"), reader.GetInt32("CarID"), reader.GetDateTime("StartDate"),
                                reader.GetDateTime("ExpectedEndDate"), reader.IsDBNull(reader.GetOrdinal("ActualEndDate")) ?
                                null : reader.GetDateTime("ActualEndDate"), reader.GetInt32("StartMileage"),
                                reader.IsDBNull(reader.GetOrdinal("EndMileage")) ? null : reader.GetInt32("EndMileage"),
                                reader.GetByte("Status"), reader.GetDecimal("DailyPrice"), reader.IsDBNull
                                (reader.GetOrdinal("Notes")) ? null :reader.GetString("Notes"));
                        }
                        return null;
                    }
                }
            }
        }
        public static Model.Rental? GetRentalInfoByRentalCode(string RentalCode)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"select Rental.RentalID,Rental.RentalCode,Rental.CustomerID,Rental.CarID,Rental.StartDate,
                                Rental.ExpectedEndDate,Rental.ActualEndDate,Rental.StartMileage,Rental.EndMileage,
                                Rental.RentalStatusID as Status,Rental.DailyPrice,Rental.Notes 
                                from Rental where Rental.RentalCode=@RentalCode;";
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RentalCode", RentalCode);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Model.Rental(reader.GetInt32("RentalID"), reader.GetString("RentalCode"),
                                reader.GetInt32("CustomerID"), reader.GetInt32("CarID"), reader.GetDateTime("StartDate"),
                                reader.GetDateTime("ExpectedEndDate"), reader.IsDBNull(reader.GetOrdinal("ActualEndDate")) ?
                                null : reader.GetDateTime("ActualEndDate"), reader.GetInt32("StartMileage"),
                                reader.IsDBNull(reader.GetOrdinal("EndMileage")) ? null : reader.GetInt32("EndMileage"),
                                reader.GetByte("Status"), reader.GetDecimal("DailyPrice"), reader.IsDBNull
                                (reader.GetOrdinal("Notes")) ? null : reader.GetString("Notes"));
                        }
                        return null;
                    }
                }
            }
        }
        public static DTO.RentalDetails_DTO? GetRentalDetailsByRentalCode(string RentalCode)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"select Rental.RentalCode,Rental.RentalStatusID,Customers.FirstName+' '+Customers.LastName as CustomerName,
                                 Customers.Phone,Customers.NationalNumber,Brand.BrandName as Brand,Car.Model,Car.LicensePlate,
                                 Rental.StartMileage,Rental.EndMileage,Rental.StartDate,Rental.ExpectedEndDate,
                                 Rental.ActualEndDate,Payments.TotalAmount
                                 from Rental join Customers on Rental.CustomerID=Customers.CustomerID join Car on 
                                 Car.CarID=Rental.CarID join Brand on Brand.BrandID=Car.BrandID join Payments on
                                 Payments.RentalID=Rental.RentalID where Rental.RentalCode=@RentalCode;";
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RentalCode", RentalCode);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new RentalDetails_DTO(reader.GetString("RentalCode"), reader.GetByte("RentalStatusID"),
                                            reader.GetString("CustomerName"), reader.GetString("Phone"), reader.GetString("NationalNumber"),
                                            reader.GetString("Brand"), reader.GetString("Model"), reader.GetString("LicensePlate"),
                                            reader.GetInt32("StartMileage"),reader.IsDBNull(reader.GetOrdinal("EndMileage"))?null:
                                            reader.GetInt32("EndMileage"),reader.GetDateTime("StartDate"),
                                            reader.GetDateTime("ExpectedEndDate"),reader.IsDBNull(reader.GetOrdinal("ActualEndDate"))?null:
                                            reader.GetDateTime("ActualEndDate"),reader.GetDecimal("TotalAmount"));
                        }
                        return null;
                    }
                }
            }
        }
        public static int CountOverdueRentals()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                string query = @"select count(*) from Rental where RentalStatusID=5 or (RentalStatusID=2 and 
                        cast(Rental.ExpectedEndDate as date) < cast(getdate() as date));";
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static int GetRentalDuration(string RentalCode)
        {

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select case 
                                when RentalStatusID in(2,5) then DATEDIFF(DAY, StartDate, ExpectedEndDate)
                                else DATEDIFF(DAY,StartDate,ActualEndDate)end from Rental where RentalCode=@Code;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", RentalCode);
                    object obj = command.ExecuteScalar();
                    if (obj == DBNull.Value)
                        return -1;

                    return Convert.ToInt32(obj);
                }
            }
        }
    }
}