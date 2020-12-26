using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    public class BasketballPlayers 
    {
        [JsonIgnore]
        [IgnoreAttribute]
        public int ID { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Career_age { get; set; }
        public string Current_team { get; set; }
        public double Height { get; set; }
        public int Weight { get; set; }
        public string Position { get; set; }
       

        public void CheckPlayerPicture(BasketballPlayers player)
        {
            if (string.IsNullOrEmpty(player.Picture))
            {
                player.Picture = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\NullPicture.png";
            }
            if (!File.Exists(player.Picture))
            {
                player.Picture = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\NullPicture.png";
            }
        }
        public bool CompletePlayerNullCheck( BasketballPlayers player)
        {
            bool check = false;
            if (string.IsNullOrEmpty(player.Current_team) || string.IsNullOrEmpty(player.Name) || string.IsNullOrEmpty(player.Age.ToString()) ||
                string.IsNullOrEmpty(player.Career_age.ToString()) || string.IsNullOrEmpty(player.Height.ToString()) || 
                string.IsNullOrEmpty(player.Weight.ToString()) || string.IsNullOrEmpty(player.Picture))
            {
                check = true;
            }
            return check;
        }
        public bool PlayerAdequacyCheck(BasketballPlayers player)
        {
            if(player.Age > 45 || player.Age < 19)
            {
                return true;
            }
            if(player.Career_age > 20 || player.Career_age < 0)
            {
                return true;
            }
            if(player.Height > 2.5 || player.Height < 1.50)
            {
                return true;
            }
            if(player.Weight > 200 || player.Weight < 50)
            {
                return true;
            }
            return false;
        }
            public BindingList<BasketballPlayers> RelatePlayerToATeam(Teams team, BindingList<BasketballPlayers> players)
        {
            BindingList<BasketballPlayers> playersToReturn = new BindingList<BasketballPlayers>();

                foreach (var item in players)
                {
                    if (team.TeamName == item.Current_team)
                    {
                    playersToReturn.Add(item);
                    }
                }

            return playersToReturn;
        }
        public BasketballPlayers()
        {
        }
    }
}
