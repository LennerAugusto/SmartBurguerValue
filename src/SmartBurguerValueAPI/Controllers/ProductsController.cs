using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
            private readonly SmartBurguerValueAPIContext _context;

            public ProductsController(SmartBurguerValueAPIContext context)
            {
                _context = context;
            }

            [HttpGet("get-all")]
            public ActionResult<IEnumerable<BaseDTO>> GetAllProducts()
            {
                var clients = _context.Products
                    .ToList();

                if (!clients.Any())
                {
                    return NotFound("Não existem produtos cadastrados");
                }

                return Ok(clients);
            }

            [HttpGet("get-by-id/{id}")]
            public async Task<IActionResult> GetProductById(Guid ProductId)
            {
                var product = _context.Products.FirstOrDefault(c => c.Id != ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }

            [HttpPost("create")]
            public async Task<ActionResult<ProductsDTO>> CreateProduct(ProductsDTO Product)
            {
                Product.Id = Guid.NewGuid();

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                    await _context.Products.AddAsync(new ProductsEntity
                    {
                        Id = Product.Id,
                        Name = Product.Name,
                        CategoryId = Product.CategoryId,
                        UnityTypeId = Product.UnityTypeId,
                        QuantityPerPackage = Product.QuantityPerPackage,
                        ValuePerPackage = Product.ValuePerPackage,
                        UnityValue = Product.UnityValue

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

                return CreatedAtAction(nameof(GetProductById), new { id = Product.Id }, Product);
            }


            [HttpPut("update/{id:guid}")]
            public ActionResult Put(Guid id, ProductsEntity product)
            {
                if (id != product.Id)
                {
                    return BadRequest("Produto solicitado não existe");
                }
                _context.Entry(product).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(product);
                ;
            }

            [HttpDelete("delete/{id:guid}")]
            public ActionResult Delete(Guid id)
            {
                var type = _context.UnityTypesProducts.FirstOrDefault(c => c.Id == id);
                if (type == null)
                {
                    return NotFound("Produto não encontrado");
                }
                _context.UnityTypesProducts.Remove(type);
                _context.SaveChanges();
                return Ok(type);
            }
        }
}
