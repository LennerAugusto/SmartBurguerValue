﻿using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.IRepository.IProducts;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IUnityOfWork
    {
        IProductRepository ProductRepository { get; }
        IEnterpriseRepository EnterpriseRepository { get; }
        IUnityTypesRepository UnityTypesRepository { get; }
        IIngredientRepository IngredientRepository { get; }
        IProductIngredientsRepository ProductsIngredientRepository { get; }
        IComboRepository ComboRepository { get; }
        IFixedCoastRepository FixedCoastRepository { get; }
        ISalesGoalRepository SalesGoalRepository { get; }
        IDailyEntryRepository DailyEntryRepository { get; }
        IDailyEntryItemRepository DailyEntryItemRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IEmployeeWorkScheduleRepository EmployeeWorkScheduleRepository { get; }
        IPurchaseRepository PurchaseRepository { get; }
        IFinancialSnapshotsRepository FinancialSnapshotsRepository { get; }
        IProductCostAnalysisRepository ProductCostAnalysisRepository { get; }
        Task CommitAsync();
    }
}
