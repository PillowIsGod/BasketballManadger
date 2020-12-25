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
        public abstract void Delete(params BasketballPlayers[] playerToDelete);
        public abstract void Delete(params Teams[] teamToDelete);
        public abstract void Append(params BasketballPlayers[] playerToAdd);
        public abstract void Append(params Teams[] teamToAdd);
        public DataProcessing(string filePath)
        {
            FilePath = filePath;
        }


    }
}
