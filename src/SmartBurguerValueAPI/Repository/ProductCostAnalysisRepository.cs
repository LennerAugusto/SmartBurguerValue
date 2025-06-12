using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class ProductCostAnalysisRepository : RepositoryBase<ProductCostAnalysisEntity>, IProductCostAnalysisRepository
    {
        public ProductCostAnalysisRepository(AppDbContext context) : base(context)
        {
        }
    }
}
