using Hector.Modele;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Hector
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            this.Text = "Fenetre principale";
        }

        private void importerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creation de la fenetre Importer

            FormImport ImportDialog = new FormImport();

            // Affichage de la fenetre Importer

            
            if (ImportDialog.ShowDialog(this) == DialogResult.OK)
            {
                
            }
            else
            {
            }

            // Suppression de la fenetre Importer

            this.RefreshTree();

            ImportDialog.Dispose();


        }

        private void fichiersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormExport Exportdialog = new FormExport();

            if (Exportdialog.ShowDialog(this) == DialogResult.OK)
            {

            }
            else
            {

            }

            // Suppression de la fenetre Importer

            Exportdialog.Dispose();

        }

        private void FormMain_Load(object sender, EventArgs e)
        {

            this.RefreshTree();

        }

        protected void RefreshTree()
        {

            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();

            treeView1.BeginUpdate();

            treeView1.Nodes.Clear();

            treeView1.Nodes.Add("Tous les articles");
            treeView1.Nodes.Add("Famille");
            treeView1.Nodes.Add("Marques");

            //Recupere liste des familles
            List<Famille> lFamille = FonctionsSQLite.SQLiteRecupererFamilles(Con);

            //Pour chaque Famille
            lFamille.ForEach(delegate(Famille Fam){

                //On ajoute la famille à l'arbre
                treeView1.Nodes[1].Nodes.Add(Fam.RefFamille.ToString(), Fam.NomFamille);

            });

            //Recupere liste des sous-familles
            List<SousFamille> lSousFamille = FonctionsSQLite.SQLiteRecupererSousFamilles(Con);

            lSousFamille.ForEach(delegate (SousFamille Sousfam){

                //On cherche le noeud de la famille de cette sous famille
                treeView1.Nodes[1].Nodes.Find(Sousfam.RefFamille.ToString(), false)

                    //On n'en trouve qu'un seul (normalement)
                    [0].Nodes

                    //On ajoute à ce noeud la SousFamille
                    .Add(Sousfam.RefSousFamille.ToString(),Sousfam.NomSousFamille
                );

            });

            //Recupere liste des marques
            List<Marque> lMarque = FonctionsSQLite.SQLiteRecupererMarques(Con);

            //Pour chaque Marque
            lMarque.ForEach(delegate (Marque marque)
            {
                //On ajoute la marque à l'arbre
                treeView1.Nodes[2].Nodes.Add(marque.RefMarque.ToString(), marque.NomMarque);
            });

            treeView1.CollapseAll();

            treeView1.EndUpdate();

            Con.Close();
            
        }

        protected void HideColumns()
        {
            listView1.Columns[0].Width = 0;
            listView1.Columns[2].Width = 0;
            listView1.Columns[3].Width = 0;
            listView1.Columns[4].Width = 0;
            listView1.Columns[5].Width = 0;
        }

        protected void ShowColumns()
        {
            listView1.Columns[0].Width = -2;
            listView1.Columns[1].Width = -2;
            listView1.Columns[2].Width = -2;
            listView1.Columns[3].Width = -2;
            listView1.Columns[4].Width = -2;
            listView1.Columns[5].Width = -2;

        }

        protected void RefreshListArticle(string Category, string Nomnoeud)
        {
            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();

            List<Article> ListArticle = FonctionsSQLite.SQLiteRecupererArticles(Con);

            List<Marque> ListMarque = FonctionsSQLite.SQLiteRecupererMarques(Con);

            List<Famille> ListFamille = FonctionsSQLite.SQLiteRecupererFamilles(Con);

            List<SousFamille> ListSousFamille = FonctionsSQLite.SQLiteRecupererSousFamilles(Con);

            this.listView1.BeginUpdate();

            listView1.Items.Clear();

            if (Category.Equals("NoeudArticle"))
            {
                ShowColumns();

                ListViewItem Item;

                string[] Array = new string[6];

                foreach(var Article in ListArticle)
                {
                    Array[0] = Article.Description;
                    Array[1] = Article.RefArticle;
                    SousFamille SousFam = ListSousFamille[Article.RefSousFamille - 1];
                    Famille Fam = ListFamille[SousFam.RefFamille - 1];
                    Array[2] = Fam.NomFamille;
                    Array[3] = SousFam.NomSousFamille;
                    Marque Marque = ListMarque[Article.RefMarque - 1];
                    Array[4] = Marque.NomMarque;
                    Array[5] = Article.Quantite.ToString();

                    Item = new ListViewItem(Array);
                    this.listView1.Items.Add(Item);

                }


            }
            else if (Category.Equals("NoeudMarque"))
            {
                HideColumns();

                string[] Array = new string[2];

                ListViewItem Item;

                foreach (var Marque in ListMarque)
                {
                    Array[1] = Marque.NomMarque;

                    Item = new ListViewItem(Array);
                    this.listView1.Items.Add(Item);
                }
            }
            else if (Category.Equals("NoeudFamille"))
            {
                HideColumns();

                string[] Array = new string[2];

                ListViewItem Item;

                foreach (var Famille in ListFamille)
                {
                    Array[1] = Famille.NomFamille;

                    Item = new ListViewItem(Array);
                    this.listView1.Items.Add(Item);
                }

            }
            else if (Category.Equals("NoeudSousFamille"))
            {

                //Chercher les sousfamilles de la famille NomNoeud
                List<SousFamille> ListSousFamille2 = FonctionsSQLite.SQLiteSousFamilleFromFamilleName(Con, Nomnoeud);

                HideColumns();

                string[] Array = new string[2];

                ListViewItem Item;

                foreach (var SousFamille in ListSousFamille2)
                {
                    Array[1] = SousFamille.NomSousFamille;

                    Item = new ListViewItem(Array);
                    this.listView1.Items.Add(Item);
                }

            }

            this.listView1.EndUpdate();
            Con.Close();

        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

            


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            TreeNode Node = treeView1.SelectedNode;

            if (Node.FullPath.Equals("Marques"))
            {
                RefreshListArticle("NoeudMarque", null);
            }
            else if (Node.FullPath.Equals("Tous les articles"))
            {
                RefreshListArticle("NoeudArticle",null);
            }
            else if(Node.FullPath.Equals("Famille"))
            {
                RefreshListArticle("NoeudFamille", null);
            }
            //Le cas sousfamille
            else if (Node.Parent.FullPath.Equals("Famille"))
            {
                RefreshListArticle("NoeudSousFamille", Node.Text);
            }
        }
    }

    



}

