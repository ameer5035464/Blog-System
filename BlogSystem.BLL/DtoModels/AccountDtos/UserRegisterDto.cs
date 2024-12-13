using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL.DtoModels.AccountDtos
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage ="First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression("^01[0125][0-9]{8}$", ErrorMessage = "invalid Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z]).*$", ErrorMessage = "Password must contain at least one uppercase letter.")]
        public string Password { get; set; }

        public IFormFile? Image { get; set; }
    }
}
