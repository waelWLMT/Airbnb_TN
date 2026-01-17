using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Shared.Dtos;
using Shared.Enums;

namespace Providers
{
    public class FacebookAuthService : IAuthenticatorService
    {
        public AuthServiceName ServiceName => AuthServiceName.Facebook;

        public Task<AuthResult?> AuthenticateAsync(AuthRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
