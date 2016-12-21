using System;

namespace DataAccess.Models
{
    public class Mark: ModelBase
    {
        public Guid HomeworkId { get; set; }
        
        public Guid UserId { get; set; }

        public float Value { get; set; }

        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid CreatorId { get; set; }

        public bool HasComment { get; set; }

        public bool HasContestation { get; set; }
    }
}
