using Microsoft.Win32;
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
        private BindingList<MenuImagesProcessing> _menuImages;
        public MainWindow()
        {
            InitializeComponent();
            FilePath = new DBProcessing(_myConnectionString);
            var teams = FilePath.GetTeams();
            var players = FilePath.GetBasketballPlayers();
            var image = new MenuImagesProcessing();
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
            _menuImages = image.GetImagesFromFile();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection.Open();
            foreach (var item in _teamsList)
            {
                item.CheckTeamPicture(item);
            }
            
            lvTeamsOutput.ItemsSource = _teamsList;
            lvTeamsOutput.SelectedIndex = 0;
            var str = lvTeamsOutput.SelectedValue;
            Teams str1 = str as Teams;
            var players = str1.BasketballPlayers;
            foreach (var item in players)
            {
                item.CheckPlayerPicture(item);
            }
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

        private BasketballPlayers CheckPlayer(BasketballPlayers player, Teams team, Positions position)
        {
            if (!string.IsNullOrEmpty(team.TeamName))
            {
                player.Current_team = team.TeamName;
            }
            if (!string.IsNullOrEmpty(tbGetName.Text))
            {
                player.Name = tbGetName.Text;
            }
            if (!string.IsNullOrEmpty(tbGetAge.Text))
            {
                player.Age = EditingInfo.ConvertNumber(tbGetAge.Text);
            }
            if (!string.IsNullOrEmpty(tbGetCareerAge.Text))
            {
                player.Career_age = EditingInfo.ConvertNumber(tbGetCareerAge.Text);
            }
            if (!string.IsNullOrEmpty(tbGetHeight.Text))
            {
                player.Height = EditingInfo.ConvertNumberToDouble(tbGetHeight.Text);
            }
            if (!string.IsNullOrEmpty(tbGetWeight.Text))
            {
                player.Weight = EditingInfo.ConvertNumber(tbGetWeight.Text);
            }
            if (!string.IsNullOrEmpty(position.Position))
            {
                player.Position = position.Position;
            }
            if (!string.IsNullOrEmpty(_playerIMG))
            {
                player.Picture = _playerIMG;
            }
            if (player.Age == -1 || player.Career_age == -1 || player.Height == -1 || player.Weight == -1)
            {
                MessageBox.Show("Please, enter the number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                player.Name = "AlbertoDelRio";
                player.Picture = null;
                ClearPlayersInterface();
                return player;
            }
            if (player.PlayerAdequacyCheck(player))
            {
                MessageBox.Show("The parameters you've entered can not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearPlayersInterface();
                player.Name = "AlbertoDelRio";
                player.Picture = null;
                return player;
            }
            return player;            
        }

        private Teams CheckTeam(Teams team)
        {
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
            return team;
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
            CheckPlayer(player1, team, position);
            if (player1.Name == "AlbertoDelRio")
            {
                return;
            }
            if (string.IsNullOrEmpty(player1.Position) || string.IsNullOrEmpty(player1.Name) || string.IsNullOrEmpty(tbGetAge.Text) ||
                string.IsNullOrEmpty(tbGetCareerAge.Text) || string.IsNullOrEmpty(tbGetHeight.Text) || string.IsNullOrEmpty(tbGetWeight.Text))
            {
                MessageBox.Show("You can't successfully add player with empty parameter", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearPlayersInterface();
                return;
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
                    
            FilePath.SaveData(currentPlayers);

            UpdateInterface();
            MessageBox.Show("Player was successfully edited", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
            CheckTeam(team);

            team.CheckTeamPicture(team);

            if (string.IsNullOrEmpty(tbGetCity.Text) || string.IsNullOrEmpty(tbgetTeamName.Text))
            {
                MessageBox.Show("You can't successfully edit team without parameters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTeamsInterface();
                return;
            }

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
            MessageBox.Show("Team was successfully edited", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
            ClearTeamsInterface();
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
            CheckPlayer(player1, team, position);
            if (player1.Name == "AlbertoDelRio")
            {
                return;
            }
            player1.CheckPlayerPicture(player1);

            if (string.IsNullOrEmpty(player1.Position) || string.IsNullOrEmpty(player1.Name) || string.IsNullOrEmpty(tbGetAge.Text) ||
                string.IsNullOrEmpty(tbGetCareerAge.Text) || string.IsNullOrEmpty(tbGetHeight.Text) || string.IsNullOrEmpty(tbGetWeight.Text))
            {
                MessageBox.Show("You can't successfully add player with empty parameter", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearPlayersInterface();
                return;
            }


            FilePath.Append(player1);

            UpdateInterface();
            MessageBox.Show("Player was successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearPlayersInterface();
        }

        private void btnConfirmDeletingPlayer_Click(object sender, RoutedEventArgs e)
        {
            var player = lvPlayers.SelectedValue;
            BasketballPlayers player1 = player as BasketballPlayers;
            FilePath.Delete(player1);
            UpdateInterface();
            MessageBox.Show("Player was successfully removed", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
            CheckTeam(team);
            if (string.IsNullOrEmpty(tbGetCity.Text) || string.IsNullOrEmpty(tbgetTeamName.Text))
            {
                MessageBox.Show("You can't add team without parameters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearTeamsInterface();
                return;
            }
           
            
            team.CheckTeamPicture(team);
            FilePath.Append(team);
            UpdateInterface();
            MessageBox.Show("Team was successfully added","Success", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearTeamsInterface();
        }

        private void btnConfirmDeletingTeam_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = lvTeamsOutput.SelectedItem;
            Teams team = selectedTeam as Teams;
            FilePath.Delete(team);
            UpdateInterface();
            MessageBox.Show("Team was successfully deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void ImportTeams(FileTypeEnum storageEnum)
        {
            var teams = FilePath.GetTeams();
            ImpExpDB.DataStorage = storageEnum;
            ImpExpDB.StorageTeamsEmptinessCheck();
            if (teams.Count <= 0)
            {
                ImpExpDB.ImportTeamDataToDB(true, false);
                MessageBox.Show("Team data was inserted to database", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ImpExpDB.ImportTeamDataToDB(false, true);
                MessageBox.Show("Database team data was updated", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ExportTeams(FileTypeEnum storageEnum)
        {
            var teams = FilePath.GetTeams();
            ImpExpDB.DataStorage = storageEnum;

            if (teams.Count > 0)
            {
                ImpExpDB.ExportTeamDataFromDB();
                MessageBox.Show("Team data was inserted from database", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Database is empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }      
        private void ImportPlayers (FileTypeEnum storageEnum)
        {
            var players = FilePath.GetBasketballPlayers();
            ImpExpDB.DataStorage = storageEnum;
            ImpExpDB.StoragePlayersEmptinessCheck();
            if (players.Count <= 0)
            {
                ImpExpDB.ImportPlayerDataToDB(true, false);
                MessageBox.Show("Basketball players data was inserted to database", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ImpExpDB.ImportPlayerDataToDB(false, true);
                MessageBox.Show("Database was updated with basketball players", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void ExportPlayers (FileTypeEnum storageEnum)
        {
            var players = FilePath.GetBasketballPlayers();
            ImpExpDB.DataStorage = storageEnum;
            if (players.Count > 0)
            {
                ImpExpDB.ExportPlayerDataFromDB();
                MessageBox.Show("Basketball players data was inserted from database", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Database is empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void miTXTimportTeams_Click(object sender, RoutedEventArgs e)
        {
            ImportTeams(FileTypeEnum.TxtPlayers);
        }

        private void miCSVimportTeams_Click(object sender, RoutedEventArgs e)
        {
            ImportTeams(FileTypeEnum.CSVTeams);
        }

        private void miJSONimportTeams_Click(object sender, RoutedEventArgs e)
        {
            ImportTeams(FileTypeEnum.JsonTeams);
        }

        private void miXMLimporTeamst_Click(object sender, RoutedEventArgs e)
        {
            ImportTeams(FileTypeEnum.XMLTeams);
        }

        private void miXLSXimportTeams_Click(object sender, RoutedEventArgs e)
        {
            ImportTeams(FileTypeEnum.ExcellTeams);
        }

        private void miTXTexportTeams_Click(object sender, RoutedEventArgs e)
        {
            ExportTeams(FileTypeEnum.TxtTeams);         
        }

        private void miCSVexportTeams_Click(object sender, RoutedEventArgs e)
        {
            ExportTeams(FileTypeEnum.CSVTeams);

        }

        private void miJSONexportTeams_Click(object sender, RoutedEventArgs e)
        {
            ExportTeams(FileTypeEnum.JsonTeams);

        }

        private void miXMLexportTeams_Click(object sender, RoutedEventArgs e)
        {
            ExportTeams(FileTypeEnum.XMLTeams);

        }

        private void miXLSXexportTeams_Click(object sender, RoutedEventArgs e)
        {
            ExportTeams(FileTypeEnum.ExcellTeams);

        }

        private void miTXTimportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ImportPlayers(FileTypeEnum.TxtPlayers);
        }

        private void miCSVimportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ImportPlayers(FileTypeEnum.CSVPlayers);
        }

        private void miJSONimportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ImportPlayers(FileTypeEnum.JsonPlayers);
        }

        private void miXMLimportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ImportPlayers(FileTypeEnum.XMLPlayers);
        }

        private void miXLSXimportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ImportPlayers(FileTypeEnum.ExcellPlayers);
        }

        private void miTXTexportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ExportPlayers(FileTypeEnum.TxtPlayers);
        }

        private void miCSVexportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ExportPlayers(FileTypeEnum.CSVPlayers);
        }

        private void miJSONexportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ExportPlayers(FileTypeEnum.JsonPlayers);
        }

        private void miXMLexportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ExportPlayers(FileTypeEnum.XMLPlayers);
        }

        private void miXLSXexportPlayers_Click(object sender, RoutedEventArgs e)
        {
            ExportPlayers(FileTypeEnum.ExcellPlayers);
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
