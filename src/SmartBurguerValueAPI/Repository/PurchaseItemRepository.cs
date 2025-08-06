using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class PurchaseItemRepository : RepositoryBase<PurchaseItemEntity>, IPurchaseItemRepository
    {
        public PurchaseItemRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<PurchaseItemEntity> GetPurchaseItemRencentlyByIngredientId(Guid IngredientId)
        {
            var ingredient = await _context.PurchaseItem
                .AsNoTracking()
                .Include(x => x.UnityOfMensure)
                .Where(x => x.IngredientId == IngredientId)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefaultAsync();

            return ingredient;

        }
    }
}
