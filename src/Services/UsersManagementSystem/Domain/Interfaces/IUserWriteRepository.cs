using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IUserWriteRepository
    {
        public Task AddAsync(User user, CancellationToken ct = default);
        public Task DeleteAsync(User user, CancellationToken ct = default);        
        public Task UpdateAsync(User user, CancellationToken ct = default);
    }
}
