using ALAIN_Business;
using ALAIN_DataAccess.Model;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RC_API.Global_Classes
{
    internal class Validation
    {
        public static bool ValidateEmail(string emailAddress)
        {
            string pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(emailAddress);
        }
        public static bool ValidateInteger(string Number)
        {
            string pattern = @"^[0-9]*$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }
        public static bool ValidateFloat(string Number)
        {
            string pattern = @"^[0-9]*(?:\.[0-9]*)?$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }
        public static bool IsNumber(string Number)
        {
            return (ValidateInteger(Number) || ValidateFloat(Number));
        }
        public static bool ValidateCar(ALAIN_DataAccess.Model.Car car)
        {
            return car != null && !string.IsNullOrWhiteSpace(car.Plate)
                && !string.IsNullOrWhiteSpace(car.BrandName) && !string.IsNullOrWhiteSpace(car.Model)
                && car.ManufactureYear > 1950 && car.ManufactureYear <= DateTime.Now.Year + 1
                && Enum.IsDefined(typeof(enFuelType), car.FuelType) && Enum.IsDefined(typeof(enTransmission), car.Transmission)
                && Enum.IsDefined(typeof(enBodyType), car.BodyType) && car.NumberOfSeats > 0 && car.NumberOfDoors > 0
                && car.Mileage >= 0 && Enum.IsDefined(typeof(enCarStatus), car.CarStatus) && car.DailyRentPrice > 0;
        }
        public static bool ValidateCustomer(ALAIN_DataAccess.Model.Customer customer)
        {
            return customer != null
                && !string.IsNullOrWhiteSpace(customer.NationalNo)
                && !string.IsNullOrWhiteSpace(customer.FirstName)
                && !string.IsNullOrWhiteSpace(customer.LastName)
                && !string.IsNullOrWhiteSpace(customer.Phone)
                && ValidateEmail(customer.Email)
                && customer.DateOfBirth < DateTime.Now.AddYears(-18)
                && customer.DateOfBirth > DateTime.Now.AddYears(-100)
                && !string.IsNullOrWhiteSpace(customer.Nationality);
        }
        public static bool ValidateRental(ALAIN_DataAccess.Model.Rental rental)
        {
            return rental.CustomerID > 0 && rental.CarID > 0 &&
                rental.StartDate != default && rental.ExpectedEndDate > rental.StartDate &&
                (rental.EndDate == null || rental.EndDate >= rental.StartDate) && rental.StartMileage >= 0
                && Enum.IsDefined(typeof(enRentalStatus), rental.Status) && rental.DailyPrice > 0;
        }
    }
}