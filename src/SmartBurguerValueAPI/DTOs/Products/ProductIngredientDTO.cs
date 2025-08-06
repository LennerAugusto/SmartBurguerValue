namespace SmartBurguerValueAPI.DTOs.Products
{
    public class ProductIngredientDTO
    {
        public string Name { get; set; }
        public string BaseUnit { get; set; }
        public Guid Id { get; set; }
        public decimal Quantity { get; set; } 
    }
}
