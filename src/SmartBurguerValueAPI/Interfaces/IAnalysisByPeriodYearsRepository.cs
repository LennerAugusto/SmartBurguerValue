using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.DTOs.Analysis;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IAnalysisByPeriodYearsRepository
    {
        Task<RevenueComparisonDTO> GetRevenueComparisonAsync(PeriodYears period, Guid enterpriseId);
        Task<RevenueComparisonDTO> GetOrdersComparisonAsync(PeriodYears period, Guid enterpriseId);
    }
}
