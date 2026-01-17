using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos;
using Shared.Enums;

namespace Application.Interfaces
{
    public interface IAuthenticatorService
    {
        AuthServiceName ServiceName { get; }
        Task<AuthResult?> AuthenticateAsync(AuthRequest request);
    }
}
