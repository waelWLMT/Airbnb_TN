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
    public class TokenService : ITokenService
    {
        private readonly ITokenGenerator _tokenProvider;

        public TokenService(ITokenGenerator tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public string GenerateToken(AuthResult user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Prenom ?? ""),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Nom ?? ""),
                new Claim(ClaimTypes.Role, user.Role.Code ?? ""),
                new Claim("RoleId", user.RoleId.ToString())
            };

            return _tokenProvider.CreateToken(claims);
        }
    }
}
