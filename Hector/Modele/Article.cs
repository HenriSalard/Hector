using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    class Article
    {
        

        public string RefArticle { get; set; }
        public int RefSousFamille { get; set; }
        public int RefMarque { get; set; }
        public string Description { get; set; }
        public float PrixHT { get; set; }

        public int Quantite { get; set; }

        public Article(string refArticle, int refSousFamille, int RefMarque, string Description, float PrixHT, int Quantite)
        {
            this.RefArticle = refArticle;
            this.RefSousFamille = refSousFamille;
            this.RefMarque = RefMarque;
            this.Description = Description;
            this.PrixHT = PrixHT;
            this.Quantite = Quantite;

        }



    }
}
