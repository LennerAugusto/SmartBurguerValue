using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.Interfaces.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class ComboProductRepository : RepositoryBase<ComboProductEntity>, IComboProductRepository
    {
        public ComboProductRepository(AppDbContext context) : base(context)
        {
        }
        public async Task RemoveAllByComboIdAsync(Guid comboId)
        {
            var list = await _context.ComboProducts
                .Where(pi => pi.ComboId == comboId)
                .ToListAsync();

            _context.ComboProducts.RemoveRange(list);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(ComboProductEntity comboProduct)
        {
            await _context.ComboProducts.AddAsync(comboProduct);
        }
    }
}
