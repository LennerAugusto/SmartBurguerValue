using SmartBurguerValueAPI.Interfaces.IProducts;
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
        Task CommitAsync();
    }
}
