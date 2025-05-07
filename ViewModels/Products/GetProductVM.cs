using System.ComponentModel.DataAnnotations;

namespace ProniaShop.ViewModels
{
    public class GetProductVM
    {
        public int Id { get; set; }
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(300)]

        public string Description { get; set; }

        public decimal Price { get; set; }
        public string SKU { get; set; }

        //relational
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string MainImage { get; set; }
    }
}
