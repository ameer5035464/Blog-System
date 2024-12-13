﻿using System.ComponentModel.DataAnnotations;

namespace BlogSystem.BLL.DtoModels.AccountDtos
{
    public class UserLoginDto
    {
        [Required(ErrorMessage ="Email field is required")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
