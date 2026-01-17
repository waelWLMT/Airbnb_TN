using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class AuthResult
    {       
        public Guid UserId { get; set; }
        public string Email { get; set; } = default!;
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public int RoleId { get; set; }
        public RoleDto Role { get; set; } = new();
        public bool IsActive { get; set; }
    }
}
