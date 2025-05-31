namespace SmartBurguerValueAPI.DTOs
{
    public class DailyEntryDTO
    {
        public DateTime EntryDate { get; set; }
        public string Revenue { get; set; }
        public string? Description { get; set; }
        public Guid EnterpriseId { get; set; }
    }
}
