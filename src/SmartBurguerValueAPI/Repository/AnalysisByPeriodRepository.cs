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

            //var totalExpansesEmployee = await _context.Employees
            //    .SumAsync(i => i != null ? i.MonthlySalary : 0);

            var totalOrders = await query.CountAsync();
            var averageTicket = totalOrders > 0 ? totalSales / totalOrders : 0;
            return new InitialAnalysiDTO
            {
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                AverageTicket = Math.Round((decimal)averageTicket, 2),
            };
        }

        public async Task<List<BestSellingProductsDTO>> GetCardsBestSellingProductsByEnterpriseId(EPeriod period, Guid enterpriseId)
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
            DateTime end = range.End.Date.AddDays(1).AddTicks(-1);

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
        public async Task<List<TotalOrdersDTO>> GetTotalOrdersByPeriod(EPeriod period, Guid enterpriseId)
        {
            var range = GetPeriod(period);
            DateTime start = range.Start;
            DateTime end = range.End.Date.AddDays(1).AddTicks(-1);

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

            List<TotalOrdersDTO> series = new();

            if (granularity == "day")
            {
                var dailyTotals = await query
                    .GroupBy(x => x.EntryDate.Date)
                    .Select(g => new { Date = g.Key, Orders = g.Sum(i => i.TotalOrders) })
                    .ToListAsync();

                for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
                {
                    var total = dailyTotals.FirstOrDefault(x => x.Date == date)?.Orders ?? 0;
                    series.Add(new TotalOrdersDTO
                    {
                        Label = date.ToString("dd/MM"),
                        Orders = total
                    });
                }
            }
            else if (granularity == "month")
            {
                var monthlyTotals = await query
                    .GroupBy(x => new { x.EntryDate.Year, x.EntryDate.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        Orders = g.Sum(i => i.TotalOrders)
                    })
                    .ToListAsync();

                for (var date = new DateTime(start.Year, start.Month, 1); date <= end; date = date.AddMonths(1))
                {
                    var total = monthlyTotals
                        .FirstOrDefault(x => x.Year == date.Year && x.Month == date.Month)?.Orders ?? 0;

                    series.Add(new TotalOrdersDTO
                    {
                        Label = date.ToString("MMM"),
                        Orders = total
                    });
                }
            }
            else
            {
                var totalOrders = await query.SumAsync(i => i.TotalOrders);
                series.Add(new TotalOrdersDTO
                {
                    Label = "Total",
                    Orders = totalOrders
                });
            }

            return series;
        }
        public async Task<GetAnalysisCardsProductsDTO> GetMarginAndProfitProductByPeriod(EPeriod Period, Guid enterpriseId)
        {

            DateTime startDate = SelectPeriod(Period);
            DateTime endDate = DateTime.UtcNow;

            var query = _context.DailyEntry
                .Include(x => x.Items)
                .Where(x => x.EntryDate >= startDate && x.EntryDate <= endDate 
                      && x.EnterpriseId == enterpriseId);

            var totalRevenue = await query
                .SelectMany(x => x.Items.Where(i => i.ComboId == null))
                .SumAsync(i => (decimal?)i.TotalRevenue ?? 0);
            var totalProfit = await query
                .SelectMany(x => x.Items.Where(i => i.ComboId == null))
                .SumAsync(i => (decimal?)(i.TotalRevenue - i.TotalCPV) ?? 0);

            var profitMargin = totalRevenue > 0 ? (totalProfit / totalRevenue) * 100 : 0;
            return new GetAnalysisCardsProductsDTO
            {
                Profit = Math.Round(totalProfit, 2),
                Margin = Math.Round(profitMargin, 2)
            };
        }
        public async Task<GetAnalysisCardsProductsDTO> GetMarginAndProfitComboByPeriod(EPeriod Period, Guid enterpriseId)
        {

            DateTime startDate = SelectPeriod(Period);
            DateTime endDate = DateTime.UtcNow;
            var query = _context.DailyEntry
                .Include(x => x.Items)
                .Where(x => x.EntryDate >= startDate && x.EntryDate <= endDate
                      && x.EnterpriseId == enterpriseId);

            var totalRevenue = await query
                .SelectMany(x => x.Items.Where(i => i.ComboId != null))
                .SumAsync(i => (decimal?)i.TotalRevenue ?? 0);
            var totalProfit = await query
                .SelectMany(x => x.Items.Where(i => i.ComboId != null))
                .SumAsync(i => (decimal?)(i.TotalRevenue - i.TotalCPV) ?? 0);
            var profitMargin = totalRevenue > 0 ? (totalProfit / totalRevenue) * 100 : 0;
            return new GetAnalysisCardsProductsDTO
            {
                Profit = Math.Round(totalProfit, 2),
                Margin = Math.Round(profitMargin, 2)
            };
        }
        public async Task<List<BestSellingProductsByPeriodDTO>> GetBestSellingCombosByPeriod(EPeriod period, Guid enterpriseId)
        {
            var range = GetPeriod(period);
            DateTime start = range.Start;
            DateTime end = range.End.Date.AddDays(1).AddTicks(-1);

            string granularity = period switch
            {
                EPeriod.LastWeek => "day",
                EPeriod.LastFourWeeks => "day",
                EPeriod.LastSemester => "month",
                EPeriod.LastYear => "month",
                EPeriod.SinceTheBeginning => "total",
                _ => "total"
            };

            var baseQuery = _context.DailyEntry
                .Where(d => d.EntryDate >= start && d.EntryDate <= end && d.EnterpriseId == enterpriseId);

            var result = new List<BestSellingProductsByPeriodDTO>();

            if (granularity == "day")
            {
                var dailyCombos = await baseQuery
                    .SelectMany(d => d.Items
                        .Where(i => i.ComboId != null) 
                        .Select(i => new
                        {
                            Date = d.EntryDate.Date,
                            ComboId = i.ComboId,
                            ComboName = i.Combo != null ? i.Combo.Name : null,
                            Quantity = i.Quantity 
                        }))
                    .GroupBy(x => new { x.Date, x.ComboId, x.ComboName })
                    .Select(g => new
                    {
                        g.Key.Date,
                        g.Key.ComboId,
                        g.Key.ComboName,
                        Quantity = g.Sum(x => (int?)x.Quantity) ?? 0
                    })
                    .ToListAsync();

                for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
                {
                    var top = dailyCombos
                        .Where(x => x.Date == date)
                        .OrderByDescending(x => x.Quantity)
                        .FirstOrDefault();

                    result.Add(new BestSellingProductsByPeriodDTO
                    {
                        Label = date.ToString("dd/MM"),
                        NameProduct = top?.ComboName ?? "",
                        Quantity = top?.Quantity ?? 0
                    });
                }
            }
            else if (granularity == "month")
            {
                var monthlyCombos = await baseQuery
                    .SelectMany(d => d.Items
                        .Where(i => i.ComboId != null)
                        .Select(i => new
                        {
                            Year = d.EntryDate.Year,
                            Month = d.EntryDate.Month,
                            ComboId = i.ComboId,
                            ComboName = i.Combo != null ? i.Combo.Name : null,
                            Quantity = i.Quantity
                        }))
                    .GroupBy(x => new { x.Year, x.Month, x.ComboId, x.ComboName })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        g.Key.ComboId,
                        g.Key.ComboName,
                        Quantity = g.Sum(x => (int?)x.Quantity) ?? 0
                    })
                    .ToListAsync();

                for (var date = new DateTime(start.Year, start.Month, 1); date <= end; date = date.AddMonths(1))
                {
                    var top = monthlyCombos
                        .Where(x => x.Year == date.Year && x.Month == date.Month)
                        .OrderByDescending(x => x.Quantity)
                        .FirstOrDefault();

                    result.Add(new BestSellingProductsByPeriodDTO
                    {
                        Label = date.ToString("MMM"),
                       NameProduct = top?.ComboName ?? "",
                        Quantity = top?.Quantity ?? 0
                    });
                }
            }
            else 
            {
                var totalCombo = await baseQuery
                    .SelectMany(d => d.Items
                        .Where(i => i.ComboId != null)
                        .Select(i => new
                        {
                            ComboId = i.ComboId,
                            ComboName = i.Combo != null ? i.Combo.Name : null,
                            Quantity = i.Quantity
                        }))
                    .GroupBy(x => new { x.ComboId, x.ComboName })
                    .Select(g => new
                    {
                        g.Key.ComboId,
                        g.Key.ComboName,
                        Quantity = g.Sum(x => (int?)x.Quantity) ?? 0
                    })
                    .OrderByDescending(x => x.Quantity)
                    .FirstOrDefaultAsync();

                result.Add(new BestSellingProductsByPeriodDTO
                {
                    Label = "Total",
                    NameProduct = totalCombo?.ComboName ?? "",
                    Quantity = totalCombo?.Quantity ?? 0
                });
            }

            return result;
        }
        public async Task<List<BestSellingProductsByPeriodDTO>> GetBestSellingProductsByPeriod(EPeriod period, Guid enterpriseId)
        {
            var range = GetPeriod(period);
            DateTime start = range.Start;
            DateTime end = range.End.Date.AddDays(1).AddTicks(-1);

            string granularity = period switch
            {
                EPeriod.LastWeek => "day",
                EPeriod.LastFourWeeks => "day",
                EPeriod.LastSemester => "month",
                EPeriod.LastYear => "month",
                EPeriod.SinceTheBeginning => "total",
                _ => "total"
            };

            var baseQuery = _context.DailyEntry
                .Where(d => d.EntryDate >= start && d.EntryDate <= end && d.EnterpriseId == enterpriseId);

            var result = new List<BestSellingProductsByPeriodDTO>();

            if (granularity == "day")
            {
                var dailyCombos = await baseQuery
                    .SelectMany(d => d.Items
                        .Where(i => i.ComboId == null)
                        .Select(i => new
                        {
                            Date = d.EntryDate.Date,
                            ProductId = i.ProductId,
                            ProductName = i.Product != null ? i.Product.Name : null,
                            Quantity = i.Quantity
                        }))
                    .GroupBy(x => new { x.Date, x.ProductId, x.ProductName })
                    .Select(g => new
                    {
                        g.Key.Date,
                        g.Key.ProductId,
                        g.Key.ProductName,
                        Quantity = g.Sum(x => (int?)x.Quantity) ?? 0
                    })
                    .ToListAsync();

                for (var date = start.Date; date <= end.Date; date = date.AddDays(1))
                {
                    var top = dailyCombos
                        .Where(x => x.Date == date)
                        .OrderByDescending(x => x.Quantity)
                        .FirstOrDefault();

                    result.Add(new BestSellingProductsByPeriodDTO
                    {
                        Label = date.ToString("dd/MM"),
                        NameProduct = top?.ProductName ?? "",
                        Quantity = top?.Quantity ?? 0
                    });
                }
            }
            else if (granularity == "month")
            {
                var monthlyCombos = await baseQuery
                    .SelectMany(d => d.Items
                        .Where(i => i.ComboId == null)
                        .Select(i => new
                        {
                            Year = d.EntryDate.Year,
                            Month = d.EntryDate.Month,
                            ProductId = i.ProductId,
                            ProductName = i.Product != null ? i.Product.Name : null,
                            Quantity = i.Quantity
                        }))
                    .GroupBy(x => new { x.Year, x.Month, x.ProductId, x.ProductName })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        g.Key.ProductId,
                        g.Key.ProductName,
                        Quantity = g.Sum(x => (int?)x.Quantity) ?? 0
                    })
                    .ToListAsync();

                for (var date = new DateTime(start.Year, start.Month, 1); date <= end; date = date.AddMonths(1))
                {
                    var top = monthlyCombos
                        .Where(x => x.Year == date.Year && x.Month == date.Month)
                        .OrderByDescending(x => x.Quantity)
                        .FirstOrDefault();

                    result.Add(new BestSellingProductsByPeriodDTO
                    {
                        Label = date.ToString("MMM"),
                        NameProduct = top?.ProductName ?? "",
                        Quantity = top?.Quantity ?? 0
                    });
                }
            }
            else
            {
                var totalCombo = await baseQuery
                    .SelectMany(d => d.Items
                        .Where(i => i.ComboId == null)
                        .Select(i => new
                        {
                            ProductId = i.ProductId,
                            ProductName = i.Product != null ? i.Product.Name : null,
                            Quantity = i.Quantity
                        }))
                    .GroupBy(x => new { x.ProductId, x.ProductName })
                    .Select(g => new
                    {
                        g.Key.ProductId,
                        g.Key.ProductName,
                        Quantity = g.Sum(x => (int?)x.Quantity) ?? 0
                    })
                    .OrderByDescending(x => x.Quantity)
                    .FirstOrDefaultAsync();

                result.Add(new BestSellingProductsByPeriodDTO
                {
                    Label = "Total",
                    NameProduct = totalCombo?.ProductName ?? "",
                    Quantity = totalCombo?.Quantity ?? 0
                });
            }

            return result;
        }
        public async Task<List<ProductsBestMarginDTO>> GetProductsBestMargin(Guid enterpriseId)
        {
            var query = await _context.Products
                .Where(d => d.EnterpriseId == enterpriseId)
                .OrderByDescending(d => d.Margin)
                .Take(5)
                .Select(d => new ProductsBestMarginDTO
                {
                    Name = d.Name,
                    Margin = d.Margin
                })
                .ToListAsync();

            return query;
        }
        public async Task<List<ProductsBestMarginDTO>> GetCombosBestMargin(Guid enterpriseId)
        {
            var query = await _context.Combos
                .Where(d => d.EnterpriseId == enterpriseId)
                .OrderByDescending(d => d.Margin)
                .Take(5)
                .Select(d => new ProductsBestMarginDTO
                {
                    Name = d.Name,
                    Margin = d.Margin
                })
                .ToListAsync();

            return query;
        }
    }
}
