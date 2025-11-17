using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
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

            var totalOrders = await query.SumAsync(x => x.TotalOrders);
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
                    .SelectMany(d => d.Items)
                        .Where(i => i.ComboId != null)
                        .Select(i => new
                        {
                            Date = i.DailyEntry.EntryDate.Date,
                            ComboId = i.ComboId,
                            ComboName = i.Combo.Name,
                            Quantity = i.Quantity
                        })
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
    .SelectMany(d => d.Items)
    .Where(i => i.ComboId != null)
    .Select(i => new
    {
        Year = i.DailyEntry.EntryDate.Year,
        Month = i.DailyEntry.EntryDate.Month,
        ComboId = i.ComboId,
        ComboName = i.Combo.Name,
        Quantity = i.Quantity
    })
    .GroupBy(x => new { x.Year, x.Month, x.ComboId, x.ComboName })
    .Select(g => new
    {
        g.Key.Year,
        g.Key.Month,
        g.Key.ComboId,
        g.Key.ComboName,
        Quantity = g.Sum(x => x.Quantity)
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
                    .SelectMany(d => d.Items)
                        .Where(i => i.ComboId != null)
                        .Select(i => new
                        {
                            ComboId = i.ComboId,
                            ComboName = i.Combo != null ? i.Combo.Name : null,
                            Quantity = i.Quantity
                        })
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
                    .SelectMany(d => d.Items)
                        .Where(i => i.ComboId == null)
                        .Select(i => new
                        {
                            Date = i.DailyEntry.EntryDate.Date,
                            ProductId = i.ProductId,
                            ProductName = i.Product != null ? i.Product.Name : null,
                            Quantity = i.Quantity
                        })
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
     .Select(d => new { d.EntryDate, Items = d.Items })   
     .SelectMany(x => x.Items, (x, i) => new { x.EntryDate, Item = i })
     .Where(x => x.Item.ComboId == null)
     .GroupBy(x => new
     {
         x.EntryDate.Year,
         x.EntryDate.Month,
         x.Item.ProductId,
         ProductName = x.Item.Product != null ? x.Item.Product.Name : null
     })
     .Select(g => new
     {
         Year = g.Key.Year,
         Month = g.Key.Month,
         ProductId = g.Key.ProductId,
         ProductName = g.Key.ProductName,
         Quantity = g.Sum(x => (int?)x.Item.Quantity) ?? 0
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
                    .SelectMany(d => d.Items)
                        .Where(i => i.ComboId == null)
                        .Select(i => new
                        {
                            ProductId = i.ProductId,
                            ProductName = i.Product != null ? i.Product.Name : null,
                            Quantity = i.Quantity
                        })
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
        public async Task<List<SalesDistributionDTO>> GetSalesDistributionByPeriod(EPeriod period, Guid enterpriseId)
        {
            DateTime startDate = SelectPeriod(period);
            DateTime endDate = DateTime.UtcNow;

            var query = _context.DailyEntry
                .Include(x => x.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.ComboProducts)
                .Where(x => x.EntryDate >= startDate &&
                            x.EntryDate <= endDate &&
                            x.EnterpriseId == enterpriseId);

            var entries = await query.ToListAsync();

            var allItems = entries.SelectMany(e => e.Items).ToList();

            if (!allItems.Any())
                return new List<SalesDistributionDTO>();

            decimal totalSales = allItems.Sum(i => (decimal)(i.Quantity ?? 0));
            var result = allItems
                .GroupBy(i => i.Product != null ? i.Product.ProductType : "Combo")
                .Select(g =>
                {
                    decimal groupSum = g.Sum(x => (decimal)(x.Quantity ?? 0));
                    decimal percentage = totalSales == 0
                        ? 0m
                        : Math.Round((groupSum / totalSales) * 100m, 2);
                    return new SalesDistributionDTO
                    {
                        Name = g.Key ?? "Não informado",
                        Percentage = percentage
                    };
                })
                .OrderByDescending(x => x.Percentage)
                .ToList();
            return result;
        }
        public async Task<GetCmvMarkupInvoicingDTO> GetCmvMarkupInvoicingByPeriod(EPeriod period, Guid enterpriseId)
        {
            DateTime startDate = SelectPeriod(period);
            DateTime endDate = DateTime.UtcNow;
            var query = _context.DailyEntry
                .Include(x => x.Items)
                .Where(x => x.EntryDate >= startDate && x.EntryDate <= endDate
                    && x.EnterpriseId == enterpriseId);
            var totalRevenue = await query
                .SelectMany(x => x.Items)
                .SumAsync(i => (decimal?)i.TotalRevenue ?? 0);
            var totalDirectCost = await query
                .SelectMany(x => x.Items)
                .SumAsync(i => (decimal?)i.TotalCPV ?? 0);
            var totalEmployeeCost = 0m;

            var employees = await _context.Employees
                .Include(e => e.EmployeeSchedules)
                .Where(e => e.EnterpriseId == enterpriseId).ToListAsync();
            foreach (var emp in employees)
            {
                if (emp.DateCreated > endDate)
                    continue;
                var effectiveStart = emp.DateCreated > startDate ? emp.DateCreated.Date : startDate.Date;
                DateTime? possibleDateUpdated = emp.DateUpdated == default(DateTime) ? (DateTime?)null : emp.DateUpdated;

                var effectiveEnd = emp.IsActive
                    ? endDate.Date
                    : (possibleDateUpdated.HasValue && possibleDateUpdated.Value < endDate.Date
                        ? possibleDateUpdated.Value.Date
                        : endDate.Date);
                if (effectiveEnd <= effectiveStart)
                    continue;
                if (emp.EmployeeSchedules == null || emp.EmployeeSchedules.Count == 0)
                {
                    var activeDays = (effectiveEnd - effectiveStart).TotalDays;
                    if (activeDays <= 0) continue;

                    var proportionalMonths = (decimal)(activeDays / 30.0);
                    totalEmployeeCost += (decimal)(emp.MonthlySalary * proportionalMonths);
                }
                else
                {
                    var workedDaysCount = 0;
                    for (var day = effectiveStart; day <= effectiveEnd; day = day.AddDays(1))
                    {
                        var dayOfWeekStr = day.DayOfWeek.ToString();
                        var schedule = emp.EmployeeSchedules
                            .FirstOrDefault(ws => ws.WeekDay.Equals(dayOfWeekStr, StringComparison.OrdinalIgnoreCase));

                        if (schedule != null)
                        {
                            workedDaysCount++;
                            totalEmployeeCost += schedule.DailyRate;
                        }
                    }
                }
            }
            var totalDays = (endDate - startDate).TotalDays;
            var months = totalDays / 30.0;
            var totalFixedExpenses = await _context.FixedCosts
                .Where(f =>
                    f.EnterpriseId == enterpriseId &&
                    f.IsPaid &&
                    f.PaymentDate >= startDate &&
                    f.PaymentDate <= endDate)
                .SumAsync(f => (decimal?)f.Value ?? 0);
            var totalGeneralCost = totalDirectCost + totalEmployeeCost + totalFixedExpenses;
            var totalProfit = totalRevenue - totalGeneralCost;
            var cmv = totalRevenue > 0
                ? (totalDirectCost / totalRevenue) * 100
                : 0;
            var margin = totalRevenue > 0
                ? (totalProfit / totalRevenue) * 100
                : 0;
            var markup = totalGeneralCost > 0
                ? (totalRevenue / totalGeneralCost) * 100
                : 0;
            var totalGeneralRevenue = await _context.DailyEntry
                .Where(x => x.EntryDate >= startDate && x.EntryDate <= endDate)
                .SelectMany(x => x.Items)
                .SumAsync(i => (decimal?)i.TotalRevenue ?? 0);
            var revenueShare = totalGeneralRevenue > 0
                ? (totalRevenue / totalGeneralRevenue) * 100
                : 0;
            return new GetCmvMarkupInvoicingDTO
            {
                Profit = Math.Round(totalProfit, 2),
                Margin = Math.Round(margin, 2),
                Markup = Math.Round(markup, 2),
                Cmv = Math.Round(cmv, 2),
                TotalCostEmployees = Math.Round(totalEmployeeCost, 2),
                TotalCostAccounts = Math.Round(totalFixedExpenses, 2),
            };
        }
        public async Task<GetPurchaseDetailsDTO> GetPurchaseDetailsByPeriod(EPeriod period, Guid enterpriseId)
        {
            DateTime startDate = SelectPeriod(period);
            DateTime endDate = DateTime.UtcNow;

            var query = _context.Purchase
                .Include(x => x.Items)
                .Where(x => x.PurchaseDate >= startDate && x.PurchaseDate <= endDate
                    && x.EnterpriseId == enterpriseId);

            var quantityPurchases = await query.CountAsync();

            var totalSpent = await query
                .SelectMany(x => x.Items)
                .SumAsync(i => (decimal?)i.UnitPrice ?? 0);

            var topSuppliers = await query
       .GroupBy(x => x.SupplierName)
       .Select(g => new
       {
           Supplier = g.Key,
           Count = g.Count()
       })
       .OrderByDescending(g => g.Count)
       .Take(5)
       .Select(g => g.Supplier)
       .ToListAsync();

            var totalDays = (endDate - startDate).Days;
            var dailyAverage = totalDays > 0 ? totalSpent / totalDays : 0;
            var nextMonth = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1);
            var daysInNextMonth = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
            var projectedExpenses = dailyAverage * daysInNextMonth;

            return new GetPurchaseDetailsDTO
            {
                TotalSpent = Math.Round(totalSpent, 2),
                ProjectedExpensesNextMonth = Math.Round(projectedExpenses, 2),
                TopSuppliers = topSuppliers,
            };
        }

        public async Task<List<TotalOrdersDTO>> GetPurchaseExpanseByPeriod(EPeriod period, Guid enterpriseId)
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

            var query = _context.Purchase
                .Where(x => x.PurchaseDate >= start && x.PurchaseDate <= end && x.EnterpriseId == enterpriseId);

            List<TotalOrdersDTO> series = new();

            if (granularity == "day")
            {
                var dailyTotals = await query
                    .GroupBy(x => x.PurchaseDate.Date)
                    .Select(g => new { Date = g.Key, Orders = g.Sum(i => i.TotalAmount) })
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
                    .GroupBy(x => new { x.PurchaseDate.Year, x.PurchaseDate.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        Orders = g.Sum(i => i.TotalAmount)
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
                var totalOrders = await query.SumAsync(i => i.TotalAmount);
                series.Add(new TotalOrdersDTO
                {
                    Label = "Total",
                    Orders = totalOrders
                });
            }
            return series;
        }
        //public async Task<GetEmployeesAnalysisDTO> GetEmployeesAnalysis(EPeriod period, Guid enterpriseId)
        //{

        //    DateTime startDate = SelectPeriod(period);
        //    DateTime endDate = DateTime.UtcNow;

        //    var employees = await _context.Employees
        //          .Where(e => e.EnterpriseId == enterpriseId)
        //          .Select(e => new { e.MonthlySalary, e.HiringDate, e.DateCreated }).ToListAsync();
        //   var totalEmployeeCost = 0m;
        //    foreach (var emp in employees)
        //    {
        //        if (emp.HiringDate > endDate)
        //            continue;

        //        var effectiveStart = emp.HiringDate > startDate ? emp.HiringDate : startDate;
        //        var effectiveEnd = endDate;
        //        var activeDays = (effectiveEnd - effectiveStart).TotalDays;
        //        if (activeDays <= 0) continue;
        //        var proportionalMonths = activeDays / 30.0;
        //       totalEmployeeCost = (decimal)(emp.MonthlySalary * (decimal)proportionalMonths);
        //    }

        //    return new GetEmployeesAnalysisDTO
        //    {
        //       TotalEmployeesActive = employees.Count(e => e.DateCreated <= endDate),
        //       TotalSalaries = Math.Round((decimal)totalEmployeeCost, 2)
        //    };
        //}
        public async Task<GetEmployeesAnalysisDTO> GetEmployeesAnalysis(EPeriod period, Guid enterpriseId)
        {
            DateTime startDate = SelectPeriod(period).Date;
            DateTime endDate = DateTime.UtcNow.Date;

            var employees = await _context.Employees
                .Where(e => e.EnterpriseId == enterpriseId)
                .Select(e => new { e.MonthlySalary, e.HiringDate, e.DateCreated, e.IsActive })
                .ToListAsync();

            decimal totalEmployeeCost = 0m;

            foreach (var emp in employees)
            {
                if (emp.HiringDate == null)
                    continue;

                var hireDate = emp.HiringDate.Value.Date;

                if (hireDate > endDate)
                    continue;

                var effectiveStart = hireDate > startDate ? hireDate : startDate;
                var effectiveEnd = endDate;

                if (effectiveStart > effectiveEnd)
                    continue;

                var activeDays = (effectiveEnd - effectiveStart).TotalDays;

                if (activeDays <= 0)
                    continue;

                var proportionalMonths = activeDays / 30.0;

                totalEmployeeCost += emp.MonthlySalary.GetValueOrDefault() * (decimal)proportionalMonths;
            }
            return new GetEmployeesAnalysisDTO
            {
                TotalEmployeesActive = employees
                                       .Count(e => e.HiringDate <= endDate && e.IsActive),
                TotalSalaries = Math.Round(totalEmployeeCost, 2)
            };
        }

        public async Task<List<GetEmployeesCostByPeriodDTO>> GetTotalEmployeeCostByPeriod(EPeriod period, Guid enterpriseId)
        {
            var range = GetPeriod(period);
            DateTime start = range.Start.Date;
            DateTime end = range.End.Date;

            string granularity = period switch
            {
                EPeriod.LastWeek => "day",
                EPeriod.LastFourWeeks => "day",
                EPeriod.LastSemester => "month",
                EPeriod.LastYear => "month",
                EPeriod.SinceTheBeginning => "total",
                _ => "total"
            };

            var employees = await _context.Employees
                .Include(e => e.EmployeeSchedules)
                .Where(e => e.EnterpriseId == enterpriseId)
                .Select(e => new
                {
                    e.Id,
                    e.MonthlySalary,
                    e.DateCreated,
                    e.IsActive,
                    e.DateUpdated,
                    WorkSchedules = e.EmployeeSchedules.Select(ws => new { ws.WeekDay, ws.DailyRate }).ToList()
                })
                .ToListAsync();

            List<GetEmployeesCostByPeriodDTO> series = new();

            if (granularity == "day")
            {
                for (var date = start; date <= end; date = date.AddDays(1))
                {
                    decimal totalCost = 0;

                    foreach (var emp in employees)
                    {
                        if (emp.DateCreated > date) continue;

                        DateTime? dateUpdated = emp.DateUpdated == default ? (DateTime?)null : emp.DateUpdated;
                        if (!emp.IsActive && dateUpdated.HasValue && dateUpdated.Value < date)
                            continue;
                        // Mensalista
                        if (emp.WorkSchedules == null || emp.WorkSchedules.Count == 0)
                        {
                            totalCost += (decimal)(emp.MonthlySalary / 30m);
                        }
                        else
                        {
                            var dayOfWeek = date.DayOfWeek.ToString();
                            var schedule = emp.WorkSchedules.FirstOrDefault(ws =>
                                ws.WeekDay.Equals(dayOfWeek, StringComparison.OrdinalIgnoreCase));

                            if (schedule != null)
                                totalCost += schedule.DailyRate;
                        }
                    }

                    series.Add(new GetEmployeesCostByPeriodDTO
                    {
                        Label = date.ToString("dd/MM"),
                        Cost = Math.Round(totalCost, 2)
                    });
                }
            }
            else if (granularity == "month")
            {
                for (var month = new DateTime(start.Year, start.Month, 1);
                     month <= end;
                     month = month.AddMonths(1))
                {
                    DateTime monthEnd = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));
                    if (monthEnd > end) monthEnd = end;

                    decimal totalCost = 0;

                    foreach (var emp in employees)
                    {
                        if (emp.DateCreated > monthEnd) continue;

                        DateTime? dateUpdated = emp.DateUpdated == default ? (DateTime?)null : emp.DateUpdated;
                        if (!emp.IsActive && dateUpdated.HasValue && dateUpdated.Value < month)
                            continue;

                        if (emp.WorkSchedules == null || emp.WorkSchedules.Count == 0)
                        {
                            // Mensalista
                            totalCost += (decimal)emp.MonthlySalary;
                        }
                        else
                        {
                            decimal total = 0;
                            for (var date = month; date <= monthEnd; date = date.AddDays(1))
                            {
                                var dayOfWeek = date.DayOfWeek.ToString();
                                var schedule = emp.WorkSchedules.FirstOrDefault(ws =>
                                    ws.WeekDay.Equals(dayOfWeek, StringComparison.OrdinalIgnoreCase));
                                if (schedule != null)
                                    total += schedule.DailyRate;
                            }
                            totalCost += total;
                        }
                    }

                    series.Add(new GetEmployeesCostByPeriodDTO
                    {
                        Label = month.ToString("MMM"),
                        Cost = Math.Round(totalCost, 2)
                    });
                }
            }
            else
            {
                decimal totalCost = 0;

                foreach (var emp in employees)
                {
                    if (emp.DateCreated > end) continue;

                    DateTime? dateUpdated = emp.DateUpdated == default ? (DateTime?)null : emp.DateUpdated;
                    DateTime effectiveStart = emp.DateCreated > start ? emp.DateCreated : start;
                    DateTime effectiveEnd = emp.IsActive ? end : (dateUpdated.HasValue && dateUpdated.Value < end ? dateUpdated.Value : end);

                    if (effectiveEnd < effectiveStart)
                        continue;

                    if (emp.WorkSchedules == null || emp.WorkSchedules.Count == 0)
                    {
                        // Mensalista
                        var activeDays = (effectiveEnd - effectiveStart).TotalDays;
                        totalCost += (decimal)(emp.MonthlySalary * (decimal)(activeDays / 30.0));
                    }
                    else
                    {
                        for (var date = effectiveStart.Date; date <= effectiveEnd.Date; date = date.AddDays(1))
                        {
                            var dayOfWeek = date.DayOfWeek.ToString();
                            var schedule = emp.WorkSchedules.FirstOrDefault(ws =>
                                ws.WeekDay.Equals(dayOfWeek, StringComparison.OrdinalIgnoreCase));
                            if (schedule != null)
                                totalCost += schedule.DailyRate;
                        }
                    }
                }
                series.Add(new GetEmployeesCostByPeriodDTO
                {
                    Label = "Total",
                    Cost = Math.Round(totalCost, 2)
                });
            }
            return series;
        }

    }

}