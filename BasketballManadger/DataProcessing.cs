using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BasketballManadger
{
    public abstract class DataProcessing
    {
        public string FilePath { get; private set; }
        public abstract void SaveData(BindingList<Teams> listToSave);
        public abstract void SaveData(BindingList<BasketballPlayers> listToSav);
        public abstract BindingList<BasketballPlayers> GetBasketballPlayers();
        public abstract BindingList<Teams> GetTeams();
        public abstract BindingList<Positions> GetPositions();
        public DataProcessing(string filePath)
        {
            FilePath = filePath;
        }


    }
}
