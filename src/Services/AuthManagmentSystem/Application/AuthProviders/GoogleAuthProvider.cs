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
    public class GoogleAuthProvider : IAuthenticatorService
    {
        public AuthServiceName ServiceName => AuthServiceName.Google;

        public async Task<AuthResult?> AuthenticateAsync(AuthRequest request)
        {
            throw new NotImplementedException("authenticate by google n'est pas encore implementé");

            // 1. Valider token Google
            // 2. Récupérer email
            // 3. Trouver ou créer user

        }
    }
}
