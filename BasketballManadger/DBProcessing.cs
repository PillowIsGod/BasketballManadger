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
        public DBProcessing(string filePath) : base (filePath)
        {
            sqlConnection = new MySqlConnection(filePath);

        }


        public override void SaveData(BindingList<Teams> listToSave)
        {
            BindingList<BasketballPlayers> playersToSave = GetBasketballPlayers();
            BindingList<Positions> currentPositions = GetPositions();

            using (MySqlConnection connection = new MySqlConnection(FilePath))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("DELETE FROM basketballplayers WHERE id <> 1", connection);
                command.ExecuteNonQuery();

            }
            using (MySqlConnection connection = new MySqlConnection(FilePath))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("DELETE FROM teams WHERE id <> 1", connection);
                command.ExecuteNonQuery();
            }
            using (MySqlConnection connection = new MySqlConnection(FilePath))
            {
                int id_position = 0;
                int id_team = 0;
                connection.Open();
                foreach (var item in listToSave)
                {
                    string sqlExpression = String.Format("INSERT INTO teams (team_name, city, logo) VALUES ('{0}', '{1}', '{2}')", item.TeamName, item.City, item.Logo.Replace(@"\", @"\\"));
                    MySqlCommand command = new MySqlCommand(sqlExpression, connection);

                    command.ExecuteNonQuery();
                }
                BindingList<Teams> teamsToUse = GetTeams();
                foreach (var item0 in playersToSave)
                {
                    foreach (var item1 in teamsToUse)
                    {

                        if (item1.TeamName == item0.Current_team)
                        {
                            id_team = item1.ID;
                            break;
                        }
                    }
                    foreach (var item2 in currentPositions)
                    {
                        if (item2.Position == item0.Position)
                        {
                            id_position = item2.ID;
                            break;
                        }
                    }
                    string pictureConverter = String.Format("{0}",item0.Picture.Replace(@"\", @"\\"));

                    string sqlExpression1 = String.Format("INSERT INTO basketballplayers (name,age, career_age, height, weight, id_team, id_position, picture)" +
                        " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", item0.Name, item0.Age, item0.Career_age, item0.Height, item0.Weight, id_team, id_position, pictureConverter);
                    MySqlCommand commandToInsert = new MySqlCommand(sqlExpression1, connection);
                    commandToInsert.ExecuteNonQuery();
                }
            }
        }
        public override void SaveData(BindingList<BasketballPlayers> listToSav)
        {

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
                player.Height = Convert.ToInt32(item[5]);
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
            BindingList < Teams > teamsToReturn = new BindingList<Teams>();
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

    }
}
