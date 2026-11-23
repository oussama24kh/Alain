using ALAIN_DataAccess;

namespace ALAIN_Business
{
    public class Rental
    {
        public static List<ALAIN_DataAccess.DTO.Rental_DTO> GetAllRentals()
        {
            return RentalData.GetAllRentals();
        }
        public static List<ALAIN_DataAccess.DTO.Rental_DTO> FilterRentalsByRentalStatus(string RentalStatus)
        {
            return RentalData.FilterRentalsByRentalStatus(RentalStatus);
        }
        public static List<ALAIN_DataAccess.DTO.Rental_DTO> FilterRentalsByRentalCode(string RentalCode)
        {
            return RentalData.FilterRentalsByRentalCode(RentalCode);
        }
        public static List<ALAIN_DataAccess.DTO.Rental_DTO> FilterRentalsByPlateNumber(string PlateNumber)
        {
            return RentalData.FilterRentalsByPlateNumber(PlateNumber);
        }
        public static List<ALAIN_DataAccess.DTO.Rental_DTO> FilterRentalsByNationalNumber(string NationalNumber)
        {
            return RentalData.FilterRentalsByNationalNumber(NationalNumber);
        }
        public static List<ALAIN_DataAccess.DTO.Rental_DTO> GetLatestRentals(int NumberOfRentals)
        {
            return RentalData.GetLatestRentals(NumberOfRentals);
        }
        public static int CountRentalsByStatus(string Status)
        {
            return RentalData.CountRentalsByStatus(Status);
        }
        public static int CountRentalsByStatus(string Status, int Year, int Month)
        {
            return RentalData.CountRentalsByStatus(Status, Year, Month);
        }
        public static int CountCompletedRentals(int Year, int Month)
        {
            return RentalData.CountCompletedRentals(Year, Month);
        }
        public static int CountActiveRentals()
        {
            return RentalData.CountActiveRentals();
        }
        public static ALAIN_DataAccess.Model.Rental? GetRentalInfoByRentalID(int RentalID)
        {
            return RentalData.GetRentalInfoByRentalID(RentalID);
        }
        public static ALAIN_DataAccess.Model.Rental? GetRentalInfoByRentalCode(string RentalCode)
        {
            return RentalData.GetRentalInfoByRentalCode(RentalCode);
        }
        public static ALAIN_DataAccess.Model.Rental? AddNewRental(ALAIN_DataAccess.Model.Rental rental)
        {
            return RentalData.AddNewRental(rental);
        }
        public static bool EditRental(ALAIN_DataAccess.Model.Rental rental)
        {
            return RentalData.EditRental(rental);
        }
        public static ALAIN_DataAccess.DTO.RentalDetails_DTO? GetRentalDetailsByRentalCode(string RentalCode)
        {
            return RentalData.GetRentalDetailsByRentalCode(RentalCode);
        }
        public static int CountOverdueRentals()
        {
            return RentalData.CountOverdueRentals();
        }
        public static int GetRentalDuration(string RentalCode)
        {
            return RentalData.GetRentalDuration(RentalCode);
        }
    }
}