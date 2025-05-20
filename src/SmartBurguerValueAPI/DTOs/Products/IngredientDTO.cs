using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.DTOs.Products
{
    public class IngredientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal PurchaseQuantity { get; set; }
        public Guid UnitOfMeasureId { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid EnterpriseId { get; set; }
    
    }
}
