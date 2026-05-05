using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ComplexeType;
using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Logement : Entity
    {
        public string Description { get; set; }
        public Adresse Adresse { get; set; }
        public TypeLogement Type {get; set;}
        public double PrixParNuit { get; set; }
        public Proprietaire Owner { get; set; }
    }
}
