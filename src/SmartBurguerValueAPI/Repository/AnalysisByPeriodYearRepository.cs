using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Analysis;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Repository.Base;
using System.Globalization;

namespace SmartBurguerValueAPI.Repository
{
    public class AnalysisByPeriodYearRepository : IAnalysisByPeriodYearsRepository
    {
        private readonly AppDbContext _context;

        public AnalysisByPeriodYearRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<RevenueComparisonDTO> GetRevenueComparisonAsync(PeriodYears period, Guid enterpriseId)
        {
            int yearsToCompare = (int)period + 1;
            if (yearsToCompare <= 0)
                yearsToCompare = 1;

            int currentYear = DateTime.Now.Year;
            int startYear = currentYear - (yearsToCompare - 1);

            var data = await _context.DailyEntry
                .Where(s => s.EnterpriseId == enterpriseId &&
                            s.EntryDate.Year >= startYear &&
                            s.EntryDate.Year <= currentYear)
                .Include(s => s.Items)
                .GroupBy(s => new { s.EntryDate.Year, s.EntryDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(s => s.Items.Sum(i => (decimal?)i.TotalRevenue) ?? 0m)
                })
                .ToListAsync();
            var lookup = data.ToLookup(x => x.Year);
            var series = Enumerable.Range(startYear, yearsToCompare)
                .Select(year => new RevenueSeriesDTO
                {
                    Name = year.ToString(),
                    Number = Enumerable.Range(1, 12)
                             .Select(month => lookup[year]
                                 .FirstOrDefault(x => x.Month == month)?.Revenue ?? 0m)
                             .ToList()
                })
                .OrderBy(s => s.Name)
                .ToList();

            return new RevenueComparisonDTO
            {
                Series = series
            };
        }
        public async Task<RevenueComparisonDTO> GetOrdersComparisonAsync(PeriodYears period, Guid enterpriseId)
        {
            int yearsToCompare = (int)period + 1;
            if (yearsToCompare <= 0)
                yearsToCompare = 1;

            int currentYear = DateTime.Now.Year;
            int startYear = currentYear - (yearsToCompare - 1);

            var data = await _context.DailyEntry
                .Where(s => s.EnterpriseId == enterpriseId &&
                            s.EntryDate.Year >= startYear &&
                            s.EntryDate.Year <= currentYear)
                .GroupBy(s => new { s.EntryDate.Year, s.EntryDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(s => s.TotalOrders ?? 0m)
                })
                .ToListAsync();
            var lookup = data.ToLookup(x => x.Year);
            var series = Enumerable.Range(startYear, yearsToCompare)
                .Select(year => new RevenueSeriesDTO
                {
                    Name = year.ToString(),
                    Number = Enumerable.Range(1, 12)
                             .Select(month => lookup[year]
                                 .FirstOrDefault(x => x.Month == month)?.Revenue ?? 0m)
                             .ToList()
                })
                .OrderBy(s => s.Name)
                .ToList();

            return new RevenueComparisonDTO
            {
                Series = series
            };
        }

    }
}
