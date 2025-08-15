namespace SmartBurguerValueAPI.DTOs
{
    public class DailyEntryDTO : BaseDTO
    {
        public DateTime EntryDate { get; set; }
        public decimal? Revenue { get; set; }
        public string? Description { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? TotalItems { get; set; }
        public decimal? Liquid { get; set; }
        public Guid EnterpriseId { get; set; }
        public List<DailyEntryItemDTO> Items { get; set; } = new List<DailyEntryItemDTO>();
    }
}
