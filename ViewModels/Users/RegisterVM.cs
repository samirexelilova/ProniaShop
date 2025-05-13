using System.ComponentModel.DataAnnotations;

namespace ProniaShop.ViewModels
{
    public class RegisterVM
    {
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Surname { get; set; }
        [MaxLength(128)]
        public string UserName { get; set; }
        [MaxLength(256)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    } 
}
