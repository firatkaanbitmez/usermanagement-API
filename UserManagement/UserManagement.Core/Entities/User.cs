using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Core.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        public DateTime DateAdded { get; set; }
        public bool IsActive { get; set; }
    }
}
