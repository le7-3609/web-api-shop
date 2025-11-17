using System.ComponentModel.DataAnnotations;

namespace WebApiShop.Controllers
{
    public class ExistUser
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [StringLength(8, ErrorMessage = "Password can be between 4 to 8 characters", MinimumLength = 4), Required]
        public string Password { get; set; }
    }
}
