using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IDailyEntryRepository : IRepositoryBase<DailyEntryEntity>
    {
        Task<IQueryable<DailyEntryDTO>> GetAllDailyEntryByEnterpriseId(Guid enterpriseId);
        Task<DailyEntryDTO> UpdateDailyEntryAsync(DailyEntryDTO dto);
        Task<DailyEntryDTO> GetDailyEntryById(Guid Id);
    }
}
