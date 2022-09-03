using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Subject")]
        public string SubjectName { get; set; }
    }
}
