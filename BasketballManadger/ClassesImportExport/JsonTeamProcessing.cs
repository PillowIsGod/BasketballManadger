using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    public class JsonTeamProcessing : FileTypesProcessing
    {
        public JsonTeamProcessing(string filePath) : base (filePath)
        {

        }
        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            throw new NotImplementedException();
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            BindingList<Teams> teamToReturn = new BindingList<Teams>();
            string content = File.ReadAllText(FileProcessingPath);

            if (string.IsNullOrEmpty(content))
            {
                return teamToReturn;
            }
            var text = JsonConvert.DeserializeObject<JsonRootModel>(content);
            foreach (var item in text.Teams)
            {
                teamToReturn.Add(item);
            }
            return teamToReturn;
        }

        public override void ImportPlayersData(BindingList<BasketballPlayers> playersToImport)
        {
            throw new NotImplementedException();
        }

        public override void ImportTeamData(BindingList<Teams> teamsToImport)
        {
            var jObj = new { Teams = teamsToImport};


            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                var str = JObject.FromObject(jObj).ToString();
                sw.WriteLine(str);
            }
        }
    }
}
