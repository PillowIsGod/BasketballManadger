using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasketballManadger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static JsonFileProcessing JsonPath = new JsonFileProcessing(@"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\content.json");
        private BindingList<Teams> _teamsList;
        public MainWindow()
        {
            InitializeComponent();
            var teams = JsonPath.GetTeams();
            var players = JsonPath.GetBasketballPlayers();
            foreach (var item in teams)
            {
                item.BasketballPlayers = players;
            }
            var basketballPlayer = new BasketballPlayers();
            foreach (var item in teams)
            {
                item.BasketballPlayers = basketballPlayer.RelatePlayerToATeam(item, players); ;
            }
            _teamsList = teams;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lvTeamsOutput.ItemsSource = _teamsList;
            lvTeamsOutput.SelectedIndex = 0;
            var str = lvTeamsOutput.SelectedValue;
            Teams str1 = str as Teams;
            var players = str1.BasketballPlayers;
            lvPlayers.ItemsSource = players;
            lvPlayers.Visibility = Visibility.Visible;
            gridPlayerButtons.Visibility = Visibility.Visible;
        }


        private void btnConfirmPlayer_Click(object sender, RoutedEventArgs e)
        {
            var player = lvPlayers.SelectedValue;
            BasketballPlayers player1 = player as BasketballPlayers;
            BindingList<BasketballPlayers> currentPlayers = JsonPath.GetBasketballPlayers();
            if (!string.IsNullOrEmpty(tbGetTeam.Text))
            {
                player1.Current_team = tbGetTeam.Text;
            }
            if (!string.IsNullOrEmpty(tbGetName.Text))
            {
                player1.Name = tbGetName.Text;
            }
            if (!string.IsNullOrEmpty(tbGetAge.Text))
            {
                player1.Age = EditingInfo.ConvertNumber(tbGetAge.Text);
            }
            if (!string.IsNullOrEmpty(tbGetCareerAge.Text))
            {
                player1.Career_age = EditingInfo.ConvertNumber(tbGetCareerAge.Text);
            }
            if (!string.IsNullOrEmpty(tbGetHeight.Text))
            {
                player1.Height = EditingInfo.ConvertNumber(tbGetHeight.Text);
            }
            if (!string.IsNullOrEmpty(tbGetWeight.Text))
            {
                player1.Weight = EditingInfo.ConvertNumber(tbGetWeight.Text);
            }
            if (!string.IsNullOrEmpty(tbGetPosition.Text))
            {
                player1.Position = tbGetPosition.Text;
            }
            foreach (var item in currentPlayers)
            {
                if (player1.Picture == item.Picture)
                {
                    currentPlayers.Remove(item);
                    currentPlayers.Add(player1);
                    break;
                }
            }
            JsonPath.SaveData(currentPlayers);
            
            gridBtnsToConfirmPlayers.Visibility = Visibility.Hidden;
            gridEditingPlayers.Visibility = Visibility.Hidden;
            tbGetAge.Clear();
            tbGetName.Clear();
            tbGetCareerAge.Clear();
            tbGetHeight.Clear();
            tbGetWeight.Clear();
            tbGetTeam.Clear();
            tbGetPosition.Clear();
        }

        private void btnDeclinePlayer_Click(object sender, RoutedEventArgs e)
        {
            gridBtnsToConfirmPlayers.Visibility = Visibility.Hidden;
            gridEditingPlayers.Visibility = Visibility.Hidden;
            tbGetAge.Clear();
            tbGetName.Clear();
            tbGetCareerAge.Clear();
            tbGetHeight.Clear();
            tbGetWeight.Clear();
            tbGetTeam.Clear();
            tbGetPosition.Clear();
        }

        private void lvTeamsOutput_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var str = lvTeamsOutput.SelectedValue;
            Teams str1 = str as Teams;
            var players = str1.BasketballPlayers;
            lvPlayers.ItemsSource = players;
            lvPlayers.Visibility = Visibility.Visible;
            gridPlayerButtons.Visibility = Visibility.Visible;
            gridEditingPlayers.Visibility = Visibility.Hidden;
            gridBtnsToConfirmPlayers.Visibility = Visibility.Hidden;
            gridEditingTeams.Visibility = Visibility.Hidden;
            gridBtnsToConfirmTeam.Visibility = Visibility.Hidden;
            tbgetTeamName.Clear();
            tbGetCity.Clear();
        }


        private void lvPlayers_MouseDoubleClickEditing(object sender, MouseButtonEventArgs e)
        {
            gridEditingTeams.Visibility = Visibility.Hidden;
            gridBtnsToConfirmTeam.Visibility = Visibility.Hidden;
            gridEditingPlayers.Visibility = Visibility.Visible;
            gridBtnsToConfirmPlayers.Visibility = Visibility.Visible;
        }

        private void lvTeamsOutput_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            gridEditingPlayers.Visibility = Visibility.Hidden;
            gridBtnsToConfirmPlayers.Visibility = Visibility.Hidden;
            gridEditingTeams.Visibility = Visibility.Visible;
            gridBtnsToConfirmTeam.Visibility = Visibility.Visible;
        }

        private void btnConfirmTeam_Click(object sender, RoutedEventArgs e)
        {

            gridEditingTeams.Visibility = Visibility.Hidden;
            gridBtnsToConfirmTeam.Visibility = Visibility.Hidden;
            tbgetTeamName.Clear();
            tbGetCity.Clear();
        }

        private void btnDeclineTeam_Click(object sender, RoutedEventArgs e)
        {
            gridEditingTeams.Visibility = Visibility.Hidden;
            gridBtnsToConfirmTeam.Visibility = Visibility.Hidden;
            tbgetTeamName.Clear();
            tbGetCity.Clear();
        }

        private void lvPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridEditingTeams.Visibility = Visibility.Hidden;
            gridBtnsToConfirmTeam.Visibility = Visibility.Hidden;
            gridEditingPlayers.Visibility = Visibility.Hidden;
            gridBtnsToConfirmPlayers.Visibility = Visibility.Hidden;
            tbGetAge.Clear();
            tbGetName.Clear();
            tbGetCareerAge.Clear();
            tbGetHeight.Clear();
            tbGetWeight.Clear();
            tbGetTeam.Clear();
            tbGetPosition.Clear();

        }





        //private void _teamsList_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
        //    {
        //        try
        //        {
        //            JsonPath.SaveData(sender);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            Close();
        //        }
        //    }



    }
}
