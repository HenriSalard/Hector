using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    class Famille
    {

        public Famille(int refFamille, string nomFamille)
        {
            this.RefFamille = refFamille;
            this.NomFamille = nomFamille;
        }

        

        public int RefFamille { get; protected set; }
        public string NomFamille { get; protected set; }
    }
}
