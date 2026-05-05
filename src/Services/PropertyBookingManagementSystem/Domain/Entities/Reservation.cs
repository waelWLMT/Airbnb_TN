using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reservation : Entity
    {
        public Logement Logement { get; set; }
        public Voyageur Locataire { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public int NbrJours { get; set; }
        public List<JourReservation> JourReservation { get; set; }
        public double CoutTotal { get; set; }
    }
}
