using AutoMapper;
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
        private readonly IMapper _map;
        public FixedCoastRepository(AppDbContext context, IMapper map) : base(context)
        {
            _map = map;
        }
        public async Task<List<FixedCoastDTO>> GetAllFixedCostByEnterpriseId(Guid enterpriseId)
        {
            var query = _context.Set<FixedCostEntity>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .AsNoTracking();
                
            return _map.Map<List<FixedCoastDTO>>(query);
        }
    }
}
