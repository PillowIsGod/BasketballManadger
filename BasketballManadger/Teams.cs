using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballManadger
{
    public class Teams
    {
        [JsonIgnore]
        public List<BasketballPlayers> BasketballPlayers { get; set; }

        [JsonIgnore]
        public List<string> Players { get; set; }


        public string Logo { get; set; }
        public string TeamName { get; set; }
        public string City { get; set; }
        public string FullTeamName { get
            {
                return this.City + " " + this.TeamName;
            } }
        public Teams()
        {
            Players = new List<string>();
        }
    }
}
