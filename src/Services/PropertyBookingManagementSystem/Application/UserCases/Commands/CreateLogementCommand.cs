using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.UserCases.Commands
{
    public class CreateLogementCommand : IRequest<int>
    {
        public required string Description { get; set; }
        public TypeLogement TypeLogement { get; set; }
        public double PrixParNuit { get; set; }
        public int OwnerId { get; set; }
        public Logement ToLogement()
        {
            var logement = new Logement()
            {
                Description = this.Description,
                Type = this.TypeLogement,
                PrixParNuit = this.PrixParNuit
            };

            return logement;
        }
    }
}
