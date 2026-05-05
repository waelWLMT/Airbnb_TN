using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly UsersDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(UsersDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task AddAsync(T t, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(t, ct);
        }
        public async Task DeleteAsync(T t, CancellationToken ct = default)
        {
            await Task.Run(() => _dbSet.Remove(t), ct);
        }
        public async Task UpdateAsync(T t, CancellationToken ct = default)
        {
            await Task.Run(() => _dbSet.Update(t), ct);
        }
    }
}
