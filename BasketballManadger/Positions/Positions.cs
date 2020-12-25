using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballManadger
{
    public class Positions
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public Positions()
        {

        }

    }
}
