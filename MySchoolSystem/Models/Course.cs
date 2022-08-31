using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        public string Subject { get; set; }

        [Required]
        public int Credits { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Last Update")]
        public DateTime LastUpdated { get; set; }

       [Required]
        public Instructor Instructor { get; set; }

    }
}
