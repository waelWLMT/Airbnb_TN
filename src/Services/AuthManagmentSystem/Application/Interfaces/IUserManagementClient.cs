using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos;

namespace Application.Interfaces
{
    public interface IUserManagementClient
    {
        public Task<AuthResult?>ValidateCredentials(string? email, string? password, CancellationToken cancellationToken = default);
    }
}
