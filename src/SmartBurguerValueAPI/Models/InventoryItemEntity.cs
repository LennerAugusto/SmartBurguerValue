using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Models
{
    public class InventoryItemEntity : BaseEntity
    {
        public string Name { get; set; }
        public string NameCategory { get; set; }
        public Guid UnityOfMensureId { get; set; }
        public Guid EnterpriseId { get; set; }
        public ICollection<PurchaseItemEntity> PurchaseItems { get; set; }
        public IngredientsEntity Ingredient { get; set; } 
    }
}
