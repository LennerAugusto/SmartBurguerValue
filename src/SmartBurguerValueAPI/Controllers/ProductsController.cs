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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProductsByEnterprise(PaginationParamiters paramiters, Guid EnterpriseId)
        {
            var Products = await _unityOfWork.ProductRepository.GetAllProductsByEnterpriseId(paramiters, EnterpriseId);

            var metadata = new
            {
                Products.TotalCount,
                Products.PageSize,
                Products.CurrentPage,
                Products.TotalPages,
                Products.HasNext,
                Products.HasPrevius
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(Products);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {
            var Products = _unityOfWork.ProductRepository.GetById(ProductId);
            return Ok(Products);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductDTO product)
        {
            var Product = _unityOfWork.ProductRepository.CreateProductAsync(product);
            _unityOfWork.Commit();
            return Ok(Product);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] ProductDTO product)
        {
            var Product = _unityOfWork.ProductRepository.UpdateProductAsync(product);
            _unityOfWork.Commit();
            return Ok(Product);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Product = await _unityOfWork.ProductRepository.GetById(id);

            await _unityOfWork.ProductRepository.Delete(Product);
            _unityOfWork.Commit();
            return Ok(Product);
        }
    }
}
