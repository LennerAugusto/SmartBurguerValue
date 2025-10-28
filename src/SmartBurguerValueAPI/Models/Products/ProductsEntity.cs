using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("Products")]
    public class ProductsEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SellingPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? DesiredMargin { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CMV { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CPV { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SuggestedPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Markup { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Margin { get; set; }
        public string? ProductType { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        [JsonIgnore]
        public ICollection<ProductsIngredientsEntity> ProductIngredients { get; set; }
        [JsonIgnore]
        public ICollection<ComboProductEntity> ComboProducts { get; set; }
        [JsonIgnore]
        public ICollection<DailyEntryItemEntity> DailyEntryItems { get; set; }
    }
}
