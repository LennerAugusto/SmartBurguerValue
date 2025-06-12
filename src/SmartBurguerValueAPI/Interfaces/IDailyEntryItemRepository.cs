using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IDailyEntryItemRepository : IRepositoryBase<DailyEntryItemEntity>
    {
        public Task<DailyEntryItemEntity?> BuildDailyEntryItem(Guid dailyEntryId, DailyEntryItemDTO item);
        public Task<DailyEntryEntity> UpdateWithItemsAsync(DailyEntryEntity entry, List<DailyEntryItemDTO> items);
    }
}
