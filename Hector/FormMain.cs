﻿using Hector.Modele;
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
            
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

            


        }
    }

    



}

