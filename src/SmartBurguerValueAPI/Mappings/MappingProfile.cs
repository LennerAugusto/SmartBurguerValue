using AutoMapper;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.ReadModels;

namespace SmartBurguerValueAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {   /*CategoryProducts*/
            CreateMap<CategoryProductsReadModel, CategoryProductsEntity>().ReverseMap();
            CreateMap<CategoryProductsDTO, CategoryProductsEntity>().ReverseMap();
            CreateMap<BaseDTO, CategoryProductsEntity>().ReverseMap();
            
            /*UnityTypesProducts*/
            CreateMap<BaseDTO, UnityTypesProductsEntity>().ReverseMap();
        }
    }
}
