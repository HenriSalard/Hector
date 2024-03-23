using Hector.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    public partial class FormImport : Form
    {

        private string FilePath;
        private int ArticleAjoutes;
        private int NbAnomaliesRefArticles;

        public FormImport()
        {
            InitializeComponent();
            this.Text = "Importer";

            // On initialise le texte du nom du fichier à vide 

            this.LabelFileName.Text = "";
            this.FilePath = string.Empty;
            this.ArticleAjoutes = 0;
            this.NbAnomaliesRefArticles = 0;
            this.progressBar1.Minimum = 1;
            this.progressBar1.Maximum = 11;
        }

        private void FormImport_Load(object sender, EventArgs e)
        {
            // On cache les boutons tant qu'aucun fichier n'est selectionné
            this.ButtonEcrasement.Enabled = false;
            this.ButtonAjout.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var FileContent = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.FilePath = openFileDialog.FileName;

                    LabelFileName.Text = Path.GetFileName(this.FilePath);

                    ButtonAjout.Enabled = true;
                    ButtonEcrasement.Enabled = true;
                }
            }
            }

        private void ButtonEcrasement_Click(object sender, EventArgs e)
        {
            //Trouve le chemin vers le fichier de la bdd
            SQLiteConnection Con = new SQLiteConnection("URI=file:" 
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) 
                + "\\Hector.sqlite");

            Con.Open();

            progressBar1.Value = 1; // On lance la barre de progression, elle augmentera a chaque etape de l'integration

            // Ouverture du fichier csv
            StreamReader Reader = File.OpenText(FilePath);

            // Creation des listes qui vont contenir les donnees du tableur csv
            List<string> ListeDescriptions = new List<string>();
            List<string> ListeRefs = new List<string>();
            List<string> ListeMarques = new List<string>();
            List<string> ListeFamilles = new List<string>();
            List<string> ListeSousFamilles = new List<string>();
            List<float> ListePrix = new List<float>();

            // Recuperation des donnees du csv

            var Ligne = Reader.ReadLine(); // Il faut sauter la premiere ligne
            while (!Reader.EndOfStream)
            {
                Ligne = Reader.ReadLine();
                var Valeurs = Ligne.Split(';');
                ListeDescriptions.Add(Valeurs[0]);
                ListeRefs.Add(Valeurs[1]);
                ListeMarques.Add(Valeurs[2]);
                ListeFamilles.Add(Valeurs[3]);
                ListeSousFamilles.Add(Valeurs[4]);
                ListePrix.Add(float.Parse(Valeurs[5]));
            }

            progressBar1.PerformStep();

            // Dans la partie suivante, on va creer les listes contenant les lignes de notre base de données
            // Chaque methode utilisee peut etre retrouvee a la suite des methodes des deux bouton d'import de la classe actuelle

            List<Marque> AnciennesMarques = new List<Marque>(); // L'ancienne liste est vide vu qu'on ecrase la bdd
            List<Marque> ListeNouvellesMarques = TrouverNouvellesMarques(ListeMarques, AnciennesMarques);

            progressBar1.PerformStep();

            List<Famille> ListeAnciennesFamilles = new List<Famille>(); // L'ancienne liste est vide vu qu'on ecrase la bdd
            List<Famille> ListeNouvellesFamilles = TrouverNouvellesFamilles(ListeFamilles, ListeAnciennesFamilles);

            progressBar1.PerformStep();

            List<SousFamille> ListeAnciennesSousFamilles = new List<SousFamille>(); // L'ancienne liste est vide vu qu'on ecrase la bdd
            List<SousFamille> ListeNouvellesSousFamilles = TrouverNouvellesSousFamilles(ListeSousFamilles, ListeAnciennesSousFamilles, ListeNouvellesFamilles, ListeFamilles);

            progressBar1.PerformStep();

            List<Article> ListeAncienArticles = new List<Article>(); // L'ancienne liste est vide vu qu'on ecrase la bdd

            // On cree la nouvelle liste d'article grace a toutes les listes qu'on a cree avant
            List<Article> ListeNouveauxArticles = TrouverNouveauxArticles(ListeDescriptions, ListeRefs, ListeMarques, ListeSousFamilles, ListePrix,
                ListeAncienArticles, ListeNouvellesMarques, ListeNouvellesSousFamilles);

            progressBar1.PerformStep();

            // On vide chaque table de la base de donnees

            SQLiteCommand CommandeDelete = new SQLiteCommand("DELETE FROM SousFamilles", Con);
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE FROM Familles";
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE FROM Marques";
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE  FROM Articles";
            CommandeDelete.ExecuteNonQuery();

            progressBar1.PerformStep();

            SQLiteCommand CommandeInsert = new SQLiteCommand(string.Empty, Con);

            // Ajout des marques dans la bdd
            foreach (Marque NouvelleMarque in ListeNouvellesMarques)
            {
                CommandeInsert.CommandText = "INSERT INTO Marques(RefMarque, Nom) Values('" + NouvelleMarque.RefMarque + "', '" + NouvelleMarque.NomMarque + "')";
                CommandeInsert.ExecuteNonQuery();
            }

            progressBar1.PerformStep();

            // Ajout des familles dans la bdd
            foreach (Famille NouvelleFamille in ListeNouvellesFamilles)
            {
                CommandeInsert.CommandText = "INSERT INTO Familles(RefFamille, Nom) VALUES( '" + NouvelleFamille.RefFamille + "', '" + NouvelleFamille.NomFamille + "' )";
                CommandeInsert.ExecuteNonQuery();
            }

            progressBar1.PerformStep();

            // Ajout des sous familles à la bdd
            foreach (SousFamille NouvelleSousFamille in ListeNouvellesSousFamilles)
            {
                CommandeInsert.CommandText = "INSERT INTO SousFamilles(RefSousFamille, RefFamille, Nom) VALUES( '" 
                    + NouvelleSousFamille.RefSousFamille + "', '" + NouvelleSousFamille.RefFamille + "', '" + NouvelleSousFamille.NomSousFamille + "' )";
                CommandeInsert.ExecuteNonQuery();
            }

            progressBar1.PerformStep();

            // Ajout des articles dans la bdd
            foreach (Article NouvelArticle in ListeNouveauxArticles)
            {
                CommandeInsert.CommandText = "INSERT INTO Articles(RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES('"
                    + NouvelArticle.RefArticle + "', '" + NouvelArticle.Description + "', '" + NouvelArticle.RefSousFamille + "', '" + NouvelArticle.RefMarque 
                    + "', '" + NouvelArticle.PrixHT + "', '" + NouvelArticle.Quantite + "')";
                CommandeInsert.ExecuteNonQuery();
            }

            progressBar1.PerformStep();

            Con.Close();
        }

        private void ButtonAjout_Click(object sender, EventArgs e)
        {
            //Trouve le chemin vers le fichier de la bdd
            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();
            progressBar1.Value = 1; // On lance la barre de progression, elle augmentera a chaque etape de l'integration

            // Ouverture du fichier csv
            StreamReader Reader = File.OpenText(FilePath);

            // Creation des listes qui vont contenir les donnees du tableur csv
            List<string> ListeDescriptions = new List<string>();
            List<string> ListeRefs = new List<string>();
            List<string> ListeMarques = new List<string>();
            List<string> ListeFamilles = new List<string>();
            List<string> ListeSousFamilles = new List<string>();
            List<float> ListePrix = new List<float>();

            // Recuperation des donnees du csv

            var Ligne = Reader.ReadLine(); // Il faut sauter la premiere ligne
            while (!Reader.EndOfStream)
            {
                Ligne = Reader.ReadLine();
                var Valeurs = Ligne.Split(';');
                ListeDescriptions.Add(Valeurs[0]);
                ListeRefs.Add(Valeurs[1]);
                ListeMarques.Add(Valeurs[2]);
                ListeFamilles.Add(Valeurs[3]);
                ListeSousFamilles.Add(Valeurs[4]);
                ListePrix.Add(float.Parse(Valeurs[5]));
            }

            progressBar1.PerformStep();

            // Dans la partie suivante, on va creer les listes contenant les lignes de notre base de données
            // Chaque methode utilisee peut etre retrouvee a la suite des methodes des deux bouton d'import de la classe actuelle

            List<Marque> AnciennesMarques = FonctionsSQLite.SQLiteRecupererMarques(Con); // On recupere la liste dans le fichier SQLite
            List<Marque> ListeNouvellesMarques = TrouverNouvellesMarques(ListeMarques, AnciennesMarques);

            progressBar1.PerformStep();

            List<Famille> ListeAnciennesFamilles = FonctionsSQLite.SQLiteRecupererFamilles(Con); // On recupere la liste dans le fichier SQLite
            List<Famille> ListeNouvellesFamilles = TrouverNouvellesFamilles(ListeFamilles, ListeAnciennesFamilles);

            progressBar1.PerformStep();

            List<SousFamille> ListeAnciennesSousFamilles = FonctionsSQLite.SQLiteRecupererSousFamilles(Con); // On recupere la liste dans le fichier SQLite
            List<SousFamille> ListeNouvellesSousFamilles = TrouverNouvellesSousFamilles(ListeSousFamilles, ListeAnciennesSousFamilles, ListeNouvellesFamilles, ListeFamilles);

            progressBar1.PerformStep();

            List<Article> ListeAncienArticles = FonctionsSQLite.SQLiteRecupererArticles(Con); // On recupere la liste dans le fichier SQLite

            // On cree la nouvelle liste d'article grace a toutes les listes qu'on a cree avant
            List<Article> ListeNouveauxArticles = TrouverNouveauxArticles(ListeDescriptions, ListeRefs, ListeMarques, ListeSousFamilles, ListePrix,
                ListeAncienArticles, ListeNouvellesMarques, ListeNouvellesSousFamilles);

            progressBar1.PerformStep();

            // On va remplacer toute la bdd donc on vide les tables

            SQLiteCommand CommandeDelete = new SQLiteCommand("DELETE FROM SousFamilles", Con);
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE FROM Familles";
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE FROM Marques";
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE  FROM Articles";
            CommandeDelete.ExecuteNonQuery();

            progressBar1.PerformStep();

            SQLiteCommand CommandeInsert = new SQLiteCommand(string.Empty, Con);

            // Ajout des marques dans la bdd
            foreach (Marque NouvelleMarque in ListeNouvellesMarques)
            {
                CommandeInsert.CommandText = "INSERT INTO Marques(RefMarque, Nom) Values('" + NouvelleMarque.RefMarque + "', '" + NouvelleMarque.NomMarque + "')";
                CommandeInsert.ExecuteNonQuery();
            }
            progressBar1.PerformStep();

            // Ajout des familles dans la bdd
            foreach (Famille NouvelleFamille in ListeNouvellesFamilles)
            {
                CommandeInsert.CommandText = "INSERT INTO Familles(RefFamille, Nom) VALUES( '" + NouvelleFamille.RefFamille + "', '" + NouvelleFamille.NomFamille + "' )";
                CommandeInsert.ExecuteNonQuery();
            }
            progressBar1.PerformStep();

            // Ajout des sous familles à la bdd
            foreach (SousFamille NouvelleSousFamille in ListeNouvellesSousFamilles)
            {
                CommandeInsert.CommandText = "INSERT INTO SousFamilles(RefSousFamille, RefFamille, Nom) VALUES( '"
                    + NouvelleSousFamille.RefSousFamille + "', '" + NouvelleSousFamille.RefFamille + "', '" + NouvelleSousFamille.NomSousFamille + "' )";
                CommandeInsert.ExecuteNonQuery();
            }
            progressBar1.PerformStep();

            // Ajout des articles dans la bdd
            foreach (Article NouvelArticle in ListeNouveauxArticles)
            {
                CommandeInsert.CommandText = "INSERT INTO Articles(RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES('"
                    + NouvelArticle.RefArticle + "', '" + NouvelArticle.Description + "', '" + NouvelArticle.RefSousFamille + "', '" + NouvelArticle.RefMarque
                    + "', '" + NouvelArticle.PrixHT + "', '" + NouvelArticle.Quantite + "')";
                CommandeInsert.ExecuteNonQuery();
            }
            progressBar1.PerformStep();

            Con.Close();
        }

        private List<Marque> TrouverNouvellesMarques(List<string> ListeMarques, List<Marque> ListeAnciennesMarques)
        {
            List<Marque> ListeNouvellesMarques = ListeAnciennesMarques;

            // On parcoure les string Marques, et on cree un nouvel objet Marque a chaque fois qu'une nouvelle apparait
            for (int IndiceMarque = 0; IndiceMarque < ListeMarques.Count; IndiceMarque++)
            {

                // On verifie si la marque (string) existe deja
                bool MarqueExiste = false;
                for (int IndiceNouvellesMarques = 0; IndiceNouvellesMarques < ListeNouvellesMarques.Count; IndiceNouvellesMarques++)
                {
                    if (ListeMarques[IndiceMarque] == ListeAnciennesMarques[IndiceNouvellesMarques].NomMarque)
                    {
                        MarqueExiste = true;
                        break;
                    }
                }

                // Si la marque n'existe pas on la cree
                if (!MarqueExiste)
                {
                    // Creation de la marque via le constructeur, ref = nombre de marque deja existantes + 1, pour commencer a 1
                    Marque NouvelleMarque = new Marque(ListeNouvellesMarques.Count + 1, ListeMarques[IndiceMarque]);
                    ListeNouvellesMarques.Add(NouvelleMarque);
                }
            }

            return ListeNouvellesMarques; // La liste a ete mise a jour
        }


        // Permet de trouver la nouvelle liste de familles, donner une liste vide pour nouvelle bdd
        private List<Famille> TrouverNouvellesFamilles(List<String> ListeFamilles, List<Famille> ListeAnciennesFamilles)
        {

            // On parcoure les string familles, et on cree un nouvel objet Famille a chaque fois qu'une nouvelle apparait
            for (int IndiceFamille = 0; IndiceFamille < ListeFamilles.Count; IndiceFamille++)
            {

                // On verifie si la famille (string) existe deja
                bool FamilleExiste = false;
                for (int IndiceNouvellesFamilles = 0; IndiceNouvellesFamilles < ListeAnciennesFamilles.Count; IndiceNouvellesFamilles++)
                {
                    if (ListeFamilles[IndiceFamille] == ListeAnciennesFamilles[IndiceNouvellesFamilles].NomFamille)
                    {
                        FamilleExiste = true;
                        break;
                    }
                }

                // Si la famille n'existe pas on la cree
                if (!FamilleExiste)
                {
                    // Creation de la famille via le constructeur, ref = nombre de Familles deja existantes + 1, pour commencer a 1
                    Famille NouvelleFamille = new Famille(ListeAnciennesFamilles.Count + 1, ListeFamilles[IndiceFamille]);
                    ListeAnciennesFamilles.Add(NouvelleFamille);
                }

            }

            return ListeAnciennesFamilles; // La liste a ete mise a jour
        }

        private List<SousFamille> TrouverNouvellesSousFamilles(List<string> ListeSousFamilles, List<SousFamille> ListeAnciennesSousFamilles, List<Famille> ListeFamilles, List<string> FamillesCorrespondantes)
        {
            List<SousFamille> ListeNouvellesSousFamilles = ListeAnciennesSousFamilles;
            int refFamille = 0;

            // On parcoure les string sous familles, et on cree un nouvel objet SousFamille a chaque fois qu'une nouvelle apparait
            for (int IndiceSousFamille = 0; IndiceSousFamille < ListeSousFamilles.Count; IndiceSousFamille++)
            {

                // On verifie si la sous famille (string) existe deja
                bool SousFamilleExiste = false;
                for (int IndiceAnciennesSousFamilles = 0; IndiceAnciennesSousFamilles < ListeAnciennesSousFamilles.Count; IndiceAnciennesSousFamilles++)
                {
                    if (ListeSousFamilles[IndiceSousFamille] == ListeNouvellesSousFamilles[IndiceAnciennesSousFamilles].NomSousFamille)
                    {
                        SousFamilleExiste = true;
                        break;
                    }
                }

                // Si la famille n'existe pas on la cree
                if (!SousFamilleExiste)
                {

                    // On doit trouver la ref de sa famille dans le csv
                    refFamille = TrouverRefFamille(FamillesCorrespondantes[IndiceSousFamille], ListeFamilles);

                    // Creation de la famille via le constructeur, ref = nombre de SousFamille deja existantes + 1, pour commencer a 1
                    SousFamille NouvelleSousFamille = new SousFamille(ListeNouvellesSousFamilles.Count + 1
                        , refFamille ,ListeSousFamilles[IndiceSousFamille]);
                    ListeNouvellesSousFamilles.Add(NouvelleSousFamille);
                }
            }

            return ListeNouvellesSousFamilles;  
    }

        private List<Article> TrouverNouveauxArticles(List<String> ListeDescriptions, List<string> ListeRefs, List<string> ListeMarques,
             List<string> ListeSousFamilles ,List<float> ListePrix, List<Article> AncienneListeArticles, List<Marque> ListeMarqueVraie, List<SousFamille> ListeSousFamillesVraie)
        {
            List<Article> NouvelleListeArticles = AncienneListeArticles;

            // On commence par regarder si l'article existe deja
            for (int IndiceArticle = 0; IndiceArticle < ListeDescriptions.Count; IndiceArticle++)
            {
                bool ArticleExiste = false;
                foreach(Article ArticleAComparer in NouvelleListeArticles)
                {
                    // Cas ou on trouve un article avec la meme reference
                    if(ListeRefs[IndiceArticle] == ArticleAComparer.RefArticle)
                    {
                        ArticleExiste = true;

                        // On verifie que la description est la meme
                        if(ListeDescriptions[IndiceArticle] != ArticleAComparer.Description)
                        {
                            // Si c'est pas la meme, on remplace l'ancienne description par la nouvelle
                            ArticleAComparer.Description = ListeDescriptions[IndiceArticle];

                            // TODO GESTION ANOMALIE
                        }

                        // On verifie aussi qu'ils ont la meme sous famille TODO
                        if(TrouverRefSousFamille(ListeSousFamilles[IndiceArticle], ListeSousFamillesVraie) != ArticleAComparer.RefSousFamille)
                        {
                            // Si anomalie on met a jour
                            ArticleAComparer.RefSousFamille = TrouverRefSousFamille(ListeSousFamilles[IndiceArticle], ListeSousFamillesVraie);

                            // TODO GESTION ANOMALIE
                        }

                        // Enfin on verifie qu'ils ont la meme marque TODO
                        if (TrouverRefMarque(ListeMarques[IndiceArticle], ListeMarqueVraie) != ArticleAComparer.RefMarque)
                        {
                            ArticleAComparer.RefMarque = TrouverRefMarque(ListeMarques[IndiceArticle], ListeMarqueVraie);

                            // TODO GESTION ANOMALIE
                        }

                        //On augmente la quantite
                        ArticleAComparer.Quantite = ArticleAComparer.Quantite + 1;
                    }
                }
                if (!ArticleExiste)
                {
                    // Creation de l'article avec les bons atributs, et une quantite egale a 1
                    Article NouvelArticle = new Article(ListeRefs[IndiceArticle], TrouverRefSousFamille(ListeSousFamilles[IndiceArticle],
                        ListeSousFamillesVraie), TrouverRefMarque(ListeMarques[IndiceArticle],
                        ListeMarqueVraie), ListeDescriptions[IndiceArticle],
                        ListePrix[IndiceArticle], 1);
                    NouvelleListeArticles.Add(NouvelArticle);
                }
                this.ArticleAjoutes++;
            }

            return NouvelleListeArticles;
        }

        private static int TrouverRefFamille(string NomDeLaFamille, List<Famille> ListeFamilles)
        {
            foreach(Famille FamilleAComparer in ListeFamilles)
            {
                if(NomDeLaFamille == FamilleAComparer.NomFamille)
                {
                    return FamilleAComparer.RefFamille;
                }
            }
            return -1;
        }

        private static int TrouverRefSousFamille(string NomDeLaFamille, List<SousFamille> ListeSousFamilles)
        {
            foreach (SousFamille SousFamilleAComparer in ListeSousFamilles)
            {
                if (NomDeLaFamille == SousFamilleAComparer.NomSousFamille)
                {
                    return SousFamilleAComparer.RefFamille;
                }
            }
            return -1;
        }

        private static int TrouverRefMarque(string NomDeLaMarque, List<Marque> ListeMarques)
        {
            foreach (Marque MarqueAComparer in ListeMarques)
            {
                if (NomDeLaMarque == MarqueAComparer.NomMarque)
                {
                    return MarqueAComparer.RefMarque;
                }
            }
            return -1;
        }


        private void LabelFileName_Click(object sender, EventArgs e)
        {

        }
    }

    }
