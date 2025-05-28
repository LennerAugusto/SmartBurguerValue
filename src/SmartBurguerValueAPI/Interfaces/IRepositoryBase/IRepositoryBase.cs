using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.IRepository.IRepositoryBase
{
    public interface IRepositoryBase<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task Create(T entity);
        Task Update(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(bool? isActive = null);
        Task Delete(T entity);
        Task SaveAsync(T model);
    }
}
