using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository
{
    public class EnterpriseRepository : RepositoryBase<EnterpriseEntity>, IEnterpriseRepository
    {
        public EnterpriseRepository(AppDbContext context) : base(context)
        {
        }
    }
}
