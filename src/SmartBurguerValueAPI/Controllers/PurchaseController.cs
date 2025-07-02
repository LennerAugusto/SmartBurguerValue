using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/purchase")]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public PurchaseController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllPurchaseByEnterprise(PaginationParamiters paramiters, Guid EnterpriseId)
        {
            var Purchases = await _unityOfWork.PurchaseRepository.GetAllPurchasesByEnterpriseId(paramiters, EnterpriseId);

            var metadata = new
            {
                Purchases.TotalCount,
                Purchases.PageSize,
                Purchases.CurrentPage,
                Purchases.TotalPages,
                Purchases.HasNext,
                Purchases.HasPrevius
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(Purchases);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetPurchaseById(Guid PurchaseId)
        {
            var Purchase = _unityOfWork.PurchaseRepository.GetByIdAsync(PurchaseId);
            return Ok(Purchase);
        }

        [HttpPost("create")]
        public async Task<ActionResult<PurchaseDTO>> CreatePurchase([FromBody] PurchaseEntity purchase)
        {
            var Purchase = _unityOfWork.PurchaseRepository.Create(purchase);
            await _unityOfWork.CommitAsync();
            return Ok(Purchase);
        }


        [HttpPut("update/")]
        public async Task<ActionResult> Put([FromBody] PurchaseEntity purchase)
        {
            _unityOfWork.PurchaseRepository.Update(purchase);
            await _unityOfWork.CommitAsync();
            return Ok(purchase);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Purchase = await _unityOfWork.PurchaseRepository.GetByIdAsync(id);

            await _unityOfWork.PurchaseRepository.Delete(Purchase);
            await _unityOfWork.CommitAsync();
            return Ok(Purchase);
        }
    }
}
