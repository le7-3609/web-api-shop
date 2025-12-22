using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public record RegisterAndUpdateDTO(
        [Required, EmailAddress] 
        string Email, 
        // [StringLength(20, ErrorMessage = "Name can be beteen 2 till 20", MinimumLength = 2)] 
        string FirstName, 
        //[StringLength(20, ErrorMessage = "Name can be beteen 2 till 20", MinimumLength = 2)] 
        string LastName,
        [Phone]
        string Phone,
        [Required]
        string Password
    );
    public record UserProfileDTO(
        int UserId, 
        [Required,EmailAddress] 
        string Email, 
        //[StringLength(20, ErrorMessage = "Name can be beteen 2 till 20", MinimumLength = 2)] 
        string FirstName, 
        //[StringLength(20, ErrorMessage = "Name can be beteen 2 till 20", MinimumLength = 2)] 
        string LastName,
        [Phone]
        string Phone
    );

    public record LoginDTO(
        [Required, EmailAddress]
        string Email,
        [Required]
        string Password
    );
}
