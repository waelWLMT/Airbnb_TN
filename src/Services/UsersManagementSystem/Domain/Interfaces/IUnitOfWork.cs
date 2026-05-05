using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public Task CommitAsync(CancellationToken ct = default);
        public Task RollBackAsync(CancellationToken ct = default);
    }
}
