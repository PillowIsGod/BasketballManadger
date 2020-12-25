using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballManadger
{
    public class JsonRootModel
    {
        public List<Teams> Teams { get; set; }
        public List<BasketballPlayers> Players { get; set; }
        public List<Positions> Positions { get; set; }
    }
}
