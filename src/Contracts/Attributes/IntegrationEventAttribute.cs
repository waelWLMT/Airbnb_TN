using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class IntegrationEventAttribute : Attribute
    {
        public string Name { get; }
        public IntegrationEventAttribute(string name)
        {
            Name = name;
        }
    }
}
