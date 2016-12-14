using System;

namespace DataAccess.Models
{
    public class Mark: ModelBase
    {
        public Guid ModuleId { get; set; }
        
        public Guid UserId { get; set; }

        public float Value { get; set; }

        public string Notes { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
