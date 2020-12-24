﻿using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Rectangle = System.Drawing.Rectangle;

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

        private ImageExtender _imageExtender;

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

            _imageExtender = new ImageExtender();
            var exportFileName = $"TXTPlayers____{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss", CultureInfo.InvariantCulture)}.txt";

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

        private BasketballPlayers GetPlayerFromForm(BasketballPlayers player, Teams team, Positions position)
        {
            //if (!string.IsNullOrEmpty(team.TeamName))
            //{
            player.Current_team = team.TeamName;
            //}
            //if (!string.IsNullOrEmpty(tbGetName.Text))
            //{
            player.Name = tbGetName.Text;
            //}
            //if (!string.IsNullOrEmpty(tbGetAge.Text))
            //{
            player.Age = EditingInfo.ConvertNumber(tbGetAge.Text);
            //}
            //if (!string.IsNullOrEmpty(tbGetCareerAge.Text))
            //{
            player.Career_age = EditingInfo.ConvertNumber(tbGetCareerAge.Text);
            //}
            //if (!string.IsNullOrEmpty(tbGetHeight.Text))
            //{
            player.Height = EditingInfo.ConvertNumberToDouble(tbGetHeight.Text);
            //}
            //if (!string.IsNullOrEmpty(tbGetWeight.Text))
            //{
            player.Weight = EditingInfo.ConvertNumber(tbGetWeight.Text);
            //}
            //if (!string.IsNullOrEmpty(position.Position))
            //{
            player.Position = position.Position;
            //}
            //if (!string.IsNullOrEmpty(_playerIMG))
            //{
            player.Picture = _playerIMG;
            //}

            return player;
        }

        //private void UAErrorCallback(Control control)
        //{
        //    control.BorderBrush = Brushes.Red;
        //    control.Focus();
        //}


        private string CombinedPlayerCheck(BasketballPlayers player)
        {
            string check = null;

            if (string.IsNullOrEmpty(player.Name))
            {
                //UAErrorCallback(tbGetName);

                check = "Enter NAME, please!";
                return check;
            }



            if (string.IsNullOrEmpty(player.Position) || string.IsNullOrEmpty(player.Name) || string.IsNullOrEmpty(tbGetAge.Text) ||
               string.IsNullOrEmpty(tbGetCareerAge.Text) || string.IsNullOrEmpty(tbGetHeight.Text) || string.IsNullOrEmpty(tbGetWeight.Text))
            {
                check = "You can't successfully add player with empty parameter";
                return check;
            }
            if (player.Age == -1 || player.Career_age == -1 || player.Height == -1 || player.Weight == -1)
            {
                check = "Please, enter the number";
                ClearPlayersInterface();
                return check;
            }
            if (player.PlayerAdequacyCheck(player))
            {
                check = "The parameters you've entered can not exist";
                ClearPlayersInterface();
                return check;
            }

            return check;
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
            string message = player1.Name;
            GetPlayerFromForm(player1, team, position);


            var errMsg = CombinedPlayerCheck(player1);

            if (!string.IsNullOrEmpty(errMsg))
            {
                MessageBox.Show(errMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);


                ClearPlayersInterface();
                UpdateInterface();
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
            ToLog($"{message} was successfully edited", MessageBoxImage.Information);

            ClearPlayersInterface();
        }

        private void btnDeclineEditingPlayer_Click(object sender, RoutedEventArgs e)
        {
            ClearPlayersInterface();
        }

        private void lvTeamsOutput_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (_toCompleteEvent)
            {
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
            tbToShowPlayerPictureFilePath.Text = player.Picture;
            _playerIMG = player.Picture;
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
            tbToShowTeamLogoFilePath.Text = team.Logo;
            _teamIMG = team.Logo;
            gridEditingTeams.Visibility = Visibility.Visible;
            gridBtnsToConfirmEditingTeam.Visibility = Visibility.Visible;
        }

        private void btnConfirmEditingTeam_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = lvTeamsOutput.SelectedItem;
            Teams team = selectedTeam as Teams;
            BindingList<Teams> currentTeams = FilePath.GetTeams();
            string message = team.TeamName;
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
            ToLog($"{message} was successfully edited", MessageBoxImage.Information);
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
            var selectedTeam = lvTeamsOutput.SelectedItem;
            var team = selectedTeam as Teams;
            int teamNameIndex = 0;
            int positionIndex = 0;
            for (int i = 0; i < _teamsList.Count; i++)
            {
                if (team.TeamName == _teamsList[i].TeamName)
                {
                    teamNameIndex = i;
                }
            }
            cbToEditOrAddPositions.SelectedIndex = positionIndex;
            cbToEditOrAddTeams.SelectedIndex = teamNameIndex;
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
            GetPlayerFromForm(player1, team, position);

            var errMsg = CombinedPlayerCheck(player1);

            if (!string.IsNullOrEmpty(errMsg))
            {
                ToLog(errMsg, MessageBoxImage.Error);


                ClearPlayersInterface();
                UpdateInterface();
                return;
            }

            FilePath.Append(player1);

            UpdateInterface();
            ToLog($"{player1.Name} was successfully added", MessageBoxImage.Information);

            ClearPlayersInterface();
        }

        private void btnConfirmDeletingPlayer_Click(object sender, RoutedEventArgs e)
        {
            var player = lvPlayers.SelectedValue;
            BasketballPlayers player1 = player as BasketballPlayers;
            string message = player1.Name;
            FilePath.Delete(player1);
            UpdateInterface();
            ToLog($"{message} was successfully removed", MessageBoxImage.Information);
            ClearPlayersInterface();
        }

        private void btnRemovePlayer_Click(object sender, RoutedEventArgs e)
        {
            ClearPlayersInterface();
            ClearTeamsInterface();
            if (lvPlayers.SelectedItem == null)
            {
                ToLog("Please, select a player to remove", MessageBoxImage.Error);
                return;
            }

            var player = lvPlayers.SelectedItem as BasketballPlayers;

            tbRemovePlayer.Text = $"Are you sure you want to delete {player.Name} ?";
            gridBtnsToConfirmDeletingPlayers.Visibility = Visibility.Visible;
        }

        private void btnConfirmAddingTeam_Click(object sender, RoutedEventArgs e)
        {
            Teams team = new Teams();
            CheckTeam(team);
            if (string.IsNullOrEmpty(tbGetCity.Text) || string.IsNullOrEmpty(tbgetTeamName.Text))
            {
                ToLog("You can't add team without parameters", MessageBoxImage.Error);
                ClearTeamsInterface();
                return;
            }


            team.CheckTeamPicture(team);
            FilePath.Append(team);
            UpdateInterface();
            ToLog($"{team.TeamName} was successfully added", MessageBoxImage.Information);
            ClearTeamsInterface();
        }

        private void btnConfirmDeletingTeam_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = lvTeamsOutput.SelectedItem;
            Teams team = selectedTeam as Teams;
            string message = team.TeamName;
            FilePath.Delete(team);
            UpdateInterface();
            MessageBox.Show($"{message} was successfully deleted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var team = lvTeamsOutput.SelectedItem as Teams;
            tbRemoveTeam.Text = $"Are you sure you want to delete {team.TeamName} ?";
            gridBtnsToConfirmDeletingTeam.Visibility = Visibility.Visible;
        }


        private string CutFilePathToFolder(string filePath)
        {
            var array = filePath.Split(@"\");
            string filePathToReturn = null;
            for (int i = 0; i < array.Length; i++)
            {
                if (i == (array.Length - 1))
                {
                    break;
                }
                filePathToReturn += array[i] + "\\";
            }
            return filePathToReturn;
        }
        private void btnPictureSelector_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";


            if (!string.IsNullOrEmpty(_playerIMG))
            {
                openFileDialog.FileName = _playerIMG;
                string folderPath = CutFilePathToFolder(_playerIMG);

                openFileDialog.InitialDirectory = folderPath;
            }
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

            if (!string.IsNullOrEmpty(_teamIMG))
            {
                openFileDialog.FileName = _teamIMG;

                string folderPath = CutFilePathToFolder(_teamIMG);

                openFileDialog.InitialDirectory = folderPath;
            }
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                _teamIMG = openFileDialog.FileName;
                tbToShowTeamLogoFilePath.Text = openFileDialog.FileName;
            }
        }


        public void ToLog(string message, MessageBoxImage messageBoxImage = MessageBoxImage.Error)
        {
            switch (messageBoxImage)
            {
                case MessageBoxImage.None:
                    break;
                case MessageBoxImage.Error:
                    MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case MessageBoxImage.Information:
                    MessageBox.Show(message, "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                default:
                    break;
            }

        }


        private void ShowDialog(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = path;
                openFileDialog.InitialDirectory = CutFilePathToFolder(path);
                openFileDialog.ShowDialog();
                return;
            }
            return;
        }
        private void ExportData(FileTypeEnum storageEnum)
        {
            var players = FilePath.GetBasketballPlayers();
            var teams = FilePath.GetTeams();
            var refer = new ImpExpDB();
            string path;
            refer.DataStorage = storageEnum;
            if (players.Count > 0 && storageEnum.ToString().Contains("player", StringComparison.OrdinalIgnoreCase))
            {
                refer.ExportPlayerDataFromDB();
                ToLog("Basketball players data was inserted from database", MessageBoxImage.Information);
                path = refer.GetFilePath();
                ShowDialog(path);
                return;
            }
            if (teams.Count > 0 && storageEnum.ToString().Contains("team", StringComparison.OrdinalIgnoreCase))
            {
                refer.ExportTeamDataFromDB();
                ToLog("Team data was inserted from database", MessageBoxImage.Information);
                path = refer.GetFilePath();
                ShowDialog(path);
                return;
            }
            else
            {
                ToLog("Database is empty", MessageBoxImage.Error);
            }
        }





        private static bool IsTextAllowed(string text)
        {

            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }
        private void InputNumbersOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }


        private void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void miImportPlayers_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opnfldlg = new OpenFileDialog();
            opnfldlg.Filter = "Storage Files(*.TXT;*.CSV;*.JSON;*.XML;*.XLSX)|*.TXT;*.CSV;*.JSON;*.XML;*.XLSX";

            string filePath = null;
            var result = opnfldlg.ShowDialog();
            if (result == true)
            {
                filePath = opnfldlg.FileName;
            }
            if (string.IsNullOrEmpty(filePath))
            {
                ToLog("You haven't chosen a file", MessageBoxImage.Error);
                return;
            }
            var refer = new ImpExpDB(filePath, true);
            refer.StoragePlayersEmptinessCheck();
            refer.ImportPlayerDataToDB();
        }

        private void miImportTeams_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opnfldlg = new OpenFileDialog();
            opnfldlg.Filter = "Storage Files(*.TXT;*.CSV;*.JSON;*.XML;*.XLSX)|*.TXT;*.CSV;*.JSON;*.XML;*.XLSX";

            string filePath = null;
            var result = opnfldlg.ShowDialog();
            if (result == true)
            {
                filePath = opnfldlg.FileName;
            }
            if (string.IsNullOrEmpty(filePath))
            {
                ToLog("You haven't chosen a file", MessageBoxImage.Error);
                return;
            }
            var refer = new ImpExpDB(filePath, false);
            refer.StorageTeamsEmptinessCheck();
            refer.ImportTeamDataToDB();
        }

        private void miExportPlayers_Click(object sender, RoutedEventArgs e)
        {

            var button = (Control)sender;
            var storagetype = new FileTypeEnum();

            if (button.Name.Contains("txt", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.TxtPlayers;
            }
            if (button.Name.Contains("csv", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.CSVPlayers;
            }
            if (button.Name.Contains("xml", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.XMLPlayers;
            }
            if (button.Name.Contains("xlsx", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.ExcellPlayers;
            }
            if (button.Name.Contains("json", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.JsonPlayers;
            }
            ExportData(storagetype);
        }

        private void miExportTeams_Click(object sender, RoutedEventArgs e)
        {
            var button = (Control)sender;
            var storagetype = new FileTypeEnum();

            if (button.Name.Contains("Txt", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.TxtTeams;
            }
            if (button.Name.Contains("csv", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.CSVTeams;
            }
            if (button.Name.Contains("xml", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.XMLTeams;
            }
            if (button.Name.Contains("xlsx", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.ExcellTeams;
            }
            if (button.Name.Contains("json", StringComparison.OrdinalIgnoreCase))
            {
                storagetype = FileTypeEnum.JsonTeams;
            }
            ExportData(storagetype);
        }

        private void cmOpenImageFolder_Click(object sender, RoutedEventArgs e)
        {
            var team = lvTeamsOutput.SelectedItem as Teams;
            var player = lvPlayers.SelectedItem as BasketballPlayers;
            string path = null;
            if (lvTeamsOutput.SelectedItem != null)
            {
                path = team.Logo;
            }
            if (lvPlayers.SelectedItem != null)
            {
                path = player.Picture;
            }
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
            ShowDialog(path);
        }

        private void cmCopy_Click(object sender, RoutedEventArgs e)
        {

            var team = lvTeamsOutput.SelectedItem as Teams;
            var player = lvPlayers.SelectedItem as BasketballPlayers;
            var img = new BitmapImage();
            if (lvTeamsOutput.SelectedItem != null)
            {
               img = ResizeImage(team.Logo, 200);
            }
            if(lvPlayers.SelectedItem != null)
            {
                img = ResizeImage(player.Picture, 200);
            }

            Clipboard.SetImage(img);
        }
        private BitmapImage ResizeImage(string imgPath, int width)
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(imgPath);         
            img.DecodePixelWidth = width;
            img.EndInit();
            return img;
        }

        private void cmExtendImageTeam_Click(object sender, RoutedEventArgs e)
        {
            _imageExtender.gridImageExtend.Children.Clear();
            _imageExtender.Width = 500;
            _imageExtender.Height = 500;
            Grid grid = new Grid();
            var img = new System.Windows.Controls.Image();
            
            var team = lvTeamsOutput.SelectedItem as Teams;
            var player = lvPlayers.SelectedItem as BasketballPlayers;
            var image = new BitmapImage();
            if (lvTeamsOutput.SelectedItem != null)
            {
                image = ResizeImage(team.Logo, 500);
            }
            if (lvPlayers.SelectedItem != null)
            {
                image = ResizeImage(player.Picture, 500);
            }
            img.Source = image;
            _imageExtender.gridImageExtend.Children.Add(img);

            _imageExtender.Show();
        }
    }
}
   