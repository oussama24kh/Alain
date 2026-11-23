namespace ALAIN_DataAccess.Model
{
    public class Customer
    {
        public Customer()
        { }
        public Customer(int Id, string NationalNo, string FirstName, string LastName, string Phone,
            string Email, string? Address, DateTime DateOfBirth, string Nationality,int TotalRentals)
        {
            this.Id = Id;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Phone = Phone;
            this.Email = Email;
            this.DateOfBirth = DateOfBirth;
            this.Nationality = Nationality;
            this.Address = Address ?? "";
            this.TotalRentals = TotalRentals;
        }
        public int Id { get; set; }
        public string NationalNo { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName { get { return FirstName + " " + LastName; }  }
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; } = "";
        public int TotalRentals { get; set; }
    }
}