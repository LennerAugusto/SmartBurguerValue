using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("ProductsIngredients")]
    public class ProductsIngredientsEntity : BaseEntity
    {
        public Guid ProductId { get; set; }
        public ProductsEntity Product { get; set; }
        public Guid IngredientId { get; set; }
        public IngredientsEntity Ingredient { get; set; }
        public decimal QuantityUsedInBase { get; set; }
    }
}
