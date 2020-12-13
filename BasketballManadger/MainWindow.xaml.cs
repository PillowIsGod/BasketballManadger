using MySql.Data.MySqlClient;
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
        private string _filePath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\content.json";
        private DataProcessing FilePath;

        private string _myConnectionString = "Database = basketballdata; Data Source = 127.0.0.1; User Id = root; Password = 7Bc145f606";
        private MySqlConnection connection = null;


        private BindingList<Teams> _teamsList;
        public MainWindow()
        {
            InitializeComponent();
            FilePath = new DBProcessing(_myConnectionString);
            var teams = FilePath.GetTeams();
            var players = FilePath.GetBasketballPlayers();
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
            connection = new MySqlConnection(_myConnectionString);
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();
            lvTeamsOutput.ItemsSource = _teamsList;
            lvTeamsOutput.SelectedIndex = 0;
            var str = lvTeamsOutput.SelectedValue;
            Teams str1 = str as Teams;
            var players = str1.BasketballPlayers;
            
            lvPlayers.ItemsSource = players;
            lvPlayers.Visibility = Visibility.Visible;
            gridPlayerButtons.Visibility = Visibility.Visible;
        }
        private void ClearPlayersInterface()
        {
            gridBtnsToConfirmEditingPlayers.Visibility = Visibility.Hidden;
            gridEditingPlayers.Visibility = Visibility.Hidden;
            gridBtnsToConfirmAddingPlayers.Visibility = Visibility.Hidden;
            gridBtnsToConfirmDeletingPlayers.Visibility = Visibility.Hidden;
            tbGetAge.Clear();
            tbGetName.Clear();
            tbGetCareerAge.Clear();
            tbGetHeight.Clear();
            tbGetWeight.Clear();
            tbGetTeam.Clear();
            tbGetPosition.Clear();
        }
        private void ClearTeamsInterface()
        {
            gridEditingTeams.Visibility = Visibility.Hidden;
            gridBtnsToConfirmEditingTeam.Visibility = Visibility.Hidden;
            gridBtnsToConfirmAddingTeam.Visibility = Visibility.Hidden;
            gridBtnsToConfirmDeletingTeam.Visibility = Visibility.Hidden;
            tbgetTeamName.Clear();
            tbGetCity.Clear();
        }

        private void btnConfirmEditingPlayer_Click(object sender, RoutedEventArgs e)
        {
            var player = lvPlayers.SelectedValue;
            BasketballPlayers player1 = player as BasketballPlayers;
            BindingList<BasketballPlayers> currentPlayers = FilePath.GetBasketballPlayers();
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
            FilePath.SaveData(currentPlayers);

            ClearPlayersInterface();
        }

        private void btnDeclineEditingPlayer_Click(object sender, RoutedEventArgs e)
        {
            ClearPlayersInterface();
        }

        private void lvTeamsOutput_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var str = lvTeamsOutput.SelectedValue;
            Teams str1 = str as Teams;
            var players = str1.BasketballPlayers;
            lvPlayers.ItemsSource = players;
            lvPlayers.Visibility = Visibility.Visible;
            gridPlayerButtons.Visibility = Visibility.Visible;
            ClearPlayersInterface();
            ClearTeamsInterface();
        }


        private void lvPlayers_MouseDoubleClickEditing(object sender, MouseButtonEventArgs e)
        {
            ClearTeamsInterface();
            gridEditingPlayers.Visibility = Visibility.Visible;
            gridBtnsToConfirmEditingPlayers.Visibility = Visibility.Visible;
        }

        private void lvTeamsOutput_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClearPlayersInterface();
            gridEditingTeams.Visibility = Visibility.Visible;
            gridBtnsToConfirmEditingTeam.Visibility = Visibility.Visible;
        }

        private void btnConfirmEditingTeam_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = lvTeamsOutput.SelectedItem;
            Teams team = selectedTeam as Teams;
            BindingList<Teams> currentTeams = FilePath.GetTeams();
            if (!string.IsNullOrEmpty(tbGetCity.Text))
            {
                team.City = tbGetCity.Text;
            }
            if (!string.IsNullOrEmpty(tbgetTeamName.Text))
            {
                team.TeamName = tbgetTeamName.Text;
            }
            foreach (var item in currentTeams)
            {
                if (team.Logo == item.Logo)
                {
                    currentTeams.Remove(item);
                    currentTeams.Add(team);
                    break;
                }
            }
            FilePath.SaveData(currentTeams);
            ClearTeamsInterface();
        }

        private void btnDeclineEditingTeam_Click(object sender, RoutedEventArgs e)
        {
            ClearTeamsInterface();
        }

        private void lvPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearPlayersInterface();
            ClearTeamsInterface();
        }

        private void btnAddPlayer_Click(object sender, RoutedEventArgs e)
        {            
            ClearPlayersInterface();
            gridEditingPlayers.Visibility = Visibility.Visible;
            gridBtnsToConfirmAddingPlayers.Visibility = Visibility.Visible;
        }

        private void btnConfirmAddingPlayer_Click(object sender, RoutedEventArgs e)
        {
            BasketballPlayers player1 = new BasketballPlayers();
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
            if (string.IsNullOrEmpty(tbGetTeam.Text) || string.IsNullOrEmpty(tbGetName.Text) || string.IsNullOrEmpty(tbGetAge.Text) || string.IsNullOrEmpty(tbGetCareerAge.Text) || string.IsNullOrEmpty(tbGetHeight.Text) || string.IsNullOrEmpty(tbGetWeight.Text) || string.IsNullOrEmpty(tbGetPosition.Text))
            {
                ClearPlayersInterface();
                return;
            }
            FilePath.Append(player1);

            ClearPlayersInterface();
        }

        private void btnConfirmDeletingPlayer_Click(object sender, RoutedEventArgs e)
        {
            var player = lvPlayers.SelectedValue;
            BasketballPlayers player1 = player as BasketballPlayers;
            FilePath.Delete(player1);
            ClearPlayersInterface();
        }

        private void btnRemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            ClearPlayersInterface();
            ClearTeamsInterface();
            gridBtnsToConfirmDeletingPlayers.Visibility = Visibility.Visible;
        }

        private void btnConfirmAddingTeam_Click(object sender, RoutedEventArgs e)
        {
            Teams team = new Teams();
            if (!string.IsNullOrEmpty(tbGetCity.Text))
            {
                team.City = tbGetCity.Text;
            }
            if (!string.IsNullOrEmpty(tbgetTeamName.Text))
            {
                team.TeamName = tbgetTeamName.Text;
            }

            if (string.IsNullOrEmpty(tbGetCity.Text) || string.IsNullOrEmpty(tbgetTeamName.Text))
            {
                ClearTeamsInterface();
                return;
            }
            FilePath.Append(team);
            ClearTeamsInterface();
        }

        private void btnConfirmDeletingTeam_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = lvTeamsOutput.SelectedItem;
            Teams team = selectedTeam as Teams;
            FilePath.Delete(team);
            ClearTeamsInterface();

        }

        private void btnAddTeam_Click(object sender, RoutedEventArgs e)
        {
            ClearPlayersInterface();
            ClearTeamsInterface();
            gridEditingTeams.Visibility = Visibility.Visible;
            gridBtnsToConfirmAddingTeam.Visibility = Visibility.Visible;
        }

        private void btnRemoveTeam_Click(object sender, RoutedEventArgs e)
        {
            ClearPlayersInterface();
            ClearTeamsInterface();
            gridBtnsToConfirmDeletingTeam.Visibility = Visibility.Visible;
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
