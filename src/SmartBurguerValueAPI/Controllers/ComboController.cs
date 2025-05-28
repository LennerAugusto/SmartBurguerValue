using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/combo")]
    public class ComboController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public ComboController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<ComboDTO>>> GetAllCombos()
        {
            var Combos = await _unityOfWork.ComboRepository.GetAllAsync();
            return Ok(Combos);
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<ComboDTO>>> GetAllComboByEnterprise(PaginationParamiters paramiters, Guid EnterpriseId)
        {
            var Combos= await _unityOfWork.ComboRepository.GetAllCombosByEnterpriseIdAsync(paramiters, EnterpriseId);

            var metadata = new
            {
                Combos.TotalCount,
                Combos.PageSize,
                Combos.CurrentPage,
                Combos.TotalPages,
                Combos.HasNext,
                Combos.HasPrevius
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(Combos);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetComboById(Guid comboId)
        {
            var Combo = _unityOfWork.ProductRepository.GetByIdAsync(comboId);
            return Ok(Combo);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ComboDTO>> CreateCombo([FromBody] ComboDTO combo)
        {
            var Combo = _unityOfWork.ComboRepository.CreateComboAsync(combo);
            await _unityOfWork.CommitAsync();
            return Ok(Combo);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] ComboDTO combo)
        {
            var Combo = _unityOfWork.ComboRepository.CreateComboAsync(combo);
            _unityOfWork.CommitAsync();
            return Ok(Combo);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Combo = await _unityOfWork.ComboRepository.GetByIdAsync(id);

            await _unityOfWork.ComboRepository.Delete(Combo);
            await _unityOfWork.CommitAsync();
            return Ok(Combo);
        }
    }
}
