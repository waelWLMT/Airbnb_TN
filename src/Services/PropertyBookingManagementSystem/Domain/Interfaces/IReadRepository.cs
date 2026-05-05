using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IReadRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<T?>> ListAsync(CancellationToken ct = default);
        Task<IEnumerable<T?>> ListAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default, params Expression<Func<T, object>>[] includes);        
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

        Task<T?> GetFirstOne(Expression<Func<T, bool>> predicate, CancellationToken ct = default, params Expression<Func<T, object>>[] includes);

        IQueryable<T> AsQueryable();
    }
}
