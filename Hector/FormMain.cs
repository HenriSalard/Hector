using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
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

        protected void RefreshTree(string sqlpath)
        {
            
            string queryString =
            "SELECT OrderID, CustomerID FROM dbo.Orders;";

            //Ouverture et lecture de la table
            using (SQLiteConnection connection =
                       new SQLiteConnection(sqlpath))
            {
                SQLiteCommand command =
                    new SQLiteCommand(queryString, connection);
                connection.Open();

                SQLiteDataReader reader = command.ExecuteReader();

                // Call Read before accessing data.
                while (reader.Read())
                {
                    //SQLiteDataReader((IDataRecord)reader);
                }

                // Call Close when done reading.
                reader.Close();
            }
                        

            //treeView1.ExpandAll();

        }
    }



}

