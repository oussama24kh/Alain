namespace ALAIN_DataAccess.DTO
{
    public class User_DTO
    {
        public User_DTO() { }
        public User_DTO(string UserName, string FirstName, string LastName, string Email, string Role,bool isActive) 
        {
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Role = Role;
            this.IsActive = isActive;
        }
        public User_DTO(int Id, string UserName, string FirstName, string LastName, string Email,
                        string Role, string Password, bool isActive)
        {
            this.Id = Id;
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Role = Role;
            this.Password = Password;
            IsActive = isActive;
        }
        public User_DTO(int Id, string UserName, string FirstName, string LastName, string Email,
                        string Role, bool isActive)
        {
            this.Id = Id;
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Role = Role;
            this.Password = Password;
            IsActive = isActive;
        }
        public int Id { get; set; }
        public string UserName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsActive { get; set; }
        public string RefreshTokenHash { get; set; } = "";
        public DateTime RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
    }
}