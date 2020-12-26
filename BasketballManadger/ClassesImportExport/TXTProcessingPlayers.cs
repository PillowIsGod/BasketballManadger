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
                line = $"Name,Current Team,Picture,Age,Career age,Height,Weight,Position";
                sw.WriteLine(line);
                line = null;
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
            for (int i = 1; i < fileContent.Count; i++) 
            {              
                var array = fileContent[i].Split(",");
                BasketballPlayers player = new BasketballPlayers();
                //if (!string.IsNullOrEmpty(array[0]) || !string.IsNullOrEmpty(array[1]) || !string.IsNullOrEmpty(array[2]) ||
                //    !string.IsNullOrEmpty(array[3]) || !string.IsNullOrEmpty(array[4]) || !string.IsNullOrEmpty(array[5]) ||  
                //    !string.IsNullOrEmpty(array[6]) || !string.IsNullOrEmpty(array[7])) {
                player.Name = array[0];
                player.Current_team = array[1];
                player.Picture = array[2];
                player.Age = Convert.ToInt32(array[3]);
                player.Career_age = Convert.ToInt32(array[4]);
                player.Height = Convert.ToDouble(array[5]);
                player.Weight = Convert.ToInt32(array[6]);
                player.Position = array[7];
                players.Add(player);
            //}
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
