using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("ProductsIngredients")]
    public class ProductsIngredientsEntity : BaseEntity
    {
        public string Name { get; set; }
        public Guid ProductId { get; set; }
        public ProductsEntity Product { get; set; }
        public Guid IngredientId { get; set; }
        public IngredientsEntity Ingredient { get; set; }
        [Column(TypeName = "decimal(8,0)")]
        public decimal QuantityUsedInBase { get; set; }
    }
}
