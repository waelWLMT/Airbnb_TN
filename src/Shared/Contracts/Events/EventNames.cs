using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events
{
    public static class EventNames
    {
        public const string VoyageurActivated = "voyageur.activated)";
        public const string VoyageurCreated = "voyageur.created";
        public const string VoyageurDeleted = "voyageur.deleted";

        public const string ProprietaireActivated = "proprietaire.activated";
        public const string ProprietaireCreated = "proprietaire.created";
        public const string ProprietaireDeleted  = "proprietaire.deleted";        
    }
}
