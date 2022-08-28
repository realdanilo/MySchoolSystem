using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Course
    {
        public Course()
        {
            this.CreatedAt = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
        public DateTime CreatedAt { get;  }
        public DateTime LastUpdated { get; set; }
    }
}
