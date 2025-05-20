using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface ISalesGoalRepository : IRepositoryBase<SalesGoalEntity>
    {
        Task<PagedList<SalesGoalDTO>> GetAllSalesGoalByEnterpriseId(PaginationParamiters paramiters, Guid EnterpriseId);
    }
}
