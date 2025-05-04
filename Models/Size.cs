using System.ComponentModel.DataAnnotations;

namespace ProniaShop.Models
{
    public class Size:BaseEntity
    {
        [MaxLength(25,ErrorMessage ="Uzunluq 25 i kece bilmez")]
        public string Name { get; set; }

    }
}
