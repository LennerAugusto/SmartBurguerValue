using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.DTOs.Analysis;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IAnalysisByPeriodRepository
    {
        Task<InitialAnalysiDTO> GetInitialAnalysisByPeriod(EPeriod Period, Guid EntepriseId);
        Task<List<BestSellingProductsDTO>> GetCardsBestSellingProductsByEnterpriseId(EPeriod Period, Guid EntepriseId);
        Task<List<InvoicingSeriesDTO>> GetInvoicingByPeriod(EPeriod Period, Guid enterpriseId);
        Task<List<TotalOrdersDTO>> GetTotalOrdersByPeriod(EPeriod Period, Guid enterpriseId);
        Task<GetAnalysisCardsProductsDTO> GetMarginAndProfitProductByPeriod(EPeriod Period, Guid enterpriseId);
        Task<GetAnalysisCardsProductsDTO> GetMarginAndProfitComboByPeriod(EPeriod Period, Guid enterpriseId);
        Task<List<BestSellingProductsByPeriodDTO>> GetBestSellingCombosByPeriod(EPeriod period, Guid enterpriseId);
        Task<List<BestSellingProductsByPeriodDTO>>GetBestSellingProductsByPeriod(EPeriod period, Guid enterpriseId);
        Task<List<ProductsBestMarginDTO>> GetProductsBestMargin(Guid enterpriseId);
        Task<List<ProductsBestMarginDTO>> GetCombosBestMargin(Guid enterpriseId);
    }
}
