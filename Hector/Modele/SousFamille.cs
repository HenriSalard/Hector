using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    public class SousFamille
    {

        public SousFamille(int refSousFamille, int refFamille, string nomSousFamille)
        {
            this.RefSousFamille = refSousFamille;
            this.RefFamille = refFamille;
            this.NomSousFamille = nomSousFamille;
        }

        public int RefSousFamille { get; set; }

        public int RefFamille { get; set; }

        public string NomSousFamille { get; set; }
    }
}
