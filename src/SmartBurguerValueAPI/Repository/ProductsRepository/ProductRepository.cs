using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.ReadModels;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly SmartBurguerValueAPIContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(SmartBurguerValueAPIContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public ProductsDTO CreateProduct(ProductsDTO Product)
        {
            if (Product is null)
            {
                throw new ArgumentNullException(nameof(Product));
            }
            var ProductCreate = _mapper.Map<ProductsEntity>(Product);
            _context.Products.Add(ProductCreate);
            _context.SaveChanges();
            return Product;
        }

        public ProductsEntity DeleteProduct(Guid id)
        {
            var ProductDelete = _context.Products.FirstOrDefault(c => c.Id == id);
            if (ProductDelete is null)
            {
                throw new ArgumentNullException(nameof(ProductDelete));
            }
            _context.Products.Remove(ProductDelete);
            _context.SaveChanges();
            return ProductDelete;
        }

        public IEnumerable<ProductsEntity> GetAllProducts()
        {
            var Product = _context.Products.ToList();
            return Product;
        }

        public ProductsEntity GetProductById(Guid Id)
        {
            var Product = _context.Products.FirstOrDefault(c => c.Id == Id);
            if(Product is null)
            {
                throw new KeyNotFoundException($"Produto {Id} não encontrado.");
            }
            return Product; 
        }

        public ProductsDTO UpdateProduct(ProductsDTO Product)
        {
            var ProductEntity = _context.Products.FirstOrDefault(c => c.Id == Product.Id);
            if (ProductEntity is null)
            {
                throw new KeyNotFoundException($"Produto com ID {(Product.Id)} não encontrada.");
            }
            _mapper.Map(Product, ProductEntity);

            _context.SaveChanges();
            return Product;
        }
    }
}
