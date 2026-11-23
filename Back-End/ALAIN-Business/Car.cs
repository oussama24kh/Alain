using ALAIN_DataAccess;
using ALAIN_DataAccess.DTO;

namespace ALAIN_Business
{
    public class Car
    {
        public static List<Car_DTO> GetAllCars()
        {
            return CarData.GetAllCars();
        }
        public static List<Car_DTO> FilterCarsByBodyType(string BodyType)
        {
            return CarData.FilterCarsByBodyType(BodyType);
        }
        public static List<Car_DTO> FilterCarsByPlateNumber(string Plate)
        {
            return CarData.FilterCarsByPlateNumber(Plate);
        }
        public static List<Car_DTO> FilterCarsByBrand(string Brand)
        {
            return CarData.FilterCarsByBrand(Brand);
        }
        public static List<Car_DTO> FilterCarsByModel(string Model)
        {
            return CarData.FilterCarsByModel(Model);
        }
        public static List<Car_DTO> FilterCarsByStatus(string Status)
        {
            return CarData.FilterCarsByStatus(Status);
        }
        public static int CountCarsByStatus(string Status)
        {
            return CarData.CountCarsByStatus(Status);
        }
        public static int CountAllCars()
        {
            return CarData.CountAllCars();
        }
        public static Dictionary<string,int> GetCarStatusCounts()
        {
            return CarData.GetCarStatusCounts();
        }
        public static Dictionary<string, int> GetCarStatusCounts(string BodyType)
        {
            return CarData.GetCarStatusCounts(BodyType);
        }
        public static ALAIN_DataAccess.Model.Car? GetCarInfoByPlateNumber(string PlateNumber)
        {
            return CarData.GetCarInfoByPlateNumber(PlateNumber);
        }
        public static ALAIN_DataAccess.Model.Car? GetCarInfoByCarID(int CarID)
        {
            return CarData.GetCarInfoByCarID(CarID);
        }
        public static int DeleteCar(string PlateNumber)
        {
            return CarData.DeleteCar(PlateNumber);
        }
        public static int? AddNewCar(ALAIN_DataAccess.Model.Car car)
        {
            return CarData.AddNewCar(car);
        }
        public static bool EditCar(ALAIN_DataAccess.Model.Car car)
        {
            return CarData.EditCar(car);
        }
        public static bool CheckIfPlateAlreadyExist(string PlateNumber)
        {
            return CarData.CheckIfPlateAlreadyExist(PlateNumber);
        }
    }
}
