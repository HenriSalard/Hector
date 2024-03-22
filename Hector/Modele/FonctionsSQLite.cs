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

           Con.Open();

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




    }
}
