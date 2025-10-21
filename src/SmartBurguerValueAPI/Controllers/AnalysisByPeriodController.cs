using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<BestSellingProductsDTO>>> GetCardsBestSellingProducts([FromBody] AnalysisRequestDTO analysis)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetCardsBestSellingProductsByEnterpriseId(analysis.Period, analysis.EnterpriseId);
            return Ok(Products);
        }
        [HttpPost("invoicing/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<InvoicingSeriesDTO>>> GetInvoicing([FromBody] AnalysisRequestDTO analysis)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetInvoicingByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Products);
        }
        [HttpPost("total-orders/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<TotalOrdersDTO>>> GetTotalOrders([FromBody] AnalysisRequestDTO analysis)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetTotalOrdersByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Products);
        }
        [HttpPost("get-margin-and-profit-products/by-enterprise-id")]
        public async Task<ActionResult<GetAnalysisCardsProductsDTO>> GetMarginAndProfitProduct([FromBody] AnalysisRequestDTO analysis)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetMarginAndProfitProductByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Products);
        }
        [HttpPost("get-margin-and-profit-combos/by-enterprise-id")]
        public async Task<ActionResult<GetAnalysisCardsProductsDTO>> GetMarginAndProfitCombo([FromBody] AnalysisRequestDTO analysis)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetMarginAndProfitComboByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Products);
        }
        [HttpPost("get-best-selling-combos/by-enterprise-id")]
        public async Task<ActionResult<List<BestSellingProductsByPeriodDTO>>> GetBestSellingCombos([FromBody] AnalysisRequestDTO analysis)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetBestSellingCombosByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Products);
        }
        [HttpPost("get-best-selling-products/by-enterprise-id")]
        public async Task<ActionResult<List<BestSellingProductsByPeriodDTO>>> GetBestSellingProducts([FromBody] AnalysisRequestDTO analysis)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetBestSellingProductsByPeriod(analysis.Period, analysis.EnterpriseId);
            return Ok(Products);
        }
        [HttpPost("products-best-margin/by-enterprise-id")]
        public async Task<ActionResult<List<ProductsBestMarginDTO>>> GetProductsBestMargin(Guid EnterpriseId)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetProductsBestMargin(EnterpriseId);
            return Ok(Products);
        }
        [HttpPost("combos-best-margin/by-enterprise-id")]
        public async Task<ActionResult<List<ProductsBestMarginDTO>>> GetCombosBestMargin(Guid EnterpriseId)
        {
            var Products = await _unityOfWork.AnalysisByPeriodRepository.GetCombosBestMargin(EnterpriseId);
            return Ok(Products);
        }
    }
}