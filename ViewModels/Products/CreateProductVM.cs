using ProniaShop.Models;
using System.ComponentModel.DataAnnotations;

namespace ProniaShop.ViewModels
{
    public class CreateProductVM
    {
        [MaxLength(50, ErrorMessage = "Name 50 simvoldan az olmalidir")]
        public string Name { get; set; }

        [MaxLength(300, ErrorMessage = "Description 50 simvoldan az olmalidir")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price minimum 1 olmalidir")]
        public decimal? Price { get; set; }
        [MaxLength(10, ErrorMessage = "SKU 10 simvoldan az olmalidir")]

        public string SKU { get; set; }

        //relational
        [Required]
        public int? CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
        public IFormFile MainPhoto { get; set; }
    }
}
