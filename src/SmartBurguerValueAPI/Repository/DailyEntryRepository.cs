using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class DailyEntryRepository : RepositoryBase<DailyEntryEntity>, IDailyEntryRepository
    {
        public DailyEntryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
