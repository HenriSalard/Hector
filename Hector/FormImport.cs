using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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

        public FormImport()
        {
            InitializeComponent();
            this.Text = "Importer";

            // On initialise le texte du nom du fichier à vide 

            this.LabelFileName.Text = "";
            this.FilePath = string.Empty;


        }

        private void FormImport_Load(object sender, EventArgs e)
        {
            // On cache les boutons tant qu'aucun fichier n'est selectionné
            this.ButtonEcrasement.Visible = false;
            this.ButtonAjout.Visible = false;
            this.progressBar1.Visible = false;
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

                    ButtonAjout.Visible = true;
                    ButtonEcrasement.Visible = true;
                }
            }
            }

        private void ButtonEcrasement_Click(object sender, EventArgs e)
        {

        }

        private void ButtonAjout_Click(object sender, EventArgs e)
        {

        }

        private void CSVParser(string FilePath)
        {

        }

        private void LabelFileName_Click(object sender, EventArgs e)
        {

        }
    }

    }
