using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("Combos")]
    public class ComboEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public ICollection<ComboProduct> ComboProducts { get; set; }
    }
}
