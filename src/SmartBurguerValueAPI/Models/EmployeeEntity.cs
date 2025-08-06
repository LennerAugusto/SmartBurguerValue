using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models
{
    public class EmployeeEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Position { get; set; }
        public string EmployeeType { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal? MonthlySalary { get; set; }
        public Guid EnterpriseId { get; set; }
        public string? UserId { get; set; }

        public ICollection<EmployeeWorkScheduleEntity> EmployeeSchedules { get; set; }
    }
}
