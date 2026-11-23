using ALAIN_DataAccess.Model;

namespace ALAIN_DataAccess.DTO
{
    public class RentalDetails_DTO
    {
        public RentalDetails_DTO()
        { }
        public RentalDetails_DTO(string rentalCode, byte status, string customerFullName, string customerPhone, 
            string customerNationalId, string carBrand, string carModel, string carLicensePlate, int startMileage, 
            int? endMileage, DateTime startDate, DateTime expectedEndDate, DateTime? actualEndDate, decimal totalAmount)
        {
            RentalCode = rentalCode;
            Status = (enRentalStatus)status;
            CustomerFullName = customerFullName;
            CustomerPhone = customerPhone;
            CustomerNationalId = customerNationalId;
            CarBrand = carBrand;
            CarModel = carModel;
            CarLicensePlate = carLicensePlate;
            StartMileage = startMileage;
            EndMileage = endMileage;
            StartDate = startDate;
            ExpectedEndDate = expectedEndDate;
            ActualEndDate = actualEndDate;
            TotalAmount = totalAmount;
        }
        public string RentalCode { get; set; } = "";
        public enRentalStatus Status { get; set; }
        public string CustomerFullName { get; set; } = "";
        public string CustomerPhone { get; set; } = "";
        public string CustomerNationalId { get; set; } = "";
        public string CarBrand { get; set; } = "";
        public string CarModel { get; set; } = "";
        public string CarLicensePlate { get; set; } = "";
        public int StartMileage { get; set; }
        public int? EndMileage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}