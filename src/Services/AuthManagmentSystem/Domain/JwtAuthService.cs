using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Clients;
using Shared.Dtos;
using Shared.Enums;

namespace Providers
{
    public class JwtAuthService : IAuthenticatorService
    {
        public AuthServiceName ServiceName => AuthServiceName.Jwt;

        private readonly IUserManagementClient _userClient;

        public JwtAuthService(IUserManagementClient userManagmentClient)
        {
            _userClient = userManagmentClient;
        }

        public async Task<AuthResult?> AuthenticateAsync(AuthRequest request)
        {
            return await _userClient.ValidateCredentials(request.Email!, request.Password!);
        }
    }
}
