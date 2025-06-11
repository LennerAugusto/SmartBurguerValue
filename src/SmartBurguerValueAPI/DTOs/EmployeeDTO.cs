namespace SmartBurguerValueAPI.DTOs
{
    public class EmployeeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string? EmploymentType { get; set; } // "monthly" ou "daily"
        public decimal? MonthlySalary { get; set; }
        public Guid EnterpriseId { get; set; }

        public List<WorkScheduleDTO> WorkSchedules { get; set; } = new();
    }
}
