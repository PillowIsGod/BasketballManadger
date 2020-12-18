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
    public static class ImpExpDB
    {

        private static string _myConnectionString = "Database = basketballdata; Data Source = 127.0.0.1; User Id = root; Password = 7Bc145f606";

        private static DataProcessing DBPath;

        private static MySqlConnection _connection;

        private static string _txtPlayersPath { get; set; }
        private static string _txtTeamsPath { get; set; }
        private static string _xmlPlayersPath { get; set; }
        private static string _xmlTeamsPath { get; set; }
        private static string _jsonPlayersPath { get; set; }
        private static string _jsonTeamsPath { get; set; }
        private static string _excellPlayersPath { get; set; }
        private static string _excellTeamsPath { get; set; }
        private static string _csvPlayersPath { get; set; }
        private static string _csvTeamsPath { get; set; }

        //TxtPlayers = 1,
        //TxtTeams = 2,
        //XMLPlayers = 3,
        //XMLTeams = 4,
        //JsonPlayers = 5,
        //JsonTeams = 6
        private static FileTypesProcessing _storage { get; set; }

        private static FileTypeEnum _storageType;

        public static FileTypeEnum DataStorage
        {
            get
            {
                return _storageType;
            }
            set
            {
                _storageType = value;
                _excellTeamsPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\ExcellTeams.xlsx";
                _excellPlayersPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\ExcellPlayers.xlsx";
                _xmlPlayersPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\XMLPlayers.xml";
                _xmlTeamsPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\XMLTeams.xml";
                _jsonPlayersPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\JsonPlayers.json";
                _jsonTeamsPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\JsonTeams.json";
                _txtPlayersPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\TXTPlayers.txt";
                _txtTeamsPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\TXTTeams.txt";
                _csvTeamsPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\CSVTeamStorage.csv";
                _csvPlayersPath = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\StoragesImportExport\CSVPlayerStorage.csv";

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


        static ImpExpDB()
        {
            DBPath = new DBProcessing(_myConnectionString);
            _connection = new MySqlConnection(_myConnectionString);
        }


        public static void ImportTeamDataToDB(bool insert = false, bool update = false)
        {
            BindingList<Teams> teamsToImport = _storage.GetTeamFromFIle();

            _connection.Open();
            if (insert)
            {
                DBPath.Append(teamsToImport.ToArray());
            }
            if (update)
            {
                DBPath.SaveData(teamsToImport);
            }


        }

        public static void ImportPlayerDataToDB(bool insert = false, bool update = false)
        {
            BindingList<BasketballPlayers> playersToImport = _storage.GetPlayersFromFile();

            _connection.Open();
            if (insert)
            {
                DBPath.Append(playersToImport.ToArray());
            }
            if (update)
            {
                DBPath.SaveData(playersToImport);
            }


        }



        public static void StorageTeamsEmptinessCheck()
        {
            BindingList<Teams> teams = _storage.GetTeamFromFIle();
            if (teams.Count <= 0)
            {
                MessageBox.Show("File is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            return;
        }

        public static void StoragePlayersEmptinessCheck()
        {
            BindingList<BasketballPlayers> players = _storage.GetPlayersFromFile();
            if (players.Count <= 0)
            {
                    MessageBox.Show("File is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                
            }
            return;
        }

        public static void ExportTeamDataFromDB()
        {
            BindingList<Teams> teamsToExport = DBPath.GetTeams();

            _storage.ImportTeamData(teamsToExport);
        }



        public static void ExportPlayerDataFromDB()
        {
            BindingList<BasketballPlayers> playersToExport = DBPath.GetBasketballPlayers();

            _storage.ImportPlayersData(playersToExport);
        }


    }
}
