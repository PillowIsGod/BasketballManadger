using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    public class ExcellTeamsProcessing : FileTypesProcessing
    {
        public ExcellTeamsProcessing(string filePath) : base(filePath)
        {

        }

        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            throw new NotImplementedException();
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {            
            BindingList<Teams> teamsToOutput = new BindingList<Teams>();


            XSSFWorkbook workbook; 
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }



            ISheet sheet = workbook.GetSheet("Sheet0");
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                Teams team = new Teams();
                if (sheet.GetRow(row) != null)
                {
                    if (!team.CompleteTeamNullCheck(team))
                    {
                        team.Logo = sheet.GetRow(row).GetCell(0).ToString();
                        team.TeamName = sheet.GetRow(row).GetCell(1).ToString();
                        team.City = sheet.GetRow(row).GetCell(2).ToString();

                        teamsToOutput.Add(team);
                    }
                } 
            }
            return teamsToOutput;
        }

        public override void ImportPlayersData(params BasketballPlayers[] playersToImport)
        {
            throw new NotImplementedException();
        }

        public override void ImportTeamData(params Teams[] teamsToImport)
        {
            IWorkbook workbook = null;
            ISheet worksheet = null;
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook();
                worksheet = workbook.CreateSheet("Sheet0");
                int row = 0;
                IRow newRow = worksheet.CreateRow(row);

                ICell cell = newRow.CreateCell(0, CellType.String);
                cell.SetCellValue("Logo");

                cell = newRow.CreateCell(1, CellType.String);
                cell.SetCellValue("Team Name");

                cell = newRow.CreateCell(2, CellType.String);
                cell.SetCellValue("City");
                row++;
                foreach (var item in teamsToImport)
                {
                    newRow = worksheet.CreateRow(row);

                    cell = newRow.CreateCell(0, CellType.String);
                    cell.SetCellValue(item.Logo);

                    cell = newRow.CreateCell(1, CellType.String);
                    cell.SetCellValue(item.TeamName);

                    cell = newRow.CreateCell(2, CellType.String);
                    cell.SetCellValue(item.City);



                    row++;

                }



                workbook.Write(fs);
            }

        }
    }
}
