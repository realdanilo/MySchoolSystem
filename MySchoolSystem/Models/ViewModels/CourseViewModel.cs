using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MySchoolSystem.Models.ViewModels
{
    public class CourseViewModel
    {
        public int? Id { get; set; }
        //public Course Course { get; set; }
        //[Required]
        //public string Subject { get; set; }
        [Required]
        [Range(0,30)]
        public int Credits { get; set; }
        //for view
        public List<SelectListItem> Instructors { get; set; }
        public List<SelectListItem> Subjects { get; set; }

        // view sets instructorId
        [Required]

        public int InstructorId { get; set; }
        [Required]

        public int SubjectId { get; set; }

        public CourseViewModel()
        {
        }
        public CourseViewModel(List<Instructor> instructors, List<Subject> subjects)
        {
            Instructors = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach(Instructor i in instructors)
            {
                Instructors.Add(
                    new SelectListItem()
                        {
                            Value = i.Id.ToString(),
                            Text = String.Concat(i.FirstName," ",i.LastName)
                        }
                    );
            }

            Subjects = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach (Subject i in subjects)
            {
                Subjects.Add(
                    new SelectListItem()
                    {
                        Value = i.Id.ToString(),
                        Text = String.Concat(i.SubjectName)
                    }
                    );
            }
        }
        
    }
}
