namespace SmartBurguerValueAPI.Models
{
    public class SalesGoalEntity : BaseEntity
    {
        public string Description { get; set; }
        public string GoalValue { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public Guid EnterpriseId { get; set; }
    }
}
