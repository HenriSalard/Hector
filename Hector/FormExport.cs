using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "csv files (*.csv)|*.csv";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.label1.Text = openFileDialog.SafeFileName;
                    


                    //TODO Exportation
                    progressBar1.Visible = true;
                    progressBar1.Minimum = 1;
                    progressBar1.Maximum = 10;
                    progressBar1.Value = 1;
                    progressBar1.Step = 1;

                    // Loop through all files to copy.
                    for (int x = 1; x <= 10; x++)
                    {
                        
                        progressBar1.PerformStep();
                        
                    }

                }
            }
        }

        private void FormExport_Load(object sender, EventArgs e)
        {
            this.label1.Text = "Veillez choisir un emplacement";
        }
    }
}
