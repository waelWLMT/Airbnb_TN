using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Utils
{
    public class FindOptions<T> where T : class
    {
        public Expression<Func<T, bool>>? Predicate { get; set; }
        public bool IsAsNoTracking { get; set; } = false;
        public Expression<Func<T, object>>[]? Includes { get; set; } = Array.Empty<Expression<Func<T, object>>>();        
        public FindOptions<T> BuildFindOptions(Expression<Func<T, bool>> predicate, bool isAsNoTracking = false, params Expression<Func<T, object>>[] includes)
        {
            return new FindOptions<T>()
            {
                Predicate = predicate,
                IsAsNoTracking = isAsNoTracking,
                Includes = includes                
            };
        }
    }
}
