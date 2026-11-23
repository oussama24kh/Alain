using Microsoft.AspNetCore.Authorization;

namespace RC_API.Attributes
{
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        public AdminOnlyAttribute()
        {
            Roles = "Administrateur";
        }
    }
}