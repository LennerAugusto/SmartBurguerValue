using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models
{
    public class DailyEntryEntity : BaseEntity
    {
        public DateTime EntryDate { get; set; }
        public string? Description { get; set; }
        public int? TotalOrders { get; set; }
        public Guid EnterpriseId { get; set; }
        [JsonIgnore]
        public ICollection<DailyEntryItemEntity> Items { get; set; }
        [JsonIgnore]
        public EnterpriseEntity Enterprise { get; set; }
    }
}
