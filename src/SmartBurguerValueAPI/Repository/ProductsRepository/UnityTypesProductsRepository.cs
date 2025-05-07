using AutoMapper;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.ReadModels;
using SmartBurguerValueAPI.Repository.Base;

namespace SmartBurguerValueAPI.Repository.ProductsRepository
{
    public class UnityTypesProductsRepository : RepositoryBase<UnityTypesProductsEntity>, IUnityTypesRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UnityTypesProductsRepository(AppDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
            _context = context;
        }
        public BaseDTO CreateUnityTypesProducts(BaseDTO unityTypes)
        {
            if (unityTypes is null)
            {
                throw new ArgumentNullException(nameof(unityTypes));
            }
            var UnityTypesCreate = _mapper.Map<UnityTypesProductsEntity>(unityTypes);
            _context.UnityTypesProducts.Add(UnityTypesCreate);
            _context.SaveChanges();
            return unityTypes;
        }

        public BaseDTO DeleteUnityTypeProducts(Guid Id)
        {
            var UnityTypesDelete = _context.UnityTypesProducts.FirstOrDefault(c => c.Id == Id);
            if (UnityTypesDelete is null)
            {
                throw new ArgumentNullException(nameof(UnityTypesDelete));
            }
            _context.UnityTypesProducts.Remove(UnityTypesDelete);
            _context.SaveChanges();
            var CategoryReponse = _mapper.Map<CategoryProductsDTO>(UnityTypesDelete);
            return CategoryReponse;
        }

        public IEnumerable<BaseDTO> GetAllUnityTypesProducts()
        {
            var UnityTypes = _context.UnityTypesProducts.ToList();
            var UnityTypesResponse = _mapper.Map<IEnumerable<BaseDTO>>(UnityTypes);
            return UnityTypesResponse;
        }

        public BaseDTO GetUnityTypeProductsById(Guid Id)
        {
            var UnityType = _context.UnityTypesProducts.FirstOrDefault(c => c.Id == Id);
            var UnityTypeResponse = _mapper.Map<BaseDTO>(UnityType);
            return UnityTypeResponse;
        }

        public BaseDTO UpdateUnityTypesProducts(BaseDTO unityType)
        {
            var UnityTypeEntity = _context.UnityTypesProducts.FirstOrDefault(c => c.Id == unityType.Id);
            if (UnityTypeEntity is null)
            {
                throw new KeyNotFoundException($"Tipo de unidade com ID {unityType.Id} não encontrado.");
            }
            _mapper.Map(unityType, UnityTypeEntity);

            _context.SaveChanges();
            return unityType;
        }
    }
}
