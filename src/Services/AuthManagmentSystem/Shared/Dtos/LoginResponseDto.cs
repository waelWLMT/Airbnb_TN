using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class LoginResponseDto
    {
        public AuthResult User { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
