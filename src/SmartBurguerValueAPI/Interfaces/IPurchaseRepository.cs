using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IRepositoryBase;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Interfaces
{
    public interface IPurchaseRepository : IRepositoryBase<PurchaseEntity>
    {
        Task <List<PurchaseDTO>> GetAllPurchasesByEnterpriseId(Guid EnterpriseId);
        Task <PurchaseDTO> GetPurchaseById(Guid EnterpriseId);
        Task<PurchaseEntity> CreatePurchase(PurchaseDTO dto);
        Task<PurchaseEntity> CreatePurchaseByXml(IFormFile file, Guid enterpriseId);
        Task UpdatePurchaseAsync(PurchaseDTO dto);
    }
}
