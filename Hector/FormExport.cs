using Hector.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    /// <summary>
    /// Fenetre d'exportation de la base de données vers un fichier csv
    /// </summary>
    public partial class FormExport : Form
    {
        /// <summary>
        /// Constructeur par defaut de la classe
        /// </summary>
        public FormExport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gere le clic sur le bouton de selection de fichier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var FileContent = string.Empty;
            var FilePath = string.Empty;

            // Connexion à la base de données
            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();

            // Recuperer les articles
            List<Article> ListArticles = FonctionsSQLite.SQLiteRecupererArticles(Con);

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.label1.Text = openFileDialog.SafeFileName;

                    // Initialisation de la barre de progression
                    progressBar1.Visible = true;
                    progressBar1.Minimum = 1;
                    progressBar1.Maximum = ListArticles.Count()+2;
                    progressBar1.Value = 1;
                    progressBar1.Step = 1;

                    //On vide le fichier
                    File.WriteAllText(openFileDialog.FileName, "");

                    using (StreamWriter Writer = File.AppendText(openFileDialog.FileName))
                    {
                    
                        // Ecriture du header du csv
                        Writer.WriteLine("Description;Ref;Marque;Famille;Sous-Famille;Prix H.T.");

                        // mise à jour de la barre de progression
                        progressBar1.PerformStep();

                        // Ecriture de chaque ligne dans le csv
                        for (int Idarticle = 0; Idarticle < ListArticles.Count; Idarticle++)
                        {
                            Marque Marque = FonctionsSQLite.SQLiteRecupererMarques(Con)[ListArticles[Idarticle].RefMarque - 1];

                            SousFamille SousFam = FonctionsSQLite.SQLiteRecupererSousFamilles(Con)[ListArticles[Idarticle].RefSousFamille - 1];

                            Famille Famille = FonctionsSQLite.SQLiteRecupererFamilles(Con)[SousFam.RefFamille - 1];

                            string LineArticle = ListArticles[Idarticle].Description + ";"
                                + ListArticles[Idarticle].RefArticle + ";" + Marque.NomMarque + ";"
                                + Famille.NomFamille + ";" + SousFam.NomSousFamille + ";" + ListArticles[Idarticle].PrixHT;

                            Writer.WriteLine(LineArticle);

                            progressBar1.PerformStep();
                        }

                        MessageBox.Show("Le fichier a bien été exporté!", "Information");
                    }
                }
            }

            // Fermeture de la base de données
            Con.Close();

            this.Close();
        }

        /// <summary>
        /// Chargement de la page d'export
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormExport_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Veillez choisir un emplacement";
        }
    }
}
