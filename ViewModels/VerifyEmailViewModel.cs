﻿using System.ComponentModel.DataAnnotations;

namespace Bhomes_ERP.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; } = "";


    }
}
