using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Infrastructure.Repositories
{
    public class OutBoxMessageReadRepository : ReadRepository<OutboxMessage>, IOutboxMessageReadRepository
    {
        public OutBoxMessageReadRepository(UsersDbContext context): base(context)
        {            
        }
        public async Task<List<OutboxMessage>> GetNotProcessededMessages(int batchSize)
        {

            var messages = await _dbSet.Where(m => m.ProcessedAt == null)                
                .OrderBy(m => m.CreatedAt)
                .Take(batchSize)
                .ToListAsync();

            return messages;
            
        }
    }
}
