using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Submitted_Assignments
    {
        [Key]
        public int Id { get; set; }

        //submit a task
        public Todo Task { get; set; }

        //submit to specific enrollment 
        public Enrollment Enrollment { get; set; }
        //uploaded file
        public string FileLocation { get; set; }


    }
}
