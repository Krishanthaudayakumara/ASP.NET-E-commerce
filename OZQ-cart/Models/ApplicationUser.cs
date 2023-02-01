using Microsoft.AspNetCore.Identity;

namespace OZQ_cart.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
    }
}
