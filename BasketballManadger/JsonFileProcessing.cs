using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        private List<T> GetDataArray<T>() where T : new()
        {
            List<T> result = new List<T>();

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

            return result;
        }

        public List<BasketballPlayers> GetBasketballPlayers()
        {
            var result = GetDataArray<BasketballPlayers>();

            return result;

            //List<BasketballPlayers> players = new List<BasketballPlayers>();
            
            //foreach (var item in text.Players)
            //{
            //    players.Add(item);
            //}
            //return players;
        }
        public List<Teams> GetTeams()
        {
            var result = GetDataArray<Teams>();

            return result;

        }
        public List<Positions> GetPositions()
        {
            return null;
        }

    }
}
