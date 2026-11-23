namespace project_03.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string NationalNo { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; } = "";
        public int TotalRentals { get; set; }
    }
}
