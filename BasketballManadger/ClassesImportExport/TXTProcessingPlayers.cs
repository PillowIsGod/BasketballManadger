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



        public override void ImportPlayersData(BindingList<BasketballPlayers> playersToImport)
        {
            string line;
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in playersToImport)
            {
                line = item.ID.ToString() + "," + item.Name + "," + item.Current_team + "," + item.Picture + "," + item.Age.ToString() + "," + item.Career_age.ToString() + "," + item.Height.ToString() + "," + item.Weight.ToString() + "," + item.Position;
               
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
                player.ID = Convert.ToInt32(array[0]);
                player.Name = array[1];
                player.Current_team = array[2];
                player.Picture = array[3];
                player.Age = Convert.ToInt32(array[4]);
                player.Career_age = Convert.ToInt32(array[5]);
                player.Height = Convert.ToDouble(array[6]);
                player.Weight = Convert.ToInt32(array[7]);
                player.Position = array[8];
                players.Add(player);
            }
            return players;
        }

        public override void ImportTeamData(BindingList<Teams> teamsToImport)
        {
            throw new NotImplementedException();
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            throw new NotImplementedException();
        }
    }
}
