using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace MySchoolSystem.Models
{
    public class MyAppDbContext : IdentityDbContext<CustomIdentityUser>
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
        {
        }
        //public DbSet<Instructor> Instructors { get; set; }
        //public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Todo> Tasks { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<LetterGrade> LetterGrades { get; set; }
        public DbSet<Submitted_Assignments> Submitted_Assignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //for identity framework, mapping
            base.OnModelCreating(modelBuilder);
            //var role = new IdentityRole (){ Name = "Admin", NormalizedName = "ADMIN" };
            List<IdentityRole> defaultRoles = new List<IdentityRole>()
            {
                new IdentityRole (){ Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole (){ Name = "Student", NormalizedName = "STUDENT" },
                new IdentityRole (){ Name = "Instructor", NormalizedName = "INSTRUCTOR" },

            };
            modelBuilder.Entity<IdentityRole>().HasData(defaultRoles);

            //var hasher = new PasswordHasher<IdentityUser>();

            //var user = new IdentityUser()
            //{
            //    UserName = "Admin",
            //    NormalizedUserName="ADMIN",
            //    Email = "admin@gmail.om",
            //    EmailConfirmed = true,
            //    PasswordHash = hasher.HashPassword(null, "Password1@")
            //};
            //modelBuilder.Entity<IdentityUser>().HasData(user);

            //modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            //new IdentityUserRole<string>
            //{
            //    RoleId = role.Id,
            //    UserId = user.Id
            //}
            //);

            //foreach (var relationship in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned()).SelectMany(e => e.GetForeignKeys()))
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
                //will have to change back to restrict, and add property to hide/delete 
            }
        }
    }
}
