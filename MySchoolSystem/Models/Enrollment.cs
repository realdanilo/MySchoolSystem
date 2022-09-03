using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        //public int StudentId { get; set; }
        //public Student Student { get; set; }

        //public int  InstructorId { get; set; }
        //public Instructor Instructor { get; set; }

        //public int CourseId { get; set; }
        //public Course Course { get; set; }

        //public int LetterGradeId { get; set; }
        //public LetterGrade Grade { get; set; }

        //public bool Dropped { get; set; }

        //refac
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

        public ICollection<Task> Tasks { get; set; } //check,

        public string Notes { get; set; }

    }
}
