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
    }
}
