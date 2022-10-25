using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MySchoolSystem.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Display(Name = "Choose default account role:")]
        public List<SelectListItem> Roles { get; set; }

        public RegisterViewModel()
        {
        }

        public RegisterViewModel(IEnumerable<IdentityRole> roles)
        {
            foreach (var role in roles)
            {
                Roles.Add( new SelectListItem() { Value = role.Id, Text = role.NormalizedName});
            }
        }
    }
}
