namespace ALAIN_DataAccess.Model
{
    public class Car
    {
        public Car()
        {

        }
        public Car(int Id, string Plate, string BrandName, string Model, short ManufactureYear,
            byte FuelType, byte Transmission, byte BodyType, byte NumberOfSeats, byte NumberOfDoors, int Mileage,
            byte CarStatus, decimal DailyRentPrice)
        {
            this.Id = Id;
            this.Plate = Plate;
            this.BrandName = BrandName;
            this.Model = Model;
            this.ManufactureYear = ManufactureYear;
            this.FuelType = (enFuelType)FuelType;
            this.Transmission = (enTransmission)Transmission;
            this.BodyType = (enBodyType)BodyType;
            this.NumberOfSeats = NumberOfSeats;
            this.NumberOfDoors = NumberOfDoors;
            this.Mileage = Mileage;
            this.CarStatus = (enCarStatus)CarStatus;
            this.DailyRentPrice = DailyRentPrice;
        }
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