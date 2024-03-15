using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    class Famille
    {
        private int refFamille;

        private string nomFamille;

        public Famille(int refFamille, string nomFamille)
        {
            this.RefFamille = refFamille;
            this.NomFamille = nomFamille;
        }

        public string NomFamille { get => nomFamille; set => nomFamille = value; }

        public int RefFamille { get => refFamille; set => refFamille = value; }
    }
}
