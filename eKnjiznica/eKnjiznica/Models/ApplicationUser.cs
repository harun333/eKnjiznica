using Microsoft.AspNetCore.Identity;

namespace eKnjiznica.Models
{
    public class ApplicationUser : IdentityUser
    {
    
        public int Credit { get; set; }
    }
}