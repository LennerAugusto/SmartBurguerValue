using SmartBurguerValueAPI.Models.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models
{
    public class PurchaseItemEntity : BaseEntity
    {
        public string NameItem { get; set; }
        public Guid PurchaseId { get; set; }
        public PurchaseEntity Purchase { get; set; }

        public Guid InventoryItemId { get; set; }
        public InventoryItemEntity InventoryItem { get; set; }
        public Guid UnityOfMensureId { get; set;}
        public UnityTypesProductsEntity UnityOfMensure { get; set; }
        [Column(TypeName = "decimal(18,0)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}
