using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Constants;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Analysis;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;

namespace SmartBurguerValueAPI.Controllers
{
    //[Authorize(Policy = "Enterprise")]
    [Microsoft.AspNetCore.Mvc.Route("api/analysis")]
    public class AnalysisByPeriodController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public AnalysisByPeriodController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }

        [HttpPost("analysis-home/get-all")]
        public async Task<ActionResult<IEnumerable<InitialAnalysiDTO>>> GetAnalyseHome([FromBody]AnalysisRequestDTO analysis)
        {
            var Combos = await _unityOfWork.AnalysisByPeriodRepository.GetInitialAnalysisByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Combos);
        }
        [HttpPost("best-selling-products/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<BestSellingProductsDTO>>> GetBestSellingProducts([FromBody] AnalysisRequestDTO analysis)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetBestSellingProductsByEnterpriseId(analysis.Period, analysis.EnterpriseId);
            return Ok(Products);
        }
    }
}