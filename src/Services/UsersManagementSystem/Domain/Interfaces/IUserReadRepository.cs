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
    public interface IUserReadRepository :IReadRepository<User>
    {      

    }
}
