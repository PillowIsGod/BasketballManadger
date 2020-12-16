using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    class TXTProcessingTeams
    {
        public string TxtFilePathTeams;
        public TXTProcessingTeams(string txtFilePathTeams)
        {
            TxtFilePathTeams = txtFilePathTeams;
        }



        public void ImportTeamDataFrom(BindingList<Teams> teamsToImport)
        {
            string line;
            foreach (var item in teamsToImport)
            {
                line = item.ID.ToString() + "," + item.City + "," + item.TeamName + "," + item.Logo;
                using (FileStream fs = new FileStream(TxtFilePathTeams, FileMode.Truncate))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(line);
                }
                line = null;
            }
        }



        public BindingList<Teams> GetTeamsFromTXT()
        {
            BindingList<Teams> teams = new BindingList<Teams>();
            List<string> fileContent = new List<string>();
                using(StreamReader sr = new StreamReader(TxtFilePathTeams))
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
