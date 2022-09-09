using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models.ViewModels
{
    public class Course_TodoViewModel
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        [Display(Name = "Enter type of assignment:")]
        public string Type { get; set; }

        //rubric dir
        [Required]
        public string Rubric { get; set; }

        public string FileLocation { get; set; } = "/";

        [Required]
        [Range(0, 100)]
        public int Points { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ExpirationDate { get; set; } = DateTime.Now;

        public List<Todo>? Todos { get; set; }

        public Course_TodoViewModel()
        {

        }
    }
}
