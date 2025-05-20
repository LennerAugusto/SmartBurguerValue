using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class FixedCoastRepository : RepositoryBase<FixedCostEntity>, IFixedCoastRepository
    {
        public FixedCoastRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<PagedList<FixedCoastDTO>> GetAllFixedCostByEnterpriseId(PaginationParamiters paramiters, Guid enterpriseId)
        {
            var query = _context.Set<FixedCoastDTO>() // ou Set<FixedCostEntity>() se preferir
                .Where(x => x.EnterpriseId == enterpriseId)
                .AsNoTracking()
                .Select(x => new FixedCoastDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value,
                    EnterpriseId = x.EnterpriseId
                });

            return PagedList<FixedCoastDTO>.ToPagedList(query, paramiters.PageNumber, paramiters.PageSize);
        }
    }
}
