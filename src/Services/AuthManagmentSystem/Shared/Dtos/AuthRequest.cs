using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Enums;

namespace Shared.Dtos
{
    public class AuthRequest
    {
        public AuthServiceName ServiceName { get; set; }   // "Jwt", "Google", "Facebook"
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ExternalToken { get; set; }
    }
}
