using Microsoft.AspNetCore.Identity;

namespace WebApplication2.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add any additional fields you want to store for all users
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
        // Navigation properties to link with profile tables
        public virtual JobSeeker? JobSeeker { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
