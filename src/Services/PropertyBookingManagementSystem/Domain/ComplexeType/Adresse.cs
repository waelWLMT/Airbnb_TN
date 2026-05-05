using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.ComplexeType
{
    public class Adresse
    {
        public required string Numero { get; set; }
        public required string Rue { get; set; }
        public required string CodePostal { get; set; }
        public required string Ville { get; set; }
        public required string Pays { get; set; }
        public string? ComplementAdresse { get; set; }
        public required string Lat {  get; set; }
        public required string Long { get; set; }

    }
}
