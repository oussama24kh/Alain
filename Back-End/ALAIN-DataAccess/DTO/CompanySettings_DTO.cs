namespace ALAIN_DataAccess.DTO
{
    public class CompanySettings_DTO
    {
        public int Id { get; set; } = 1;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}