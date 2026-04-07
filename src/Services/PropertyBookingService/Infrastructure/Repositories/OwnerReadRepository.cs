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
    public class OwnerReadRepository : ReadRepository<Owner>, IOwnerReadRepository
    {
        public OwnerReadRepository(PropertyBookingDbContext dbContext): base(dbContext)
        {          
        }
    }
}
