using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.DTOs;
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
        Task<List<SalesDistributionDTO>> GetSalesDistributionByPeriod(EPeriod Period, Guid enterpriseId);
        Task<GetCmvMarkupInvoicingDTO> GetCmvMarkupInvoicingByPeriod(EPeriod period, Guid enterpriseId);
        Task<GetPurchaseDetailsDTO> GetPurchaseDetailsByPeriod(EPeriod Period, Guid enterpriseId);
        Task<List<TotalOrdersDTO>> GetPurchaseExpanseByPeriod(EPeriod Period, Guid enterpriseId);
        Task<GetEmployeesAnalysisDTO>GetEmployeesAnalysis(EPeriod period, Guid EnteprriseId);
        Task<List<GetEmployeesCostByPeriodDTO>> GetTotalEmployeeCostByPeriod(EPeriod period, Guid EnteprriseId);
    }
}
