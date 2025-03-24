using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/unit-types-products")]
    public class UnitTypesProductsController : ControllerBase
    {
        private readonly SmartBurguerValueAPIContext _context;

        public UnitTypesProductsController(SmartBurguerValueAPIContext context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public ActionResult<IEnumerable<BaseDTO>> GetAllUnityTypesProducts()
        {
            var clients = _context.CategoryProducts
                .ToList();

            if (!clients.Any())
            {
                return NotFound("Não existem tipos de produtos cadastrados");
            }

            return Ok(clients);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetUnityTypeById(Guid UnitTypeId)
        {
            var unity = _context.UnityTypesProducts.FirstOrDefault(c => c.Id != UnitTypeId);
            if (unity == null)
            {
                return NotFound();
            }

            return Ok(unity);
        }

        [HttpPost("create")]
        public async Task<ActionResult<BaseDTO>> CreateUnityType(BaseDTO UnityTypeProducts)
        {
            UnityTypeProducts.Id = Guid.NewGuid();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.UnityTypesProducts.AddAsync(new UnityTypesProductsEntity
                    {
                        Id = UnityTypeProducts.Id,
                        Name = UnityTypeProducts.Name

                    });

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            return CreatedAtAction(nameof(GetUnityTypeById), new { id = UnityTypeProducts.Id }, UnityTypeProducts);
        }


        [HttpPut("update/{id:guid}")]
        public ActionResult Put(Guid id, UnityTypesProductsEntity category)
        {
            if (id != category.Id)
            {
                return BadRequest("Tipo de produto solicitado não existe");
            }
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
            ;
        }

        [HttpDelete("delete/{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var type = _context.UnityTypesProducts.FirstOrDefault(c => c.Id == id);
            if (type == null)
            {
                return NotFound("Tipo de produto não encontrado");
            }
            _context.UnityTypesProducts.Remove(type);
            _context.SaveChanges();
            return Ok(type);
        }
    }
}

