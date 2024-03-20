
namespace Hector
{
    partial class FormImport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.LabelFileName = new System.Windows.Forms.Label();
            this.ButtonEcrasement = new System.Windows.Forms.Button();
            this.ButtonAjout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(96, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(186, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Importer un fichier CSV";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LabelFileName
            // 
            this.LabelFileName.AutoSize = true;
            this.LabelFileName.Location = new System.Drawing.Point(122, 72);
            this.LabelFileName.Name = "LabelFileName";
            this.LabelFileName.Size = new System.Drawing.Size(124, 17);
            this.LabelFileName.TabIndex = 1;
            this.LabelFileName.Text = "Nom du fichier csv";
            // 
            // ButtonEcrasement
            // 
            this.ButtonEcrasement.Location = new System.Drawing.Point(51, 128);
            this.ButtonEcrasement.Name = "ButtonEcrasement";
            this.ButtonEcrasement.Size = new System.Drawing.Size(278, 38);
            this.ButtonEcrasement.TabIndex = 2;
            this.ButtonEcrasement.Text = "Integrer en mode écrasement";
            this.ButtonEcrasement.UseVisualStyleBackColor = true;
            this.ButtonEcrasement.Click += new System.EventHandler(this.ButtonEcrasement_Click);
            // 
            // ButtonAjout
            // 
            this.ButtonAjout.Location = new System.Drawing.Point(51, 183);
            this.ButtonAjout.Name = "ButtonAjout";
            this.ButtonAjout.Size = new System.Drawing.Size(278, 42);
            this.ButtonAjout.TabIndex = 3;
            this.ButtonAjout.Text = "Integrer en mode ajout";
            this.ButtonAjout.UseVisualStyleBackColor = true;
            this.ButtonAjout.Click += new System.EventHandler(this.ButtonAjout_Click);
            // 
            // FormImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 353);
            this.Controls.Add(this.ButtonAjout);
            this.Controls.Add(this.ButtonEcrasement);
            this.Controls.Add(this.LabelFileName);
            this.Controls.Add(this.button1);
            this.Name = "FormImport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormImport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label LabelFileName;
        private System.Windows.Forms.Button ButtonEcrasement;
        private System.Windows.Forms.Button ButtonAjout;
    }
}