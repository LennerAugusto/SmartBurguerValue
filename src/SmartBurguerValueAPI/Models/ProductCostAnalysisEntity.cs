using SmartBurguerValueAPI.Models.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models
{
    public class ProductCostAnalysisEntity : BaseEntity
    {
        public Guid ProductId { get; set; }
        public ProductsEntity Product { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public DateTime AnalisysDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? UnitCost { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SellingPrice { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? SellingPriceSuggested { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Markup{get;set;}
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Margin{get;set;}
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CPV{get;set;}
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CMV{get;set;}
    }
}
