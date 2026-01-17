using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Shared.Dtos;

namespace Application.Services
{
    public class AuthOrchestrator : IAuthOrchestrator
    {
        private readonly IEnumerable<IAuthenticatorService> _providers;

        public AuthOrchestrator(IEnumerable<IAuthenticatorService> providers)
        {
            _providers = providers;
        }

        public async Task<AuthResult?> AuthenticateAsync(AuthRequest request)
        {
            var provider = _providers
                .FirstOrDefault(p => p.ServiceName == request.ServiceName);

            if (provider == null)
                throw new NotSupportedException("Auth provider not supported");

            return await provider.AuthenticateAsync(request);
        }
    
    }

}
