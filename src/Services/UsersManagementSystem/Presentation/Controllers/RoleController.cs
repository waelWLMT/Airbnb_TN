using Application.Dtos;
using Application.Services;
using Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetRoles")]
        public async Task<List<RoleReadDto>?> GetAllRoles()
        {
            var query = new GetAllRolesQuery();
            var roles = await _mediator.Send(query);

            if(roles == null) return new List<RoleReadDto>();

            return UserRoleBuilderService.BuildUserRoleReadDtoList(roles);
        }



    }
}
