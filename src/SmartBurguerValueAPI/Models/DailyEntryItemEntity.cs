using SmartBurguerValueAPI.Models.Products;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SellingPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CPV { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalRevenue { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCPV { get; set; }
    }
}
