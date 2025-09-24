using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Migrations;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class DailyEntryRepository : RepositoryBase<DailyEntryEntity>, IDailyEntryRepository
    {
        public DailyEntryRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IQueryable<DailyEntryDTO>> GetAllDailyEntryByEnterpriseId(Guid enterpriseId)
        {
            var dailyEntries = _context.DailyEntry
                .Where(x => x.EnterpriseId == enterpriseId)
                .Include(i => i.Items)
                .Select(x => new DailyEntryDTO
                {
                    Id = x.Id,
                    EntryDate = x.EntryDate,
                    Description = x.Description,
                    EnterpriseId = x.EnterpriseId,
                    TotalOrders = x.TotalOrders,
                    TotalItems = x.Items.Sum(i => i.Quantity) ?? 0,
                    TotalCost = x.Items.Sum(i => i.TotalCPV) ?? 0,
                    Revenue = x.Items.Sum(i => i.TotalRevenue) ?? 0,
                    Liquid = x.Items.Sum(i => i.TotalRevenue) - x.Items.Sum(i => i.TotalCPV),
                    Items = x.Items.Select(i => new DailyEntryItemDTO
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Quantity = i.Quantity,
                        TotalCPV = i.TotalCPV,
                        TotalRevenue = i.TotalRevenue
                    }).ToList(),
                });
            return dailyEntries;
        }

        public async Task<DailyEntryDTO> GetDailyEntryById(Guid Id)
        {
            var dailyEntrie = await _context.DailyEntry
                .Where(x => x.Id == Id)
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)  
                .Select(x => new DailyEntryDTO
                {
                    Id = x.Id,
                    EntryDate = x.EntryDate,
                    Description = x.Description,
                    EnterpriseId = x.EnterpriseId,
                    TotalOrders = x.TotalOrders,
                    TotalItems = x.Items.Sum(i => i.Quantity) ?? 0,
                    TotalCost = x.Items.Sum(i => i.TotalCPV) ?? 0,
                    Revenue = x.Items.Sum(i => i.TotalRevenue) ?? 0,
                    Liquid = x.Items.Sum(i => i.TotalRevenue) - x.Items.Sum(i => i.TotalCPV),
                    Items = x.Items.Select(i => new DailyEntryItemDTO
                    {
                        DailyEntryId = i.DailyEntryId,
                        Id = i.Id,
                        Name = i.Name ?? i.Product.Name ?? "Sem nome",
                        Quantity = i.Quantity,
                        TotalCPV = i.TotalCPV,
                        TotalRevenue = i.TotalRevenue
                    }).ToList(),
                }).FirstOrDefaultAsync();

            return dailyEntrie;
        }
        public async Task UpdateDailyEntryAsync(DailyEntryDTO dto)
        {
            var entry = await _context.DailyEntry
                .Include(e => e.Items)
                .FirstOrDefaultAsync(e => e.Id == dto.Id);

            if (entry == null)
                throw new KeyNotFoundException("Lançamento diário não encontrado.");

            entry.EntryDate = dto.EntryDate;
            entry.Description = dto.Description;
            entry.DateUpdated = DateTime.UtcNow;

            entry.TotalOrders = dto.TotalOrders;

            _context.DailyEntryItem.RemoveRange(entry.Items);

            var newItems = dto.Items.Select(i => new DailyEntryItemEntity
            {
                DailyEntryId = entry.Id,
                ProductId = i.ProductId,
                ComboId = i.ComboId,
                Name = i.Name,
                Quantity = i.Quantity,
                TotalCPV = i.TotalCPV,
                TotalRevenue = i.TotalRevenue,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                IsActive = true
            }).ToList();

            _context.DailyEntryItem.AddRange(newItems);

            await _context.SaveChangesAsync();
        }


    }
}
