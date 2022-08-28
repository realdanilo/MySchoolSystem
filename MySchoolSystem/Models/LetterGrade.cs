using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class LetterGrade
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Letter Grade")]
        public string Grade { get; set; }

        public float Weight { get; set; }

    }
}
