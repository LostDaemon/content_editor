using Microsoft.AspNetCore.Identity;

namespace Editor.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
    }
}
