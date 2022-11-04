using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name ="Role Name")]
        public string RoleName { get; set; }

        #nullable enable
        public string? Id { get; set; }
    }
}
