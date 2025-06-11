using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class EmployeeRepository : RepositoryBase<EmployeeEntity>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }
        public async Task CreateEmployee(EmployeeDTO dto)
        {
            var employee = new EmployeeEntity
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Position = dto.Position,
                EmployeeType = dto.EmploymentType,
                MonthlySalary = dto.EmploymentType == "monthly" ? dto.MonthlySalary : null,
                EnterpriseId = dto.EnterpriseId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            };

            await _context.Employees.AddAsync(employee);

            if (dto.EmploymentType == "daily")
            {
                foreach (var ws in dto.WorkSchedules)
                {
                    var schedule = new EmployeeWorkScheduleEntity
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = employee.Id,
                        WeekDay = ws.Weekday,
                        DailyRate = ws.DailyRate,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                        IsActive = true
                    };
                    await _context.EmployeesWorkSchedule.AddAsync(schedule);
                }
            }

             _context.SaveChangesAsync();
        }
        public async Task<PagedList<EmployeeDTO>> GetAllEmployeeByEnterpriseId(PaginationParamiters paramiters, Guid enterpriseId)
        {
            var query = _context.Set<EmployeeDTO>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .AsNoTracking()
                .Select(x => new EmployeeDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Position = x.Position,
                    EmploymentType = x.EmploymentType,
                    MonthlySalary = x.MonthlySalary,
                    EnterpriseId = x.EnterpriseId
                });

            return PagedList<EmployeeDTO>.ToPagedList(query, paramiters.PageNumber, paramiters.PageSize);
        }
        public async Task EmployeeUpdate(EmployeeDTO dto)
        {
            var employee = await _context.Employees
           .Include(e => e.EmployeeSchedules)
           .FirstOrDefaultAsync(e => e.Id == dto.Id);

            if (employee == null)
                 throw new Exception($"Employee not found: {dto.Id}"); ;

            employee.Name = dto.Name;
            employee.Position = dto.Position;
            employee.EmployeeType = dto.EmploymentType;
            employee.MonthlySalary = dto.EmploymentType == "monthly" ? dto.MonthlySalary : null;
            employee.DateUpdated = DateTime.UtcNow;

            if (employee.EmployeeType == "daily")
            {
                var existing = await _context.EmployeesWorkSchedule
                    .Where(e => e.EmployeeId == employee.Id)
                    .ToListAsync();

                _context.EmployeesWorkSchedule.RemoveRange(existing);

                foreach (var ws in dto.WorkSchedules)
                {
                    var schedule = new EmployeeWorkScheduleEntity
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = employee.Id,
                        WeekDay = ws.Weekday,
                        DailyRate = ws.DailyRate,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                        IsActive = true
                    };
                    await _context.AddAsync(schedule);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
