using System.Net.NetworkInformation;

namespace ALAIN_DataAccess.DTO
{
    public class Rental_DTO
    {
        public Rental_DTO()
        { }
        public Rental_DTO(string RentalCode, string CarFullName, string CarPlate, string CustomerName,
                            string NationalNo, DateTime StartDate, DateTime EndDate, string Status)
        {
            this.RentalCode = RentalCode;
            this.CarFullName = CarFullName;
            this.CarPlate = CarPlate;
            this.CustomerName = CustomerName;
            this.NationalNo = NationalNo;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.Status = Status;
        }
        public string RentalCode { get; set; } = "";
        public string CarFullName { get; set; } = "";
        public string CarPlate { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string NationalNo { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "";
        public string RentalPeriodText { get { return $"{StartDate:dd/MM/yyyy} - {EndDate:dd/MM/yyyy}"; } }
        public int DaysCount { get { return (EndDate - StartDate).Days; } }

    }
}