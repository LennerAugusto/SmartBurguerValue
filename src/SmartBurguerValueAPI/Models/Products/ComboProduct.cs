using System.ComponentModel.DataAnnotations.Schema;

namespace SmartBurguerValueAPI.Models.Products
{
    [Table("ComboProduct")]
    public class ComboProduct : BaseEntity
    {
        public Guid ComboId { get; set; }
        public ComboEntity Combo { get; set; }
        public Guid ProductId { get; set; }
        public ProductsEntity Product { get; set; }
    }
}
