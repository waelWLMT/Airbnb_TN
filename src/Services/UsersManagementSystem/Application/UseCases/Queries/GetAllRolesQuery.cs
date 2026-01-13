using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.Models;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllRolesQuery : IRequest<List<Role>?>
    {
    }
}
