using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class LogementReadRepository : ReadRepository<Logement>, ILogementReadRepository
    {
        public LogementReadRepository(PropertyBookingDbContext dbContext) : base(dbContext)
        {            
        }
    }
}
