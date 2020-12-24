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
                _excellTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}ExcellTeams";
                _excellPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}ExcellPlayers.xlsx";
                _xmlPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}XMLPlayers.xml";
                _xmlTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}XMLTeams.xml";
                _jsonPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}JsonPlayers.json";
                _jsonTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}JsonTeams.json";
                _txtPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}TXTPlayers.txt";
                _txtTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}TXTTeams.txt";
                _csvTeamsPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}CSVTeamStorage.csv";
                _csvPlayersPath = @$"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}CSVPlayerStorage.csv";

                switch (_storageType)
                {
                    case FileTypeEnum.TxtPlayers:
                        _storage = new TXTProcessingPlayers(_txtPlayersPath);
                        break;
                    case FileTypeEnum.TxtTeams:
                        _storage = new TXTProcessingTeams(_txtTeamsPath);
                        break;
                    case FileTypeEnum.XMLPlayers:
                        _storage = new XMLPlayersProcessing(_xmlPlayersPath);
                        break;
                    case FileTypeEnum.XMLTeams:
                        _storage = new XMLTeamsProcessing(_xmlTeamsPath);
                        break;
                    case FileTypeEnum.JsonPlayers:
                        _storage = new JsonPlayersProcessing(_jsonPlayersPath);
                        break;
                    case FileTypeEnum.JsonTeams:
                        _storage = new JsonTeamProcessing(_jsonTeamsPath);
                        break;
                    case FileTypeEnum.ExcellPlayers:
                        _storage = new ExcellPlayersProcessing(_excellPlayersPath) ;
                        break;
                    case FileTypeEnum.ExcellTeams:
                        _storage = new ExcellTeamsProcessing(_excellTeamsPath);
                        break;
                    case FileTypeEnum.CSVPlayers:
                        _storage = new CSVPlayersProcessing(_csvPlayersPath);
                        break;
                    case FileTypeEnum.CSVTeams:
                        _storage = new CSVTeamsProcessing(_csvTeamsPath);
                        break;
                    default:
                        break;
                }

            }
        }


        public ImpExpDB()
        {
            DBPath = new DBProcessing(_myConnectionString);
            _connection = new MySqlConnection(_myConnectionString);
        }
        public ImpExpDB(string filePath, bool player = false)
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
            DBPath = new DBProcessing(_myConnectionString);
            _connection = new MySqlConnection(_myConnectionString);
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

        public void ImportPlayerDataToDB()
        {
            BindingList<BasketballPlayers> playersToImport = _storage.GetPlayersFromFile();
            _connection.Open();
            var currentplayers = DBPath.GetBasketballPlayers();
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

        public void ExportTeamDataFromDB()
        {
            BindingList<Teams> teamsToExport = DBPath.GetTeams();

            _storage.ImportTeamData(teamsToExport);
        }



        public void ExportPlayerDataFromDB()
        {
            BindingList<BasketballPlayers> playersToExport = DBPath.GetBasketballPlayers();

            _storage.ImportPlayersData(playersToExport);
        }


    }
}
