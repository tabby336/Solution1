using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Mark
    {
        public Guid Id { get; set; }

        public Guid ModuleId { get; set; }
        
        public Guid UserId { get; set; }

        public float Value { get; set; }

        public string Notes { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
