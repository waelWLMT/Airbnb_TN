
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthOrchestrator _authOrchestrator;
        private readonly ITokenService _tokenService;

        public AuthenticationsController(IAuthOrchestrator authOrchestrator, ITokenService tokenService)
        {
            _authOrchestrator = authOrchestrator;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthRequest request)
        {
            var authResult = await _authOrchestrator.AuthenticateAsync(request);

            if (authResult == null || !authResult.IsActive)
                return Unauthorized();

            var token = _tokenService.GenerateToken(authResult);

            var response = new LoginResponseDto
            {
                User = authResult,
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            return Ok(response);

        }


    }
}
