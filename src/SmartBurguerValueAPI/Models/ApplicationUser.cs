using Microsoft.AspNetCore.Identity;

namespace SmartBurguerValueAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExiryTime { get; set; }
    }
}
