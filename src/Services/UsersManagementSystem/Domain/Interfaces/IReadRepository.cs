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
    public interface IReadRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<T>> ListAsync(CancellationToken ct = default, bool isAsNoTracking = false);
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> predicate, bool isAsNoTracking = false, CancellationToken ct = default, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync(CancellationToken cancellation, FindOptions<T>? findOptions = null);
        Task<T?> GetOneAsync(CancellationToken cancellation, FindOptions<T>? findOptions = null);
    }
}
