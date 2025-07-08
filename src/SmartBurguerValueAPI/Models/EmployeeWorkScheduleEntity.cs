using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models
{
    public class EmployeeWorkScheduleEntity : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public string WeekDay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyRate { get; set; }
        public EmployeeEntity Employee { get; set; }
    }
}
