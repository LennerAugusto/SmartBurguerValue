using AutoMapper;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class UnityTypesProductsRepository : RepositoryBase<UnityTypesProductsEntity>, IUnityTypesRepository
    {
        private readonly AppDbContext _context;
       
        public UnityTypesProductsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
      
    }
}
