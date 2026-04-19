using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace DTO
{
    //public record PasswordDTO 
    //{
    //    [Required]
    //    public string Password { get; set; }
    //    public int Strength { get; set; }
    //}

    public record PasswordStrengthDTO
    {
        public int Strength { get; set; }
    }
}
