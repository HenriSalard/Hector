using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    /// <summary>
    /// La classe qui instancie les sous-familles dans l'application
    /// </summary>
    public class SousFamille
    {
        /// <summary>
        /// Constructeur de SousFamille
        /// </summary>
        /// <param name="refSousFamille">int: la reference de la sous-famille</param>
        /// <param name="refFamille">int: La reference de la famille liée à la osus-famille</param>
        /// <param name="nomSousFamille">string: Le nom de la sous-famille</param>
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
