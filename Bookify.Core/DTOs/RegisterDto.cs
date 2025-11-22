using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Core.DTOs
{
    public class RegisterDto
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
       
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Role { get; set; }
        
        
        [Required]
        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
