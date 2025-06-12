namespace SmartBurguerValueAPI.DTOs.Products
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal SellingPrice { get; set; }
        public string? ImageUrl { get; set; }
        public Guid EnterpriseId { get; set; }
        public List<ProductIngredientDTO> Ingredients { get; set; }
    }
}
