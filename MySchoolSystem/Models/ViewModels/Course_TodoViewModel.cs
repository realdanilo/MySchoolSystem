using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MySchoolSystem.Models.ViewModels
{
    public class Course_TodoViewModel
    {
        //verification
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Enter type of assignment:")]
        public string Type { get; set; }

        //rubric dir
        //[Required]
        //[FileExtensions(Extensions ="txt", ErrorMessage = ".txt extension only")]
        public IFormFile Rubric { get; set; }

        //public string FileLocation { get; set; } = "/";

        [Required]
        [Range(0, 100)]
        public int Points { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ExpirationDate { get; set; } = DateTime.Now;

        //instead of ViewBag
        #nullable enable
        public string? Subject { get; set; }
        public List<Todo>? Todos { get; set; }

        public Course_TodoViewModel()
        {

        }
    }
}
