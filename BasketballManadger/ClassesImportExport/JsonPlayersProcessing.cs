﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    public class JsonPlayersProcessing : FileTypesProcessing
    {
        public JsonPlayersProcessing(string filePath) : base(filePath)
        {

        }
        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            BindingList<BasketballPlayers> playersToReturn = new BindingList<BasketballPlayers>();
            string content = File.ReadAllText(FileProcessingPath);

            if (string.IsNullOrEmpty(content))
            {
                return playersToReturn;
            }
            playersToReturn = JsonConvert.DeserializeObject<BindingList<BasketballPlayers>>(content);
            return playersToReturn;
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            throw new NotImplementedException();
        }

        public override void ImportPlayersData(BindingList<BasketballPlayers> playersToImport)
        {
            var jObj = new { Players = playersToImport };


            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                var str = JObject.FromObject(jObj).ToString();
                sw.WriteLine(str);
            }
        }

        public override void ImportTeamData(BindingList<Teams> teamsToImport)
        {
            throw new NotImplementedException();
        }
    }
}
