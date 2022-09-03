using System;
using System.ComponentModel.DataAnnotations;

namespace MySchoolSystem.Models
{
    public class Period
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }
        public DateTime Starts { get; set; }
        public DateTime Ends { get; set; }

    }
}
