using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MySchoolSystem.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public int? Id { get; set; }

        public int CourseId { get; set; }
        public List<SelectListItem> Courses { get; set; }

        public int StudentId { get; set; }
        public List<SelectListItem> Students { get; set; }

        public int PeriodId { get; set; }
        public List<SelectListItem> Period { get; set; }

        public int Year { get; set; }

        public int? GradeId { get; set; }
        public List<SelectListItem> Grade { get; set; }

        public bool Dropped { get; set; }
        [Display(Name = "Open for enrollment")]
        public bool OpenForEnrollment { get; set; }

        public string Notes { get; set; }
        //task
        //submitted_assignments
        public EnrollmentViewModel()
        {

        }
        public EnrollmentViewModel(List<Course> courses, List<Student> students, List<Period> periods, List<LetterGrade> grades)
        {
            Courses = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach (Course i in courses)
            {
                Courses.Add(
                        new SelectListItem()
                        {
                            Value = i.Id.ToString(),
                            Text = String.Concat(i.Subject.SubjectName + " - " + i.Instructor.FirstName + " " + i.Instructor.LastName)
                        }
                    );
            }
            Students = new List<SelectListItem>() { new SelectListItem { Value = "", Text = "" } };
            foreach (Student i in students)
            {
                Students.Add(
                        new SelectListItem()
                        {
                            Value = i.Id.ToString(),
                            Text = String.Concat(i.FirstName + " " + i.LastName)
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
