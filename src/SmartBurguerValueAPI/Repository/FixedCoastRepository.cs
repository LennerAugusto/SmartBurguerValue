using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class FixedCoastRepository : RepositoryBase<FixedCostEntity>, IFixedCoastRepository
    {
        public FixedCoastRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<FixedCoastDTO>> GetAllFixedCostByEnterpriseId(Guid enterpriseId)
        {
            return await _context.Set<FixedCoastDTO>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
