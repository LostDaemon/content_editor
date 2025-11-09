using Microsoft.AspNetCore.Identity;

namespace UniversalContentEditor.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
    }
}
