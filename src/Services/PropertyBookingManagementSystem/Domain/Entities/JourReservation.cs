using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class JourReservation : Entity
    {
        public Reservation Reservation { get; set; }
        public string Jour { get; set; }
        public DateTime Date { get; set; }
    }
}
