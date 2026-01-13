using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllUsersQuery : IRequest<List<User>>
    {
        public bool UserWithRole { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
