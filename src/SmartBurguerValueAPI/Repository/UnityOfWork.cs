using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Repository.ProductsRepository;

namespace SmartBurguerValueAPI.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        public IProductRepository? _ProductRep;
        public IEnterpriseRepository? _EnterpriseRep;
        public IUnityTypesRepository? _UnityTypesRep;
        public IIngredientRepository? _IngredientRep;
        public IProductIngredientsRepository? _ProductIngredientsRep;
        public IComboRepository? _ComboRep;
        public IFixedCoastRepository? _FixedCoastRep;
        public ISalesGoalRepository _SalesGoalRep;

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
        public IIngredientRepository IngredientRepository
        {
            get { return _IngredientRep = _IngredientRep ?? new IngredientRepository(_context); }
        }
        public IProductIngredientsRepository ProductsIngredientRepository
        {
            get { return _ProductIngredientsRep = _ProductIngredientsRep ?? new ProductIngredientRepository(_context); }
        }
        public IComboRepository ComboRepository
        {
            get { return _ComboRep = _ComboRep ?? new ComboRepository(_context); }
        }
        public IFixedCoastRepository FixedCoastRepository
        {
            get { return _FixedCoastRep = _FixedCoastRep ?? new FixedCoastRepository(_context); }
        }
        public ISalesGoalRepository SalesGoalRepository
        {
            get { return _SalesGoalRep = _SalesGoalRep ?? new SalesGoalRepository(_context); }
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
