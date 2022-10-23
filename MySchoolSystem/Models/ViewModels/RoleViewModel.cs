using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models.ViewModels
{
    public class RoleViewModel
    {
        public string? Id { get; set; }
        [Required]
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }
    }
}
