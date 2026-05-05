using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class UserWriteRepository : Repository<User>, IUserWriteRepository
    {
        public UserWriteRepository(UsersDbContext context) : base(context)  {  }
    }
}
