namespace ALAIN_DataAccess.DTO
{
    public class Car_DTO
    {
        public Car_DTO(string Plate, string Brand, string Model, string BodyType,
            byte NumberOfSeats, decimal DailyRentPrice, string Status)
        {
            this.Plate = Plate;
            this.Brand = Brand;
            this.Model = Model;
            this.BodyType = BodyType;
            this.NumberOfSeats = NumberOfSeats;
            this.DailyRentPrice = DailyRentPrice;
            this.Status = Status;
        }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
        public string BodyType { get; set; }
        public byte NumberOfSeats { get; set; }
        public string Status { get; set; }
        public decimal DailyRentPrice { get; set; }
    }
}
