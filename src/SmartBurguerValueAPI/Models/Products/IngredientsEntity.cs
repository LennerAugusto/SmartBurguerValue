using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("Ingredients")]
    public class IngredientsEntity : BaseEntity
    {
        public string Name { get; set; }
 
        public Guid UnitOfMeasureId { get; set; }
        public Guid InventoryItemId { get; set; }
        [JsonIgnore]
        public InventoryItemEntity InventoryItem { get; set; }
        [JsonIgnore]
        public UnityTypesProductsEntity UnitOfMeasure { get; set; }
        public Guid EnterpriseId { get; set; }
        [JsonIgnore]
        public EnterpriseEntity Enterprise { get; set; }
        [JsonIgnore]
        public ICollection<ProductsIngredientsEntity> ProductIngredients { get; set; }
    }
}
