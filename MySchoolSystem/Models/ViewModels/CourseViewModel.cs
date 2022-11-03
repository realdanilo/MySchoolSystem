using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MySchoolSystem.Models.ViewModels
{
    public class CourseViewModel
    {
        public int? Id { get; set; }
        
        //for view
        public List<SelectListItem> Instructors { get; set; }
        public List<SelectListItem> Subjects { get; set; }
        public List<SelectListItem> Period { get; set; }

        // view sets instructorId
        [Required]
        public string InstructorId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int PeriodId { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Max # of students")]
        public int MaxNumberStudents { get; set; }
        [Display(Name = "Open for enrollment")]
        public bool OpenForEnrollment { get; set; } = true;
        [Required]
        [Range(0, 30)]
        public int Credits { get; set; }
        public string Notes { get; set; }
        public bool Dropped { get; set; }


        public CourseViewModel()
        {
        }
        public CourseViewModel(IEnumerable<CustomIdentityUser> instructors, List<Subject> subjects, List<Period> periods)
        {
            Instructors = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach(CustomIdentityUser i in instructors)
            {
                Instructors.Add(
                    new SelectListItem()
                        {
                            Value = i.Id.ToString(),
                            Text = String.Concat(i.FirstName, " ", i.LastName)
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
            Period = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach (Period i in periods)
            {
                Period.Add(
                        new SelectListItem()
                        {
                            Value = i.Id.ToString(),
                            Text = i.SeasonName
                        }
                    );
            }
        }
        
    }
}
