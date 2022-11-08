using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models.ViewModels
{
    public class ResetPasswordViewModel
    {
        //the user to change the password
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "New passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "If Admin, click true")]
        public bool IsAdmin { get; set; } = false;
    }
}
