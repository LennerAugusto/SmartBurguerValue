using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;

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

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<BaseDTO>>> GetAllProducts()
        {
            var Products = await _unityOfWork.ProductRepository.GetAll();
            return Ok(Products);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {
            var Products = _unityOfWork.ProductRepository.GetById(ProductId);
            return Ok(Products);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ProductsDTO>> CreateProduct([FromBody] ProductsEntity Product)
        {
            var UnityTypes = _unityOfWork.ProductRepository.Create(Product);
            _unityOfWork.Commit();
            return Ok(UnityTypes);
        }


        [HttpPut("update/")]
        public ActionResult Put([FromBody] ProductsEntity product)
        {
            var Product = _unityOfWork.ProductRepository.Update(product);
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
