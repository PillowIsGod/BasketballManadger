﻿using Microsoft.Win32;
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


        private bool _toCompleteEvent = false;
        private string _playerIMG;
        private string _teamIMG;

        private BindingList<Teams> _teamsList;
        private BindingList<Positions> _positions;
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
            var team = new Teams();
            foreach (var item in players)
            {
                basketballPlayer.CheckPlayerPicture(item);
            }
            foreach (var item in teams)
            {
                team.CheckTeamPicture(item);
            }
            foreach (var item in teams)
            {
                item.BasketballPlayers = basketballPlayer.RelatePlayerToATeam(item, players); ;
            }
            _teamsList = teams;
            connection = new MySqlConnection(_myConnectionString);
            _positions = FilePath.GetPositions();
            
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
            cbToEditOrAddTeams.ItemsSource = _teamsList;
            cbToEditOrAddPositions.ItemsSource = _positions;
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
            tbToShowPlayerPictureFilePath.Clear();
            _playerIMG = null;
        }
        private void ClearTeamsInterface()
        {
            gridEditingTeams.Visibility = Visibility.Hidden;
            gridBtnsToConfirmEditingTeam.Visibility = Visibility.Hidden;
            gridBtnsToConfirmAddingTeam.Visibility = Visibility.Hidden;
            gridBtnsToConfirmDeletingTeam.Visibility = Visibility.Hidden;
            tbgetTeamName.Clear();
            tbGetCity.Clear();
            tbToShowTeamLogoFilePath.Clear();
            _teamIMG = null;
        }
        private void UpdateInterface()
        {
            _toCompleteEvent = true;
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
            lvTeamsOutput.ItemsSource = null;
            lvTeamsOutput.ItemsSource = teams;
            _toCompleteEvent = false;
        }

        private void btnConfirmEditingPlayer_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = cbToEditOrAddTeams.SelectedItem;
            var selectedPosition = cbToEditOrAddPositions.SelectedItem;
            Positions position = selectedPosition as Positions;
            Teams team = selectedTeam as Teams;
            var player = lvPlayers.SelectedValue;
            BasketballPlayers player1 = player as BasketballPlayers;
            BindingList<BasketballPlayers> currentPlayers = FilePath.GetBasketballPlayers();
            if (!string.IsNullOrEmpty(team.TeamName))
            {
                player1.Current_team = team.TeamName;
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
                player1.Height = EditingInfo.ConvertNumberToDouble(tbGetHeight.Text);
            }
            if (!string.IsNullOrEmpty(tbGetWeight.Text))
            {
                player1.Weight = EditingInfo.ConvertNumber(tbGetWeight.Text);
            }
            if (!string.IsNullOrEmpty(position.Position))
            {
                player1.Position = position.Position;
            }
            if(!string.IsNullOrEmpty(_playerIMG))
            {
                player1.Picture = _playerIMG;
            }
            foreach (var item in currentPlayers)
            {
                if (player1.ID == item.ID)
                {
                    currentPlayers.Remove(item);
                    currentPlayers.Add(player1);
                    break;
                }
            }
            if (player1.Age == -1 || player1.Career_age == -1 || player1.Height == -1 || player1.Weight == -1)
            {
                MessageBox.Show("Please, enter the number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (player1.PlayerAdequacyCheck(player1))
            {
                MessageBox.Show("The parameters you've entered can not exist", "Error",  MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            FilePath.SaveData(currentPlayers);


            UpdateInterface();

            ClearPlayersInterface();
        }

        private void btnDeclineEditingPlayer_Click(object sender, RoutedEventArgs e)
        {
            ClearPlayersInterface();
        }

        private void lvTeamsOutput_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (_toCompleteEvent) {
                return;
            }
            
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
            var selectedPlayer = lvPlayers.SelectedItem;
            BasketballPlayers player = selectedPlayer as BasketballPlayers;
            tbGetAge.Text = player.Age.ToString();
            tbGetCareerAge.Text = player.Career_age.ToString();
            tbGetHeight.Text = player.Height.ToString();
            tbGetWeight.Text = player.Weight.ToString();
            tbGetName.Text = player.Name;
            int teamNameIndex = 0;
            int positionIndex = 0;
            for (int i = 0; i < _teamsList.Count; i++)
            {
                if (player.Current_team == _teamsList[i].TeamName)
                {
                    teamNameIndex = i;
                }
            }
            for (int i = 0; i < _positions.Count; i++)
            {
                if (player.Position == _positions[i].Position)
                {
                    positionIndex = i;
                }
            }
            cbToEditOrAddPositions.SelectedIndex = positionIndex;
            cbToEditOrAddTeams.SelectedIndex = teamNameIndex;
            gridEditingPlayers.Visibility = Visibility.Visible;
            gridBtnsToConfirmEditingPlayers.Visibility = Visibility.Visible;
        }

        private void lvTeamsOutput_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClearPlayersInterface();
            var selectedTeam = lvTeamsOutput.SelectedItem;
            Teams team = selectedTeam as Teams;
            tbGetCity.Text = team.City;
            tbgetTeamName.Text = team.TeamName;
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
            if (!string.IsNullOrEmpty(_teamIMG))
            {
                team.Logo = _teamIMG;
            }

            team.CheckTeamPicture(team);


            foreach (var item in currentTeams)
            {
                if (team.ID == item.ID)
                {
                    currentTeams.Remove(item);
                    currentTeams.Add(team);
                    break;
                }
            }
            FilePath.SaveData(currentTeams);
            UpdateInterface();
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
            ClearTeamsInterface();
            
            var selectedTeam = cbToEditOrAddTeams.SelectedItem;
            var selectedPosition = cbToEditOrAddPositions.SelectedItem;
            Positions position = selectedPosition as Positions;
            Teams team = selectedTeam as Teams;
            BasketballPlayers player1 = new BasketballPlayers();
            if (!string.IsNullOrEmpty(team.TeamName))
            {
                player1.Current_team = team.TeamName;
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
                player1.Height = EditingInfo.ConvertNumberToDouble(tbGetHeight.Text);
            }
            if (!string.IsNullOrEmpty(tbGetWeight.Text))
            {
                player1.Weight = EditingInfo.ConvertNumber(tbGetWeight.Text);
            }
            if (!string.IsNullOrEmpty(position.Position))
            {
                player1.Position = position.Position;
            }
            if (!string.IsNullOrEmpty(_playerIMG))
            {
                player1.Picture = _playerIMG;
            }
            player1.CheckPlayerPicture(player1);
            if (string.IsNullOrEmpty(position.Position) || string.IsNullOrEmpty(tbGetName.Text) || string.IsNullOrEmpty(tbGetAge.Text) ||
                string.IsNullOrEmpty(tbGetCareerAge.Text) || string.IsNullOrEmpty(tbGetHeight.Text) || string.IsNullOrEmpty(tbGetWeight.Text) || string.IsNullOrEmpty(position.Position))
            {
                ClearPlayersInterface();
                return;
            }
            if(player1.Age == -1 || player1.Career_age == -1 || player1.Height == -1|| player1.Weight == -1)
            {
                    MessageBox.Show("Please, enter the number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }
            if (player1.PlayerAdequacyCheck(player1))
            {
                MessageBox.Show("The parameters you've entered can not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            FilePath.Append(player1);

            UpdateInterface();

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
            if (!string.IsNullOrEmpty(_teamIMG))
            {
                team.Logo = _teamIMG;
            }
            team.CheckTeamPicture(team);
            FilePath.Append(team);
            UpdateInterface();
            ClearTeamsInterface();
        }

        private void btnConfirmDeletingTeam_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = lvTeamsOutput.SelectedItem;
            Teams team = selectedTeam as Teams;
            FilePath.Delete(team);
            UpdateInterface();
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

        private void btnPictureSelector_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                _playerIMG = openFileDialog.FileName;
                tbToShowPlayerPictureFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnLogoSelector_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                _teamIMG = openFileDialog.FileName;
                tbToShowTeamLogoFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            ImpExpDB.DataStorage = FileTypeEnum.ExcellTeams;
            ImpExpDB.ImportTeamDataToDB();
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
