using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ComplexeType;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Dtos
{
    public class LogementReadDto : EntityReadDto
    {
        public string Description { get; set; }
        public Adresse Adresse { get; set; }
        public TypeLogement Type { get; set; }
        public double PrixParNuit { get; set; }
        public OwnerReadDto OwnerReadDto { get; set; }

    }
}
