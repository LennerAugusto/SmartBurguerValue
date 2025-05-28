using System.ComponentModel.DataAnnotations;

namespace SmartBurguerValueAPI.DTOs.IdentityDTOs
{
    public class LoginModelDTO
    {
        [Required(ErrorMessage = "User name is requeired")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "User password is requeired")]
        public string? UserPassword { get; set; }
    }
}
