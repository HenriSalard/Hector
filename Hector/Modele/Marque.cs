using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    class Marque
    {

        private int refMarque;

        private string nomMarque;

        public Marque(int refMarque, string nomMarque)
        {
            this.refMarque = refMarque;
            this.nomMarque = nomMarque;
        }

        public string NomMarque { get => nomMarque; set => nomMarque = value; }

        public int RefMarque { get => refMarque; set => refMarque = value; }
    }
}
