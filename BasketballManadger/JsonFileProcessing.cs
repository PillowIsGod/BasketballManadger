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
    public class JsonFileProcessing
    {
        public readonly string JsonPath;
        public JsonFileProcessing(string jsonPath)
        {
            JsonPath = jsonPath;
        }
        
        private BindingList<T> GetDataArray<T>() where T : new()
        {
            BindingList<T> result = new BindingList<T>();


            string content = File.ReadAllText(JsonPath);

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


        public BindingList<BasketballPlayers> GetBasketballPlayers()
        {
            
            var result = GetDataArray<BasketballPlayers>();

            return result;

        }
        public BindingList<Teams> GetTeams()
        {
            var result = GetDataArray<Teams>();

            return result;

        }
        public BindingList<Positions> GetPositions()
        {
            var result = GetDataArray<Positions>();

            return result;
        }
        public void SaveData(BindingList<Teams> listToSave)
        {
            var listPlayers = GetBasketballPlayers();
            var listPositions = GetPositions();
            var jObj = new { Teams = listToSave, Players = listPlayers, Positions = listPositions };
            using (FileStream fs = new FileStream(JsonPath, FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                var str = JObject.FromObject(jObj).ToString();
                sw.WriteLine(str);
            }
        }
        public void SaveData(BindingList<BasketballPlayers> listToSave)
        {
            var listTeams = GetTeams();
            var listPositions = GetPositions();
            var jObj = new { Teams = listTeams, Players = listToSave, Positions = listPositions };


            using (FileStream fs = new FileStream(JsonPath, FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                var str = JObject.FromObject(jObj).ToString();
                sw.WriteLine(str);
            }
        }

    }
}
