namespace SmartBurguerValueAPI.DTOs.Analysis
{
    public class BestSellingProductsDTO
    {
        public Guid? ProductId { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? ImageUrl { get; set; }
    }
}
