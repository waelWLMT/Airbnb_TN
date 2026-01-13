using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Utils;

namespace Domain.Interfaces
{   
    public interface IUserReadRepository
    {
        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
        public Task<IEnumerable<User>> ListAsync(CancellationToken ct = default, bool isAsNoTracking = false);
        public Task<IEnumerable<User>> ListAsync(Expression<Func<User, bool>> predicate, bool isAsNoTracking = false, CancellationToken ct = default, params Expression<Func<User, object>>[] includes);
        public Task<List<User>> GetAllAsync(CancellationToken cancellation, FindOptions? findOptions = null);
        public Task<User?> GetOneAsync(CancellationToken cancellation, FindOptions? findOptions = null);

    }
}
