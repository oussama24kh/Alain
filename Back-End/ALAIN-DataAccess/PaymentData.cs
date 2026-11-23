using ALAIN_DataAccess.DTO;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;

namespace ALAIN_DataAccess
{
    public static class PaymentData
    {
        public static Model.Payment? GetPaymentInfoByRentalID(int RentalID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Payments.PaymentID,Payments.RentalID,Payments.AmountPaid,Payments.TotalAmount,
                                 PaymentStatus.StatusName as Status from Payments join PaymentStatus 
                                 on Payments.StatusID = PaymentStatus.StatusId where Payments.RentalID=@RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RentalID", RentalID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return new Model.Payment(reader.GetInt32("PaymentID"), reader.GetInt32("RentalID"),
                                reader.GetDecimal("AmountPaid"), reader.GetString("Status"),
                                reader.GetDecimal("TotalAmount"));
                        }
                        return null;
                    }
                }
            }
        }
        public static int? AddNewPayment(Model.Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"insert into Payments (RentalID, AmountPaid,StatusID,TotalAmount) values
                               (@RentalID,@Amount,(select StatusId from PaymentStatus where StatusName=@Status)
                               ,@TotalAmount); select SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RentalID", payment.RentalID);
                    command.Parameters.AddWithValue("@Amount", payment.AmountPaid);
                    command.Parameters.AddWithValue("@Status", payment.PaymentStatus);
                    command.Parameters.AddWithValue("@TotalAmount", payment.TotalAmount);

                    int? Id = Convert.ToInt32(command.ExecuteScalar());
                    if (Id != null && Id > 0)
                    {
                        payment.PaymentID = Id.Value;
                        return Id;
                    }
                    
                    return null;
                }
            }
        }
        public static bool EditPayment(Model.Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"update Payments set RentalID=@RentalID, AmountPaid=@Amount,StatusID=
                               (select StatusId from PaymentStatus where StatusName=@Status),
                               TotalAmount=@TotalAmount where PaymentID=@PaymentID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PaymentID", payment.PaymentID);
                    command.Parameters.AddWithValue("@RentalID", payment.RentalID);
                    command.Parameters.AddWithValue("@Amount", payment.AmountPaid);
                    command.Parameters.AddWithValue("@Status", payment.PaymentStatus);
                    command.Parameters.AddWithValue("@TotalAmount", payment.TotalAmount);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static decimal? GetTotalPrice(int RentalID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select TotalAmount from Payments where RentalID=@RentalID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RentalID", RentalID);
                    return Convert.ToDecimal(command.ExecuteScalar());
                }
            }
        }
        public static decimal CountRevenue(int Year, int Month)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select sum(AmountPaid) from Payments where StatusID=2 
                            and YEAR(PaymentDate)=@Year and MONTH(PaymentDate)=@Month;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", Year);
                    command.Parameters.AddWithValue("@Month", Month);
                    object? sum = command.ExecuteScalar();
                    if (sum == DBNull.Value)
                        return 0;
                    return Convert.ToDecimal(sum);
                }
            }
        }
        public static decimal CountRevenue(int Year, int Month, int Day)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select sum(AmountPaid) from Payments where StatusID=2 
                            and YEAR(PaymentDate)=@Year and MONTH(PaymentDate)=@Month and DAY(PaymentDate)=@Day;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", Year);
                    command.Parameters.AddWithValue("@Month", Month);
                    command.Parameters.AddWithValue("@Day", Day);
                    object? sum = command.ExecuteScalar();
                    if (sum == DBNull.Value)
                        return 0;
                    return Convert.ToDecimal(sum);
                }
            }
        }
        public static decimal CountRevenue()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select sum(AmountPaid) from Payments where StatusID=2;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object? sum = command.ExecuteScalar();
                    if (sum == DBNull.Value)
                        return 0;
                    return Convert.ToDecimal(sum);
                }
            }
        }
        public static decimal CountRevenueCurrentWeek()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"SET DATEFIRST 1;
                                select sum(AmountPaid) from Payments
                                WHERE StatusID=2 and PaymentDate >= DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()), 0) + 1
                                AND PaymentDate <  DATEADD(WEEK, DATEDIFF(WEEK, 0, GETDATE()) + 1, 0) + 1;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object? sum = command.ExecuteScalar();
                    if (sum == DBNull.Value)
                        return 0;
                    return Convert.ToDecimal(sum);
                }
            }
        }
        public static int CountPayment(int Year, int Month)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select count(*) from Payments where StatusID=2 
                            and Year(PaymentDate)=@Year and Month(PaymentDate)=@Month;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", Year);
                    command.Parameters.AddWithValue("@Month", Month);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static List<Payment_DTO> GetAllPayments()
        {
            List<Payment_DTO> payments = new List<Payment_DTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Customers.FirstName+' '+Customers.LastName as CustomerName,
                                Customers.NationalNumber,Brand.BrandName+' '+Car.Model as CarFullName, Car.LicensePlate,
                                Payments.AmountPaid,Payments.PaymentDate from Rental join Customers on 
                                Rental.CustomerID=Customers.CustomerID join Car on Car.CarID=Rental.CarID join 
                                Brand on Brand.BrandID=Car.BrandID join Payments on Payments.RentalID=Rental.RentalID
                                where Payments.StatusID=2 order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment_DTO(reader.GetString("RentalCode"), reader.GetString("CustomerName"),
                                reader.GetString("NationalNumber"), reader.GetString("CarFullName"),
                                reader.GetString("LicensePlate"), reader.GetDateTime("PaymentDate"), 
                                reader.GetDecimal("AmountPaid")));
                        }
                    }
                }
            }
            return payments;
        }
        public static List<Payment_DTO> FilterPaymentByRentalCode(string RentalCode)
        {
            List<Payment_DTO> payments = new List<Payment_DTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select * from(
                                  select Rental.RentalID,Rental.RentalCode,Customers.FirstName+' '+Customers.LastName as CustomerName,
                                  Customers.NationalNumber,Brand.BrandName+' '+Car.Model as CarFullName, Car.LicensePlate,
                                  Payments.AmountPaid,Payments.PaymentDate,
                                  RIGHT(Rental.RentalCode, LEN(Rental.RentalCode) - CHARINDEX('-', Rental.RentalCode)) as SmallCode
                                  from Rental join Customers on 
                                  Rental.CustomerID=Customers.CustomerID join Car on Car.CarID=Rental.CarID join 
                                  Brand on Brand.BrandID=Car.BrandID join Payments on Payments.RentalID=Rental.RentalID
                                  where Payments.StatusID=2)x where x.SmallCode like @Code order by x.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", RentalCode + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment_DTO(reader.GetString("RentalCode"), reader.GetString("CustomerName"),
                                reader.GetString("NationalNumber"), reader.GetString("CarFullName"),
                                reader.GetString("LicensePlate"), reader.GetDateTime("PaymentDate"),
                                reader.GetDecimal("AmountPaid")));
                        }
                    }
                }
            }
            return payments;
        }
        public static decimal GetTotalRevenueByRentalCode(string RentalCode)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select sum(AmountPaid) from Payments join Rental on Payments.RentalID=Rental.RentalID
                        where StatusID=2 and RIGHT(Rental.RentalCode, LEN(Rental.RentalCode) - CHARINDEX('-', Rental.RentalCode))
                        like @Code;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", RentalCode + "%");
                    object? sum = command.ExecuteScalar();
                    if (sum == DBNull.Value)
                        return 0;
                    return Convert.ToDecimal(sum);
                }
            }
        }
        public static List<Payment_DTO> FilterPaymentByPlateNumber(string PlateNumber)
        {
            List<Payment_DTO> payments = new List<Payment_DTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Customers.FirstName+' '+Customers.LastName as CustomerName,
                                Customers.NationalNumber,Brand.BrandName+' '+Car.Model as CarFullName, Car.LicensePlate,
                                Payments.AmountPaid,Payments.PaymentDate from Rental join Customers on 
                                Rental.CustomerID=Customers.CustomerID join Car on Car.CarID=Rental.CarID join 
                                Brand on Brand.BrandID=Car.BrandID join Payments on Payments.RentalID=Rental.RentalID
                                where Payments.StatusID=2 and Car.LicensePlate like @Plate order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Plate", PlateNumber + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment_DTO(reader.GetString("RentalCode"), reader.GetString("CustomerName"),
                                reader.GetString("NationalNumber"), reader.GetString("CarFullName"),
                                reader.GetString("LicensePlate"), reader.GetDateTime("PaymentDate"),
                                reader.GetDecimal("AmountPaid")));
                        }
                    }
                }
            }
            return payments;
        }
        public static decimal GetTotalRevenueByPlateNumber(string PlateNumber)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select sum(AmountPaid) from Payments join Rental on Payments.RentalID=Rental.RentalID
                                 join Car on Rental.CarID=Car.CarID where StatusID=2 and Car.LicensePlate like @Plate;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Plate", PlateNumber + "%");
                    object? sum = command.ExecuteScalar();
                    if (sum == DBNull.Value)
                        return 0;
                    return Convert.ToDecimal(sum);
                }
            }
        }
        public static List<Payment_DTO> FilterPaymentByNationalNumber(string NationalNumber)
        {
            List<Payment_DTO> payments = new List<Payment_DTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Customers.FirstName+' '+Customers.LastName as CustomerName,
                                Customers.NationalNumber,Brand.BrandName+' '+Car.Model as CarFullName, Car.LicensePlate,
                                Payments.AmountPaid,Payments.PaymentDate from Rental join Customers on 
                                Rental.CustomerID=Customers.CustomerID join Car on Car.CarID=Rental.CarID join 
                                Brand on Brand.BrandID=Car.BrandID join Payments on Payments.RentalID=Rental.RentalID
                                where Payments.StatusID=2 and Customers.NationalNumber like @NationalNumber 
                                order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNumber", NationalNumber + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment_DTO(reader.GetString("RentalCode"), reader.GetString("CustomerName"),
                                reader.GetString("NationalNumber"), reader.GetString("CarFullName"),
                                reader.GetString("LicensePlate"), reader.GetDateTime("PaymentDate"),
                                reader.GetDecimal("AmountPaid")));
                        }
                    }
                }
            }
            return payments;
        }
        public static decimal GetTotalRevenueByNationalNumber(string NationalNumber)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select sum(AmountPaid) from Payments join Rental on Payments.RentalID=Rental.RentalID
                                join Customers on Rental.CustomerID=Customers.CustomerID 
                                where StatusID=2 and Customers.NationalNumber like @NatNo;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NatNo", NationalNumber + "%");
                    object? sum = command.ExecuteScalar();
                    if (sum == DBNull.Value)
                        return 0;
                    return Convert.ToDecimal(sum);
                }
            }
        }
        public static List<Payment_DTO> FilterPaymentByMonth(int Month)
        {
            List<Payment_DTO> payments = new List<Payment_DTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Customers.FirstName+' '+Customers.LastName as CustomerName,
                                Customers.NationalNumber,Brand.BrandName+' '+Car.Model as CarFullName, Car.LicensePlate,
                                Payments.AmountPaid,Payments.PaymentDate from Rental join Customers on 
                                Rental.CustomerID=Customers.CustomerID join Car on Car.CarID=Rental.CarID join 
                                Brand on Brand.BrandID=Car.BrandID join Payments on Payments.RentalID=Rental.RentalID
                                where Payments.StatusID=2 and Month(Payments.PaymentDate)=@Month order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Month", Month);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment_DTO(reader.GetString("RentalCode"), reader.GetString("CustomerName"),
                                reader.GetString("NationalNumber"), reader.GetString("CarFullName"),
                                reader.GetString("LicensePlate"), reader.GetDateTime("PaymentDate"),
                                reader.GetDecimal("AmountPaid")));
                        }
                    }
                }
            }
            return payments;
        }
        public static decimal GetTotalRevenueByMonth(int Month)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select sum(AmountPaid) from Payments
                                where StatusID=2 and month(PaymentDate)=@Month;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Month", Month);
                    object? sum = command.ExecuteScalar();
                    if (sum == DBNull.Value)
                        return 0;
                    return Convert.ToDecimal(sum);
                }
            }
        }
        public static List<Payment_DTO> FilterPaymentsByPeriod(int Year, int Month)
        {
            List<Payment_DTO> payments = new List<Payment_DTO>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Rental.RentalCode,Customers.FirstName+' '+Customers.LastName as CustomerName,
                                Customers.NationalNumber,Brand.BrandName+' '+Car.Model as CarFullName, Car.LicensePlate,
                                Payments.AmountPaid,Payments.PaymentDate from Rental join Customers on 
                                Rental.CustomerID=Customers.CustomerID join Car on Car.CarID=Rental.CarID join 
                                Brand on Brand.BrandID=Car.BrandID join Payments on Payments.RentalID=Rental.RentalID
                                where Payments.StatusID=2 and Month(Payments.PaymentDate)=@Month and 
                                Year(Payments.PaymentDate)=@Year  order by Rental.RentalID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Year", Year);
                    command.Parameters.AddWithValue("@Month", Month);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            payments.Add(new Payment_DTO(reader.GetString("RentalCode"), reader.GetString("CustomerName"),
                                reader.GetString("NationalNumber"), reader.GetString("CarFullName"),
                                reader.GetString("LicensePlate"), reader.GetDateTime("PaymentDate"),
                                reader.GetDecimal("AmountPaid")));
                        }
                    }
                }
            }
            return payments;
        }
        public static List<int> GetPaymentYears()
        {
            List<int> Years = new List<int>();
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select distinct YEAR(PaymentDate) as PaymentYear from Payments where StatusID=2;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Years.Add(reader.GetInt32("PaymentYear"));
                        }
                    }
                }
            }
            return Years;
        }
    }
}