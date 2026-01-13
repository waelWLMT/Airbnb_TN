using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Utils
{
    public class FindOptions
    {
        public Expression<Func<User, bool>>? Predicate { get; set; }
        public bool IsAsNoTracking { get; set; } = false;
        public Expression<Func<User, object>>[]? Includes { get; set; } = Array.Empty<Expression<Func<User, object>>>();
        
        public FindOptions BuildFindOptions(Expression<Func<User, bool>> predicate, bool isAsNoTracking = false, params Expression<Func<User, object>>[] includes)
        {
            return new FindOptions()
            {
                Predicate = predicate,
                IsAsNoTracking = isAsNoTracking,
                Includes = includes                
            };
        }

    }
}
