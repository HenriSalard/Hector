using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace Hector.Modele
{
    class FonctionsSQLite
    {
        public static List<Marque> SQLiteRecupererMarques(SQLiteConnection Con)
        {
            List<Marque> Liste = new List<Marque>();

            string requette = "SELECT RefMarque, Nom FROM Marques";


            SQLiteCommand CommandeSQLite = new SQLiteCommand(requette, Con);

            SQLiteDataReader Lecteur = CommandeSQLite.ExecuteReader();

            while (Lecteur.Read())
            {

                // Créé un objet Marque pour chaque element de la table Marques de la base de données
                Marque NouvelleMarque = new Marque(Lecteur.GetInt32(0), Lecteur.GetString(1));
                Liste.Add(NouvelleMarque);
            }
            return Liste;
        }

        public static List<Famille> SQLiteRecupererFamilles(SQLiteConnection Con)
        {
            List<Famille> Liste = new List<Famille>();

            string requette = "SELECT RefFamille, Nom FROM Familles";


            SQLiteCommand CommandeSQLite = new SQLiteCommand(requette, Con);

            SQLiteDataReader Lecteur = CommandeSQLite.ExecuteReader();

            while (Lecteur.Read())
            {

                // Créé un objet Familles pour chaqueligne de la table Familles de la base de données
                Famille NouvelleFamille = new Famille(Lecteur.GetInt32(0), Lecteur.GetString(1));
                Liste.Add(NouvelleFamille);
            }
            return Liste;
        }

        public static List<SousFamille> SQLiteRecupererSousFamilles(SQLiteConnection Con)
        {
            List<SousFamille> Liste = new List<SousFamille>();
            string requette = "SELECT RefSousFamille, RefFamille, Nom FROM SousFamilles";


            SQLiteCommand CommandeSQLite = new SQLiteCommand(requette, Con);

            SQLiteDataReader Lecteur = CommandeSQLite.ExecuteReader();

            while (Lecteur.Read())
            {

                // Créé un objet Familles pour chaqueligne de la table Familles de la base de données
                SousFamille NouvelleSousFamille = new SousFamille(Lecteur.GetInt32(0), Lecteur.GetInt32(1), Lecteur.GetString(2));
                Liste.Add(NouvelleSousFamille);
            }
            return Liste;
        }

        public static List<Article> SQLiteRecupererArticles(SQLiteConnection Con)
        {
            List<Article> Liste = new List<Article>();


            string requette = "SELECT RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite FROM Articles";


            SQLiteCommand CommandeSQLite = new SQLiteCommand(requette, Con);

            SQLiteDataReader Lecteur = CommandeSQLite.ExecuteReader();

            while (Lecteur.Read())
            {
                // Créé un objet Article pour chaque ligne de la table Articles de la base de données
                Article NouvelArticle = new Article(Lecteur.GetString(0), Lecteur.GetInt32(2), Lecteur.GetInt32(3), Lecteur.GetString(1), float.Parse(Lecteur.GetString(4)), Lecteur.GetInt32(5));

                Liste.Add(NouvelArticle);
            }
            return Liste;
        }



    }
}
