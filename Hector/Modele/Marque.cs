using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    /// <summary>
    /// La classe qui instancie les marques dans l'application
    /// </summary>
    public class Marque
    {

        /// <summary>
        /// Constructeur de Marque
        /// </summary>
        /// <param name="refMarque">int: la reference de la marque (autoincrement)</param>
        /// <param name="nomMarque">string: le nom de la marque</param>
        public Marque(int refMarque, string nomMarque)
        {
            this.RefMarque = refMarque;
            this.NomMarque = nomMarque;
        }

        public string NomMarque { get; set; }

        public int RefMarque { get; set; }
    }
}
