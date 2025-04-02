using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.IRepository.IProducts;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/unit-types-products")]
    public class UnitTypesProductsController : ControllerBase
    {
        private readonly SmartBurguerValueAPIContext _context;
        private readonly IUnityTypesRepository _repository;

        public UnitTypesProductsController(SmartBurguerValueAPIContext context, IUnityTypesRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        [HttpGet("get-all")]
        public ActionResult<IEnumerable<BaseDTO>> GetAllUnityTypesProducts()
        {
            var UnityTypes = _repository.GetAllUnityTypesProducts();
            return Ok(UnityTypes);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetUnityTypeById(Guid unityTypeId)
        {
            var UnityTypes = _repository.GetUnityTypeProductsById(unityTypeId);
            return Ok(UnityTypes);
        }

        [HttpPost("create")]
        public async Task<ActionResult<BaseDTO>> CreateUnityType([FromBody] BaseDTO UnityTypeProducts)
        {
            var UnityType = _repository.CreateUnityTypesProducts(UnityTypeProducts);
            return Ok(UnityType);
        }


        [HttpPut("update/")]
        public ActionResult Put(BaseDTO unityTypeProducts)
        {
            var UnityType = _repository.UpdateUnityTypesProducts(unityTypeProducts);
            return Ok(UnityType);
        }

        [HttpDelete("delete/{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var UnityType = _repository.DeleteUnityTypeProducts(id);
            return Ok(UnityType);
        }
    }
}

