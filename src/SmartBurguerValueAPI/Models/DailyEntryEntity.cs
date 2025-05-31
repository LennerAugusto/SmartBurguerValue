namespace SmartBurguerValueAPI.Models
{
    public class DailyEntryEntity : BaseEntity
    {
        public DateTime EntryDate { get; set; }
        public string Revenue { get; set; }
        public string? Description { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
    }
}
