using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IFinancialSnapshotsRepository  : IRepositoryBase<FinancialSnapshotEntity>
    {
        public Task<FinancialSnapshotEntity> GetAnalyse(DateTime date, Guid enterpriseId);
    }
}
