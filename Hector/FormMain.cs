using Hector.Modele;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Hector
{
    public partial class FormMain : Form
    {

        List<Article> ListArticle;

        List<Famille> ListFamille;

        List<SousFamille> ListSousFamille;

        List<Marque> ListMarque;

        private ListViewColumnSorter lvwColumnSorter;

        public void LoadLists()
        {
            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();

            ListArticle = FonctionsSQLite.SQLiteRecupererArticles(Con);

            ListMarque = FonctionsSQLite.SQLiteRecupererMarques(Con);

            ListFamille = FonctionsSQLite.SQLiteRecupererFamilles(Con);

            ListSousFamille = FonctionsSQLite.SQLiteRecupererSousFamilles(Con);

            Con.Close();
        }








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
                // Suppression de la fenetre Importer

                this.RefreshTree();

                ImportDialog.Dispose();
            }
            else
            {
            }

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

            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            listView1.FullRowSelect = true;

        }

        protected void RefreshTree()
        {

            LoadLists();
            
            treeView1.BeginUpdate();

            treeView1.Nodes.Clear();

            treeView1.Nodes.Add("Tous les articles");
            treeView1.Nodes.Add("Famille");
            treeView1.Nodes.Add("Marques");
            

            //Pour chaque Famille
            ListFamille.ForEach(delegate(Famille Fam){

                //On ajoute la famille à l'arbre
                treeView1.Nodes[1].Nodes.Add(Fam.RefFamille.ToString(), Fam.NomFamille);

            });

            ListSousFamille.ForEach(delegate (SousFamille Sousfam){

                //On cherche le noeud de la famille de cette sous famille
                treeView1.Nodes[1].Nodes.Find(Sousfam.RefFamille.ToString(), false)

                    //On n'en trouve qu'un seul (normalement)
                    [0].Nodes

                    //On ajoute à ce noeud la SousFamille
                    .Add(Sousfam.RefSousFamille.ToString(),Sousfam.NomSousFamille
                );

            });
            
            //Pour chaque Marque
            ListMarque.ForEach(delegate (Marque marque)
            {
                //On ajoute la marque à l'arbre
                treeView1.Nodes[2].Nodes.Add(marque.RefMarque.ToString(), marque.NomMarque);
            });

            treeView1.CollapseAll();

            treeView1.EndUpdate();
            
        }

        protected void HideColumns()
        {
            listView1.Columns[0].Width = 0;
            listView1.Columns[2].Width = 0;
            listView1.Columns[3].Width = 0;
            listView1.Columns[4].Width = 0;
            listView1.Columns[5].Width = 0;
            listView1.Columns[6].Width = 0;
        }

        protected void ShowColumns()
        {
            listView1.Columns[0].Width = -2;
            listView1.Columns[1].Width = -2;
            listView1.Columns[2].Width = -2;
            listView1.Columns[3].Width = -2;
            listView1.Columns[4].Width = -2;
            listView1.Columns[5].Width = -2;
            listView1.Columns[6].Width = -2;

        }

        protected void RefreshListArticle(string Category, string Refnoeud)
        {           

            this.listView1.BeginUpdate();

            listView1.Items.Clear();

            if (Category.Equals("NoeudArticle"))
            {
                ShowColumns();

                ListViewItem Item;

                string[] Array = new string[7];

                foreach (var Article in ListArticle)
                {
                    Array[0] = Article.RefArticle;
                    Array[1] = Article.Description;
                    SousFamille SousFam = ListSousFamille[Article.RefSousFamille - 1];
                    Famille Fam = ListFamille[SousFam.RefFamille - 1];
                    Array[2] = Fam.NomFamille;
                    Array[3] = SousFam.NomSousFamille;
                    Marque Marque = ListMarque[Article.RefMarque - 1];
                    Array[4] = Marque.NomMarque;
                    Array[5] = Article.Quantite.ToString();
                    Array[6] = Article.PrixHT.ToString();

                    Item = new ListViewItem(Array);
                    this.listView1.Items.Add(Item);

                }


            }
            else if (Category.Equals("NoeudMarque"))
            {
                HideColumns();

                string[] Array = new string[7];

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

                string[] Array = new string[7];

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

                SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

                Con.Open();

                //Chercher les sousfamilles de la famille NomNoeud
                List<SousFamille> ListSousFamille2 = FonctionsSQLite.SQLiteSousFamilleFromFamilleName(Con, Refnoeud);

                Con.Close();

                HideColumns();

                string[] Array = new string[7];

                ListViewItem Item;

                foreach (var SousFamille in ListSousFamille2)
                {
                    Array[1] = SousFamille.NomSousFamille;

                    Item = new ListViewItem(Array);
                    this.listView1.Items.Add(Item);
                }

            }
            else if (Category.Equals("SousArticle"))
            {

                ShowColumns();

                ListViewItem Item;

                string[] Array = new string[7];

                List<Article> ResultArticle = ListArticle.FindAll(
                    delegate (Article Article)
                    {
                        return Article.RefSousFamille.ToString() == Refnoeud;
                    }
                );
                if (ResultArticle != null)
                {

                    
                    

                    foreach (var Article in ResultArticle)
                    {
                        Array[1] = Article.Description;
                        Array[0] = Article.RefArticle;
                        SousFamille SousFam = ListSousFamille[Article.RefSousFamille - 1];
                        Famille Fam = ListFamille[SousFam.RefFamille - 1];
                        Array[2] = Fam.NomFamille;
                        Array[3] = SousFam.NomSousFamille;
                        Marque Marque = ListMarque[Article.RefMarque - 1];
                        Array[4] = Marque.NomMarque;
                        Array[5] = Article.Quantite.ToString();
                        Array[6] = Article.PrixHT.ToString();

                        Item = new ListViewItem(Array);
                        this.listView1.Items.Add(Item);
                    }

                }

            }
            else if (Category.Equals("MarqueArticle"))
            {
                ShowColumns();

                ListViewItem Item;

                string[] Array = new string[7];

                List<Article> ResultArticle = ListArticle.FindAll(
                    delegate (Article Article)
                    {
                        return Article.RefMarque.ToString() == Refnoeud;
                    }
                );
                if (ResultArticle != null)
                {
                    foreach (var Article in ResultArticle)
                    {
                        Array[1] = Article.Description;
                        Array[0] = Article.RefArticle;
                        SousFamille SousFam = ListSousFamille[Article.RefSousFamille - 1];
                        Famille Fam = ListFamille[SousFam.RefFamille - 1];
                        Array[2] = Fam.NomFamille;
                        Array[3] = SousFam.NomSousFamille;
                        Marque Marque = ListMarque[Article.RefMarque - 1];
                        Array[4] = Marque.NomMarque;
                        Array[5] = Article.Quantite.ToString();
                        Array[6] = Article.PrixHT.ToString();

                        Item = new ListViewItem(Array);
                        this.listView1.Items.Add(Item);
                    }

                }

            }

            this.listView1.EndUpdate();

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
            //Le cas des Articles des Marques
            else if (Node.Parent.FullPath.Equals("Marques"))
            {
                RefreshListArticle("MarqueArticle", Node.Name);
            }
            //Le cas Article de sous famille
            else if (Node.Parent.Parent.FullPath.Equals("Famille"))
            {
                RefreshListArticle("SousArticle", Node.Name);
            }
            
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
        }

        public class ListViewColumnSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            private int ColumnToSort;
            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort;
            /// <summary>
            /// Case insensitive comparer object
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            /// <summary>
            /// Class constructor.  Initializes various elements
            /// </summary>
            public ListViewColumnSorter()
            {
                // Initialize the column to '0'
                ColumnToSort = 0;

                // Initialize the sort order to 'none'
                OrderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // Cast the objects to be compared to ListViewItem objects
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                decimal num = 0;
                if (decimal.TryParse(listviewX.SubItems[ColumnToSort].Text, out num))
                {
                    compareResult = decimal.Compare(num, Convert.ToDecimal(listviewY.SubItems[ColumnToSort].Text));
                }
                else
                {
                    // Compare the two items
                    compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
                }

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }

            /// <summary>
            /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            /// <summary>
            /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
            /// </summary>
            public SortOrder Order
            {
                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }

        }

        private void actualiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AllRefresh();
        }

        private void AllRefresh()
        {
            LoadLists();
            RefreshTree();
            RefreshListArticle("NoeudArticle", null);
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
            {
                AllRefresh();
            }
            if(e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                ListViewItem Item = listView1.SelectedItems[0];

                FormAjouterModifierArticle ImportDialog =
                            new FormAjouterModifierArticle(false, ListFamille, ListSousFamille, ListMarque, ListArticle.Find(x => x.RefArticle == Item.Text));

                // Affichage de la fenetre Importer


                if (ImportDialog.ShowDialog(this) == DialogResult.OK)
                {
                    // Suppression de la fenetre Importer

                    this.RefreshTree();

                    ImportDialog.Dispose();
                }
            }
            if(e.KeyCode == Keys.Delete)
            {
                //Dialogue de suppresion

            }
        }
        

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem Item = listView1.SelectedItems[0];

            FormAjouterModifierArticle ImportDialog =
                        new FormAjouterModifierArticle(false, ListFamille, ListSousFamille, ListMarque, ListArticle.Find(x => x.RefArticle == Item.Text));

            // Affichage de la fenetre Importer


            if (ImportDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Suppression de la fenetre Importer

                this.RefreshTree();

                ImportDialog.Dispose();
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (e.Button == MouseButtons.Right)
            {                
                supprimerToolStripMenuItem.Enabled = true;
                modifierToolStripMenuItem.Enabled = true;                
                contextMenuStrip1.Show(c, e.Location);
            }
        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem Item = listView1.SelectedItems[0];

            FormAjouterModifierArticle ImportDialog =
                        new FormAjouterModifierArticle(false, ListFamille, ListSousFamille, ListMarque, ListArticle.Find(x => x.RefArticle == Item.Text));

            // Affichage de la fenetre Importer


            if (ImportDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Suppression de la fenetre Importer

                this.RefreshTree();

                ImportDialog.Dispose();
            }
        }

        private void ajouterToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ListViewItem Item = listView1.SelectedItems[0];

            FormAjouterModifierArticle ImportDialog =
                        new FormAjouterModifierArticle(true, ListFamille, ListSousFamille, ListMarque, ListArticle.Find(x => x.RefArticle == Item.Text));

            // Affichage de la fenetre Importer


            if (ImportDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Suppression de la fenetre Importer

                this.RefreshTree();

                ImportDialog.Dispose();
            }

        }

        private void FormMain_MouseClick(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (e.Button == MouseButtons.Right)
            {
                supprimerToolStripMenuItem.Enabled = false;
                modifierToolStripMenuItem.Enabled = false;
                contextMenuStrip1.Show(c, e.Location);
            }
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string message = "Etes vous sûr de supprimer cet ligne?\nCette action est irrévocable.";
            const string caption = "Supprimer élément";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.OKCancel,
                                         MessageBoxIcon.Warning);

            
            if (result == DialogResult.Yes)
            {
                // TODO Mettre code pour supprimer élément







            }
        }
    }

    



}

