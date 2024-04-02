
namespace Hector
{
    partial class FormAjouterModifierArticle
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelRef = new System.Windows.Forms.Label();
            this.LabelDescription = new System.Windows.Forms.Label();
            this.LabelPrix = new System.Windows.Forms.Label();
            this.ComboBoxFamille = new System.Windows.Forms.ComboBox();
            this.LabelMarque = new System.Windows.Forms.Label();
            this.LabelFamille = new System.Windows.Forms.Label();
            this.LabelSousFamille = new System.Windows.Forms.Label();
            this.ComboBoxSousFamille = new System.Windows.Forms.ComboBox();
            this.ComboBoxMarque = new System.Windows.Forms.ComboBox();
            this.TextBoxRef = new System.Windows.Forms.TextBox();
            this.TextBoxPrix = new System.Windows.Forms.TextBox();
            this.TextBoxDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxQuantite = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelRef
            // 
            this.labelRef.AutoSize = true;
            this.labelRef.Location = new System.Drawing.Point(35, 72);
            this.labelRef.Name = "labelRef";
            this.labelRef.Size = new System.Drawing.Size(74, 17);
            this.labelRef.TabIndex = 1;
            this.labelRef.Text = "Reference";
            // 
            // LabelDescription
            // 
            this.LabelDescription.AutoSize = true;
            this.LabelDescription.Location = new System.Drawing.Point(35, 115);
            this.LabelDescription.Name = "LabelDescription";
            this.LabelDescription.Size = new System.Drawing.Size(79, 17);
            this.LabelDescription.TabIndex = 2;
            this.LabelDescription.Text = "Description";
            // 
            // LabelPrix
            // 
            this.LabelPrix.AutoSize = true;
            this.LabelPrix.Location = new System.Drawing.Point(35, 178);
            this.LabelPrix.Name = "LabelPrix";
            this.LabelPrix.Size = new System.Drawing.Size(54, 17);
            this.LabelPrix.TabIndex = 3;
            this.LabelPrix.Text = "Prix HT";
            // 
            // ComboBoxFamille
            // 
            this.ComboBoxFamille.FormattingEnabled = true;
            this.ComboBoxFamille.Location = new System.Drawing.Point(156, 263);
            this.ComboBoxFamille.Name = "ComboBoxFamille";
            this.ComboBoxFamille.Size = new System.Drawing.Size(191, 24);
            this.ComboBoxFamille.TabIndex = 4;
            // 
            // LabelMarque
            // 
            this.LabelMarque.AutoSize = true;
            this.LabelMarque.Location = new System.Drawing.Point(35, 225);
            this.LabelMarque.Name = "LabelMarque";
            this.LabelMarque.Size = new System.Drawing.Size(56, 17);
            this.LabelMarque.TabIndex = 5;
            this.LabelMarque.Text = "Marque";
            // 
            // LabelFamille
            // 
            this.LabelFamille.AutoSize = true;
            this.LabelFamille.Location = new System.Drawing.Point(35, 270);
            this.LabelFamille.Name = "LabelFamille";
            this.LabelFamille.Size = new System.Drawing.Size(52, 17);
            this.LabelFamille.TabIndex = 6;
            this.LabelFamille.Text = "Famille";
            // 
            // LabelSousFamille
            // 
            this.LabelSousFamille.AutoSize = true;
            this.LabelSousFamille.Location = new System.Drawing.Point(35, 318);
            this.LabelSousFamille.Name = "LabelSousFamille";
            this.LabelSousFamille.Size = new System.Drawing.Size(85, 17);
            this.LabelSousFamille.TabIndex = 7;
            this.LabelSousFamille.Text = "Sous-famille";
            // 
            // ComboBoxSousFamille
            // 
            this.ComboBoxSousFamille.FormattingEnabled = true;
            this.ComboBoxSousFamille.Location = new System.Drawing.Point(156, 311);
            this.ComboBoxSousFamille.Name = "ComboBoxSousFamille";
            this.ComboBoxSousFamille.Size = new System.Drawing.Size(191, 24);
            this.ComboBoxSousFamille.TabIndex = 8;
            // 
            // ComboBoxMarque
            // 
            this.ComboBoxMarque.FormattingEnabled = true;
            this.ComboBoxMarque.Location = new System.Drawing.Point(156, 218);
            this.ComboBoxMarque.Name = "ComboBoxMarque";
            this.ComboBoxMarque.Size = new System.Drawing.Size(191, 24);
            this.ComboBoxMarque.TabIndex = 9;
            // 
            // TextBoxRef
            // 
            this.TextBoxRef.Location = new System.Drawing.Point(156, 67);
            this.TextBoxRef.Name = "TextBoxRef";
            this.TextBoxRef.Size = new System.Drawing.Size(121, 22);
            this.TextBoxRef.TabIndex = 10;
            // 
            // TextBoxPrix
            // 
            this.TextBoxPrix.Location = new System.Drawing.Point(156, 175);
            this.TextBoxPrix.Name = "TextBoxPrix";
            this.TextBoxPrix.Size = new System.Drawing.Size(121, 22);
            this.TextBoxPrix.TabIndex = 11;
            // 
            // TextBoxDescription
            // 
            this.TextBoxDescription.Location = new System.Drawing.Point(156, 112);
            this.TextBoxDescription.Multiline = true;
            this.TextBoxDescription.Name = "TextBoxDescription";
            this.TextBoxDescription.Size = new System.Drawing.Size(250, 39);
            this.TextBoxDescription.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 367);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Quantite";
            // 
            // TextBoxQuantite
            // 
            this.TextBoxQuantite.Location = new System.Drawing.Point(156, 361);
            this.TextBoxQuantite.Name = "TextBoxQuantite";
            this.TextBoxQuantite.Size = new System.Drawing.Size(100, 22);
            this.TextBoxQuantite.TabIndex = 14;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(181, 412);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Valider";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormAjouterModifierArticle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 490);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TextBoxQuantite);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextBoxDescription);
            this.Controls.Add(this.TextBoxPrix);
            this.Controls.Add(this.TextBoxRef);
            this.Controls.Add(this.ComboBoxMarque);
            this.Controls.Add(this.ComboBoxSousFamille);
            this.Controls.Add(this.LabelSousFamille);
            this.Controls.Add(this.LabelFamille);
            this.Controls.Add(this.LabelMarque);
            this.Controls.Add(this.ComboBoxFamille);
            this.Controls.Add(this.LabelPrix);
            this.Controls.Add(this.LabelDescription);
            this.Controls.Add(this.labelRef);
            this.Controls.Add(this.label1);
            this.Name = "FormAjouterModifierArticle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormAjouterModifier_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelRef;
        private System.Windows.Forms.Label LabelDescription;
        private System.Windows.Forms.Label LabelPrix;
        private System.Windows.Forms.ComboBox ComboBoxFamille;
        private System.Windows.Forms.Label LabelMarque;
        private System.Windows.Forms.Label LabelFamille;
        private System.Windows.Forms.Label LabelSousFamille;
        private System.Windows.Forms.ComboBox ComboBoxSousFamille;
        private System.Windows.Forms.ComboBox ComboBoxMarque;
        private System.Windows.Forms.TextBox TextBoxRef;
        private System.Windows.Forms.TextBox TextBoxPrix;
        private System.Windows.Forms.TextBox TextBoxDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxQuantite;
        private System.Windows.Forms.Button button1;
    }
}