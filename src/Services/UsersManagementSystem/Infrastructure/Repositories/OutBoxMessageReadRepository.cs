using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Infrastructure.Repositories
{
    public class OutBoxMessageReadRepository : ReadRepository<OutboxMessage>, IOutboxMessageReadRepository
    {
        public OutBoxMessageReadRepository(UsersDbContext context) : base(context)
        {
        }
        public async Task<List<OutboxMessage>> GetPendingMessagesAsync(int batchSize)
        {

            var messages = await _dbSet
                .Where(x =>
                (x.ProcessedAt == null) &&
                (x.LockedUntil == null || x.LockedUntil < DateTime.UtcNow) &&
                (x.Status == OutboxStatus.Pending || x.Status == OutboxStatus.Retrying))
                .OrderBy(x => x.CreatedAt)
                .Take(batchSize)
                .ToListAsync();

            return messages;

        }
    }
}
