using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Repository.ProductsRepository;

namespace SmartBurguerValueAPI.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        public IProductRepository? _ProductRep;
        public IEnterpriseRepository? _EnterpriseRep;
        public IUnityTypesRepository _UnityTypesRep;

        public AppDbContext _context;

        public UnityOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IProductRepository ProductRepository
        {
            get{return _ProductRep = _ProductRep ?? new ProductRepository(_context);}
        }
        public IEnterpriseRepository EnterpriseRepository
        {
           get{ return _EnterpriseRep = _EnterpriseRep ?? new EnterpriseRepository(_context);}
        }
        public IUnityTypesRepository UnityTypesRepository
        {
            get { return _UnityTypesRep = _UnityTypesRep ?? new UnityTypesProductsRepository(_context); }
        } 
        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
