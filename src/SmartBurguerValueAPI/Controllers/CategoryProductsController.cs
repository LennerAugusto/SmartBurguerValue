using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.ReadModels;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/category-products")]
    [ApiController]
    public class CategoryProductsController : ControllerBase
    {
        private readonly SmartBurguerValueAPIContext _context;

        public CategoryProductsController(SmartBurguerValueAPIContext context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public ActionResult<IEnumerable<CategoryProductsReadModel>> GetAllCategoryProducts()
        {
            var categories = _context.CategoryProducts
                .Select(c=> new CategoryProductsReadModel
                { 
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

            if (!categories.Any())
            {
                return NotFound("Não existem categorias de produtos cadastradas");
            }

            return Ok(categories);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetCategoryById(Guid CategoryId)
        {
            var category = _context.CategoryProducts.FirstOrDefault(c => c.Id != CategoryId);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost("create")]
        public async Task<ActionResult<CategoryProductsDTO>> CreateCategoryProduct(CategoryProductsDTO CreateCategoryProducts)
        {
            CreateCategoryProducts.Id = Guid.NewGuid();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.CategoryProducts.AddAsync(new CategoryProductsEntity
                    {
                        Id = CreateCategoryProducts.Id,
                        Name = CreateCategoryProducts.Name,
                        IsActive = true

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

            return CreatedAtAction(nameof(GetCategoryById), new { id = CreateCategoryProducts.Id }, CreateCategoryProducts);
        }


        [HttpPut("update")]
        public ActionResult Put(Guid id, CategoryProductsDTO category)
        {
            if (id != category.Id)
            {
                return BadRequest("Categoria solicitada não existe");
            }
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(category);
            ;
        }

        [HttpDelete("delete")]
        public ActionResult Delete(Guid id)
        {
            var client = _context.CategoryProducts.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound("Categoria não encontrado");
            }
            _context.CategoryProducts.Remove(client);
            _context.SaveChanges();
            return Ok(client);
        }
    }
}

