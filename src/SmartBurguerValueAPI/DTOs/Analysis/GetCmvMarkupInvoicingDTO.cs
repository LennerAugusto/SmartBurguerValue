namespace SmartBurguerValueAPI.DTOs.Analysis
{
    public class GetCmvMarkupInvoicingDTO
    {
        public decimal Cmv { get; set; }
        public decimal Markup { get; set; }
        public decimal Margin { get; set; }
        public decimal Profit { get; set; }
    }
}
