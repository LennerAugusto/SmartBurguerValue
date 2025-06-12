using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class FinancialSnapshotsRepository : RepositoryBase<FinancialSnapshotEntity>, IFinancialSnapshotsRepository
    {
        public FinancialSnapshotsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
