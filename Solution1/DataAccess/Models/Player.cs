using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Player: ModelBase
    {
        public Player()
        {
            this.Courses = new List<Course>();
        }

        [Required]
        [MinLength(2)]
        public string CollegeId { get; set; }

        [Required]
        [StringLength(512)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(512)]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; } = DateTime.MinValue;

        [Required]
        [Range(1.0, 10.0)]
        public int Semester { get; set; } = 1;

        [Required]
        public string PhotoUrl { get; set; } = "defaultPlayer.png";

        public virtual ICollection<Course> Courses { get; set; }
    }
}
