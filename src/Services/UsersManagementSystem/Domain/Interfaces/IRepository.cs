using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task AddAsync(T t, CancellationToken ct = default);
        public Task DeleteAsync(T t, CancellationToken ct = default);
        public Task UpdateAsync(T t, CancellationToken ct = default);
        public Task UpdateRangeAsync(IEnumerable<T> t, CancellationToken ct = default);

    }
}
