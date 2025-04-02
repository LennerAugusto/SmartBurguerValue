using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.ReadModels;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class CategoryProductsRepository : ICategoryProducts
    {
        private readonly SmartBurguerValueAPIContext _context;
        private readonly IMapper _mapper;

        public CategoryProductsRepository(SmartBurguerValueAPIContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public CategoryProductsDTO CreateCategoryProducts(CategoryProductsDTO category)
        {
            if(category is null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            var CategoryCreate = _mapper.Map<CategoryProductsEntity>(category);
            _context.CategoryProducts.Add(CategoryCreate);
            _context.SaveChanges();
            return category;
        }

        public CategoryProductsDTO DeleteCategoryProducts(Guid id)
        {
           var CategoryDelete = _context.CategoryProducts.FirstOrDefault(c => c.Id == id);
            if (CategoryDelete is null)
            {
                throw new ArgumentNullException(nameof(CategoryDelete));
            }
            _context.CategoryProducts.Remove(CategoryDelete);
            _context.SaveChanges();
            var CategoryReponse = _mapper.Map<CategoryProductsDTO>(CategoryDelete);
            return CategoryReponse;
        }

        public IEnumerable<CategoryProductsReadModel> GetAllCategoryProducts()
        {
            var Category = _context.CategoryProducts.ToList();
            var CategoryReadModel = _mapper.Map<IEnumerable<CategoryProductsReadModel>>(Category);
            return CategoryReadModel;
        }

        public BaseDTO GetCategoryProductsById(Guid id)
        {
            var Category = _context.CategoryProducts.FirstOrDefault(c => c.Id == id);
            var CategoryResponse  = _mapper.Map<BaseDTO>(Category);
            return CategoryResponse;
        }

        public CategoryProductsDTO UpdateCategoryProducts(CategoryProductsDTO category)
        {
            var categoryEntity = _context.CategoryProducts.FirstOrDefault(c => c.Id == category.Id);
            if (categoryEntity is null)
            {
                throw new KeyNotFoundException($"Categoria com ID {category.Id} não encontrada.");
            }
            _mapper.Map(category, categoryEntity);

            _context.SaveChanges();
            return category;
        }

    }
}
