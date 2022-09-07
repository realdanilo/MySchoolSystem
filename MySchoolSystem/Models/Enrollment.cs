using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Course Course { get; set; }
        [Required]
        public Student Student{ get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public Period Period { get; set; } 

        public LetterGrade Grade { get; set; }

        public bool Dropped { get; set; }

        [Display(Name = "Open for enrollment")]
        public bool OpenForEnrollment { get; set; }

        public ICollection<Todo> Todos { get; set; } //check
        [Display(Name = "Submitted Assignments")]
        public ICollection<Submitted_Assignments> Submitted_Assignments { get; set; } //check

        public string Notes { get; set; }

    }
}
