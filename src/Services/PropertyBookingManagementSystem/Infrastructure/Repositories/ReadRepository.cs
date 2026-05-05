using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : Entity
    {
        protected readonly PropertyBookingDbContext _dbContext;
        protected readonly IQueryable<T> _dbSet;
        public ReadRepository(PropertyBookingDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        {
            return  await _dbSet.AnyAsync(predicate, ct);
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
        public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            //return await _dbSet.FirstAsync(e => EF.Property<Guid>(e, "Id") == id, ct);
        }

        public async Task<T?> GetFirstOne(Expression<Func<T, bool>> predicate, CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(ct);
        }

        public async Task<IEnumerable<T>> ListAsync(CancellationToken ct = default)
        {
            return await _dbSet.ToListAsync(ct);
        }
        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync(ct);

        }




    }
}
