using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Varsayılan değer atama
        public string Email { get; set; } = string.Empty; // Varsayılan değer atama
        public DateTime DateAdded { get; set; }
    }
}
