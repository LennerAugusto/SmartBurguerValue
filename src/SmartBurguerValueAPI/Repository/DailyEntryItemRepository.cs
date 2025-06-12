using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class DailyEntryItemRepository : RepositoryBase<DailyEntryItemEntity>, IDailyEntryItemRepository
    {
        public DailyEntryItemRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<DailyEntryItemEntity?> BuildDailyEntryItem(Guid dailyEntryId, DailyEntryItemDTO item)
        {
            decimal? sellingPrice, cpv;

            if (item.ProductId != Guid.Empty)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                var analysis = await _context.ProductCostAnalysis
                    .Where(a => a.ProductId == product.Id)
                    .OrderByDescending(a => a.AnalisysDate)
                    .FirstOrDefaultAsync();

                sellingPrice = product.SellingPrice;
                cpv = analysis?.CPV ?? 0;

                return new DailyEntryItemEntity
                {
                    Id = Guid.NewGuid(),
                    DailyEntryId = dailyEntryId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    SellingPrice = sellingPrice,
                    CPV = cpv,
                    TotalRevenue = sellingPrice * item.Quantity,
                    TotalCPV = cpv * item.Quantity
                };
            }

            if (item.ComboId != Guid.Empty)
            {
                var combo = await _context.Combos.FindAsync(item.ComboId);
                sellingPrice = combo.SellingPrice;
                var cpvCombo = await CalcularCPVDoCombo(combo.Id);

                return new DailyEntryItemEntity
                {
                    Id = Guid.NewGuid(),
                    DailyEntryId = dailyEntryId,
                    ComboId = item.ComboId,
                    Quantity = item.Quantity,
                    SellingPrice = sellingPrice,
                    CPV = cpvCombo,
                    TotalRevenue = sellingPrice * item.Quantity,
                    TotalCPV = cpvCombo * item.Quantity
                };
            }

            return null;
        }
        public async Task<DailyEntryEntity> UpdateWithItemsAsync(DailyEntryEntity entry, List<DailyEntryItemDTO> items)
        {
            var existing = await _context.DailyEntry.FindAsync(entry.Id);
            if (existing == null) throw new Exception("Fechamento não encontrado");

            existing.EntryDate = entry.EntryDate;
            existing.EnterpriseId = entry.EnterpriseId;
            existing.Description = entry.Description;
            existing.DateUpdated = DateTime.UtcNow;

            var oldItems = _context.DailyEntryItem.Where(i => i.DailyEntryId == entry.Id);
            _context.DailyEntryItem.RemoveRange(oldItems);

            foreach (var item in items)
            {
                var entryItem = await BuildDailyEntryItem(entry.Id, item);
                if (entryItem != null)
                    await _context.DailyEntryItem.AddAsync(entryItem);
            }

            return existing;
        }
        private async Task<decimal> CalcularCPVDoCombo(Guid comboId)
        {
            var comboProducts = await _context.ComboProducts
                .Where(cp => cp.ComboId == comboId)
                .ToListAsync();

            decimal? totalCPV = 0;
            foreach (var cp in comboProducts)
            {
                var analysis = await _context.ProductCostAnalysis
                    .Where(p => p.ProductId == cp.ProductId && p.IsActive)
                    .OrderByDescending(p => p.AnalisysDate)
                    .FirstOrDefaultAsync();

                if (analysis != null)
                {
                    totalCPV += analysis.CPV;
                }
            }

            return (decimal)totalCPV;
        }
    }
}
