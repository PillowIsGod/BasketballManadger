using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    public class TXTProcessingPlayers : FileTypesProcessing
    {
        public TXTProcessingPlayers(string filePath) : base (filePath)
        {
        }



        public override void ImportPlayersData(params BasketballPlayers[] playersToImport)
        {
            string line;
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in playersToImport)
            {
                line = item.Name + "," + item.Current_team + "," + item.Picture + "," + item.Age.ToString() + "," + item.Career_age.ToString() + "," + item.Height.ToString() + "," + item.Weight.ToString() + "," + item.Position;
               
                    sw.WriteLine(line);
                
                line = null;
            }
            }
        }



        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            BindingList<BasketballPlayers> players = new BindingList<BasketballPlayers>();
            List<string> fileContent = new List<string>();
            using (StreamReader sr = new StreamReader(FileProcessingPath))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    fileContent.Add(line);
                }
            }
            foreach (var item in fileContent)
            {
                var array = item.Split(",");
                BasketballPlayers player = new BasketballPlayers();
                player.Name = array[0];
                player.Current_team = array[1];
                player.Picture = array[2];
                player.Age = Convert.ToInt32(array[3]);
                player.Career_age = Convert.ToInt32(array[4]);
                player.Height = Convert.ToDouble(array[5]);
                player.Weight = Convert.ToInt32(array[6]);
                player.Position = array[7];
                players.Add(player);
            }
            return players;
        }

        public override void ImportTeamData(params Teams[] teamsToImport)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            throw new NotImplementedException();
        }
    }
}
