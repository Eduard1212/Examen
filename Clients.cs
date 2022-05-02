using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen
{
    internal class Clients
    {
        public static Entities entities;
        public static Entities GetClients()
        {
            if (entities == null)
            {
                entities = new Entities();
            }
            return entities;

        }

    }
}
