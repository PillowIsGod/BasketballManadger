using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    class JsonFileProcessing
    {
        public readonly string JsonPath;
        public JsonFileProcessing(string jsonPath)
        {
            JsonPath = jsonPath;
        }
        public List<BasketballPlayers> GetBasketballPlayers()
        {
            List<BasketballPlayers> players = new List<BasketballPlayers>();
            string content = File.ReadAllText(JsonPath);

            if (string.IsNullOrEmpty(content))
            {
                return players;
            }

            var text = JsonConvert.DeserializeObject<List<BasketballPlayers>>(content);
            foreach (var item in text)
            {
                players.Add(item);
            }
            return players;
        }
        public List<Teams> GetTeams()
        {
            return null;
        }
        public List<Positions> GetPositions()
        {
            return null;
        }

    }
}
