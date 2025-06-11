using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class PurchaseRepository : RepositoryBase<PurchaseEntity>, IPurchaseRepository
    {
        public PurchaseRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<PagedList<PurchaseDTO>> GetAllPurchasesByEnterpriseId(PaginationParamiters paramiters, Guid enterpriseId)
        {
            var query = _context.Purchase
                .Where(x => x.EnterpriseId == enterpriseId)
                .OrderBy(x => x.Id)
                .Select(x => new PurchaseDTO
                {
                    Id = x.Id,
                    SupplierName = x.SupplierName,
                    PurchaseDate = x.PurchaseDate,
                    TotalAmount = x.TotalAmount,
                    PurchaseItems = x.Items.Select(i => new PurchaseItemDTO
                    {
                        NameItem = i.NameItem,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                    }).ToList()
                });

            var productsOrder = PagedList<PurchaseDTO>.ToPagedList(query, paramiters.PageNumber, paramiters.PageSize);
            return productsOrder;
        }

    }
}
