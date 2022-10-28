using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MySchoolSystem.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public int? Id { get; set; }

        public List<SelectListItem> Courses { get; set; }
        public List<SelectListItem> Students { get; set; }
        public List<SelectListItem> Grade { get; set; }

        public int CourseId { get; set; }
        public string StudentId { get; set; }
        public int? GradeId { get; set; }

        public bool Dropped { get; set; }
        public string Notes { get; set; }

        public EnrollmentViewModel()
        {
        }

        public EnrollmentViewModel(List<Course> courses, IEnumerable<CustomIdentityUser> students, List<LetterGrade> grades)
        {
            Courses = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach (Course i in courses)
            {
                Courses.Add(
                        new SelectListItem()
                        {
                            Value = i.Id.ToString(),
                            Text = String.Concat(i.Subject.SubjectName + " - " + i.Instructor.NormalizedUserName)
                        }
                    );
            }
            Students = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach (CustomIdentityUser i in students)
            {
                Students.Add(
                        new SelectListItem()
                        {
                            Value = i.Id.ToString(),
                            Text = String.Concat(i.NormalizedUserName + " " + i.Email)
                        }
                    );
            }
            
            Grade = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach (LetterGrade i in grades)
            {
                Grade.Add(
                        new SelectListItem()
                        {
                            Value = i.Id.ToString(),
                            Text = i.Grade
                        }
                    );
            }
        }
    }
}
