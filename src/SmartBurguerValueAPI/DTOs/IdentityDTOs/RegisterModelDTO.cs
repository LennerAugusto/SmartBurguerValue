using System.ComponentModel.DataAnnotations;

namespace SmartBurguerValueAPI.DTOs.IdentityDTOs
{
    public class RegisterModelDTO
    {

        [Required(ErrorMessage = "User name is requeired")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "User password is requeired")]
        public string? UserPassword { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Email is requeired")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? EnterpriseId { get; set; }
    }
}
