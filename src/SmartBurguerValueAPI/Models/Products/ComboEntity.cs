using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("Combos")]
    public class ComboEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? ProductType { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SellingPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SugestedPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CPV { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CMV { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Markup { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Margin { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? DesiredMargin { get; set; }
        public Guid EnterpriseId { get; set; }
        [JsonIgnore]
        public EnterpriseEntity Enterprise { get; set; }
        public ICollection<ComboProductEntity> Products { get; set; }
    }
}
