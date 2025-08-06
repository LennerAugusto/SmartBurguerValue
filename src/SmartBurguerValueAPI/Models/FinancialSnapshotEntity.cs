using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models
{
    public class FinancialSnapshotEntity : BaseEntity
    {
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public DateTime SnapshotDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalRevenue {get;set;}
        [Column(TypeName = "decimal(10,2)")]
        public decimal? TotalCost{get;set;}
        [Column(TypeName = "decimal(10,2)")]
        public decimal? GrossProfit {get;set;}
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Markup{get;set;}
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Margin{get;set;}
        [Column(TypeName = "decimal(10,2)")]
        public decimal? CPV{get;set;}
    }
}
