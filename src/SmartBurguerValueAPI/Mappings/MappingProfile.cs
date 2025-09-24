using AutoMapper;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;


namespace SmartBurguerValueAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BaseDTO, UnityTypesProductsEntity>().ReverseMap();
            CreateMap<IngredientDTO, IngredientsEntity>().ReverseMap();
            CreateMap<EmployeeDTO, EmployeeEntity>().ReverseMap();
            CreateMap<WorkScheduleDTO, EmployeeWorkScheduleEntity>().ReverseMap();
            CreateMap<DailyEntryDTO, DailyEntryItemEntity>().ReverseMap();
            CreateMap<PurchaseItemDTO, PurchaseItemEntity>().ReverseMap();
            CreateMap<PurchaseDTO, PurchaseEntity>().ReverseMap();
            CreateMap<FixedCoastDTO, FixedCostEntity>().ReverseMap();
        }
    }
}
