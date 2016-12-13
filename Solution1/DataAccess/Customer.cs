using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess
{
    public class Customer
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

    }
}
