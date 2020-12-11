using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace BasketballManadger
{
    class DBProcessing
    {
        private string _myConnectionString = "Database = basketballdata; Data Source = 127.0.0.1; User Id = root; Password = 7Bc145f606";
        private MySqlConnection sqlConnection = null;
        private MySqlDataAdapter _mysqlDataAdapter = null;
        private DataSet _dataset = null;
        public DBProcessing(string connectionString)
        {
            sqlConnection = new MySqlConnection(connectionString);
        }


        public void SaveData(BindingList<Teams> listToSave)
        {

        }
        public void SaveData(BindingList<BasketballPlayers> listToSav)
        {

        }
        public BindingList<BasketballPlayers> GetBasketballPlayers()
        {
            sqlConnection.Open();
            _mysqlDataAdapter = new MySqlDataAdapter("SELECT id, picture, name, age, career_age, height, weight, (SELECT team_name FROM teams WHERE ID = id_team) AS team," +
                " (SELECT position FROM positions WHERE ID = id_position) AS position FROM basketballplayers", sqlConnection);
            BindingList<BasketballPlayers> playersToReturn = new BindingList<BasketballPlayers>();
            BasketballPlayers player = new BasketballPlayers();
            _dataset = new DataSet();
            _mysqlDataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                player.ID = Convert.ToInt32(item[0]);
                player.Picture = Convert.ToString(item[1]);
                player.Name = Convert.ToString(item[2]);
                player.Age = Convert.ToInt32(item[3]);
                player.Career_age = Convert.ToInt32(item[4]);
                player.Height = Convert.ToInt32(item[5]);
                player.Weight = Convert.ToInt32(item[6]);
                player.Current_team = Convert.ToString(item[7]);
                player.Position = Convert.ToString(item[8]);
                playersToReturn.Add(player);
            }
            return playersToReturn;
        }
        public BindingList<Teams> GetTeams()
        {
            sqlConnection.Open();
            _mysqlDataAdapter = new MySqlDataAdapter("SELECT id, logo, team_name, city FROM teams", sqlConnection);
            BindingList < Teams > teamsToReturn = new BindingList<Teams>();
            Teams team = new Teams();
            _dataset = new DataSet();
            _mysqlDataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                team.ID = Convert.ToInt32(item[0]);
                team.Logo = Convert.ToString(item[1]);
                team.TeamName = Convert.ToString(item[2]);
                team.City = Convert.ToString(item[3]);
                teamsToReturn.Add(team);
            }
            return teamsToReturn;
        }
        public BindingList<Positions> GetPositions()
        {
            sqlConnection.Open();
            _mysqlDataAdapter = new MySqlDataAdapter("SELECT id, position, role from positions", sqlConnection);
            BindingList<Positions> positionsToReturn = new BindingList<Positions>();
            Positions position = new Positions();
            _mysqlDataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                position.ID = Convert.ToInt32(item[0]);
                position.Position = Convert.ToString(item[1]);
                position.Position = Convert.ToString(item[2]);
                positionsToReturn.Add(position);
            }
            return positionsToReturn;
        }

    }
}
