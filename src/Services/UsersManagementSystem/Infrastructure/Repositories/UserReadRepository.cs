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
    public class UserReadRepository : ReadRepository<User>, IUserReadRepository
    {
        public UserReadRepository(UsersDbContext context) : base(context)
        {            
        }     
    }
}
