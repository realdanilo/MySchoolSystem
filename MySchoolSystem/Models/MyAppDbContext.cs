using System;
using System.Linq;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Enrollment>()
            //    .HasOne<Course>(e => e.Course)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Submitted_Assignments>()
            //    .HasOne<Todo>(s => s.Task)
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Cascade);

            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.SetNull;
                //will have to change back to restrict, and add property to hide/delete
            }
            //base.OnModelCreating(modelBuilder);
        }
    }
}
