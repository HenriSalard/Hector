using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector
{
    /// <summary>
    /// La classe qui instancie les familles
    /// </summary>
    public class Famille
    {

        /// <summary>
        /// Constructeur de Famille
        /// </summary>
        /// <param name="refFamille">int: La reference de la famille</param>
        /// <param name="nomFamille">string: le nom de la famille</param>
        public Famille(int refFamille, string nomFamille)
        {
            this.RefFamille = refFamille;
            this.NomFamille = nomFamille;
        }

        

        public int RefFamille { get; protected set; }
        public string NomFamille { get; protected set; }
    }
}
