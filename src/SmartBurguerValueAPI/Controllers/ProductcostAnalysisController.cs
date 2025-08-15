
using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;

namespace SmartBurguerValueAPI.Controllers
{
    //[Authorize(Policy = "Enterprise")]
    [Microsoft.AspNetCore.Mvc.Route("api/product-analyses")]
    public class ProductcostAnalysisController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly AppDbContext _context;

        public ProductcostAnalysisController(IUnityOfWork unityOfWork, AppDbContext context)
        {
            _unityOfWork = unityOfWork;
            _context = context;
        }

        [HttpGet("get-analyses/by-product-id")]
        public async Task<ProductCostAnalysisEntity> GetAllAnalyses(Guid productId)
        {
            var Analyse = await _unityOfWork.ProductCostAnalysisRepository.GetAnalyse(productId);
            return Analyse;
        }
    }
}
