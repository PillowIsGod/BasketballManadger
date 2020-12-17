using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    public class ExcellPlayersProcessing : FileTypesProcessing
    {
        public ExcellPlayersProcessing(string filePath) : base(filePath)
        {

        }

        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            BindingList<BasketballPlayers> playersToOutput = new BindingList<BasketballPlayers>();


            XSSFWorkbook workbook;
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fs);
            }



            ISheet sheet = workbook.GetSheet("Sheet0");
            int number = 0;
            for (int row = 0; row <= sheet.LastRowNum; row++)
            {
                BasketballPlayers player = new BasketballPlayers();
                if (sheet.GetRow(row) != null)
                {
                    if (int.TryParse(sheet.GetRow(row).GetCell(0).ToString(), out number))
                    {
                        player.ID = number;
                    }
                    else
                    {
                        return playersToOutput;
                    }
                    player.Picture = sheet.GetRow(row).GetCell(1).ToString();
                    player.Name = sheet.GetRow(row).GetCell(2).ToString();
                    player.Position = sheet.GetRow(row).GetCell(3).ToString();
                

                    playersToOutput.Add(player);
                    if (int.TryParse(sheet.GetRow(row).GetCell(4).ToString(), out number))
                    {
                        player.Age = number;
                    }
                    else
                    {
                        return playersToOutput;
                    }
                    if (int.TryParse(sheet.GetRow(row).GetCell(5).ToString(), out number))
                    {
                        player.Career_age = number;
                    }
                    else
                    {
                        return playersToOutput;
                    }
                    if (int.TryParse(sheet.GetRow(row).GetCell(6).ToString(), out number))
                    {
                        player.Height = number;
                    }
                    else
                    {
                        return playersToOutput;
                    }
                    if (int.TryParse(sheet.GetRow(row).GetCell(7).ToString(), out number))
                    {
                        player.Weight = number;
                    }
                    else
                    {
                        return playersToOutput;
                    }
                    player.Current_team = sheet.GetRow(row).GetCell(8).ToString();
                }
            }
            return playersToOutput;
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            throw new NotImplementedException();
        }

        public override void ImportPlayersData(BindingList<BasketballPlayers> playersToImport)
        {
            IWorkbook workbook = null;
            ISheet worksheet = null;
            using (FileStream fs = new FileStream(FileProcessingPath, FileMode.Truncate, FileAccess.ReadWrite))
            {
                workbook = new XSSFWorkbook();
                worksheet = workbook.CreateSheet("Sheet0");
                int row = 0;
                foreach (var item in playersToImport)
                {
                    IRow newRow = worksheet.CreateRow(row);

                    ICell cell = newRow.CreateCell(0, CellType.String);
                    cell.SetCellValue(item.ID);


                    cell = newRow.CreateCell(1, CellType.String);
                    cell.SetCellValue(item.Picture);

                    cell = newRow.CreateCell(2, CellType.String);
                    cell.SetCellValue(item.Name);

                    cell = newRow.CreateCell(3, CellType.String);
                    cell.SetCellValue(item.Position);

                    cell = newRow.CreateCell(4, CellType.String);
                    cell.SetCellValue(item.Age);

                    cell = newRow.CreateCell(5, CellType.String);
                    cell.SetCellValue(item.Career_age);

                    cell = newRow.CreateCell(6, CellType.String);
                    cell.SetCellValue(item.Height);

                    cell = newRow.CreateCell(7, CellType.String);
                    cell.SetCellValue(item.Weight);

                    cell = newRow.CreateCell(8, CellType.String);
                    cell.SetCellValue(item.Current_team);


                    row++;

                }



                workbook.Write(fs);
            }
        }

        public override void ImportTeamData(BindingList<Teams> teamsToImport)
        {
            throw new NotImplementedException();
        }
    }
}
