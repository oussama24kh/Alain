namespace ALAIN_DataAccess.Model
{
    public class Payment
    {
        public Payment()
        { }
        public Payment(int PaymentID, int RentalID, decimal AmountPaid, string PaymentStatus, decimal TotalAmount)
        {
            this.PaymentID = PaymentID;
            this.RentalID = RentalID;
            this.AmountPaid = AmountPaid;
            this.PaymentStatus = PaymentStatus;
            this.TotalAmount = TotalAmount;
        }
        public int PaymentID { get; set; }
        public int RentalID { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; } = "";
        public decimal TotalAmount { get; set; }
    }
}