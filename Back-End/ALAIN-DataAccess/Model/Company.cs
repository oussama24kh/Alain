namespace ALAIN_DataAccess.Model
{
    public class Company
    {
        public Company() { }
        public Company(int Id, string Name, string Email, string Phone, string Address, string City)
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.Phone = Phone;
            this.Address = Address;
            this.City = City;
        }
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
    }
}