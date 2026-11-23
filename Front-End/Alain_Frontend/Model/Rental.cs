namespace project_03.Model
{
    public class Rental
    {
        public Rental()
        { }
        public Rental(DateTime StartDate, DateTime ExpectedEndDate, enRentalStatus Status)
        {
            this.StartDate = StartDate;
            this.ExpectedEndDate = ExpectedEndDate;
            this.Status = Status;
        }
        public int RentalID { get; set; }
        public string? RentalCode { get; set; }
        public int CustomerID { get; set; }
        public int CarID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StartMileage { get; set; }
        public int? EndMileage { get; set; }
        public enRentalStatus Status { get; set; }
        public decimal DailyPrice { get; set; }
        public string? Notes { get; set; }
    }
}