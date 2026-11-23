using project_03.Model;

namespace project_03.DTO
{
    public class RentalDetails_DTO
    {
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