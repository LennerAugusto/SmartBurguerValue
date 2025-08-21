using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.DTOs.Analysis;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IAnalysisByPeriodRepository
    {
        Task<InitialAnalysiDTO> GetInitialAnalysisByPeriod(EPeriod Period, Guid EntepriseId);
        Task<List<BestSellingProductsDTO>> GetBestSellingProductsByEnterpriseId(EPeriod Period, Guid EntepriseId);
    }
}
