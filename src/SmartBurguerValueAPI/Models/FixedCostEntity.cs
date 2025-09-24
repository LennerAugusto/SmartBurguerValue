using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models
{
    public class FixedCostEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Value { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool IsPaid { get; set; }
        public Guid EnterpriseId { get; set; }
        [JsonIgnore]
        public EnterpriseEntity Enterprise { get; set; }
    }
}
