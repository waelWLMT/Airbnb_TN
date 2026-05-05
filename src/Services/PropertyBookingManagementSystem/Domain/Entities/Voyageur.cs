using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Voyageur : Entity
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
