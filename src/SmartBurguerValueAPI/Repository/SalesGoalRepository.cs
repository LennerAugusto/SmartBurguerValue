using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class SalesGoalRepository : RepositoryBase<SalesGoalEntity>, ISalesGoalRepository
    {
        public SalesGoalRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<PagedList<SalesGoalDTO>> GetAllSalesGoalByEnterpriseId(PaginationParamiters paramiters, Guid enterpriseId)
        {
            var query = _context.Set<SalesGoalEntity>()
                .Where(x => x.EnterpriseId == enterpriseId)
                .Select(x => new SalesGoalDTO
                {
                    Id = x.Id,
                    Description= x.Description,
                    GoalValue = x.GoalValue,
                    EndDate = x.EndDate,
                    StartDate = x.StartDate,
                    EnterpriseId = x.EnterpriseId
                });

            return PagedList<SalesGoalDTO>.ToPagedList(query, paramiters.PageNumber, paramiters.PageSize);
        }

    }
}
