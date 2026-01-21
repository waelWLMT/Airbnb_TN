using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Shared.Dtos;
using Shared.Enums;

namespace Application.AuthProviders
{
    public class JwtAuthProvider : IAuthenticatorService
    {
        public AuthServiceName ServiceName => AuthServiceName.Jwt;

        private readonly IUserManagementHttpClient _userClient;

        public JwtAuthProvider(IUserManagementHttpClient userManagmentClient)
        {
            _userClient = userManagmentClient;
        }

        public async Task<AuthResult?> AuthenticateAsync(AuthRequest request)
        {
            return await _userClient.ValidateCredentials(request.Email!, request.Password!);
        }
    }
}
