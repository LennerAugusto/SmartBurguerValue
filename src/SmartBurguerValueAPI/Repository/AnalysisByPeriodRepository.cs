using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Analysis;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class AnalysisByPeriodRepository : IAnalysisByPeriodRepository
    {
        private readonly AppDbContext _context;

        public AnalysisByPeriodRepository(AppDbContext context)
        {
            _context = context;
        }

        private DateTime SelectPeriod(EPeriod period)
        {
            DateTime startDate = period switch
            {
                EPeriod.SinceTheBeginning => DateTime.MinValue,
                EPeriod.LastYear => DateTime.Now.AddYears(-1),
                EPeriod.LastSemester => DateTime.Now.AddMonths(-6),
                EPeriod.LastFourWeeks => DateTime.Now.AddDays(-30),
                EPeriod.LastWeek => DateTime.Now.AddDays(-7),
                _ => DateTime.MinValue
            };
            return startDate;
        }
        public async Task<InitialAnalysiDTO> GetInitialAnalysisByPeriod(EPeriod period, Guid enterpriseId)
        {

            DateTime startDate = SelectPeriod(period);
            DateTime endDate = DateTime.UtcNow;

            var query = _context.DailyEntry
                .Include(x => x.Items)
                .Where(x => x.EntryDate >= startDate && x.EntryDate <= endDate && x.EnterpriseId == enterpriseId);

            var totalSales = await query
                .SelectMany(x => x.Items.DefaultIfEmpty())
                .SumAsync(i => i != null ? i.SellingPrice : 0);
            var totalExpanses = await query
                .SelectMany(x => x.Items.DefaultIfEmpty())
                .SumAsync(i => i != null ? i.TotalCPV : 0);
            var totalOrders = await query.CountAsync();

            return new InitialAnalysiDTO
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                TotalExpanses = totalExpanses,
            };
        }

        public async Task<List<BestSellingProductsDTO>> GetBestSellingProductsByEnterpriseId(EPeriod period, Guid enterpriseId)
        {
            DateTime startDate = SelectPeriod(period);
            DateTime endDate = DateTime.UtcNow;

            var query = _context.DailyEntry
                .Include(x => x.Items)
                .Where(x => x.EntryDate >= startDate
                         && x.EntryDate <= endDate
                         && x.EnterpriseId == enterpriseId);

            var result = await query
                .SelectMany(x => x.Items)
                .GroupBy(i => i.ProductId)
                .Select(g => new BestSellingProductsDTO
                {
                    ProductId = g.Key,
                    Name = g.FirstOrDefault().Product.Name,
                    Quantity = g.Sum(i => i.Quantity),
                    SellingPrice = g.FirstOrDefault().SellingPrice,
                    ImageUrl = g.FirstOrDefault().Product.ImageUrl
                })
                .Where(x => x.ProductId != null)
                .OrderByDescending(x => x.Quantity) 
                .ToListAsync();

            return result;
        }

    }
}
