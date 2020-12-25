using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace BasketballManadger
{
    public class JsonFileProcessing : DataProcessing
    {
        public JsonFileProcessing(string filePath) : base(filePath)
        {
        }


        public override void Append(params BasketballPlayers[] playerToAdd)
        {
            var currentPlayers = GetBasketballPlayers();
            foreach (var player in playerToAdd)
            {
                currentPlayers.Add(player);
            }
            SaveData(currentPlayers);
        }
        public override void Append(params Teams[] teamToAdd) {
            var currentTeams = GetTeams();
            foreach (var team in teamToAdd)
            {
                currentTeams.Add(team);

            }
            SaveData(currentTeams);
        }
        

        public override void Delete(params BasketballPlayers[] playerToDelete) 
        {
            var currentPlayers = GetBasketballPlayers();
            foreach (var player in playerToDelete)
            {
                foreach (var item in currentPlayers)
                {
                    if (player.Age == item.Age && player.Career_age == item.Career_age && player.Name == item.Name && player.Picture == item.Picture)
                    {
                        currentPlayers.Remove(item);
                        break;
                    }
                }
                
            }
            SaveData(currentPlayers);


        }
        public override void Delete(params Teams[] teamToDelete) 
        {

            var currentTeams = GetTeams();
            foreach (var team in teamToDelete)
            {
                foreach (var item in currentTeams)
                {
                    if(team.City == item.City && item.Logo == team.Logo && item.TeamName == team.TeamName)
                    {
                        currentTeams.Remove(item);
                        break;
                    }
                }
            }
            SaveData(currentTeams);
        }
        private BindingList<T> GetDataArray<T>() where T : new()
        {
            BindingList<T> result = new BindingList<T>();


            string content = File.ReadAllText(FilePath);

                if (string.IsNullOrEmpty(content))
            {
                return result;
            }
            

            var text = JsonConvert.DeserializeObject<JsonRootModel>(content);


            if(typeof(T) == typeof(BasketballPlayers))
            {
                foreach (var item in text.Players)
                {
                    var tType = (T)Convert.ChangeType(item, typeof(BasketballPlayers));
                    result.Add(tType);
                }

            }
            if (typeof(T) == typeof(Teams))
            {
                foreach (var item in text.Teams)
                {
                    var tType = (T)Convert.ChangeType(item, typeof(Teams));
                    result.Add(tType);
                }
            }

            if (typeof(T) == typeof(Positions))
            {
                foreach (var item in text.Positions)
                {
                    var tType = (T)Convert.ChangeType(item, typeof(Positions));
                    result.Add(tType);
                }

            }
            return result;
        }


        public override BindingList<BasketballPlayers> GetBasketballPlayers()
        {
            
            var result = GetDataArray<BasketballPlayers>();

            return result;

        }
        public override BindingList<Teams> GetTeams()
        {
            var result = GetDataArray<Teams>();

            return result;

        }
        public override BindingList<Positions> GetPositions()
        {
            var result = GetDataArray<Positions>();

            return result;
        }
        public override void SaveData(BindingList<Teams> listToSave)
        {
            var listPlayers = GetBasketballPlayers();
            var listPositions = GetPositions();
            var jObj = new { Teams = listToSave, Players = listPlayers, Positions = listPositions };
            using (FileStream fs = new FileStream(FilePath, FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                var str = JObject.FromObject(jObj).ToString();
                sw.WriteLine(str);
            }
        }
        public override void SaveData(BindingList<BasketballPlayers> listToSave)
        {
            var listTeams = GetTeams();
            var listPositions = GetPositions();
            var jObj = new { Teams = listTeams, Players = listToSave, Positions = listPositions };


            using (FileStream fs = new FileStream(FilePath, FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                var str = JObject.FromObject(jObj).ToString();
                sw.WriteLine(str);
            }
        }

    }
}
