using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("Ingredients")]
    public class IngredientsEntity : BaseEntity
    {
        public string Name { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public Guid UnitOfMeasureId { get; set; }
        [JsonIgnore]
        public UnityTypesProductsEntity UnitOfMeasure { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid EnterpriseId { get; set; }
        [JsonIgnore]
        public EnterpriseEntity Enterprise { get; set; }
        [JsonIgnore]
        public ICollection<ProductsIngredientsEntity> ProductIngredients { get; set; }
    }
}
