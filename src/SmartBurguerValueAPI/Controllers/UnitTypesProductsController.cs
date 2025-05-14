using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Repository;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/unit-types")]
    public class UnitTypesProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public UnitTypesProductsController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<UnityTypesDTO>>> GetAllUnityTypes()
        {
            var UnityTypes = await _unityOfWork.UnityTypesRepository.GetAll();
            return Ok(UnityTypes);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetUnityTypeById(Guid unityTypeId)
        {
            var UnityTypes = await _unityOfWork.UnityTypesRepository.GetById(unityTypeId);
            return Ok(UnityTypes);
        }

        [HttpPost("create")]
        public async Task<ActionResult<UnityTypesDTO>> CreateUnityType([FromBody] UnityTypesProductsEntity unityType)
        {
            var UnityType =  _unityOfWork.UnityTypesRepository.Create(unityType);
            _unityOfWork.Commit();
            return Ok(UnityType);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody]UnityTypesProductsEntity unityType)
        {
            var UnityType = _unityOfWork.UnityTypesRepository.Update(unityType);
            _unityOfWork.Commit();
            return Ok(UnityType);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Entity = await _unityOfWork.UnityTypesRepository.GetById(id);
            
            if (Entity == null)
                return NotFound();
            
            await _unityOfWork.UnityTypesRepository.Delete(Entity);
            _unityOfWork.Commit();
            return Ok(Entity);
        }
    }
}

