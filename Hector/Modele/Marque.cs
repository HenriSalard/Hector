using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    public class Marque
    {

        public Marque(int refMarque, string nomMarque)
        {
            this.RefMarque = refMarque;
            this.NomMarque = nomMarque;
        }

        public string NomMarque { get; set; }

        public int RefMarque { get; set; }
    }
}
