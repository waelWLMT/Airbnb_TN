using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Shared.Dtos;

namespace Application.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        private readonly ITokenGenerator _tokenGenerator;

        public AuthTokenService(ITokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(AuthResult user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Prenom ?? ""),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Nom ?? ""),
                new Claim("RoleId", user.RoleId.ToString())
            };

            return _tokenGenerator.CreateToken(claims);
        }
    }
}
