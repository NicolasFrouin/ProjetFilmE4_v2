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

namespace FilmWPF
{
    /// <summary>
    /// Logique d'interaction pour SpectateurGerer.xaml
    /// </summary>
    public partial class SpectateurGerer : Window
    {
        filmEntities gst;
        public SpectateurGerer()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gst = new filmEntities();
            lstFilms.ItemsSource = gst.film.ToList();
            cboSpectateurs.ItemsSource = gst.spectateur.ToList();
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

        private void cboSpectateurs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // afficher les inscriptions du spectateur sélectionné
            int numSpectateur = (cboSpectateurs.SelectedItem as spectateur).Id;
            var lesReservations = from f in gst.film
                                  join r in gst.reserver on f.Id equals r.NumFilm
                                  join s in gst.spectateur on r.NumSpectateur equals s.Id
                                  where s.Id == numSpectateur
                                  select f;
            lstReservation.ItemsSource = lesReservations.ToList();
        }

        private void btnInscription_Click(object sender, RoutedEventArgs e)
        {
            // ajouter l'inscription
            if (txtPrenomSpectateur.Text == "")
            {
                MessageBox.Show("Veuillez saisir le prénom du spectateur");
            }
            else if (txtNomSpectateur.Text == "")
            {
                MessageBox.Show("Veuillez saisir le nom du spectateur");
            }
            else if (txtMailSpectateur.Text == "")
            {
                MessageBox.Show("Veuillez saisir le mail du spectateur");
            }
            else if (txtDateNaissanceSpectateur.Text == "")
            {
                MessageBox.Show("Veuillez saisir la date de naissance du spectateur");
            }
            else if (txtAdresseSpectateur.Text == "")
            {
                MessageBox.Show("Veuillez saisir l'adresse du spectateur");
            }
            else
            {
                gst.personne.Add(new personne 
                { 
                    Nom = txtNomSpectateur.Text, 
                    Prenom = txtPrenomSpectateur.Text, 
                    DateNaissance = DateTime.Parse(txtDateNaissanceSpectateur.Text)
                });
                gst.SaveChanges();
                gst.spectateur.Add(new spectateur
                {
                    Id = gst.personne.ToList().FindLast(p=>p.Nom == txtNomSpectateur.Text && p.Prenom == txtPrenomSpectateur.Text).Id,
                    Adresse = txtAdresseSpectateur.Text,
                    Mail = txtMailSpectateur.Text
                });
                gst.SaveChanges();
                cboSpectateurs.ItemsSource = gst.spectateur.ToList();
            }
        }

        private void lstReservation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // il ne se passe rien
        }

        private void btnReservation_Click(object sender, RoutedEventArgs e)
        {
            // ajouter le reserver aux bons film et spectateurs
            if (cboSpectateurs.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un spectateur");
            }
            else if (dpDate.Text == "")
            {
                MessageBox.Show("Veuillez sélectionner ou saisir une date");
            }
            else if (lstFilms.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un film auquel vous réserver");
            }
            else if (dpDate.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("La date sélectionnée est déjà passée");
            }
            else
            {

                gst.reserver.Add(new reserver
                {
                    NumFilm = (lstFilms.SelectedItem as film).Id,
                    NumSpectateur = (cboSpectateurs.SelectedItem as spectateur).Id,
                    DateHeure = DateTime.Parse(dpDate.Text)
                });
                gst.SaveChanges();

                lstReservation.ItemsSource = "";
                // obligé de réécrire tout cela car les IQueriable ne semblent pas vouloir être déclaré en variable globale correctement pour cette utilisation 
                // ET que le .Refresh() ne fonctionne pas
                int numSpectateur = (cboSpectateurs.SelectedItem as spectateur).Id;
                var lesReservations = from f in gst.film
                                      join r in gst.reserver on f.Id equals r.NumFilm
                                      join s in gst.spectateur on r.NumSpectateur equals s.Id
                                      where s.Id == numSpectateur
                                      select f;
                lstReservation.ItemsSource = lesReservations.ToList();
            }
        }
    }
}
