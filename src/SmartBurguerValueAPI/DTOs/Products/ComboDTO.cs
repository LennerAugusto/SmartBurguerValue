namespace SmartBurguerValueAPI.DTOs.Products
{
    public class ComboDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? ProductType { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? SugestedPrice { get; set; }
        public decimal? CPV { get; set; }
        public decimal? CMV { get; set; }
        public decimal? Markup { get; set; }
        public decimal? Margin { get; set; }
        public decimal? DesiredMargin { get; set; }
        public Guid EnterpriseId { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
