﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("Products")]
    public class ProductsEntity : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? DesiredMargin { get; set; } 
        public decimal? CMV { get; set; } 
        public decimal? CPV { get; set; } 
        public decimal? SuggestedPrice { get; set; }
        public Guid EnterpriseId { get; set; }
        public EnterpriseEntity Enterprise { get; set; }
        [JsonIgnore]
        public ICollection<ProductsIngredientsEntity> ProductIngredients { get; set; }
        [JsonIgnore]
        public ICollection<ComboProductEntity> ComboProducts { get; set; }
    }
}
