namespace SmartBurguerValueAPI.DTOs
{
    public class EmployeeDTO : BaseDTO
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string? EmploymentType { get; set; } 
        public decimal? MonthlySalary { get; set; }
        public Guid EnterpriseId { get; set; }
        public DateTime? HiringDate { get; set; }

        public List<WorkScheduleDTO> WorkSchedules { get; set; } = new();
    }
}
