using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace BasketballManadger.ClassesImportExport
{
    public class CSVTeamsProcessing : FileTypesProcessing
    {

        public CSVTeamsProcessing(string filePath) : base(filePath)
        {

        }
        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            throw new NotImplementedException();
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            BindingList<Teams> teamsToOutput = new BindingList<Teams>();
            using var streamReader = File.OpenText(FileProcessingPath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

            var users = csvReader.GetRecords<Teams>();

            foreach (var item in users)
            {
                teamsToOutput.Add(item);
            }

            return teamsToOutput;
        }

        public override void ImportPlayersData(BindingList<BasketballPlayers> playersToImport)
        {
            throw new NotImplementedException();
        }

        public override void ImportTeamData(BindingList<Teams> teamsToImport)
        {
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.Truncate, FileAccess.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                using (CsvWriter cwriter = new CsvWriter(sw, CultureInfo.CurrentCulture))
                {
                    cwriter.WriteField("ID");
                    cwriter.WriteField("Logo");
                    cwriter.WriteField("City");
                    cwriter.WriteField("TeamName");
                    cwriter.NextRecord();
                    foreach (var item in teamsToImport)
                    {
                        cwriter.WriteField(item.ID);
                        cwriter.WriteField(item.Logo);
                        cwriter.WriteField(item.City);
                        cwriter.WriteField(item.TeamName);
                        cwriter.NextRecord();
                    }
                }
            }
        }
    }
}
