using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models
{
    [Table("Users")]
    public class UsersEntity :BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
    }
}
