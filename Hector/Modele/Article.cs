using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hector.Modele
{
    /// <summary>
    /// La classe qui instancie des articles
    /// </summary>
    public class Article
    {
        

        public string RefArticle { get; set; }
        public int RefSousFamille { get; set; }
        public int RefMarque { get; set; }
        public string Description { get; set; }
        public float PrixHT { get; set; }

        public int Quantite { get; set; }

        /// <summary>
        /// Constructeur de Article
        /// </summary>
        /// <param name="refArticle">string: la reference de l'article</param>
        /// <param name="refSousFamille">int: la reference de la sous famille de l'article</param>
        /// <param name="RefMarque">int: la reference de la marque de l'article</param>
        /// <param name="Description">string: la description</param>
        /// <param name="PrixHT">float: le prix de l'article</param>
        /// <param name="Quantite">int: la quantite de l'article</param>
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
