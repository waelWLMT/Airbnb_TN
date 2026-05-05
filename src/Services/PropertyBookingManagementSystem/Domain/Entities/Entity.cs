using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Domain.Entities
{
    public class Entity : IEntity
    {
        public int Id { get;  set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime CreatedAt => DateTime.Now;

        public DateTime UpdatedAt => DateTime.Now;
    }
}
