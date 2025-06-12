using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IProductCostAnalysisRepository : IRepositoryBase<ProductCostAnalysisEntity>
    {
        public Task<ProductCostAnalysisEntity> GetAnalyse(Guid productId);
    }
}
