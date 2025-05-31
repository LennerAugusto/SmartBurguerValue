namespace SmartBurguerValueAPI.Models
{
    public class EmployeeWorkScheduleEntity : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public string WeekDay { get; set; }
        public decimal DailyRate { get; set; }
        public EmployeeEntity Employee { get; set; }
    }
}
