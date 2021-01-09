using BasketballManadger.ClassesImportExport;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace BasketballManadger
{

    public abstract class ExportDB 
    {
        public void ImportFromFile(string filePath, bool needPlayers = false)
        {
            FileTypesProcessing ftp = null;
            var currentExtension = System.IO.Path.GetExtension(filePath);
            //switch (currentExtension)
            //{
            //    case "txt":
            //    ftp = new TXTProcessing(filePath, player);
            //    break;
            //}

            var teams = ftp.GetTeamFromFIle();
            TeamsToDb(teams);
        }
        public void ImportFromFilePlayers(string filePath)
        {
            FileTypesProcessing ftp = null;
            var currentExtension = System.IO.Path.GetExtension(filePath);
            //switch (currentExtension)
            //{
            //    case "txt":
            //    ftp = new TXTProcessing(filePath, player);
            //    break;
            //}

            var teams = ftp.GetPlayersFromFile();
            PlayersToDb(teams);
        }



        public abstract string EportData(IEnumerable<Teams> teams, bool withPlayers = false);
        public abstract string EportData(IEnumerable<BasketballPlayers> players);

        protected abstract void TeamsToDb(IEnumerable<Teams> teams);
        protected abstract void PlayersToDb(IEnumerable<BasketballPlayers> players);
    }


    public class ImpExpDB
    {

        private string _myConnectionString = "Database = basketballdata; Data Source = 127.0.0.1; User Id = root; Password = 7Bc145f606";

        private DataProcessing DBPath;

        private MySqlConnection _connection;

        private string _txtPlayersPath { get; set; }
        private string _txtTeamsPath { get; set; }
        private string _xmlPlayersPath { get; set; }
        private string _xmlTeamsPath { get; set; }
        private string _jsonPlayersPath { get; set; }
        private string _jsonTeamsPath { get; set; }
        private string _excellPlayersPath { get; set; }
        private string _excellTeamsPath { get; set; }
        private string _csvPlayersPath { get; set; }
        private string _csvTeamsPath { get; set; }

        string _exportDefaultFolder { get; set; }

        //TxtPlayers = 1,
        //TxtTeams = 2,
        //XMLPlayers = 3,
        //XMLTeams = 4,
        //JsonPlayers = 5,
        //JsonTeams = 6
        private FileTypesProcessing _storage { get; set; }

        private FileTypeEnum _storageType;

        public FileTypeEnum DataStorage
        {
            get
            {
                return _storageType;
            }
            set
            {

                _storageType = value;
                //_excellTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}ExcellTeams.xlsx";
                //_excellPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}ExcellPlayers.xlsx";
                //_xmlPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}XMLPlayers.xml";
                //_xmlTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}XMLTeams.xml";
                //_jsonPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}JsonPlayers.json";
                //_jsonTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}JsonTeams.json";
                //_txtPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}TXTPlayers.txt";
                //_txtTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}TXTTeams.txt";
                //_csvTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}CSVTeamStorage.csv";
                //_csvPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}CSVPlayerStorage.csv";

                var pathFile = "";

       

                switch (_storageType)
                {
                    case FileTypeEnum.TxtPlayers:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_TXTPlayers.txt");

                        _storage = new TXTProcessingPlayers(pathFile);
                        break;
                    case FileTypeEnum.TxtTeams:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_TXTTeams.txt");
                        _storage = new TXTProcessingTeams(pathFile);
                        break;
                    case FileTypeEnum.XMLPlayers:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_XMLplayers.txt");
                        _storage = new XMLPlayersProcessing(pathFile);
                        break;
                    case FileTypeEnum.XMLTeams:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_XMLteams.txt");
                        _storage = new XMLTeamsProcessing(pathFile);
                        break;
                    case FileTypeEnum.JsonPlayers:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_JsonPlayers.txt");
                        _storage = new JsonPlayersProcessing(pathFile);
                        break;
                    case FileTypeEnum.JsonTeams:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_JsonTeams.txt");
                        _storage = new JsonTeamProcessing(pathFile);
                        break;
                    case FileTypeEnum.ExcellPlayers:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_ExcellPlayers.txt");
                        _storage = new ExcellPlayersProcessing(pathFile) ;
                        break;
                    case FileTypeEnum.ExcellTeams:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_ExcellTeams.txt");
                        _storage = new ExcellTeamsProcessing(pathFile);
                        break;
                    case FileTypeEnum.CSVPlayers:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_CsvPlayers.txt");
                        _storage = new CSVPlayersProcessing(pathFile);
                        break;
                    case FileTypeEnum.CSVTeams:
                        pathFile = System.IO.Path.Combine(_exportDefaultFolder, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}_CsvTeams.txt");
                        _storage = new CSVTeamsProcessing(pathFile);
                        break;
                    default:
                        break;
                }

            }
        }


        public ImpExpDB()
        {
            _exportDefaultFolder = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport";

            if (!System.IO.Directory.Exists(_exportDefaultFolder))
            {
                System.IO.Directory.CreateDirectory(_exportDefaultFolder);
            }

            DBPath = new DBProcessing(_myConnectionString);
            _connection = new MySqlConnection(_myConnectionString);
        }
        public ImpExpDB(string filePath, bool player = false) : this()
        {
          


            if (filePath.Contains("txt", StringComparison.OrdinalIgnoreCase) && player == false)
            {
                _storage = new TXTProcessingTeams(filePath);
            }
            if (filePath.Contains("txt", StringComparison.OrdinalIgnoreCase) && player == true)
            {
                _storage = new TXTProcessingPlayers(filePath);
            }
            if (filePath.Contains("csv", StringComparison.OrdinalIgnoreCase) && player == false)
            {
                _storage = new CSVTeamsProcessing(filePath);
            }
            if (filePath.Contains("csv", StringComparison.OrdinalIgnoreCase) && player == true)
            {
                _storage = new CSVPlayersProcessing(filePath);
            }
            if (filePath.Contains("json", StringComparison.OrdinalIgnoreCase) && player == false)
            {
                _storage = new JsonTeamProcessing(filePath);
            }
            if (filePath.Contains("json", StringComparison.OrdinalIgnoreCase) && player == true)
            {
                _storage = new JsonPlayersProcessing(filePath);
            }
            if (filePath.Contains("xml", StringComparison.OrdinalIgnoreCase) && player == false)
            {
                _storage = new XMLTeamsProcessing(filePath);
            }
            if (filePath.Contains("xml", StringComparison.OrdinalIgnoreCase) && player == true)
            {
                _storage = new XMLPlayersProcessing(filePath);
            } 
            if (filePath.Contains("xlsx", StringComparison.OrdinalIgnoreCase) && player == false)
            {
                _storage = new ExcellTeamsProcessing(filePath);
            } 
            if (filePath.Contains("xlsx", StringComparison.OrdinalIgnoreCase) && player == true)
            {
                _storage = new ExcellPlayersProcessing(filePath);
            }
        }


        public void ImportTeamDataToDB()
        {
            BindingList<Teams> teamsToImport = _storage.GetTeamFromFIle();

            _connection.Open();
            var currentTeams = DBPath.GetTeams();
            var refer = new MainWindow();
            for (int i = teamsToImport.Count-1; i != -1; i--)
            {
                foreach (var item1 in currentTeams)
                {
                    if (teamsToImport[i].TeamName == item1.TeamName && teamsToImport[i].City == item1.City)
                    {
                        teamsToImport.Remove(teamsToImport[i]);
                        break;
                    }
                }
            }

            DBPath.Append(teamsToImport.ToArray());
            refer.ToLog($"{teamsToImport.Count} teams were inserted into database", MessageBoxImage.Information);
            _connection.Close();

        }

        public void ImportPlayerDataToDB(bool selected = false, params Teams[] team)
        {
            BindingList<BasketballPlayers> playersToImport = _storage.GetPlayersFromFile();
            _connection.Open();
            var currentplayers = DBPath.GetBasketballPlayers();
            if(selected)
            {
                foreach (var item in playersToImport)
                {
                    item.Current_team = team[0].TeamName;
                }
            }
            var refer = new MainWindow();
            for (int i = playersToImport.Count-1; i != -1; i--) { 
                foreach (var item1 in currentplayers)
                {
                    if (playersToImport[i].Name == item1.Name && playersToImport[i].Current_team == item1.Current_team)
                    {
                        playersToImport.Remove(playersToImport[i]);
                        break;
                    }
                }
            }
            
                DBPath.Append(playersToImport.ToArray());
            refer.ToLog($"{playersToImport.Count} players were inserted into database", MessageBoxImage.Information);
            _connection.Close();
        }



        public void StorageTeamsEmptinessCheck()
        {
            BindingList<Teams> teams = _storage.GetTeamFromFIle();
            if (teams.Count <= 0)
            {
                MessageBox.Show("File is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            return;
        }

        public void StoragePlayersEmptinessCheck()
        {
            BindingList<BasketballPlayers> players = _storage.GetPlayersFromFile();
            if (players.Count <= 0)
            {
                    MessageBox.Show("File is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                
            }
            return;
        }

        public void ExportTeamDataFromDB(params Teams [] team)
        {
                _storage.ImportTeamData(team);

        }

        public string GetFilePath()
        {
            string pathToReturn = null;
            if (!string.IsNullOrEmpty(_storage.FileProcessingPath))
            {
               pathToReturn = _storage.FileProcessingPath;
                return pathToReturn;
            }
            return pathToReturn;
        }

        public void ExportPlayerDataFromDB(params BasketballPlayers[] players)
        {
                _storage.ImportPlayersData(players);
            
        }


    }
}
