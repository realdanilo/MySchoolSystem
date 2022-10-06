using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Todo
    {
       [Key]
        public int  Id { get; set; }

        //hw, test, quiz
        public string Type { get; set; }

        //rubric, file from instructor
        public string Rubric { get; set; }

        //uploaded file from student >> MOVED TO SUBMITTED_ASSIGMENTS
        //[Required]
        //public string FileLocation { get; set; } 

        //check
        [Required]
        [Range(0,100)]
        public int  Points { get; set; }

        [Display (Name = "Due Date")]
        public DateTime ExpirationDate { get; set; }

    }
}
