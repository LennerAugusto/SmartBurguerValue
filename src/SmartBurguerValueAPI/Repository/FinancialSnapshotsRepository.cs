using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class FinancialSnapshotsRepository : RepositoryBase<FinancialSnapshotEntity>, IFinancialSnapshotsRepository
    {
        public FinancialSnapshotsRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<FinancialSnapshotEntity> GetAnalyse(DateTime date, Guid enterpriseId)
        {
            var entries = await _context.DailyEntry
            .Where(e => e.EntryDate == date && e.EnterpriseId == enterpriseId && e.IsActive)
            .Include(e => e.Items)
            .ToListAsync();

            var revenue = entries.SelectMany(e => e.Items).Sum(i => i.TotalRevenue);
            var cpv = entries.SelectMany(e => e.Items).Sum(i => i.TotalCPV);

            var fixedCosts = await _context.FixedCosts
                .Where(f => f.EnterpriseId == enterpriseId && f.IsActive)
                .SumAsync(f => f.Value);

            var employeeCost = await CalcularCustoFuncionarios(date, enterpriseId);

            var totalCost = cpv + fixedCosts + employeeCost;
            var grossProfit = revenue - totalCost;
            var markup = totalCost > 0 ? grossProfit / totalCost : 0;
            var margin = revenue > 0 ? grossProfit / revenue : 0;

            var snapshot = new FinancialSnapshotEntity
            {
                Id = Guid.NewGuid(),
                EnterpriseId = enterpriseId,
                SnapshotDate = date,
                TotalRevenue = revenue,
                TotalCost = totalCost,
                GrossProfit = grossProfit,
                Markup = markup,
                Margin = margin,
                CPV = cpv,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            };

            await _context.FinancialSnapshots.AddAsync(snapshot);
            await _context.SaveChangesAsync();

            return snapshot;
        }
        private async Task<decimal> CalcularCustoFuncionarios(DateTime data, Guid enterpriseId)
        {
            var employees = await _context.Employees
                .Where(e => e.EnterpriseId == enterpriseId && e.IsActive)
                .Include(e => e.EmployeeSchedules)
                .ToListAsync();

            decimal? total = 0;
            var diaSemana = data.DayOfWeek.ToString();

            foreach (var e in employees)
            {
                if (e.EmployeeType.ToLower() == "monthly")
                {
                    total += e.MonthlySalary / 30;
                }
                else
                {
                    var escala = e.EmployeeSchedules.FirstOrDefault(s => s.WeekDay == diaSemana);
                    if (escala != null) total += escala.DailyRate;
                }
            }

            return (decimal)total;
        }
    }
}

