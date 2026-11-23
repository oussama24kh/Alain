namespace ALAIN_DataAccess.Model
{
    public class Rental
    {
        public Rental()
        { }
        public Rental(int RentalID, string RentalCode, int CustomerID, int CarID, DateTime StartDate, DateTime ExpectedEndDate,
            DateTime? EndDate, int StartMileage, int? EndMileage, byte Status, decimal DailyPrice, string? Notes)
        {
            this.RentalID = RentalID;
            this.RentalCode = RentalCode;
            this.CustomerID = CustomerID;
            this.CarID = CarID;
            this.StartDate = StartDate;
            this.ExpectedEndDate = ExpectedEndDate;
            this.EndDate = EndDate;
            this.StartMileage = StartMileage;
            this.EndMileage = EndMileage;
            this.DailyPrice = DailyPrice;
            this.Notes = Notes;

            if (EndDate == null && ExpectedEndDate.Date < DateTime.Today)
                this.Status = enRentalStatus.EN_RETARD;
            else
                this.Status = (enRentalStatus)Status;
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