using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hector.Modele;

namespace Hector
{
    class CSVLecteur
    {
       /** public static List<Article> ReadArticles(string Path)
        {
            List<Article> ListeArticles = new List<Article>();

            StreamReader Reader = File.OpenText(Path);

            while (!Reader.EndOfStream)
            {
                var Ligne = Reader.ReadLine();

                var Valeurs = Ligne.Split(';');

                Article NouvelArticle = new Article(Valeurs[1], 0, 0, Valeurs[0], float.Parse(Valeurs[5]));
            }
        }**/

    }
}
