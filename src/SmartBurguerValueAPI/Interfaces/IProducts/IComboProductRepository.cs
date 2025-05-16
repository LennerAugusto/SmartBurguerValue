using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Interfaces.IProducts
{
    public interface IComboProductRepository : IRepositoryBase<ComboProductEntity>
    {
        Task RemoveAllByComboIdAsync(Guid comboId);
        Task AddAsync(ComboProductEntity comboProduct);
    }
}
