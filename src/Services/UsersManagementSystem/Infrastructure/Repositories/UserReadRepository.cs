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
    public class UserReadRepository : IUserReadRepository
    {
        protected readonly UsersDbContext _context;        
        public UserReadRepository(UsersDbContext context)
        {
            _context = context;            
        }
        public IQueryable<User> AsQueryable()
        {
            return _context.Users;
        }
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Users.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, ct);
        }       
        public async Task<List<User>> GetAllAsync(CancellationToken cancellation, FindOptions? findOptions = null)
        {
            var query = AsQueryable();

            if(findOptions != null)
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

            return await query.ToListAsync(cancellation) ?? await Task.Run(() => new List<User>());

        }
        public async Task<User?> GetOneAsync(CancellationToken cancellation, FindOptions? findOptions = null)
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

        public async Task<IEnumerable<User>> ListAsync(CancellationToken ct = default, bool isAsNoTracking = false)
        {
            var query = _context.Users.AsQueryable();
            
            if(isAsNoTracking)  return  await query.AsNoTracking().ToListAsync(ct);

            return await query.ToListAsync(ct);
        }

        public async Task<IEnumerable<User>> ListAsync(Expression<Func<User, bool>> predicate, bool isAsNoTracking = false, CancellationToken ct = default, params Expression<Func<User, object>>[] includes)
        {
            var query = _context.Users.AsQueryable();

            if(isAsNoTracking)
                query = query.AsNoTracking();

            if(predicate != null)
                query = query.Where(predicate);

            if(includes != null)                
                foreach(var include in includes)                
                    query = query.Include(include);
                
            return await query.ToListAsync(ct);

        }
    }
}
