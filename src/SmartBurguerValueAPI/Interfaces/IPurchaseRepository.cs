using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IPurchaseRepository : IRepositoryBase<PurchaseEntity>
    {
        Task<PagedList<PurchaseDTO>> GetAllPurchasesByEnterpriseId(PaginationParamiters pagination, Guid EnterpriseId);
    }
}
