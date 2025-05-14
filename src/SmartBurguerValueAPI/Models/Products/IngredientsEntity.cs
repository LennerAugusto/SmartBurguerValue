using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("Ingredients")]
    public class IngredientsEntity : BaseEntity
    {
        public string Name { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public Guid UnitOfMeasureId { get; set; }
        public UnityTypesProductsEntity UnitOfMeasure { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public ICollection<ProductsIngredientsEntity> ProductIngredients { get; set; }
    }
}
