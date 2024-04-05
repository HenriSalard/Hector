using Hector.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    public partial class FormAjouterModifierArticle : Form
    {

        private bool EstAjouter; // Renseigne si on ajoute un article

        private Article ArticleAModifier; // Renseigne l'article à modifier, dans le cas ou on modifie

        private List<Famille> ListeFamilles;

        private List<SousFamille> ListeSousFamilles;

        private List<Marque> ListeMarques;

        private List<SousFamille> ListeSousFamillesPossibles;

        private Article NouvelArticle; // le nouvel article a ajouter

        public FormAjouterModifierArticle(bool EstAjouter, List<Famille> Familles , List<SousFamille> SousFamilles, 
            List<Marque> Marques ,Article ArticleAModif = null)
        {
            ArticleAModifier = ArticleAModif;
            ListeFamilles = Familles;
            ListeMarques = Marques;
            ListeSousFamilles = SousFamilles;
            ListeSousFamillesPossibles = new List<SousFamille>();
            NouvelArticle = null;


            this.EstAjouter = EstAjouter;
            InitializeComponent();
        }

        private void FormAjouterModifier_Load(object sender, EventArgs e)
        {

            if (EstAjouter)
            {
                label1.Text = "Creer un article";
            }

            else
            {
                label1.Text = "Modifier un article";
            }

            // On met les familles dans le ComboBox
            ComboBoxFamille.DataSource = ListeFamilles;

            // Le comboBox affichera l'attribut nomFamille 
            ComboBoxFamille.DisplayMember = "NomFamille";

            ComboBoxSousFamille.Enabled =  false;

            ComboBoxMarque.DataSource = ListeMarques;

            ComboBoxMarque.DisplayMember = "NomMarque";


            // CAS PAGE MODIFIER ARTICLE
            // On va mettre des place holder dans chaque textBox etc... avec les bonnes infos
            if (!EstAjouter && ArticleAModifier != null)
            {
                label1.Text = "Modification de l'article";

                // On met la ref et on ne peut pas la modifier (clé principale)
                TextBoxRef.Text = ArticleAModifier.RefArticle;
                TextBoxRef.ReadOnly = true;

                TextBoxDescription.Text = ArticleAModifier.Description;
                TextBoxDescription.MaxLength = 150; // 150 caracteres max

                TextBoxPrix.Text = ArticleAModifier.PrixHT.ToString();

                TextBoxQuantite.Text = ArticleAModifier.Quantite.ToString();

                // Dans la partie suivante, on initialise les valeurs des comboBox à leurs valeurs initiales

                foreach(SousFamille SF in ListeSousFamilles)
                {

                    // Recuperation de la sous famille initiale
                    if(SF.RefSousFamille == ArticleAModifier.RefSousFamille)
                    {

                        foreach (Famille Fam in ListeFamilles)
                        {

                            // Recuperation de la famille initiale
                            if (Fam.RefFamille == SF.RefFamille)
                            {
                                ComboBoxFamille.SelectedItem = Fam;

                                // recuperation des sous familles a afficher (on affiche que celles liées à la famille séléctionnée)
                                ListeSousFamillesPossibles = TrouverSousFamilles(ListeFamilles[ComboBoxFamille.SelectedIndex]);

                                ComboBoxSousFamille.DataSource = ListeSousFamillesPossibles;

                                ComboBoxSousFamille.DisplayMember = "NomSousFamille";

                                ComboBoxSousFamille.Enabled = true;

                                break;

                            }

                        }

                    }
                }

                foreach(Marque BoucleMarque in ListeMarques)
                {
                    if(BoucleMarque.RefMarque == ArticleAModifier.RefMarque)
                    {
                        ComboBoxMarque.SelectedItem = BoucleMarque;
                    }
                }

            }
        }



        /// <summary>
        /// Verifie la validité des données saisies. Puis met à jour l'article dans l'application et la base de données
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // Affichage d'un message d'erreur en cas d'invalidite d'un champ
            if (TextBoxRef.Text.Length == 0 && EstAjouter)
            {
                string Message = "Erreur! Veuillez saisir une reference";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }
            
            if (TextBoxDescription.Text.Length == 0)
            {
                string Message = "Erreur! Veuillez saisir un description";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            if(TextBoxDescription.Text.Length > 150)
            {
                string Message = "Erreur! Description trop longue!";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            if(TextBoxPrix.Text.Length == 0)
            {
                string Message = "Erreur! Veuillez saisir un prix!";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            if (TextBoxQuantite.Text.Length == 0)
            {
                string Message = "Erreur! Veuillez saisir une quantité.";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            // Verrification que le prix est un float
            if (!float.TryParse(TextBoxPrix.Text, out float resultatPrix))
            {
                string Message = "Erreur! Prix incorrect! (format: 20,5)";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            // Verrification que la quantite est un int et > 0
            if (!int.TryParse(TextBoxQuantite.Text, out int resultatquantite))
            {
                string Message = "Erreur! La quantite est inccorecte";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            if (ComboBoxMarque.SelectedIndex == -1)
            {
                string Message = "Erreur! Veuillez choisir une marque.";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            if (ComboBoxFamille.SelectedIndex == -1)
            {
                string Message = "Erreur! Veuillez choisir une famille.";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            if (ComboBoxSousFamille.SelectedIndex == -1)
            {
                string Message = "Erreur! Veuillez choisir une sous-Famille.";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            // Verrification que la sous famille selectionnée va bien avec la famille selectionnée
            if(ListeFamilles[ComboBoxFamille.SelectedIndex].RefFamille 
                != ListeSousFamillesPossibles[ComboBoxSousFamille.SelectedIndex].RefFamille)
            {
                string Message = "Erreur! La famille et la sous-famille ne sont pas liées.";
                MessageBox.Show(this, Message, "Erreur");
                return;
            }

            // Une fois les verifications faites on peut mettre a jour l'article

            //Trouve le chemin vers le fichier de la bdd
            SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

            Con.Open();

            // Cas modification
            if (!EstAjouter)
            {
                ArticleAModifier.Description = TextBoxDescription.Text;
                ArticleAModifier.PrixHT = float.Parse(TextBoxPrix.Text);
                ArticleAModifier.Quantite = int.Parse(TextBoxQuantite.Text);
                ArticleAModifier.RefMarque = ListeMarques[ComboBoxMarque.SelectedIndex].RefMarque;
                ArticleAModifier.RefSousFamille = ListeSousFamillesPossibles[ComboBoxSousFamille.SelectedIndex].RefSousFamille;

                SQLiteCommand CommandeInsert = new SQLiteCommand(string.Empty, Con); // Definition de la commande a utiliser pour modifier la bdd

                CommandeInsert.CommandText = "UPDATE Articles SET Description = '" + ArticleAModifier.Description + "', RefSousFamille = '" + ArticleAModifier.RefSousFamille + "'," +
                    " PrixHT = '" + ArticleAModifier.PrixHT.ToString("0.00", new CultureInfo("fr-FR")) + "', RefMarque = '" + ArticleAModifier.RefMarque + "' WHERE RefArticle = '" + ArticleAModifier.RefArticle + "'";
                
                CommandeInsert.ExecuteNonQuery();

                Con.Close();

                // Affichage d'une confirmation de la modification

                DialogResult result;

                result = MessageBox.Show(this, "La modification a bien été prise en compte.", "Importation terminée", MessageBoxButtons.OK);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // ferme la fenetre et continue l'execution de formMain
                    this.DialogResult = DialogResult.OK;
                }
            }

            // Cas ajout
            else
            {
                NouvelArticle = new Article(TextBoxRef.Text, ListeSousFamillesPossibles[ComboBoxSousFamille.SelectedIndex].RefSousFamille, ListeMarques[ComboBoxMarque.SelectedIndex].RefMarque,
                    TextBoxDescription.Text, float.Parse(TextBoxPrix.Text), int.Parse(TextBoxQuantite.Text));

                SQLiteCommand CommandeInsert = new SQLiteCommand(string.Empty, Con);

                CommandeInsert.CommandText = "INSERT INTO Articles(RefArticle, Description, RefSousFamille, RefMarque, PrixHT, Quantite) VALUES('"
                    + NouvelArticle.RefArticle + "', '" + NouvelArticle.Description + "', '" + NouvelArticle.RefSousFamille + "', '" + NouvelArticle.RefMarque
                     + "', '" + NouvelArticle.PrixHT.ToString("0.00", new CultureInfo("fr-FR")) + "', '" + NouvelArticle.Quantite + "')";
                CommandeInsert.ExecuteNonQuery();

                Con.Close();

                // Affichage d'une confirmation de l'ajout

                DialogResult result;

                result = MessageBox.Show(this, "L'article a bien été ajouté.", "Ajout terminé", MessageBoxButtons.OK);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // ferme la fenetre et continue l'execution de formMain
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void ComboBoxFamille_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListeSousFamillesPossibles = TrouverSousFamilles(ListeFamilles[ComboBoxFamille.SelectedIndex]);

            ComboBoxSousFamille.DataSource = ListeSousFamillesPossibles;

            ComboBoxSousFamille.DisplayMember = "NomSousFamille";

            ComboBoxSousFamille.Enabled = true;
        }

        private List<SousFamille> TrouverSousFamilles(Famille FamilleSelection)
        {
            List<SousFamille> SousFamillesPossibles = new List<SousFamille>();
            int RefFamille = FamilleSelection.RefFamille;

            foreach (SousFamille SF in ListeSousFamilles)
            {
                if (SF.RefFamille == RefFamille)
                {
                    SousFamillesPossibles.Add(SF);
                }
            }

            return SousFamillesPossibles;
        }

        public Article GetArticle()
        {
            if (!EstAjouter)
            {
                return ArticleAModifier;
            }
            else
            {
                return NouvelArticle;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
