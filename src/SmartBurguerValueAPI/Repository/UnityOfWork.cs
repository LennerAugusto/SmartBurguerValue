using AutoMapper;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Repository.ProductsRepository;

namespace SmartBurguerValueAPI.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        private readonly IMapper _map;
        public IProductRepository? _ProductRep;
        public IEnterpriseRepository? _EnterpriseRep;
        public IUnityTypesRepository? _UnityTypesRep;
        public IIngredientRepository? _IngredientRep;
        public IProductIngredientsRepository? _ProductIngredientsRep;
        public IComboRepository? _ComboRep;
        public IComboProductRepository? _ComboProductRep;
        public IFixedCoastRepository? _FixedCoastRep;
        public ISalesGoalRepository _SalesGoalRep;
        public IDailyEntryRepository _DailyEntryRep;
        public IDailyEntryItemRepository _DailyEntryItemRep;
        public IEmployeeRepository _EmployeeRep;
        public IEmployeeWorkScheduleRepository _EmployeeWorkRep;
        public IPurchaseRepository _PurchaseRep;
        public IPurchaseItemRepository _PurchaseItemRep;
        public IFinancialSnapshotsRepository _FinancialSnapshotsRep;
        public IProductCostAnalysisRepository _ProductCostAnalysisRep;
        public IAnalysisByPeriodRepository _AnalysisByPeriodRep;
        public IAnalysisByPeriodYearsRepository _AnalysisByPeriodYearsRep;
        public AppDbContext _context;

        public UnityOfWork(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _map = mapper;
        }
        public IProductRepository ProductRepository
        {
            get{return _ProductRep = _ProductRep ?? new ProductRepository(_context, this);}
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
            get { return _ComboRep = _ComboRep ?? new ComboRepository(this, _context); }
        }
        public IComboProductRepository ComboProductRepository
        {
            get { return _ComboProductRep = _ComboProductRep ?? new ComboProductRepository(_context); }
        }
        public IFixedCoastRepository FixedCoastRepository
        {
            get { return _FixedCoastRep = _FixedCoastRep ?? new FixedCoastRepository(_context, _map); }
        }
        public ISalesGoalRepository SalesGoalRepository
        {
            get { return _SalesGoalRep = _SalesGoalRep ?? new SalesGoalRepository(_context); }
        }
        public IDailyEntryRepository DailyEntryRepository
        {
            get { return _DailyEntryRep = _DailyEntryRep ?? new DailyEntryRepository(_context); }
        }
        public IDailyEntryItemRepository DailyEntryItemRepository
        {
            get { return _DailyEntryItemRep = _DailyEntryItemRep ?? new DailyEntryItemRepository(_context); }
        }
        public IEmployeeRepository EmployeeRepository
        {
            get { return _EmployeeRep = _EmployeeRep ?? new EmployeeRepository(_context); }
        }
        public IEmployeeWorkScheduleRepository EmployeeWorkScheduleRepository
        {
            get { return _EmployeeWorkRep = _EmployeeWorkRep ?? new EmployeeWorkScheduleRepository(_context); }
        }
        public IPurchaseRepository PurchaseRepository
        {
            get { return _PurchaseRep = _PurchaseRep ?? new PurchaseRepository(_context, this, _map); }
        }
        public IPurchaseItemRepository PurchaseItemRepository
        {
            get { return _PurchaseItemRep = _PurchaseItemRep ?? new PurchaseItemRepository(_context); }
        }
        public IFinancialSnapshotsRepository FinancialSnapshotsRepository
        {
            get { return _FinancialSnapshotsRep = _FinancialSnapshotsRep ?? new FinancialSnapshotsRepository(_context); }
        }
        public IProductCostAnalysisRepository ProductCostAnalysisRepository
        {
            get { return _ProductCostAnalysisRep = _ProductCostAnalysisRep ?? new ProductCostAnalysisRepository(_context); }
        }
        public IAnalysisByPeriodRepository AnalysisByPeriodRepository
        {
            get { return _AnalysisByPeriodRep = _AnalysisByPeriodRep ?? new AnalysisByPeriodRepository(_context); }
        }
        public IAnalysisByPeriodYearsRepository AnalysisByPeriodYearsRepository
        {
            get { return _AnalysisByPeriodYearsRep = _AnalysisByPeriodYearsRep ?? new AnalysisByPeriodYearRepository(_context); }
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
