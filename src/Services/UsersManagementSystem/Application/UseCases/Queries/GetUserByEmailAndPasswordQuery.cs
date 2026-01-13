using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetUserByEmailAndPasswordQuery : IRequest<User?>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
