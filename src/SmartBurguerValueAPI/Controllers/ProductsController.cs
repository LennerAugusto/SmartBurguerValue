using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Models.Products;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly SmartBurguerValueAPIContext _context;
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository, SmartBurguerValueAPIContext context)
        {
            _context = context;
            _repository = repository;
        }

        [HttpGet("get-all")]
        public ActionResult<IEnumerable<BaseDTO>> GetAllProducts()
        {
            var Products = _repository.GetAllProducts();
            return Ok(Products);
        }

        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {
            var Products = _repository.GetProductById(ProductId);
            return Ok(Products);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ProductsDTO>> CreateProduct(ProductsDTO Product)
        {
            var UnityTypes = _repository.CreateProduct(Product);
            return Ok(UnityTypes);
        }


        [HttpPut("update/")]
        public ActionResult Put(ProductsDTO product)
        {
            var Product = _repository.CreateProduct(product);
            return Ok(Product);
        }

        [HttpDelete("delete/{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var Product = _repository.DeleteProduct(id);
            return Ok(Product);
        }
    }
}
