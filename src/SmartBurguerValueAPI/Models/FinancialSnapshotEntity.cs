namespace SmartBurguerValueAPI.Models
{
    public class FinancialSnapshotEntity : BaseEntity
    {
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        public DateTime SnapshotDate { get; set; }
        public decimal? TotalRevenue {get;set;}
        public decimal? TotalCost{get;set;}
        public decimal? GrossProfit {get;set;}
        public decimal? Markup{get;set;}
        public decimal? Margin{get;set;}
        public decimal? CPV{get;set;}
    }
}
