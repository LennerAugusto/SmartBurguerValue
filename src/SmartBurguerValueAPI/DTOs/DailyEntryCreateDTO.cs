using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.DTOs
{
    public class DailyEntryCreateDTO
    {
        public Guid Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Description { get; set; }
        public Guid EnterpriseId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? DateUpdated { get; set; } = DateTime.UtcNow;

        public List<DailyEntryItemDTO> Items { get; set; } = new List<DailyEntryItemDTO>();
    }
}
