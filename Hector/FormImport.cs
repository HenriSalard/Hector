using Hector.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
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
        private int NbAnomalies;

        public FormImport()
        {
            InitializeComponent();
            this.Text = "Importer";

            // On initialise le texte du nom du fichier à vide 

            this.LabelFileName.Text = "";
            this.FilePath = string.Empty;
            this.ArticleAjoutes = 0;
            this.NbAnomalies = 0;
            this.progressBar1.Minimum = 1;
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

        /// <summary>
        /// Gere l'importation du csv et la mise à jour de la base en mode ecrasement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEcrasement_Click(object sender, EventArgs e)
        {
            //Trouve le chemin vers le fichier de la bdd
            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();

            // Ouverture du fichier csv
            StreamReader Reader = File.OpenText(FilePath);

            // On compte le nombre de lignes du csv pour definir la barre de progression

            int NbLignes = 0;
            ArticleAjoutes = 0;

            while (Reader.ReadLine() != null)
            {
                NbLignes++;
            }

            // On remet la position du StreamReader au début du fichier
            Reader.BaseStream.Seek(0, SeekOrigin.Begin);

            progressBar1.Maximum = NbLignes + 1; // Le remplissage de la barre est proportionnel au nombre d'articles a ajouter
            progressBar1.Value = 1; // On lance la barre de progression, elle augmentera a chaque etape de l'integration

            // On vide chaque table de la base de donnees

            SQLiteCommand CommandeDelete = new SQLiteCommand("DELETE FROM SousFamilles", Con);
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE FROM Familles";
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE FROM Marques";
            CommandeDelete.ExecuteNonQuery();

            CommandeDelete.CommandText = "DELETE  FROM Articles";
            CommandeDelete.ExecuteNonQuery();

            //Creation des liste contenant les donnees, pour le moment vide car on a ecrase la bdd 
            List<Article> ListeArticles = new List<Article>();
            List<Marque> ListeMarques = new List<Marque>();
            List<Famille> ListeFamilles = new List<Famille>();
            List<SousFamille> ListeSousFamilles = new List<SousFamille>();

            // Definition des donnees du csv
            string Description = string.Empty;
            string RefArticle = string.Empty;
            string StringMarque = string.Empty;
            string StringFamille = string.Empty;
            string StringSousFamille = String.Empty;
            float PrixHT = 0.0F;

            int RefSousFamille = 0;
            int RefFamille = 0;
            int RefMarque = 0;
            bool ArticleExiste = false;

            SQLiteCommand CommandeInsert = new SQLiteCommand(string.Empty, Con); // Definition de la commande a utiliser pour modifier la bdd

            var Ligne = Reader.ReadLine(); // Il faut sauter la premiere ligne

            // Maintenant on recupere et anlyse chaque ligne du csv, representant un article
            while (!Reader.EndOfStream)
            {
                Ligne = Reader.ReadLine();
                var Valeurs = Ligne.Split(';');
                Description = Valeurs[0];
                RefArticle = Valeurs[1];
                StringMarque = Valeurs[2];
                StringFamille = Valeurs[3];
                StringSousFamille = Valeurs[4];
                ArticleExiste = false;

                // On verrifie s'il y a une anomalie, c'est a dire qu'une colonne de l'article est vide, dans ce cas on ajoute pas l'article
                if (Description == string.Empty || RefArticle == string.Empty || StringMarque == string.Empty ||
                    StringFamille == string.Empty || StringSousFamille == string.Empty || Valeurs[5] == string.Empty)
                {
                    NbAnomalies++;
                }
                else
                {
                    PrixHT = float.Parse(Valeurs[5]);

                    // En premier on verifie si la famille existe
                    // TrouverRefFamille retourne la ref de la famille, ou -1 si cette famille n'existe pas encore
                    RefFamille = TrouverRefFamille(StringFamille, ListeFamilles);
                    if (RefFamille == -1)
                    {
                        // On cree la nouvelle famille et on l'ajoute a la liste, sa reference est egale au nombre de familles existantes + 1 (auto increment)
                        Famille NouvelleFamille = new Famille(ListeFamilles.Count + 1, StringFamille);
                        ListeFamilles.Add(NouvelleFamille);
                        RefFamille = NouvelleFamille.RefFamille;

                        // On peut ajouter la famille a la bdd
                        CommandeInsert.CommandText = "INSERT INTO Familles(RefFamille, Nom) VALUES( '" + NouvelleFamille.RefFamille + "', '" + NouvelleFamille.NomFamille + "' )";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    // Ensuite on verifie si la sous famille existe
                    // TrouverRefSousFamille retourne la ref de la sous famille, ou -1 si cette sous famille n'existe pas encore
                    RefSousFamille = TrouverRefSousFamille(StringSousFamille, ListeSousFamilles);
                    if (RefSousFamille == -1)
                    {

                        // On cree la nouvelle sous famille et on l'ajoute a la liste, sa reference est egale au nombre de sous familles existantes + 1 (auto increment)
                        // Sa cle etrangere RefFamille a ete trouvee a l'etape precedente
                        SousFamille NouvelleSousFamille = new SousFamille(ListeSousFamilles.Count + 1, RefFamille, StringSousFamille);
                        ListeSousFamilles.Add(NouvelleSousFamille);
                        RefSousFamille = NouvelleSousFamille.RefSousFamille;

                        // On peut ajouter la sous famille a la bdd
                        CommandeInsert.CommandText = "INSERT INTO SousFamilles(RefSousFamille, RefFamille, Nom) VALUES( '"
                            + NouvelleSousFamille.RefSousFamille + "', '" + NouvelleSousFamille.RefFamille + "', '" + NouvelleSousFamille.NomSousFamille + "' )";
                        CommandeInsert.ExecuteNonQuery();
                    }


                    // Ensuite on verifie si la marque existe
                    // TrouverRefMarque retourne la ref de la marque, ou -1 si cette sous marque n'existe pas encore
                    RefMarque = TrouverRefMarque(StringMarque, ListeMarques);
                    if (RefMarque == -1)
                    {

                        // On cree la nouvelle marque et on l'ajoute a la liste, sa reference est egale au nombre de marques existantes + 1 (auto increment)
                        Marque NouvelleMarque = new Marque(ListeMarques.Count + 1, StringMarque);
                        ListeMarques.Add(NouvelleMarque);
                        RefMarque = NouvelleMarque.RefMarque;

                        // On peut ajouter la sous marque a la bdd
                        CommandeInsert.CommandText = "INSERT INTO Marques(RefMarque, Nom) Values('" + NouvelleMarque.RefMarque + "', '" + NouvelleMarque.NomMarque + "')";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    // Enfin on verifie si l'article existe deja ou non.
                    for (int IndiceArticle = 0; IndiceArticle < ListeArticles.Count; IndiceArticle++)
                    {
                        if (ListeArticles[IndiceArticle].RefArticle == RefArticle)
                        {
                            ArticleExiste = true;

                            // On met a jour la bdd, ici ca servira que si un article apparait plusieurs fois dans le csv
                            CommandeInsert.CommandText = "UPDATE Articles SET Description = '" + Description + "', RefSousFamille = '" + RefSousFamille + "'," +
                                " PrixHT = '" + PrixHT.ToString("0.00", new CultureInfo("fr-FR")) + "', RefMarque = '" + RefMarque + "' WHERE RefArticle = '" + RefArticle + "'";
                            CommandeInsert.ExecuteNonQuery();
                        }
                    }

                    if (!ArticleExiste)
                    {
                        // s'il n'existe pas on le cree puis on l'ajoute dans la bdd
                        Article NouvelArticle = new Article(RefArticle, RefSousFamille, RefMarque, Description, PrixHT, 1);
                        ListeArticles.Add(NouvelArticle);
                        CommandeInsert.CommandText = "INSERT INTO Articles(RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES('"
                            + NouvelArticle.RefArticle + "', '" + NouvelArticle.Description + "', '" + NouvelArticle.RefSousFamille + "', '" + NouvelArticle.RefMarque
                             + "', '" + NouvelArticle.PrixHT.ToString("0.00", new CultureInfo("fr-FR")) + "', '" + NouvelArticle.Quantite + "')";
                        CommandeInsert.ExecuteNonQuery();

                        ArticleAjoutes++;
                    }
                    progressBar1.PerformStep(); // On remplit la barre de progression
                }
            }

            Con.Close();

            // Affichage de la fenetre resultat a la fin
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            string Message = "Importation terminée \n " + ArticleAjoutes + " articles ajoutés \n" + NbAnomalies + " anomalies trouvées.";

            result = MessageBox.Show(this, Message, "Importation terminée", buttons);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // ferme formImport et continue l'execution de formMain
                this.DialogResult = DialogResult.OK;
            }
        }

        private void ButtonAjout_Click(object sender, EventArgs e)
        {
            //Trouve le chemin vers le fichier de la bdd
            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();

            // Ouverture du fichier csv
            StreamReader Reader = File.OpenText(FilePath);

            // On compte le nombre de lignes du csv pour definir la barre de progression

            int NbLignes = 0;

            while (Reader.ReadLine() != null)
            {
                NbLignes++;
            }

            // On remet la position du StreamReader au début du fichier
            Reader.BaseStream.Seek(0, SeekOrigin.Begin);

            progressBar1.Maximum = NbLignes + 1; // Le remplissage de la barre est proportionnel au nombre d'articles a ajouter
            progressBar1.Value = 1; // On lance la barre de progression, elle augmentera a chaque etape de l'integration


            //Creation des liste contenant les donnees, on va les chercher directement dans la bdd
            List<Article> ListeArticles = FonctionsSQLite.SQLiteRecupererArticles(Con);
            List<Marque> ListeMarques = FonctionsSQLite.SQLiteRecupererMarques(Con);
            List<Famille> ListeFamilles = FonctionsSQLite.SQLiteRecupererFamilles(Con);
            List<SousFamille> ListeSousFamilles = FonctionsSQLite.SQLiteRecupererSousFamilles(Con);

            // Definition des donnees du csv
            string Description = string.Empty;
            string RefArticle = string.Empty;
            string StringMarque = string.Empty;
            string StringFamille = string.Empty;
            string StringSousFamille = String.Empty;
            float PrixHT = 0.0F;

            int RefSousFamille = 0;
            int RefFamille = 0;
            int RefMarque = 0;
            bool ArticleExiste = false;

            SQLiteCommand CommandeInsert = new SQLiteCommand(string.Empty, Con); // Definition de la commande a utiliser pour modifier la bdd

            var Ligne = Reader.ReadLine(); // Il faut sauter la premiere ligne

            // Maintenant on recupere et anlyse chaque ligne du csv, representant un article
            while (!Reader.EndOfStream)
            {
                Ligne = Reader.ReadLine();
                var Valeurs = Ligne.Split(';');
                Description = Valeurs[0];
                RefArticle = Valeurs[1];
                StringMarque = Valeurs[2];
                StringFamille = Valeurs[3];
                StringSousFamille = Valeurs[4];
                ArticleExiste = false;


                // On verrifie s'il y a une anomalie, c'est a dire qu'une colonne de l'article est vide, dans ce cas on ajoute pas l'article
                if (Description == string.Empty || RefArticle == string.Empty || StringMarque == string.Empty ||
                    StringFamille == string.Empty || StringSousFamille == string.Empty || Valeurs[5] == string.Empty)
                {
                    NbAnomalies++;
                }
                else
                {
                    PrixHT = float.Parse(Valeurs[5]);

                    // En premier on verifie si la famille existe
                    // TrouverRefFamille retourne la ref de la famille, ou -1 si cette famille n'existe pas encore
                    RefFamille = TrouverRefFamille(StringFamille, ListeFamilles);
                    if (RefFamille == -1)
                    {
                        // On cree la nouvelle famille et on l'ajoute a la liste, sa reference est egale au nombre de familles existantes + 1 (auto increment)
                        Famille NouvelleFamille = new Famille(ListeFamilles.Count + 1, StringFamille);
                        ListeFamilles.Add(NouvelleFamille);
                        RefFamille = NouvelleFamille.RefFamille;

                        // On peut ajouter la famille a la bdd
                        CommandeInsert.CommandText = "INSERT INTO Familles(RefFamille, Nom) VALUES( '" + NouvelleFamille.RefFamille + "', '" + NouvelleFamille.NomFamille + "' )";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    // Ensuite on verifie si la sous famille existe
                    // TrouverRefSousFamille retourne la ref de la sous famille, ou -1 si cette sous famille n'existe pas encore
                    RefSousFamille = TrouverRefSousFamille(StringSousFamille, ListeSousFamilles);
                    if (RefSousFamille == -1)
                    {

                        // On cree la nouvelle sous famille et on l'ajoute a la liste, sa reference est egale au nombre de sous familles existantes + 1 (auto increment)
                        // Sa cle etrangere RefFamille a ete trouvee a l'etape precedente
                        SousFamille NouvelleSousFamille = new SousFamille(ListeSousFamilles.Count + 1, RefFamille, StringSousFamille);
                        ListeSousFamilles.Add(NouvelleSousFamille);
                        RefSousFamille = NouvelleSousFamille.RefSousFamille;

                        // On peut ajouter la sous famille a la bdd
                        CommandeInsert.CommandText = "INSERT INTO SousFamilles(RefSousFamille, RefFamille, Nom) VALUES( '"
                            + NouvelleSousFamille.RefSousFamille + "', '" + NouvelleSousFamille.RefFamille + "', '" + NouvelleSousFamille.NomSousFamille + "' )";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    // Ensuite on verifie si la marque existe
                    // TrouverRefMarque retourne la ref de la marque, ou -1 si cette sous marque n'existe pas encore
                    RefMarque = TrouverRefMarque(StringMarque, ListeMarques);
                    if (RefMarque == -1)
                    {

                        // On cree la nouvelle marque et on l'ajoute a la liste, sa reference est egale au nombre de marques existantes + 1 (auto increment)
                        Marque NouvelleMarque = new Marque(ListeMarques.Count + 1, StringMarque);
                        ListeMarques.Add(NouvelleMarque);
                        RefMarque = NouvelleMarque.RefMarque;

                        // On peut ajouter la sous marque a la bdd
                        CommandeInsert.CommandText = "INSERT INTO Marques(RefMarque, Nom) Values('" + NouvelleMarque.RefMarque + "', '" + NouvelleMarque.NomMarque + "')";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    // Enfin on verifie si l'article existe deja ou non.
                    for (int IndiceArticle = 0; IndiceArticle < ListeArticles.Count; IndiceArticle++)
                    {
                        if (ListeArticles[IndiceArticle].RefArticle == RefArticle)
                        {
                            ArticleExiste = true;

                            // On met a jour la bdd, ici ca servira que si un article apparait plusieurs fois dans le csv
                            CommandeInsert.CommandText = "UPDATE Articles SET Description = '" + Description + "', RefSousFamille = '" + RefSousFamille + "'," +
                                " PrixHT = '" + PrixHT.ToString("0.00", new CultureInfo("fr-FR")) + "', RefMarque = '" + RefMarque + "' WHERE RefArticle = '" + RefArticle + "'";
                            CommandeInsert.ExecuteNonQuery();
                        }
                    }
                    if (!ArticleExiste)
                    {
                        // s'il n'existe pas on le cree puis on l'ajoute dans la bdd
                        Article NouvelArticle = new Article(RefArticle, RefSousFamille, RefMarque, Description, PrixHT, 1);
                        ListeArticles.Add(NouvelArticle);
                        CommandeInsert.CommandText = "INSERT INTO Articles(RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES('"
                            + NouvelArticle.RefArticle + "', '" + NouvelArticle.Description + "', '" + NouvelArticle.RefSousFamille + "', '" + NouvelArticle.RefMarque
                             + "', '" + NouvelArticle.PrixHT.ToString("0.00", new CultureInfo("fr-FR")) + "', '" + NouvelArticle.Quantite + "')";
                        CommandeInsert.ExecuteNonQuery();

                        ArticleAjoutes++;
                    }
                    progressBar1.PerformStep(); // On remplit la barre de progression
                }
            }

            Con.Close();

            // Affichage de la fenetre resultat a la fin
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            string Message = "Importation terminée \n " + ArticleAjoutes + " articles ajoutés \n" + NbAnomalies + " anomalies trouvées.";

            result = MessageBox.Show(this,Message, "Importation terminée", buttons);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // ferme formImport et continue l'execution de formMain
                this.DialogResult = DialogResult.OK;
            }

        }

        /// <summary>
        /// Retourne la reference d'une famille en fonction de son nom
        /// </summary>
        /// <param name="NomDeLaFamille">Nom de la famille dont on veut connaitre la reference </param>
        /// <param name="ListeFamilles">La liste des familles de notre base</param>
        /// <returns></returns>
        private static int TrouverRefFamille(string NomDeLaFamille, List<Famille> ListeFamilles)
        {
            foreach (Famille FamilleAComparer in ListeFamilles)
            {
                if (NomDeLaFamille == FamilleAComparer.NomFamille)
                {
                    return FamilleAComparer.RefFamille;
                }
            }
            return -1;
        }

        /// <summary>
        /// Retourne la reference d'une sous famille en fonction de son nom
        /// </summary>
        /// <param name="NomDeLaFamille"> Nom de la famille dont on veut connaitre la reference </param>
        /// <param name="ListeSousFamilles"> La liste des sous familles de notre base </param>
        /// <returns></returns>
        private static int TrouverRefSousFamille(string NomDeLaFamille, List<SousFamille> ListeSousFamilles)
        {
            foreach (SousFamille SousFamilleAComparer in ListeSousFamilles)
            {
                if (NomDeLaFamille == SousFamilleAComparer.NomSousFamille)
                {
                    return SousFamilleAComparer.RefSousFamille;
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

        private void FormImport_Load_1(object sender, EventArgs e)
        {

        }
    }

}
