using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace UnitTests.Helpers
{
    public static class RolesTestData
    {
        public static List<Role> GetSampleRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Libelle = "Admin", Code = "Admin" },
                new Role { Id = 2, Libelle = "Voyageur", Code = "Voyageur" },
                new Role { Id = 3, Libelle = "Proprietaire", Code = "Proprietaire" }
            };
        }
    }
}
