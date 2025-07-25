namespace SmartBurguerValueAPI.DTOs.Products
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? DesiredMargin { get; set; } 
        public decimal? CPV { get; set; } 
        public decimal? CMV { get; set; } 
        public decimal? SuggestedPrice { get; set; }
        public string? ImageUrl { get; set; }
        public Guid EnterpriseId { get; set; }
        public List<ProductIngredientDTO> Ingredients { get; set; }
    }
}
