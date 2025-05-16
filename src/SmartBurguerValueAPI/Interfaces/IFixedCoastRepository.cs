using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IFixedCoastRepository : IRepositoryBase<FixedCostEntity>
    {
        Task<IEnumerable<FixedCoastDTO>> GetAllFixedCostByEnterpriseId(Guid EnterpriseId);
    }
}
