using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Models
{
    public class ProductCostAnalysisEntity : BaseEntity
    {
        public Guid ProductId { get; set; }
        public ProductsEntity Product { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public DateTime AnalisysDate { get; set; }
        public decimal UnitCost { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal SellingPriceSuggested { get; set; }
        public decimal Markup{get;set;}
        public decimal Margin{get;set;}
        public decimal CPV{get;set;}
    }
}
