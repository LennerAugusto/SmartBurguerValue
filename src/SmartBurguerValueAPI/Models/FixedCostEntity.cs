using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models
{
    public class FixedCostEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Value { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
    }
}
