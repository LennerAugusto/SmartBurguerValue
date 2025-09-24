using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models
{
    public class PurchaseEntity : BaseEntity
    {

        public string SupplierName { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public ICollection<PurchaseItemEntity> Items { get; set; } = new List<PurchaseItemEntity>();
    }
}
