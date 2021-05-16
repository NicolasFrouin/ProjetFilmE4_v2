using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualBasic;

namespace FilmWPF
{
    /// <summary>
    /// Logique d'interaction pour FilmGerer.xaml
    /// </summary>
    public partial class FilmGerer : Window
    {
        filmEntities gst;
        public FilmGerer()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gst = new filmEntities();
            lstFilms.ItemsSource = gst.film.ToList();
            cboGenre.ItemsSource = gst.genre.ToList();
            cboRealisateur.ItemsSource = gst.realisateur.ToList();
            lstAllActeurs.ItemsSource = gst.acteur.ToList();
        }

        private void lstFilms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var idFilm = (lstFilms.SelectedItem as film).Id; // car un Linq ne veut pas de relation comme cela
            var lesActeurs = from a in gst.acteur
                                     join j in gst.jouer on a.Id equals j.NumActeur
                                     join f in gst.film on j.NumFilm equals f.Id
                                     where f.Id == idFilm
                                     select a;
            lstActeurs.ItemsSource = lesActeurs.ToList();
        }

        private void lstActeurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // il ne se passe rien
        }

        private void cboGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // il ne se passe rien
        }

        private void cboRealisateur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // il ne se passe rien
        }

        private void lstAllActeurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // il ne se passe rien
        }

        private void btnAjoutGenre_Click(object sender, RoutedEventArgs e)
        {
            if (txtLibelleGenre.Text == "")
            {
                MessageBox.Show("Veuillez entrer le libellé du genre");
            }
            else
            {
                gst.genre.Add(new genre() { Libelle = txtLibelleGenre.Text });
                gst.SaveChanges();
            }
        }

        private void btnAjouterFilm_Click(object sender, RoutedEventArgs e)
        {
            if (txtNomFilm.Text == "")
            {
                MessageBox.Show("Veuillez entrer le nom du film");
            }
            else if (txtNbEntrees.Text == "")
            {
                MessageBox.Show("Veuillez entrer le nombre d'entrées du film");
            }
            else if (txtDureeFilm.Text == "")
            {
                MessageBox.Show("Veuillez entrer la durée du film");
            }
            else if (cboGenre.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner le genre");
            }
            else if (cboRealisateur.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner le réalisateur");
            }
            else if (lstAllActeurs.SelectedItem == null)
            {
                MessageBox.Show("Un film sans acteurs me paraît compliqué, pas vous ?");
            }
            else
            {
                film f = new film
                {
                    NbEntrees = int.Parse(txtNbEntrees.Text),
                    Duree = TimeSpan.Parse(txtDureeFilm.Text),
                    NumRealisateur = (cboRealisateur.SelectedItem as realisateur).Id,
                    Titre = txtNomFilm.Text,
                    Image = ""
                };
                gst.film.Add(f);
                gst.SaveChanges(); // pour que l'Id du film soit donnée par la bdd en auto-incrément
                foreach (acteur a in lstAllActeurs.SelectedItems)
                {
                    string leRole = Interaction.InputBox("Quel rôle a joué " + a.personne.Prenom + " " + a.personne.Nom + " ?", "Définition des rôles"); // pour utiliser le Interactoin.InputBox, il faut "using Microsoft.VisualBasic;"
                    jouer j = new jouer
                    {
                        NumFilm = gst.film.ToList().FindLast(fi => fi.Titre == txtNomFilm.Text).Id,
                        NumActeur = a.Id,
                        Role = leRole
                    };
                    gst.jouer.Add(j);
                }
                gst.SaveChanges();
            }
        }
    }
}
