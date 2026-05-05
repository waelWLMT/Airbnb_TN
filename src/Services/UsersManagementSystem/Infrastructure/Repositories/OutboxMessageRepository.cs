using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class OutboxMessageRepository : Repository<OutboxMessage>, IOutboxMessageRepository
    {
        public OutboxMessageRepository(UsersDbContext context): base(context)
        {

        }
    }
}
