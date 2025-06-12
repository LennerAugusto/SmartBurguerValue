using System.Security.Cryptography.X509Certificates;

namespace SmartBurguerValueAPI.DTOs
{
    public class DailyEntryItemDTO
    {
        public Guid Id { get; set; }
        public Guid DailyEntryId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ComboId { get; set; }
        public int Quantity { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal CPV { get; set; }
        public decimal TotalRevenue{ get; set; }
        public decimal TotalCPV { get; set; }
        
    }
}
