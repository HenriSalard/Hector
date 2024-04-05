using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace Hector.Modele
{
    /// <summary>
    /// Classe qui repertorie les differentes methodes pour acceder à la base de données SQLite
    /// </summary>
    class FonctionsSQLite
    {
        /// <summary>
        /// Requete SQL pour recuperer la liste des marques de la base de données
        /// </summary>
        /// <param name="Con"></param>
        /// <returns>La liste des marques</returns>
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

        /// <summary>
        /// Requete SQL pour recuperer la liste des Familles
        /// </summary>
        /// <param name="Con"></param>
        /// <returns>La liste des familles présentes dans la base</returns>
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

        /// <summary>
        /// Requete SQL pour recuperer la liste des Familles
        /// </summary>
        /// <param name="Con"></param>
        /// <returns>La liste des sous familles présentes dans la bas</returns>
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

        /// <summary>
        /// Requete SQL pour recuperer la liste des Articles
        /// </summary>
        /// <param name="Con"></param>
        /// <returns>La liste des articles de la base de données</returns>
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

        /// <summary>
        /// Requete qui permet de recuperer les sous-familles de la base
        /// </summary>
        /// <param name="Con"></param>
        /// <param name="FamilleName"></param>
        /// <returns>La liste des sous familles</returns>
        public static List<SousFamille> SQLiteSousFamilleFromFamilleName(SQLiteConnection Con,string FamilleName)
        {
            List<SousFamille> Liste = new List<SousFamille>();
            
            string requette = "SELECT RefSousFamille, SousFamilles.RefFamille, SousFamilles.Nom FROM SousFamilles, Familles WHERE SousFamilles.RefFamille = Familles.RefFamille and Familles.Nom = '" + FamilleName+"'";


            SQLiteCommand CommandeSQLite = new SQLiteCommand(requette, Con);

            SQLiteDataReader Lecteur = CommandeSQLite.ExecuteReader();

            while (Lecteur.Read())
            {

                // Créé un objet Familles pour chaqueligne de la table Familles de la base de données
                SousFamille NouvelleSousFamille = new SousFamille(Lecteur.GetInt32(0), Lecteur.GetInt32(1), Lecteur.GetString(2));
                Liste.Add(NouvelleSousFamille);
            }
            Lecteur.Close();

            return Liste;
        }

    }
}
