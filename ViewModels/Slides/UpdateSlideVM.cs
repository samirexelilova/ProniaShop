using System.ComponentModel.DataAnnotations;

namespace ProniaShop.ViewModels
{
    public class UpdateSlideVM
    {
        [MaxLength(50, ErrorMessage = "Title 50 simvoldan az olmalidir")]
        public string Title { get; set; }

        [MaxLength(100, ErrorMessage = "SubTitle 100 simvoldan az olmalidir")]
        public string SubTitle { get; set; }

        [MaxLength(300, ErrorMessage = "Description 300 simvoldan az olmalidir")]
        public string Description { get; set; }
        public string? Image { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Order minimum 1 olmalidir")]
        public int Order { get; set; }
        public IFormFile? Photo { get; set; }


    }
}
