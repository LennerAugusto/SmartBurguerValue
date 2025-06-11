using SmartBurguerValueAPI.Models.Products;

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
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
