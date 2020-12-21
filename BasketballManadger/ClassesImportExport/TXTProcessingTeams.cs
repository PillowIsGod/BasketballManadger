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
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in teamsToImport)
            {
                line = item.City + "," + item.TeamName + "," + item.Logo;
                
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
                team.City = array[0];
                team.TeamName = array[1];
                team.Logo = array[2];
                teams.Add(team);
            }
            return teams;
        }
    }
}
