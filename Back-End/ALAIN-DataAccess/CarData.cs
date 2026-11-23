using ALAIN_DataAccess.DTO;
using ALAIN_DataAccess.Model;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.NetworkInformation;

namespace ALAIN_DataAccess
{
    public class CarData
    {
        public static List<Car_DTO> GetAllCars()
        {
            List<Car_DTO> cars = new List<Car_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Car.LicensePlate, Brand.BrandName as Brand, Car.Model, 
	                            BodyType.BodyTypeName as BodyType, Car.NumberOfSeats,
	                            Car.DailyRentalPrice, CarStatus.StatusName as Status
	                            from Car join Brand on Car.BrandID=Brand.BrandID join BodyType 
	                            on BodyType.BodyTypeID=Car.BodyTypeID join CarStatus 
	                            on CarStatus.CarStatusID=Car.CarStatusID;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car_DTO(reader.GetString("LicensePlate"), reader.GetString("Brand"),
                                reader.GetString("Model"), reader.GetString("BodyType"), reader.GetByte("NumberOfSeats"),
                                reader.GetDecimal("DailyRentalPrice"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return cars;
        }
        public static List<Car_DTO> FilterCarsByPlateNumber(string plateNumber)
        {
            List<Car_DTO> cars = new List<Car_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Car.LicensePlate, Brand.BrandName as Brand, Car.Model, 
	                            BodyType.BodyTypeName as BodyType, Car.NumberOfSeats,
	                            Car.DailyRentalPrice, CarStatus.StatusName as Status
	                            from Car join Brand on Car.BrandID=Brand.BrandID join BodyType 
	                            on BodyType.BodyTypeID=Car.BodyTypeID join CarStatus 
	                            on CarStatus.CarStatusID=Car.CarStatusID where LicensePlate like @Plate;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Plate", plateNumber + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car_DTO(reader.GetString("LicensePlate"), reader.GetString("Brand"),
                                reader.GetString("Model"), reader.GetString("BodyType"), reader.GetByte("NumberOfSeats"),
                                reader.GetDecimal("DailyRentalPrice"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return cars;
        }
        public static List<Car_DTO> FilterCarsByBrand(string Brand)
        {
            List<Car_DTO> cars = new List<Car_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Car.LicensePlate, Brand.BrandName as Brand, Car.Model, 
	                            BodyType.BodyTypeName as BodyType, Car.NumberOfSeats,
	                            Car.DailyRentalPrice, CarStatus.StatusName as Status
	                            from Car join Brand on Car.BrandID=Brand.BrandID join BodyType 
	                            on BodyType.BodyTypeID=Car.BodyTypeID join CarStatus 
	                            on CarStatus.CarStatusID=Car.CarStatusID where BrandName like @Brand;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Brand", Brand + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car_DTO(reader.GetString("LicensePlate"), reader.GetString("Brand"),
                                reader.GetString("Model"), reader.GetString("BodyType"), reader.GetByte("NumberOfSeats"),
                                reader.GetDecimal("DailyRentalPrice"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return cars;
        }
        public static List<Car_DTO> FilterCarsByModel(string Model)
        {
            List<Car_DTO> cars = new List<Car_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Car.LicensePlate, Brand.BrandName as Brand, Car.Model, 
	                            BodyType.BodyTypeName as BodyType, Car.NumberOfSeats,
	                            Car.DailyRentalPrice, CarStatus.StatusName as Status
	                            from Car join Brand on Car.BrandID=Brand.BrandID join BodyType 
	                            on BodyType.BodyTypeID=Car.BodyTypeID join CarStatus 
	                            on CarStatus.CarStatusID=Car.CarStatusID where Model like @Model;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Model", Model + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car_DTO(reader.GetString("LicensePlate"), reader.GetString("Brand"),
                                reader.GetString("Model"), reader.GetString("BodyType"), reader.GetByte("NumberOfSeats"),
                                reader.GetDecimal("DailyRentalPrice"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return cars;
        }
        public static List<Car_DTO> FilterCarsByBodyType(string BodyType)
        {
            List<Car_DTO> cars = new List<Car_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Car.LicensePlate, Brand.BrandName as Brand, Car.Model, 
	                            BodyType.BodyTypeName as BodyType, Car.NumberOfSeats,
	                            Car.DailyRentalPrice, CarStatus.StatusName as Status
	                            from Car join Brand on Car.BrandID=Brand.BrandID join BodyType 
	                            on BodyType.BodyTypeID=Car.BodyTypeID join CarStatus 
	                            on CarStatus.CarStatusID=Car.CarStatusID where BodyTypeName=@Body;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Body", BodyType);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car_DTO(reader.GetString("LicensePlate"), reader.GetString("Brand"),
                                reader.GetString("Model"), reader.GetString("BodyType"), reader.GetByte("NumberOfSeats"),
                                reader.GetDecimal("DailyRentalPrice"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return cars;
        }
        public static List<Car_DTO> FilterCarsByStatus(string Status)
        {
            List<Car_DTO> cars = new List<Car_DTO>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Car.LicensePlate, Brand.BrandName as Brand, Car.Model, 
	                            BodyType.BodyTypeName as BodyType, Car.NumberOfSeats,
	                            Car.DailyRentalPrice, CarStatus.StatusName as Status
	                            from Car join Brand on Car.BrandID=Brand.BrandID join BodyType 
	                            on BodyType.BodyTypeID=Car.BodyTypeID join CarStatus 
	                            on CarStatus.CarStatusID=Car.CarStatusID where StatusName=@Status;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", Status);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car_DTO(reader.GetString("LicensePlate"), reader.GetString("Brand"),
                                reader.GetString("Model"), reader.GetString("BodyType"), reader.GetByte("NumberOfSeats"),
                                reader.GetDecimal("DailyRentalPrice"), reader.GetString("Status")));
                        }
                    }
                }
            }
            return cars;
        }
        public static int CountCarsByStatus(string Status)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select count(*) from Car where CarStatusID=
                        (select CarStatusID from CarStatus where StatusName=@Status);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", Status);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static int CountAllCars()
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"select count(*) from Car where Car.CarStatusID in(1,2,3);", connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
        public static Dictionary<string, int> GetCarStatusCounts()
        {
            Dictionary<string, int> carStatusCounts = new Dictionary<string, int>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select CarStatus.StatusName,count(Car.CarID) as StatusCount 
                                from CarStatus left join Car on Car.CarStatusID=CarStatus.CarStatusID 
                                group by CarStatus.StatusName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carStatusCounts.Add(reader.GetString("StatusName"), reader.GetInt32("StatusCount"));
                        }
                    }
                }
            }
            return carStatusCounts;
        }
        public static Dictionary<string, int> GetCarStatusCounts(string BodyType)
        {
            Dictionary<string, int> carStatusCounts = new Dictionary<string, int>();

            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select CarStatus.StatusName,count(Car.CarID) as StatusCount 
                        from CarStatus left join Car on Car.CarStatusID=CarStatus.CarStatusID 
                        where Car.BodyTypeID=(select BodyTypeID from BodyType where BodyTypeName=@BodyType) 
                        group by CarStatus.StatusName;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BodyType", BodyType);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carStatusCounts.Add(reader.GetString("StatusName"), reader.GetInt32("StatusCount"));
                        }
                    }
                }
            }
            return carStatusCounts;
        }
        public static Model.Car? GetCarInfoByCarID(int carID)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Car.CarID, Car.LicensePlate,Brand.BrandName as Brand, 
                                    Car.Model, Car.ManufactureYear, Car.FuelTypeID, Car.Transmission, 
                                    Car.BodyTypeID, Car.NumberOfSeats, Car.NumberOfDoors,Car.Mileage, 
                                    Car.CarStatusID, Car.DailyRentalPrice from 
                                    Car join Brand on Car.BrandID=Brand.BrandID where Car.CarID=@carID;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@carID", carID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Model.Car(reader.GetInt32("CarID"), reader.GetString("LicensePlate"),
                                reader.GetString("Brand"), reader.GetString("Model"),
                                reader.GetInt16("ManufactureYear"), reader.GetByte("FuelTypeID"),
                                reader.GetByte("Transmission"), reader.GetByte("BodyTypeID"), reader.GetByte("NumberOfSeats")
                                , reader.GetByte("NumberOfDoors"), reader.GetInt32("Mileage"), reader.GetByte("CarStatusID"),
                                reader.GetDecimal("DailyRentalPrice"));
                        }
                        return null;
                    }
                }
            }
        }
        public static Model.Car? GetCarInfoByPlateNumber(string PlateNumber)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"select Car.CarID, Car.LicensePlate,Brand.BrandName as Brand, 
                                    Car.Model, Car.ManufactureYear, Car.FuelTypeID, Car.Transmission, 
                                    Car.BodyTypeID, Car.NumberOfSeats, Car.NumberOfDoors,Car.Mileage, 
                                    Car.CarStatusID, Car.DailyRentalPrice from 
                                    Car join Brand on Car.BrandID=Brand.BrandID where Car.LicensePlate=@Plate;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Plate", PlateNumber);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Model.Car(reader.GetInt32("CarID"), reader.GetString("LicensePlate"),
                                reader.GetString("Brand"), reader.GetString("Model"),
                                reader.GetInt16("ManufactureYear"), reader.GetByte("FuelTypeID"),
                                reader.GetByte("Transmission"), reader.GetByte("BodyTypeID"), reader.GetByte("NumberOfSeats")
                                , reader.GetByte("NumberOfDoors"), reader.GetInt32("Mileage"), reader.GetByte("CarStatusID"),
                                reader.GetDecimal("DailyRentalPrice"));
                        }
                        return null;
                    }
                }
            }
        }
        public static int DeleteCar(string PlateNumber)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"delete from Car where LicensePlate=@Plate;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Plate", PlateNumber);
                    if (command.ExecuteNonQuery() > 0)
                    {
                        command.CommandText = @"select count(*) from Car;";
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    return -1;
                }
            }
        }
        public static int? AddNewCar(Car car)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"insert into Car(LicensePlate,BrandID,Model,ManufactureYear,FuelTypeID,Transmission,
                                BodyTypeID,NumberOfSeats,NumberOfDoors,Mileage,CarStatusID,DailyRentalPrice)values
                                (@Plate,(select BrandID from Brand where BrandName=@Brand),@Model,@Year,@Fuel,@Transmission,
                                @Body,@Seats,@Doors,@Mileage,@Status,@Price); select SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Plate", car.Plate);
                    command.Parameters.AddWithValue("@Brand", car.BrandName);
                    command.Parameters.AddWithValue("@Model", car.Model);
                    command.Parameters.AddWithValue("@Year", car.ManufactureYear);
                    command.Parameters.AddWithValue("@Fuel", (byte)car.FuelType);
                    command.Parameters.AddWithValue("@Transmission", (byte)car.Transmission);
                    command.Parameters.AddWithValue("@Body", (byte)car.BodyType);
                    command.Parameters.AddWithValue("@Seats", car.NumberOfSeats);
                    command.Parameters.AddWithValue("@Doors", car.NumberOfDoors);
                    command.Parameters.AddWithValue("@Mileage", car.Mileage);
                    command.Parameters.AddWithValue("@Status", (byte)car.CarStatus);
                    command.Parameters.AddWithValue("@Price", car.DailyRentPrice);

                    int? Id = Convert.ToInt32(command.ExecuteScalar());
                    if (Id != null && Id > 0)
                    {
                        car.Id = Id.Value;
                        return Id;
                    }
                    return null;
                }
            }
        }
        public static bool EditCar(Car car)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = @"update Car set LicensePlate=@Plate,BrandID=(select BrandID from Brand where BrandName=@Brand),
                            Model=@Model,ManufactureYear=@Year,FuelTypeID=@Fuel,Transmission=@Transmission,
                            BodyTypeID=@Body,NumberOfSeats=@Seats,NumberOfDoors=@Doors,Mileage=@Mileage,
                            CarStatusID=@Status,DailyRentalPrice=@Price where CarID=@Id;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", car.Id);
                    command.Parameters.AddWithValue("@Plate", car.Plate);
                    command.Parameters.AddWithValue("@Brand", car.BrandName);
                    command.Parameters.AddWithValue("@Model", car.Model);
                    command.Parameters.AddWithValue("@Year", car.ManufactureYear);
                    command.Parameters.AddWithValue("@Fuel", (byte)car.FuelType);
                    command.Parameters.AddWithValue("@Transmission", (byte)car.Transmission);
                    command.Parameters.AddWithValue("@Body", (byte)car.BodyType);
                    command.Parameters.AddWithValue("@Seats", car.NumberOfSeats);
                    command.Parameters.AddWithValue("@Doors", car.NumberOfDoors);
                    command.Parameters.AddWithValue("@Mileage", car.Mileage);
                    command.Parameters.AddWithValue("@Status", (byte)car.CarStatus);
                    command.Parameters.AddWithValue("@Price", car.DailyRentPrice);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public static bool CheckIfPlateAlreadyExist(string PlateNumber)
        {
            using (SqlConnection connection = new SqlConnection(DataAccessSettings.connectionString))
            {
                connection.Open();
                string query = "select iif(exists(select 1 from Car where LicensePlate=@Plate),1,0);";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Plate", PlateNumber);
                    return Convert.ToBoolean(command.ExecuteScalar());
                }
            }
        }
    }
}