using System.ComponentModel.DataAnnotations;

namespace WebApiShop.Controllers
{
    public class ExistUser
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [StringLength(8, ErrorMessage = "password Can be between 4 till 8 chars", MinimumLength = 4), Required]
        public string Password { get; set; }
    }
}
