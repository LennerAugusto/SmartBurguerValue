using SmartBurguerValueAPI.IRepository.IProducts;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IUnityOfWork
    {
        IProductRepository ProductRepository { get; }
        IEnterpriseRepository EnterpriseRepository { get; }
        IUnityTypesRepository UnityTypesRepository { get; }
        void Commit();
    }
}
