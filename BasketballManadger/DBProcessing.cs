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
    public class DBProcessing : DataProcessing
    {

        private MySqlConnection sqlConnection = null;
        private MySqlDataAdapter _mysqlDataAdapter = null;
        private DataSet _dataset = null;
        public DBProcessing(string filePath) : base(filePath)
        {
            sqlConnection = new MySqlConnection(filePath);

        }

        public override void Delete(params BasketballPlayers[] playersToDelete)
        {
            var currentPlayers = GetBasketballPlayers();
            foreach (var player in playersToDelete)
            {
                foreach (var item in currentPlayers)
                {
                    if (item.ID == player.ID)
                    {
                        using (MySqlConnection connection = new MySqlConnection(FilePath))
                        {
                            connection.Open();
                            string sqlExpression = String.Format("DELETE FROM basketballplayers WHERE id = '{0}'", player.ID);
                            MySqlCommand command = new MySqlCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            }
        }
        public override void Delete(params Teams[] teamsToDelete)
        {
            var currentPlayers = GetBasketballPlayers();
            var currentTeams = GetTeams();
            foreach (var team in teamsToDelete)
            {
                foreach (var item in currentTeams)
                {
                    if (item.ID == team.ID)
                    {
                        foreach (var player in currentPlayers) 
                        {
                            int id_team = GetPlayersTeamID(player);
                            if(id_team == team.ID)
                            {
                                using(MySqlConnection connection = new MySqlConnection(FilePath))
                                {
                                    connection.Open();
                                    string sqlExpression = String.Format("DELETE FROM basketballplayers WHERE id_team = '{0}'", id_team);
                                    MySqlCommand command = new MySqlCommand(sqlExpression, connection);
                                    command.ExecuteNonQuery();
                                    connection.Close();
                                }
                            }
                        }
                        using (MySqlConnection connection = new MySqlConnection(FilePath))
                        {
                            connection.Open();

                            string sqlExpression = String.Format("DELETE FROM teams WHERE id = '{0}'", team.ID);
                            MySqlCommand command = new MySqlCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                }
            }
        }
        public override void Append(params BasketballPlayers[] playerToAdd)
        {
            foreach (var player in playerToAdd)
            {
                using (MySqlConnection connection = new MySqlConnection(FilePath))
                {
                    connection.Open();
                    int id_team = GetPlayersTeamID(player);
                    int id_position = GetPlayersPositionID(player);

                    string sqlExpression = String.Format("INSERT INTO basketballplayers (name,age, career_age, height, weight, id_team, id_position, picture) VALUES ('{0}','{1}','{2}','{3}'," +
                        "'{4}','{5}','{6}', '{7}')", player.Name, player.Age, player.Career_age, player.Height, player.Weight, id_team, id_position, player.Picture.Replace(@"\", @"\\"));


                    MySqlCommand command = new MySqlCommand(sqlExpression, connection);

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }
        public override void Append(params Teams[] teamToAdd)
        {
            foreach( var team in teamToAdd)
            {
                using (MySqlConnection connection = new MySqlConnection(FilePath))
                {
                    connection.Open();

                    string sqlExpression = String.Format("INSERT INTO teams (team_name, city, logo) VALUES ('{0}', '{1}', '{2}')", team.TeamName, team.City, team.Logo.Replace(@"\", @"\\"));

                    MySqlCommand command = new MySqlCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();

                    connection.Close();

                }
            }   
        }
        public override void SaveData(BindingList<Teams> listToSave)
        {
            BindingList<Teams> currentDBteams = GetTeams();
            foreach (var item in listToSave)
            {
                foreach (var item1 in currentDBteams)
                {
                    if (item.ID == item1.ID)
                    {
                        using (MySqlConnection connection = new MySqlConnection(FilePath))
                        {
                            connection.Open();
                            string sqlExpression = String.Format("UPDATE teams SET team_name = '{0}', city = '{1}', logo = '{2}' WHERE id = '{3}'", item.TeamName, item.City, item.Logo.Replace(@"\", @"\\"), item.ID);
                            MySqlCommand command = new MySqlCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }

                }

            }

        }

        public override void SaveData(BindingList<BasketballPlayers> listToSav)
        {
            BindingList<BasketballPlayers> currentDBplayers = GetBasketballPlayers();
            foreach (var item in listToSav)
            {
                foreach (var item1 in currentDBplayers)
                {
                    if (item.ID == item1.ID)
                    {

                        using (MySqlConnection connection = new MySqlConnection(FilePath))
                        {

                            int id_team = GetPlayersTeamID(item);
                            int id_position = GetPlayersPositionID(item);
                            connection.Open();
                            string sqlExpression = String.Format("UPDATE basketballplayers SET name = '{0}',age = '{1}', career_age = '{2}', height = '{3}', weight = '{4}', id_team = '{5}', id_position = '{6}', picture = '{7}'" +
                                " WHERE id = '{8}'", item.Name, item.Age, item.Career_age, item.Height, item.Weight, id_team, id_position, item.Picture.Replace(@"\", @"\\"), item.ID);
                            MySqlCommand command = new MySqlCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                            connection.Close();

                        }
                    }

                }
            }

        }
        public override BindingList<BasketballPlayers> GetBasketballPlayers()
        {
            _mysqlDataAdapter = new MySqlDataAdapter("SELECT id, picture, name, age, career_age, height, weight, (SELECT team_name FROM teams WHERE ID = id_team) AS team," +
                " (SELECT position FROM positions WHERE ID = id_position) AS position FROM basketballplayers", sqlConnection);
            BindingList<BasketballPlayers> playersToReturn = new BindingList<BasketballPlayers>();

            _dataset = new DataSet();
            _mysqlDataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                BasketballPlayers player = new BasketballPlayers();
                player.ID = Convert.ToInt32(item[0]);
                player.Picture = Convert.ToString(item[1]);
                player.Name = Convert.ToString(item[2]);
                player.Age = Convert.ToInt32(item[3]);
                player.Career_age = Convert.ToInt32(item[4]);
                player.Height = Convert.ToDouble(item[5]);
                player.Weight = Convert.ToInt32(item[6]);
                player.Current_team = Convert.ToString(item[7]);
                player.Position = Convert.ToString(item[8]);
                playersToReturn.Add(player);
            }
            return playersToReturn;
        }
        public override BindingList<Teams> GetTeams()
        {
            _mysqlDataAdapter = new MySqlDataAdapter("SELECT id, logo, team_name, city FROM teams", sqlConnection);
            BindingList<Teams> teamsToReturn = new BindingList<Teams>();
            _dataset = new DataSet();
            _mysqlDataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                Teams team = new Teams();
                team.ID = Convert.ToInt32(item[0]);
                team.Logo = Convert.ToString(item[1]);
                team.TeamName = Convert.ToString(item[2]);
                team.City = Convert.ToString(item[3]);
                teamsToReturn.Add(team);
            }
            return teamsToReturn;
        }
        public override BindingList<Positions> GetPositions()
        {
            _mysqlDataAdapter = new MySqlDataAdapter("SELECT id, position, role from positions", sqlConnection);
            BindingList<Positions> positionsToReturn = new BindingList<Positions>();
            _dataset = new DataSet();
            _mysqlDataAdapter.Fill(_dataset);
            DataTable dt = _dataset.Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                Positions position = new Positions();
                position.ID = Convert.ToInt32(item[0]);
                position.Position = Convert.ToString(item[1]);
                position.Role = Convert.ToString(item[2]);
                positionsToReturn.Add(position);
            }
            return positionsToReturn;
        }



        public int GetPlayersTeamID(BasketballPlayers player)
        {
            BindingList<Teams> currentTeams = GetTeams();
            int id_team = 0;

            foreach (var currentTeam in currentTeams)
            {

                if (currentTeam.TeamName == player.Current_team)
                {

                    id_team = currentTeam.ID;
                    return id_team;
                }
            }
            return id_team;
        }

        public int GetPlayersPositionID(BasketballPlayers player)
        {
            BindingList<Positions> currentPositions = GetPositions();
            int id_position = 0;

            foreach (var position in currentPositions)
            {
                if (position.Position == player.Position)
                {

                    id_position = position.ID;
                    return id_position;
                }
            }
            return id_position;
        }
    }
}
