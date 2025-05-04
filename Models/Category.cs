using System.ComponentModel.DataAnnotations;

namespace ProniaShop.Models
{
    public class Category:BaseEntity
    {
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        //relational
        public List<Product>? Products { get; set; }
    }
}
