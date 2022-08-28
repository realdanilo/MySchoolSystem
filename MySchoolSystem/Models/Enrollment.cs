using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int  InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int LetterGradeId { get; set; }
        public LetterGrade Grade { get; set; }

        public bool Dropped { get; set; }

        public string Notes { get; set; }
    }
}
