using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.BLL.DtoModels.AccountDtos
{
    public class UserEditDto
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [RegularExpression("^01[0125][0-9]{8}$", ErrorMessage = "invalid Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
