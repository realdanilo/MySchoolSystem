using System;
using Microsoft.EntityFrameworkCore;
namespace MySchoolSystem.Models
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<LetterGrade> LetterGrades { get; set; }

    }
}
