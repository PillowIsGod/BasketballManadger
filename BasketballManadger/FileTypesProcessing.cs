using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BasketballManadger
{
    public abstract class FileTypesProcessing
    {

        public string FileProcessingPath;
        public FileTypesProcessing(string filePath)
        {
            FileProcessingPath = filePath;
        }


        public abstract BindingList<BasketballPlayers> GetPlayersFromFile();
        public abstract void ImportPlayersData(BindingList<BasketballPlayers> playersToImport);
        public abstract void ImportTeamData(BindingList<Teams> teamsToImport);

        public abstract BindingList<Teams> GetTeamFromFIle();
    }
}
