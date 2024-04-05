using Hector.Modele;
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
    /// <summary>
    /// Fenetre d'ajout/modification des marques,familles et sous-familles
    /// </summary>
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


        /// <summary>
        /// Constructeur de la pfenetre d'ajout/modification d'une marque/famille/sous famille
        /// </summary>
        /// <param name="Ajouter"> Bool : Dit si on c'est une fenetre d'ajout ou de modification</param>
        /// <param name="Type"> String : "Marque" ou "Famille" ou "SousFamille" , donne le type d'element à modifier</param>
        /// <param name="ListeFamilles"> La liste de toutes les familles </param>
        /// <param name="ListeSousFamilles"> La liste de toutes les sous familles </param>
        /// <param name="ListeMarques">La liste de toutes les marques</param>
        /// <param name="MarqueAModifier">La marque a modifier, null par defaut</param>
        /// <param name="FamilleAModifier">La famille a modifier, null par defaut</param>
        /// <param name="SousFamilleAModifier">La sous famille a modifier, null par defaut</param>
        public FormAjouterModifierMarqueFamille(bool Ajouter, string Type, List<Famille> ListeFamilles, List<SousFamille> ListeSousFamilles, List<Marque> ListeMarques,
            Marque MarqueAModifier, Famille FamilleAModifier, SousFamille SousFamilleAModifier)
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

        /// <summary>
        /// Gere l'affichage initial de la page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormAjouterModifierMarqueFamille_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = 50;

            // Load page d'ajout
            if (EstAjouter)
            {
                if(TypeDePage == "Marque")
                {
                    label1.Text = "Creer une marque";

                    ComboBoxFamille.Visible = false;

                    label3.Visible = false;
                }

                if(TypeDePage == "Famille")
                {
                    label1.Text = "Creer une famille";

                    ComboBoxFamille.Visible = false;

                    label3.Visible = false;
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

                    ComboBoxFamille.Visible = false;

                    label3.Visible = false;
                }

                if(TypeDePage == "Famille")
                {
                    label1.Text = "Modifier la famille n°" + FamilleAModifier.RefFamille;

                    textBox1.Text = FamilleAModifier.NomFamille;

                    ComboBoxFamille.Visible = false;

                    label3.Visible = false;
                }

                if(TypeDePage == "SousFamille")
                {
                    label1.Text = "Modifier la sous-famille n°" + SousFamilleAModifier.RefSousFamille;

                    textBox1.Text = SousFamilleAModifier.NomSousFamille;

                    ComboBoxFamille.Visible = true;

                    // On met les familles dans le ComboBox
                    ComboBoxFamille.DataSource = ListeFamilles;

                    // Le comboBox affichera l'attribut nomFamille 
                    ComboBoxFamille.DisplayMember = "NomFamille";

                    // On cherche la famille de la sousFamille a modifier et on place le comboBox dessus
                    foreach (Famille Fam in ListeFamilles)
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

        /// <summary>
        /// Gere le clic sur le bouton de validation, et donc l'ajout ou la modification de l'element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                    SQLiteCommand CommandeRead = new SQLiteCommand(string.Empty, Con); // Definition de la commande a utiliser pour lire les references

                    if (TypeDePage == "Marque")
                    {

                        // Ajout de la marque à la bdd
                        CommandeInsert.CommandText = "INSERT INTO Marques(Nom) Values('" +  textBox1.Text + "')";

                        CommandeInsert.ExecuteNonQuery();

                        CommandeRead.CommandText = "SELECT RefMarque FROM Marques WHERE Nom = '" + textBox1.Text + "'";

                        // On recupere la reference qui a été generée en auto increment
                        SQLiteDataReader Lecteur = CommandeRead.ExecuteReader();

                        Lecteur.Read();

                        // Creation de la marque a ajouter qui sera renvoyé à formMain
                        MarqueAModifier = new Marque(Lecteur.GetInt32(0), textBox1.Text);
                        
                    }

                    if(TypeDePage == "Famille")
                    {

                        // Ajout de la famille a la bdd
                        CommandeInsert.CommandText = "INSERT INTO Familles(Nom) VALUES( '" + textBox1.Text + "' )";
                        CommandeInsert.ExecuteNonQuery();

                        CommandeRead.CommandText = "SELECT RefFamille FROM Familles WHERE Nom = '" + textBox1.Text + "'";

                        // On recupere la reference qui a été generée en auto increment
                        SQLiteDataReader Lecteur = CommandeRead.ExecuteReader();

                        Lecteur.Read();

                        // Creation de la famille a ajouter qui sera renvoyé à formMain
                        FamilleAModifier = new Famille(Lecteur.GetInt32(0), textBox1.Text);

                    }

                    if (TypeDePage == "SousFamille")
                    {

                        // Ajout de la sous famille à la bdd
                        CommandeInsert.CommandText = "INSERT INTO SousFamilles(RefFamille, Nom) VALUES( '"
                         + ListeFamilles[ComboBoxFamille.SelectedIndex].RefFamille + "', '" + textBox1.Text + "' )";
                        CommandeInsert.ExecuteNonQuery();

                        CommandeRead.CommandText = "SELECT RefSousFamille FROM SousFamilles WHERE Nom = '" + textBox1.Text + "'";

                        // On recupere la reference qui a été generée en auto increment
                        SQLiteDataReader Lecteur = CommandeRead.ExecuteReader();

                        Lecteur.Read();

                        // Creation de la famille a ajouter qui sera renvoyé à formMain
                        SousFamilleAModifier = new SousFamille(Lecteur.GetInt32(0), ListeFamilles[ComboBoxFamille.SelectedIndex].RefFamille, textBox1.Text);

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

        /// <summary>
        /// Accesseur en lecture de FamilleAModifier
        /// </summary>
        /// <returns>La famille modifiée ou ajouté</returns>
        public Famille GetFamille()
        {
            return FamilleAModifier;
        }

        /// <summary>
        /// Accesseur en lecture de SousFamilleAModifier
        /// </summary>
        /// <returns>La sous-famille modifiée ou ajouté</returns>
        public SousFamille GetSousFamille()
        {
            return SousFamilleAModifier;
        }

        /// <summary>
        /// Accesseur en lecture de MarqueAModifier
        /// </summary>
        /// <returns>La marque modifiée ou ajouté</returns>
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
