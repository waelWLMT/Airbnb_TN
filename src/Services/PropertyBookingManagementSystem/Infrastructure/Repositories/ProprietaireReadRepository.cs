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
    public class ProprietaireReadRepository : ReadRepository<Proprietaire>, IProprietaireReadRepository
    {
        public ProprietaireReadRepository(PropertyBookingDbContext dbContext): base(dbContext)
        {          
        }
    }
}
