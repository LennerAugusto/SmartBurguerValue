using Microsoft.AspNetCore.Mvc;
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
        public record PeriodRange(DateTime Start, DateTime End);

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
        public static PeriodRange GetPeriod(EPeriod period)
        {
            var today = DateTime.Now.Date;

            return period switch
            {
                EPeriod.SinceTheBeginning => new PeriodRange(DateTime.MinValue, today),
                EPeriod.LastYear => new PeriodRange(today.AddYears(-1), today),
                EPeriod.LastSemester => new PeriodRange(today.AddMonths(-6), today),
                EPeriod.LastFourWeeks => new PeriodRange(today.AddDays(-30), today),
                EPeriod.LastWeek => new PeriodRange(today.AddDays(-7), today),
                _ => new PeriodRange(DateTime.MinValue, today)
            };
        }
        public static PeriodRange GetPreviousPeriod(EPeriod period)
        {
            var current = GetPeriod(period);
            var rangeLength = current.End - current.Start;
            return new PeriodRange(current.Start.AddDays(-rangeLength.Days), current.Start);
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
                .SumAsync(i => i != null ? i.TotalRevenue : 0);

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

        public async Task<List<InvoicingSeriesDTO>> GetInvoicingByPeriod(EPeriod period, Guid enterpriseId)
        {
            var range = GetPeriod(period);
            DateTime start = range.Start;
            DateTime end = range.End;

            string granularity = period switch
            {
                EPeriod.LastWeek => "day",
                EPeriod.LastFourWeeks => "day",
                EPeriod.LastSemester => "month",
                EPeriod.LastYear => "month",
                EPeriod.SinceTheBeginning => "total",
                _ => "total"
            };

            var query = _context.DailyEntry
                .Include(x => x.Items)
                .Where(x => x.EntryDate >= start && x.EntryDate <= end && x.EnterpriseId == enterpriseId);

            List<InvoicingSeriesDTO> series = new();
            if (granularity == "day")
            {
                for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
                {
                    var dailyItems = query
                        .Where(x => x.EntryDate.Date == date)
                        .SelectMany(x => x.Items.DefaultIfEmpty());

                    series.Add(new InvoicingSeriesDTO
                    {
                        Label = date.ToString("dd/MM"),
                        Invoicing = dailyItems.Sum(i => i != null ? i.TotalRevenue : 0),
                        NetValue = dailyItems.Sum(i => i != null ? i.TotalRevenue - i.TotalCPV : 0)
                    });
                }
            }
            else if (granularity == "month")
            {
                for (var date = new DateTime(start.Year, start.Month, 1); date <= end; date = date.AddMonths(1))
                {
                    var monthlyItems = query
                        .Where(x => x.EntryDate.Year == date.Year && x.EntryDate.Month == date.Month)
                        .SelectMany(x => x.Items.DefaultIfEmpty());

                    series.Add(new InvoicingSeriesDTO
                    {
                        Label = date.ToString("MMM"),
                        Invoicing = monthlyItems.Sum(i => i != null ? i.TotalRevenue : 0),
                        NetValue = monthlyItems.Sum(i => i != null ? i.TotalRevenue - i.TotalCPV : 0)
                    });
                }
            }
            else 
            {
                var allItems = query.SelectMany(x => x.Items.DefaultIfEmpty());
                series.Add(new InvoicingSeriesDTO
                {
                    Label = "Total",
                    Invoicing = allItems.Sum(i => i != null ? i.TotalRevenue : 0),
                    NetValue = allItems.Sum(i => i != null ? i.TotalRevenue - i.TotalCPV : 0)
                });
            }
            return series;
        }

    }
}
