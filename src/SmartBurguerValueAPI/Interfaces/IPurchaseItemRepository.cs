using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IPurchaseItemRepository : IRepositoryBase<PurchaseItemEntity>
    {
        Task<PurchaseItemEntity> GetPurchaseItemRencentlyByIngredientId(Guid IngredientId);
    }
}
