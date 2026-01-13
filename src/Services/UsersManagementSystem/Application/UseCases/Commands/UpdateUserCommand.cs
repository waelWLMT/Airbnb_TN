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
    public class UpdateUserCommand : IRequest<User?>
    {
        public required Guid Id { get; set; }
        public required UserUpdateDto UserUpdateDto { get; set; }
    }
}
