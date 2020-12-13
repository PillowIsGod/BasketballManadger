using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BasketballManadger
{
    public class BasketballPlayers 
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Career_age { get; set; }
        public string Current_team { get; set; }
        public double Height { get; set; }
        public int Weight { get; set; }
        public string Position { get; set; }
       
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
