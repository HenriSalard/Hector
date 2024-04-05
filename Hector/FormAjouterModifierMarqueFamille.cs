using Hector.Modele;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hector
{
    public partial class FormAjouterModifierMarqueFamille : Form
    {
        private List<Famille> ListeFamilles;

        private List<SousFamille> ListeSousFamilles;

        private List<Marque> ListeMarques;

        private string TypeDePage; // Definie si on gere une famile ou une sous-famille ou une marque

        private bool EstAjouter;

        private Marque MarqueAModifier;

        private Famille FamilleAModifier;

        SousFamille SousFamilleAModifier;



        public FormAjouterModifierMarqueFamille(bool Ajouter, string Type, List<Famille> ListeFamilles, List<SousFamille> ListeSousFamilles, List<Marque> ListeMarques,
            Marque MarqueAModifier = null, Famille FamilleAModifier=null, SousFamille SousFamilleAModifier = null)
        {
            EstAjouter = Ajouter;

            TypeDePage = Type;

            this.ListeFamilles = ListeFamilles;

            this.ListeSousFamilles = ListeSousFamilles;

            this.ListeMarques = ListeMarques;

            this.MarqueAModifier = MarqueAModifier;

            this.FamilleAModifier = FamilleAModifier;

            this.SousFamilleAModifier = SousFamilleAModifier;

            InitializeComponent();
        }

        private void FormAjouterModifierMarqueFamille_Load(object sender, EventArgs e)
        {

            // Load page d'ajout
            if (EstAjouter)
            {
                if(TypeDePage == "Marque")
                {
                    label1.Text = "Creer une marque";

                    ComboBoxFamille.Visible = false;
                }

                if(TypeDePage == "Famille")
                {
                    label1.Text = "Creer une famille";

                    ComboBoxFamille.Visible = false;
                }

                // Y a que pour les sous familles qu'on affiche le choix de la famille
                if (TypeDePage == "SousFamille")
                {
                    label1.Text = "Creer une sous-famille";

                    ComboBoxFamille.Visible = true;

                    // On met les familles dans le ComboBox
                    ComboBoxFamille.DataSource = ListeFamilles;

                    // Le comboBox affichera l'attribut nomFamille 
                    ComboBoxFamille.DisplayMember = "NomFamille";

                }

                // Cas d'erreur
                if(TypeDePage != "Marque" && TypeDePage != "Famille" && TypeDePage != "SousFamille")
                {
                    MessageBox.Show(this, "Erreur");
                    this.DialogResult = DialogResult.OK;
                }
            }

            // Cas modification d'un element, on affiche ses attributs actuels
            else
            {
                if(TypeDePage == "Marque")
                {
                    label1.Text = "Modifier la marque n° " + MarqueAModifier.RefMarque;

                    textBox1.Text = MarqueAModifier.NomMarque;
                }

                if(TypeDePage == "Famille")
                {
                    label1.Text = "Modifier la famille n°" + FamilleAModifier.RefFamille;

                    textBox1.Text = FamilleAModifier.NomFamille;
                }
                if(TypeDePage == "SousFamille")
                {
                    label1.Text = "Modifier la sous-famille n°" + SousFamilleAModifier.RefSousFamille;

                    // On cherche la famille de la sousFamille a modifier et on place le comboBox dessus
                    foreach(Famille Fam in ListeFamilles)
                    {
                        if(Fam.RefFamille == SousFamilleAModifier.RefFamille)
                        {
                            ComboBoxFamille.SelectedItem = Fam;
                            break;
                        }
                    }

                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length == 0)
            {
                MessageBox.Show(this, "Veuillez saisir un nom", "Erreur");
                return;
            }
            else
            {
                SQLiteConnection Con = new SQLiteConnection("URI=file:"
                + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                + "\\Hector.sqlite");

                Con.Open();

                // Cas Ajout
                if (EstAjouter)
                {

                    SQLiteCommand CommandeInsert = new SQLiteCommand(string.Empty, Con); // Definition de la commande a utiliser pour modifier la bdd

                    if (TypeDePage == "Marque")
                    {
                        // Creation de la marque a ajouter qui sera renvoyé à formMain
                        MarqueAModifier = new Marque(ListeMarques.Count + 1, textBox1.Text);

                        // Ajout de la marque à la bdd
                        CommandeInsert.CommandText = "INSERT INTO Marques(RefMarque, Nom) Values('" + ListeMarques.Count + 1 + "', '" + textBox1.Text + "')";

                        CommandeInsert.ExecuteNonQuery();
                    }

                    if(TypeDePage == "Famille")
                    {
                        // Creation de la famille a ajouter qui sera renvoyé à formMain
                        FamilleAModifier = new Famille(ListeFamilles.Count + 1, textBox1.Text);

                        // Ajout de la famille a la bdd
                        CommandeInsert.CommandText = "INSERT INTO Familles(RefFamille, Nom) VALUES( '" + ListeFamilles.Count + 1 + "', '" + textBox1.Text + "' )";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    if (TypeDePage == "SousFamille")
                    {
                        // Creation de la sous famille qui sera renvoyé à formMain
                        SousFamilleAModifier = new SousFamille(ListeSousFamilles.Count + 1, ListeFamilles[ComboBoxFamille.SelectedIndex].RefFamille, textBox1.Text);

                        // Ajout de la sous famille à la bdd
                        CommandeInsert.CommandText = "INSERT INTO SousFamilles(RefSousFamille, RefFamille, Nom) VALUES( '"
                        + ListeSousFamilles.Count + 1 + "', '" + ListeFamilles[ComboBoxFamille.SelectedIndex].RefFamille + "', '" + textBox1.Text + "' )";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    Con.Close();

                    // Affichage d'une confirmation de l'ajout

                    DialogResult result;

                    result = MessageBox.Show(this, "L'element a bien été ajouté.", "Ajout terminé", MessageBoxButtons.OK);

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        // ferme la fenetre et continue l'execution de formMain
                        this.DialogResult = DialogResult.OK;
                    }
                }

                // Cas modification
                else
                {
                    SQLiteCommand CommandeInsert = new SQLiteCommand(string.Empty, Con); // Definition de la commande a utiliser pour modifier la bdd

                    if (TypeDePage == "Marque")
                    {

                        // Modification de la marque dans la bdd
                        CommandeInsert.CommandText = "UPDATE Marques SET Nom = '" + textBox1.Text + "' WHERE RefMarque = '" + MarqueAModifier.RefMarque + "'";

                        CommandeInsert.ExecuteNonQuery();
                    }

                    if (TypeDePage == "Famille")
                    {

                        // Modification de la famille dans la bdd
                        CommandeInsert.CommandText = "UPDATE Familles SET Nom = '" + textBox1.Text + "' WHERE RefFamille = '" + FamilleAModifier.RefFamille + "'";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    if (TypeDePage == "SousFamille")
                    {

                        // Modification de la sous famille dans la bdd
                        CommandeInsert.CommandText = "UPDATE SousFamilles SET Nom = '" + textBox1.Text + "', RefFamille = '" + ListeFamilles[ComboBoxFamille.SelectedIndex].RefFamille
                            + "' WHERE RefSousFamille = '" + SousFamilleAModifier.RefSousFamille + "'";
                        CommandeInsert.ExecuteNonQuery();
                    }

                    Con.Close();

                    // Affichage d'une confirmation de l'ajout

                    DialogResult result;

                    result = MessageBox.Show(this, "L'element a bien été modifié.", "MAJ terminée", MessageBoxButtons.OK);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        // ferme la fenetre et continue l'execution de formMain
                        this.DialogResult = DialogResult.OK;
                    }
                }
            }

        }

        public Famille GetFamille()
        {
            return FamilleAModifier;
        }

        public SousFamille GetSousFamille()
        {
            return SousFamilleAModifier;
        }

        public Marque GetMarque()
        {
            return MarqueAModifier;
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
