using Microsoft.AspNetCore.Mvc;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/ingredient")]
    public class IngredientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public IngredientController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<IngredientDTO>>> GetAllIngredients()
        {
            var Ingredients = await _unityOfWork.IngredientRepository.GetAll();
            return Ok(Ingredients);
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<IngredientDTO>>> GetAllIngredientsByEnterprise(Guid EnterpriseId)
        {
            var Ingredients = await _unityOfWork.IngredientRepository.GetAllIngredientByEnterpriseId(EnterpriseId);
            return Ok(Ingredients);
        }
        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetIngredientById(Guid IngredientId)
        {
            var Ingredient = _unityOfWork.IngredientRepository.GetById(IngredientId);
            return Ok(Ingredient);
        }

        [HttpPost("create")]
        public async Task<ActionResult<IngredientDTO>> CreateIngredient([FromBody] IngredientsEntity ingredient)
        {
            var Ingredient = _unityOfWork.IngredientRepository.Create(ingredient);
            _unityOfWork.Commit();
            return Ok(Ingredient);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] IngredientsEntity ingredient)
        {
            var Ingredient = _unityOfWork.IngredientRepository.Update(ingredient);
            _unityOfWork.Commit();
            return Ok(Ingredient);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Ingredient = await _unityOfWork.IngredientRepository.GetById(id);

            await _unityOfWork.IngredientRepository.Delete(Ingredient);
            _unityOfWork.Commit();
            return Ok(Ingredient);
        }
    }
}
