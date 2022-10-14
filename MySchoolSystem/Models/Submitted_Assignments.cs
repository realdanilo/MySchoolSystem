using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Submitted_Assignments
    {
        [Key]
        public int Id { get; set; }

        //uploaded file
        [Required]
        public string FileLocation { get; set; }

        //Graded Assignment points
        public int GradedPoints { get; set; } = -1;

        //submit a task
        public Todo Task { get; set; }

        //submit to specific enrollment 
        public Enrollment Enrollment { get; set; }
        
    }
}
