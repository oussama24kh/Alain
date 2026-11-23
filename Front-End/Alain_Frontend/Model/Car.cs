namespace project_03.Model
{
    public class Car
    {
        public int Id { get; set; }
        public string Plate { get; set; } = "";
        public string BrandName { get; set; } = "";
        public string Model { get; set; } = "";
        public short ManufactureYear { get; set; }
        public enFuelType FuelType { get; set; }
        public enTransmission Transmission { get; set; }
        public enBodyType BodyType { get; set; }
        public byte NumberOfSeats { get; set; }
        public byte NumberOfDoors { get; set; }
        public int Mileage { get; set; }
        public enCarStatus CarStatus { get; set; }
        public decimal DailyRentPrice { get; set; }
    }
}