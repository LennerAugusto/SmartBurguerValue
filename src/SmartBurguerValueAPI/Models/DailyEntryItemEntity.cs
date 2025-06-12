using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Models
{
    public class DailyEntryItemEntity : BaseEntity
    {
        public Guid DailyEntryId { get; set; }
        public DailyEntryEntity DailyEntry { get; set; }
        public Guid? ProductId { get; set; }
        public ProductsEntity Product { get; set; }
        public Guid? ComboId { get; set; }
        public ComboEntity Combo { get; set; }
        public int Quantity { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? CPV { get; set; }
        public decimal? TotalRevenue { get; set; }
        public decimal? TotalCPV { get; set; }
    }
}
