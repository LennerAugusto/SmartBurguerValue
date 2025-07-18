﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;

namespace SmartBurguerValueAPI.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/ingredient")]
    public class IngredientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _unityOfWork;

        public IngredientController(AppDbContext context, IUnityOfWork unityOfWork)
        {
            _context = context;
            _unityOfWork = unityOfWork;
        }
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<IngredientDTO>>> GetAllIngredients()
        {
            var Ingredients = await _unityOfWork.IngredientRepository.GetAllAsync();
            return Ok(Ingredients);
        }
        [HttpGet("get-all/by-enterprise-id")]
        public async Task<ActionResult<IEnumerable<IngredientDTO>>> GetAllIngredientByEnterpriseId(Guid enterpriseId)
        {
            var ingredients = await _unityOfWork.IngredientRepository.GetAllIngredientByEnterpriseId(enterpriseId);

            return Ok(ingredients);
        }
        [HttpGet("get-by-id/")]
        public async Task<IActionResult> GetIngredientById(Guid IngredientId)
        {
            var Ingredient = await _unityOfWork.IngredientRepository.GetByIdAsync(IngredientId);
            return Ok(Ingredient);
        }

        [HttpPost("create")]
        public async Task<ActionResult<IngredientDTO>> CreateIngredient([FromBody] IngredientsEntity ingredient)
        {
            Console.WriteLine(ingredient);
            var Ingredient = await _unityOfWork.IngredientRepository.CreateIngredient(ingredient);
            await _unityOfWork.CommitAsync();
            return Ok(Ingredient);
        }


        [HttpPut("update/")]
        public async Task<IActionResult> Put([FromBody] IngredientsEntity ingredient)
        {
            _unityOfWork.IngredientRepository.Update(ingredient);
            await _unityOfWork.CommitAsync();

            var updatedIngredient = await _unityOfWork.IngredientRepository.GetByIdAsync(ingredient.Id);
            return Ok(updatedIngredient);
        }


        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var Ingredient = await _unityOfWork.IngredientRepository.GetByIdAsync(id);

            await _unityOfWork.IngredientRepository.Delete(Ingredient);
            await _unityOfWork.CommitAsync();
            return Ok(Ingredient);
        }
    }
}
