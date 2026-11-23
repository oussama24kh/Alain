namespace project_03.DTO
{
    public class Payment_DTO
    {
        public Payment_DTO()
        { }
        public string RentalCode { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string NationalNo { get; set; } = "";
        public string CarFullName { get; set; } = "";
        public string CarPlate { get; set; } = "";
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}