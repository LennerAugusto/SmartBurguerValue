using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.DTOs.Products
{
    public class IngredientDTO : BaseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? PurchaseQuantity { get; set; }
        public Guid UnitOfMeasureId { get; set; }
        public Guid? InventoryItemId { get; set; }
        public string? UnitOfMeasureName { get; set; }
        public decimal? ConversionFactor { get; set; }
        public decimal? PurchasePrice { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public Guid EnterpriseId { get; set; }
    
    }
}
