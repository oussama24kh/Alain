namespace project_03.Model
{
    public class Payment
    {
        public Payment()
        { }
        public int PaymentID { get; set; }
        public int RentalID { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentStatus { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
    }
}