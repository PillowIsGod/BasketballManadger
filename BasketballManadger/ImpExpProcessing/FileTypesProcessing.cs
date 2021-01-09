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
        public abstract void ImportPlayersData(params BasketballPlayers [] playersToImport);
        public abstract void ImportTeamData(params Teams [] teamsToImport);

        public abstract BindingList<Teams> GetTeamFromFIle();
    }
}
