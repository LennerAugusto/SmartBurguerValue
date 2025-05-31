using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class EmployeeWorkScheduleRepository : RepositoryBase<EmployeeWorkScheduleEntity>, IEmployeeWorkScheduleRepository
    {
        public EmployeeWorkScheduleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
