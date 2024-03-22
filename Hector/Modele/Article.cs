using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    class Article
    {
        

        public int RefArticle { get; set; }
        public int RefSousFamille { get; set; }
        public int RefMarque { get; set; }
        public string Description { get; set; }
        public float PrixHT { get; set; }

        public Article(int refArticle, int refSousFamille, int RefMarque, string Description, float PrixHT)
        {
            this.RefArticle = refArticle;
            this.RefSousFamille = refSousFamille;
            this.RefMarque = RefMarque;
            this.Description = Description;
            this.PrixHT = PrixHT;

        }



    }
}
