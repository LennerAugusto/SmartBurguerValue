 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public ProductsController(IUnityOfWork unityOfWork, AppDbContext context)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }

        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProductsByEnterprise( Guid EnterpriseId)
        {
            var Products = await _unityOfWork.ProductRepository.GetAllProductsByEnterpriseId( EnterpriseId);

            return Ok(Products);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {
            var Products = await _unityOfWork.ProductRepository.GetByIdAsync(ProductId);
            return Ok(Products);
        }
        [HttpGet("get-by-id/with-ingredients")]
        public async Task<ProductDTO> GetProductByWhiteIngredientsId(Guid ProductId)
        {
            var Products = await _unityOfWork.ProductRepository.GetByIdWithIngredientsAsync(ProductId);
            return Products;
        }
        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateProduct([FromBody] ProductDTO product)
        {
            var Product = await _unityOfWork.ProductRepository.CreateProductAsync(product);
            await _unityOfWork.CommitAsync();
            return Ok(Product);
        }


        [HttpPut("update/")]
        public async Task<ActionResult<Guid>> Put([FromBody] ProductDTO product)
        {
            await _unityOfWork.ProductRepository.UpdateProductAsync(product);
            await _unityOfWork.CommitAsync();
            return Ok();
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Product = await _unityOfWork.ProductRepository.GetByIdAsync(id);

            await _unityOfWork.ProductRepository.Delete(Product);
            await _unityOfWork.CommitAsync();
            return Ok(Product);
        }
    }
}
