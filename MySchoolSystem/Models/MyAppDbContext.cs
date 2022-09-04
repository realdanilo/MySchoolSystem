using System;
using Microsoft.EntityFrameworkCore;
namespace MySchoolSystem.Models
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {
        }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Todo> Tasks { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<LetterGrade> LetterGrades { get; set; }
        public DbSet<Submitted_Assignments> Submitted_Assignments { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging();
        //}
    }
}
