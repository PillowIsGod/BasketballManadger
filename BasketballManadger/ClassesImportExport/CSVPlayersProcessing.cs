﻿using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace BasketballManadger.ClassesImportExport
{
    public class CSVPlayersProcessing : FileTypesProcessing
    {
        public CSVPlayersProcessing(string filePath) : base(filePath)
        {

        }
        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            BindingList<BasketballPlayers> playerToOutput = new BindingList<BasketballPlayers>();
            using var streamReader = File.OpenText(FileProcessingPath);
            using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

            var users = csvReader.GetRecords<BasketballPlayers>();

            foreach (var item in users)
            {
                playerToOutput.Add(item);
            }

            return playerToOutput;
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            throw new NotImplementedException();
        }

        public override void ImportPlayersData(BindingList<BasketballPlayers> playersToImport)
        {
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.Truncate, FileAccess.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                using (CsvWriter cwriter = new CsvWriter(sw, CultureInfo.CurrentCulture))
                {
                    cwriter.WriteField("ID");
                    cwriter.WriteField("Picture");
                    cwriter.WriteField("Name");
                    cwriter.WriteField("Current_team");
                    cwriter.WriteField("Position");
                    cwriter.WriteField("Age");
                    cwriter.WriteField("Career_age");
                    cwriter.WriteField("Height");
                    cwriter.WriteField("Weight");
                    cwriter.NextRecord();
                    foreach (var item in playersToImport)
                    {
                        cwriter.WriteField(item.ID);
                        cwriter.WriteField(item.Picture);
                        cwriter.WriteField(item.Name);
                        cwriter.WriteField(item.Current_team);
                        cwriter.WriteField(item.Position);
                        cwriter.WriteField(item.Age);
                        cwriter.WriteField(item.Career_age);
                        cwriter.WriteField(item.Height);
                        cwriter.WriteField(item.Weight);
                        cwriter.NextRecord();
                    }
                }
            }
        }

        public override void ImportTeamData(BindingList<Teams> teamsToImport)
        {
            throw new NotImplementedException();
        }
    }
}
