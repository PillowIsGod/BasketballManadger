using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace BasketballManadger
{
    public class BasketballPlayers 
    {
        [JsonIgnore]
        [IgnoreAttribute]
        public int ID { get; set; }

        private string _picture;
        public string Picture 
        { 
            get 
            {

                return _picture;
            } 
            set => _picture = value;
        }

        public string Name { get; set; }
        public int Age { get; set; }
        public int Career_age { get; set; }
        public string Current_team { get; set; }
        public double Height { get; set; }
        public int Weight { get; set; }
        public string Position { get; set; }
       

        public void CheckPlayerPicture(BasketballPlayers player)
        {
            if (string.IsNullOrEmpty(player.Picture))
            {
                player.Picture = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\NullPicture.png";
            }
            if (!File.Exists(player.Picture))
            {
                player.Picture = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\NullPicture.png";
            }
        }
        public bool CompletePlayerNullCheck(string name, string position, string current_team, string age, string height, string weight, string career_age, string picture)
        {
            bool check = false;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(position) || string.IsNullOrEmpty(current_team) || string.IsNullOrEmpty(picture) || string.IsNullOrEmpty(age) ||
                string.IsNullOrEmpty(career_age) || string.IsNullOrEmpty(height) || string.IsNullOrEmpty(weight)) 
            {
                check = true;
            }
            return check;
        }
        public bool PlayerAdequacyCheck()
        {
            BasketballPlayers player = this;
            if (player.Age > 45 || player.Age < 19)
            {
                return true;
            }
            if(player.Career_age > 20 || player.Career_age < 0)
            {
                return true;
            }
            if(player.Height > 2.5 || player.Height < 1.50)
            {
                return true;
            }
            if(player.Weight > 200 || player.Weight < 50)
            {
                return true;
            }
            return false;
        }
        public BindingList<BasketballPlayers> SortPlayersByName(BindingList<BasketballPlayers> playersToSort, string name)
        {
            BindingList<BasketballPlayers> playersOutput = new BindingList<BasketballPlayers>();

            foreach (var item in playersToSort) 
            {
                if (item.Name.Contains(name,StringComparison.OrdinalIgnoreCase))
                {
                    playersOutput.Add(item);
                }
            }
            return playersOutput;
        }
        public BindingList<BasketballPlayers> SortPlayersByAge(BindingList<BasketballPlayers> playersToSort, int age)
        {
            BindingList<BasketballPlayers> playersOutput = new BindingList<BasketballPlayers>();

            foreach (var item in playersToSort)
            {
                if (item.Age == age)
                {
                    playersOutput.Add(item);
                }
            }
            return playersOutput;
        }
        public BindingList<BasketballPlayers> SortByPosition(BindingList<BasketballPlayers> playersToSort, string position)
        {
            BindingList<BasketballPlayers> playersOutput = new BindingList<BasketballPlayers>();
            foreach (var item in playersToSort)
            {

                if(item.Position == position)
                {
                    playersOutput.Add(item);
                }
            }
            return playersOutput;
        }
        public BindingList<BasketballPlayers> SortPlayers(BindingList<BasketballPlayers> playersToSort, string position, string name, int age)
        {
                BindingList<BasketballPlayers> playersToOutput = new BindingList<BasketballPlayers>();

                if (!string.IsNullOrEmpty(position) && !string.IsNullOrEmpty(name) && age != 0)
                {
                    foreach (var item in playersToSort)
                    {
                        if (item.Name.Contains(name, StringComparison.OrdinalIgnoreCase) && item.Age == age && item.Position == position)
                        {
                            playersToOutput.Add(item);
                        }
                    }
                    return playersToOutput;
                }

                if (string.IsNullOrEmpty(position) && !string.IsNullOrEmpty(name) && age != 0)
                {
                    foreach (var item in playersToSort)
                    {

                        if (item.Name.Contains(name, StringComparison.OrdinalIgnoreCase) && item.Age == age)
                        {
                            playersToOutput.Add(item);
                        }
                    }
                    return playersToOutput;
                }

                if ((string.IsNullOrEmpty(age.ToString()) || age == 0) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(position))
                {
                    foreach (var item in playersToSort)
                    {

                        if (item.Position == position && item.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                        {
                            playersToOutput.Add(item);
                        }
                    }
                    return playersToOutput;
                }
                if (age != 0 && string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(position))
                {
                    foreach (var item in playersToSort)
                    {

                        if (item.Position == position && item.Age == age)
                        {
                            playersToOutput.Add(item);
                        }
                    }
                    return playersToOutput;
                }


                if (string.IsNullOrEmpty(position) && string.IsNullOrEmpty(name) && age != 0)
                {
                    playersToOutput = SortPlayersByAge(playersToSort, age);
                    return playersToOutput;
                }
                if ((string.IsNullOrEmpty(age.ToString()) || age == 0) && string.IsNullOrEmpty(position) && !string.IsNullOrEmpty(name))
                {
                    playersToOutput = SortPlayersByName(playersToSort, name);
                    return playersToOutput;
                }
                if ((string.IsNullOrEmpty(age.ToString()) || age == 0) && string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(position))
                {
                    playersToOutput = SortByPosition(playersToSort, position);
                    return playersToOutput;
                }
            
            return playersToOutput;
        }
        public BindingList<BasketballPlayers> RelatePlayerToATeam(Teams team, BindingList<BasketballPlayers> players)
        {
            BindingList<BasketballPlayers> playersToReturn = new BindingList<BasketballPlayers>();

                foreach (var item in players)
                {
                    if (team.TeamName == item.Current_team)
                    {
                    playersToReturn.Add(item);
                    }
                }

            return playersToReturn;
        }
       
    }
}
