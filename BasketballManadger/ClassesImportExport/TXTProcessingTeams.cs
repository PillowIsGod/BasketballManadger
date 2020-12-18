using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
   public class TXTProcessingTeams : FileTypesProcessing
    {

        public TXTProcessingTeams(string filePath) : base (filePath)
        {
        }




        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            throw new NotImplementedException();
        }

        public override void ImportPlayersData(BindingList<BasketballPlayers> playersToImport)
        {
            throw new NotImplementedException();
        }

        public override void ImportTeamData(BindingList<Teams> teamsToImport)
        {
            string line;
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.Truncate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in teamsToImport)
            {
                line = item.ID.ToString() + "," + item.City + "," + item.TeamName + "," + item.Logo;
                
                    sw.WriteLine(line);
               
                line = null;
            }
            }
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            BindingList<Teams> teams = new BindingList<Teams>();
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
                Teams team = new Teams();
                team.ID = Convert.ToInt32(array[0]);
                team.City = array[1];
                team.TeamName = array[2];
                team.Logo = array[3];
                teams.Add(team);
            }
            return teams;
        }
    }
}
