using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Decko.Models.Login
{
    public class ForgotPassViewModel
    {
        [Display(Name="Email")]
        public string Email { get; set; }
    }
    public class LoginViewModel
    {
        [Display(Name = "User")]
        [Required(ErrorMessage="Username is required")]
        public string User { get; set; }
        [Display(Name = "Pass")]
        [Required(ErrorMessage="Password is required")]
        public string Pass { get; set; }

        public bool RememberMe { get; set; }
    }
    public class RegisterViewModel
    {
        [Display(Name="Email")]
        [Required(ErrorMessage="Email is required")]
        public string Email { get; set; }

        [Display(Name = "User")]
        [Required(ErrorMessage = "Username is required")]
        public string User { get; set; }
        [Display(Name = "Pass")]
        [Required(ErrorMessage = "Password is required")]
        public string Pass { get; set; }
        [Display(Name = "Confirm")]
        [Compare("Pass", ErrorMessage="Passwords do not match")]
        public string PassConfirm { get; set; }
        public bool Accept { get; set; }
    }
}