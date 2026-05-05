using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly PropertyBookingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(PropertyBookingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task AddAsync(T entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
        }
        public async Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            await Task.Run(() => _dbSet.Update(entity), ct);
        }
        public async Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            await Task.Run(() => _dbSet.Update(entity), ct);
        }
    }
}

