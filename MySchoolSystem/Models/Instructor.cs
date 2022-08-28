using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

    }
}
