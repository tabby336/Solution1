using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess
{
    public class Mark
    {
        public Guid HomeworkId { get; set; }
        
        public Guid StudentId { get; set; }

        public float Value { get; set; }

        public string Review { get; set; }
    }
}
