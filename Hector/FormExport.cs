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
    public partial class FormExport : Form
    {
        public FormExport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var FileContent = string.Empty;
            var FilePath = string.Empty;

            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();

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

                    progressBar1.Visible = true;
                    progressBar1.Minimum = 1;
                    progressBar1.Maximum = ListArticles.Count()+2;
                    progressBar1.Value = 1;
                    progressBar1.Step = 1;

                    using (StreamWriter Writer = File.AppendText(openFileDialog.FileName))
                    {
                        Writer.WriteLine("Description;Ref;Marque;Famille;Sous-Famille;Prix H.T.");

                        progressBar1.PerformStep();

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
                    }
                }
            }

            Con.Close();
        }

        private void FormExport_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Veillez choisir un emplacement";
        }
    }
}
