using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

        public new DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}
