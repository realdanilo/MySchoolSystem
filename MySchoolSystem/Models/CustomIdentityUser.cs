using System;
using Microsoft.AspNetCore.Identity;

namespace MySchoolSystem.Models
{
    public class CustomIdentityUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
    }
}
