using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.IRepository.IProducts;
using SmartBurguerValueAPI.Migrations;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.ReadModels;

namespace SmartBurguerValueAPI.Controllers
{
    [Route("api/category-products")]
    [ApiController]
    public class CategoryProductsController : ControllerBase
    {
        private readonly ICategoryProducts _repository;

        public CategoryProductsController(ICategoryProducts repository)
        {
            _repository = repository;
        }

        [HttpGet("get-all")]
        public ActionResult<IEnumerable<CategoryProductsReadModel>> GetAllCategories()
        {
            var Categories = _repository.GetAllCategoryProducts();
            return Ok(Categories);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetCategoryById(Guid categoryId)
        {
            var Category = _repository.GetCategoryProductsById(categoryId);
            return Ok(Category);
        }

        [HttpPost("create")]
        public async Task<ActionResult<CategoryProductsDTO>> CreateCategoryProduct(CategoryProductsDTO createCategoryProducts)
        {
            var Category = _repository.CreateCategoryProducts(createCategoryProducts);
            return Ok(Category);
        }


        [HttpPut("update")]
        public ActionResult Put(Guid id, CategoryProductsDTO category)
        {
            var Category = _repository.UpdateCategoryProducts(category);
            return Ok(Category);
        }

        [HttpDelete("delete")]
        public ActionResult Delete(Guid id)
        {
            var Category = _repository.DeleteCategoryProducts(id);
            return Ok(Category);
        }
    }
}

