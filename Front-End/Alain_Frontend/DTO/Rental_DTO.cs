namespace project_03.DTO
{
    public class Rental_DTO
    {
        public string RentalCode { get; set; } = "";
        public string CarFullName { get; set; } = "";
        public string CarPlate { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string NationalNo { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "";
        public string RentalPeriodText { get; set; } = "";
        public int DaysCount { get; set; }
    }
}