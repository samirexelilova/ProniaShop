using System.ComponentModel.DataAnnotations;

namespace ProniaShop.ViewModels
{
    public class GetSlideVM
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Title 50 simvoldan az olmalidir")]
        public string Title { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Order minimum 1 olmalidir")]
        public int Order { get; set; }
        public string Image { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
