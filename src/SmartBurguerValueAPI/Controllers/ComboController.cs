using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;

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
        public async Task<ActionResult<IEnumerable<ComboDTO>>> GetAllComboByEnterprise(Guid EnterpriseId)
        {
            var Combos = await _unityOfWork.ComboRepository.GetAllCombosByEnterpriseId(EnterpriseId);
            return Ok(Combos);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetComboById(Guid comboId)
        {
            var Combo = _unityOfWork.ProductRepository.GetById(comboId);
            return Ok(Combo);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ComboDTO>> CreateCombo([FromBody] ComboDTO combo)
        {
            var Combo = _unityOfWork.ComboRepository.CreateComboAsync(combo);
            _unityOfWork.Commit();
            return Ok(Combo);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] ComboDTO combo)
        {
            var Combo = _unityOfWork.ComboRepository.CreateComboAsync(combo);
            _unityOfWork.Commit();
            return Ok(Combo);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Combo = await _unityOfWork.ComboRepository.GetById(id);

            await _unityOfWork.ComboRepository.Delete(Combo);
            _unityOfWork.Commit();
            return Ok(Combo);
        }
    }
}
