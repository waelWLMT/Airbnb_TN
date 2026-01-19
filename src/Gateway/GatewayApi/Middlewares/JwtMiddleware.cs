using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace GatewayApi.Middlewares
{
    public class JwtMiddleware
    {

        
        private readonly RequestDelegate _next;
        private readonly List<string> _publicRoutes;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            _publicRoutes = getPublicRoutes();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Vérifier si la route est publique
            var path = context.Request.Path.Value;
            
            if (_publicRoutes.Exists(route => path.StartsWith(route, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            // Vérifier le header Authorization
            if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: No token provided");
                return;
            }

            var token = authHeader.ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Malformed token");
                return;
            }

            try
            {
                var jwtSection = _configuration.GetSection("Jwt");
                var key = Convert.FromBase64String(jwtSection["Key"]!);                
                var issuer = jwtSection["Issuer"];
                var audience = jwtSection["Audience"];

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // On peut filtrer ou mapper les claims si nécessaire
                var identity = new ClaimsIdentity(principal.Claims, "jwt");

                // Ajouter RoleId en claim int si nécessaire
                var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == "RoleId");
                if (roleClaim != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                }

                context.User = new ClaimsPrincipal(identity);

                await _next(context);
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid token");
            }
        }    
    
        private List<string> getPublicRoutes()
        {
            var publicRoutesSection = _configuration.GetSection("ReverseProxy:PublicRoutes");
            var publicPaths = new List<string>();

            foreach (var route in publicRoutesSection.GetChildren())
            {
                // Lire le path depuis la clé "Match:Path"
                var path = route.GetSection("Match:Path").Value;
                if (!string.IsNullOrEmpty(path))
                {
                    publicPaths.Add(path);
                }
            }

            return publicPaths;
        }
    
    }
       
}
