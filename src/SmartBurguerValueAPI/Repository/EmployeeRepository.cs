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
                MonthlySalary = dto.MonthlySalary,
                EnterpriseId = dto.EnterpriseId,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            };

            await _context.Employees.AddAsync(employee);

            if (dto.EmploymentType == "Daily")
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
        }
        public async Task <List<EmployeeDTO>> GetAllEmployeeByEnterpriseId(Guid enterpriseId)
        {
            var query = _context.Set<EmployeeEntity>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .AsNoTracking()
                .Select(x => new EmployeeDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Position = x.Position,
                    EmploymentType = x.EmployeeType,
                    MonthlySalary = x.MonthlySalary,
                    EnterpriseId = x.EnterpriseId,
                    IsActive = x.IsActive,
                    WorkSchedules = x.EmployeeSchedules.Select(ws => new WorkScheduleDTO
                      {
                          Weekday = ws.WeekDay,
                          DailyRate = ws.DailyRate
                      }).ToList()
                });

            return await query.ToListAsync();
        }
        public async Task UpdateEmployee(EmployeeDTO dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == dto.Id);

            if (employee == null)
                throw new Exception("Funcionário não encontrado.");

            employee.Name = dto.Name;
            employee.Position = dto.Position;
            employee.EmployeeType = dto.EmploymentType;
            employee.MonthlySalary = dto.MonthlySalary;
            employee.DateUpdated = DateTime.UtcNow;

            if (dto.EmploymentType == "Daily")
            {
                    var oldSchedules = await _context.EmployeesWorkSchedule
                    .Where(ws => ws.EmployeeId == employee.Id)
                    .ToListAsync();

                _context.EmployeesWorkSchedule.RemoveRange(oldSchedules);

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
            else
            {
                var oldSchedules = await _context.EmployeesWorkSchedule
                    .Where(ws => ws.EmployeeId == employee.Id)
                    .ToListAsync();

                _context.EmployeesWorkSchedule.RemoveRange(oldSchedules);
            }

            await _context.SaveChangesAsync();
        }
    }
}
