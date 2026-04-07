using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEntity
    {
        int Id { get; }
        int CreatedBy {  get; }
        int UpdatedBy { get; }
        DateTime CreatedAt {  get; }
        DateTime UpdatedAt { get; }
    }
}
