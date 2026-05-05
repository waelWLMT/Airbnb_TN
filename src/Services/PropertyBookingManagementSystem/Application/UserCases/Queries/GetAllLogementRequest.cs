using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.UserCases.Queries
{
    public class GetAllLogementRequest : IRequest<List<Logement>>
    {
    }
}
