using Application.Dtos;
using Application.Services;
using Application.UseCases.Commands;
using Application.UseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllUsers")]
        public async Task<List<UserReadDto>> GetAllUsers(bool userWithRole, bool isReadOnly)
        {
            var query = new GetAllUsersQuery()
            {
                UserWithRole = userWithRole,
                IsReadOnly = isReadOnly
            };

            var users = await _mediator.Send(query);

            if(users == null) return new List<UserReadDto>();

            return UserBuilderService.BuildUserReadDtoList(users);
            
        }

        [HttpGet("GetUserById")]
        public async Task<UserReadDto?> GetUserById(Guid id, bool userWithRole, bool isReadOnly)
        {
            var query = new GetUserByIdQuery()
            {
                Id = id
            };

            var user = await _mediator.Send(query);

            if (user == null) return null;
            

            return UserBuilderService.BuildUserReadDto(user);
            
        }

        [HttpPost("CreateUser")]
        public async Task<UserReadDto?> CreateUser(UserCreateDto userCreateDto)
        {
            var command = new CreateUserCommand()
            {
                UserCreateDto = userCreateDto
            };

            var user = await _mediator.Send(command);

            if (user == null) return null;

            return UserBuilderService.BuildUserReadDto(user);

        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand()
            {
                Id = id
            };
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("UpdateUser")]
        public async Task<UserReadDto?> UpdateUser(UserUpdateDto userUpdateDto, Guid id)
        {
            var command = new UpdateUserCommand()
            {
                Id = id,
                UserUpdateDto = userUpdateDto
            };

            var user = await _mediator.Send(command);

            if (user == null) return null;

            return UserBuilderService.BuildUserReadDto(user);

        }

        [HttpGet("GetUserByEmailAndPassword")]
        public async Task<UserReadDto?> GetUserByEmailAndPassword(string email, string password)
        {
            var query = new GetUserByEmailAndPasswordQuery()
            {
                Email = email,
                Password = password
            };
            var user = await _mediator.Send(query);

            if (user == null) return null;

            return UserBuilderService.BuildUserReadDto(user);

        }
    
    
    }
}
