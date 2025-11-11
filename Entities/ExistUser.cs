using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ExistUser
    {
        [EmailAddress, Required]
        public string Email { get; set; }
        [StringLength(8, ErrorMessage = "password Can be between 4 till 8 chars", MinimumLength = 4), Required]
        public string Password { get; set; }
    }
}
