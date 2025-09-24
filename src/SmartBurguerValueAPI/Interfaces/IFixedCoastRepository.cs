using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IFixedCoastRepository : IRepositoryBase<FixedCostEntity>
    {
        Task<List<FixedCoastDTO>> GetAllFixedCostByEnterpriseId(Guid EnterpriseId);
    }
}
