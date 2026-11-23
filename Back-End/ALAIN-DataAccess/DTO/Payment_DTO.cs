namespace ALAIN_DataAccess.DTO
{
    public class Payment_DTO
    {
        public Payment_DTO()
        { }
        public Payment_DTO(string rentalCode, string customerName, string nationalNo,
            string carFullName, string carPlate, DateTime paymentDate, decimal amount)
        {
            RentalCode = rentalCode;
            CustomerName = customerName;
            NationalNo = nationalNo;
            CarFullName = carFullName;
            CarPlate = carPlate;
            PaymentDate = paymentDate;
            Amount = amount;
        }

        public string RentalCode { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string NationalNo { get; set; } = "";
        public string CarFullName { get; set; } = "";
        public string CarPlate { get; set; } = "";
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}