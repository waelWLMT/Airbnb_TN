using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        public required UserCreateDto? UserCreateDto { get; set; }
    }
}
