using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : class
    {
        private readonly UsersDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public ReadRepository(UsersDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, ct);
        }
        public async Task<List<T>> GetAllAsync(CancellationToken cancellation, FindOptions<T>? findOptions = null)
        {
            var query = AsQueryable();

            if (findOptions != null)
            {
                if (findOptions.IsAsNoTracking)
                    query = query.AsNoTracking();

                if (findOptions.Predicate != null)
                    query = query.Where(findOptions.Predicate);

                if (findOptions.Includes != null)
                {
                    foreach (var include in findOptions.Includes)
                    {
                        query = query.Include(include);
                    }
                }
            }

            return await query.ToListAsync(cancellation) ?? await Task.Run(() => new List<T>());

        }
        public async Task<T?> GetOneAsync(CancellationToken cancellation, FindOptions<T>? findOptions = null)
        {
            var query = AsQueryable();

            if (findOptions != null)
            {
                if (findOptions.IsAsNoTracking)
                    query = query.AsNoTracking();

                if (findOptions.Predicate != null)
                    query = query.Where(findOptions.Predicate);

                if (findOptions.Includes != null)
                {
                    foreach (var include in findOptions.Includes)
                    {
                        query = query.Include(include);
                    }
                }
            }

            return await query.FirstOrDefaultAsync(cancellation);

        }
        public async Task<IEnumerable<T>> ListAsync(CancellationToken ct = default, bool isAsNoTracking = false)
        {
            var query = _dbSet.AsQueryable();

            if (isAsNoTracking) return await query.AsNoTracking().ToListAsync(ct);

            return await query.ToListAsync(ct);
        }
        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, bool isAsNoTracking = false, CancellationToken ct = default, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();

            if (isAsNoTracking)
                query = query.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync(ct);

        }





    }
}
