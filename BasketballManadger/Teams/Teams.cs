using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace BasketballManadger
{
    public class Teams : INotifyPropertyChanged
    {
        [JsonIgnore]
        public BindingList<BasketballPlayers> BasketballPlayers { get; set; }
        
        [JsonIgnore]
        [IgnoreAttribute]
        public int ID { get; set; }
        private string _teamName;
        private string _city;
        public string Logo { get; set; }
        public string TeamName
        {
            get { return _teamName; }
            set
            {
                if (_teamName == value)
                {
                    return;
                }
                _teamName = value;
                OnPropertyChanged("TeamName");
            }
        }
        public string City
        {
            get { return _city; }
            set
            {
                if (_city == value)
                {
                    return;
                }
                _city = value;
                OnPropertyChanged("City");
            }
        }
        [JsonIgnore]
        public string FullTeamName { get
            {
                return this.City + " " + this.TeamName;
            } }

        [JsonIgnore]

        public double AvgTeamHeight { get
            {
                return GetAvgTeamHeight(this.BasketballPlayers);
            } }
        [JsonIgnore]
        public double AvgTeamAge { get
            {
                return GetAvgTeamAge(this.BasketballPlayers);
            }
            }
        [JsonIgnore]
        public int AmountOfTeamMembers { get {
                return this.BasketballPlayers.Count;
            } }
        public Teams()
        {
        }
        public BindingList<Teams> TeamsSort(BindingList<Teams> teamsToSort, int type)
        {
            var teamsOutput = new BindingList<Teams>();
            Teams[] arr = new Teams[teamsToSort.Count];
            switch (type)
            {
                case 1:
                    if (teamsToSort[0].AmountOfTeamMembers >= teamsToSort[1].AmountOfTeamMembers)
                    {
                        arr = teamsToSort.OrderBy(x => x.AmountOfTeamMembers).ToArray();
                    }
                    if (teamsToSort[0].AmountOfTeamMembers <= teamsToSort[1].AmountOfTeamMembers)
                    {
                        arr = teamsToSort.OrderByDescending(x => x.AmountOfTeamMembers).ToArray();
                    }
                    break;
                case 2:
                    if (teamsToSort[0].AvgTeamAge >= teamsToSort[1].AvgTeamAge)
                    {
                        arr = teamsToSort.OrderBy(x => x.AvgTeamAge).ToArray();
                    }
                    if (teamsToSort[0].AvgTeamAge <= teamsToSort[1].AvgTeamAge)
                    {
                        arr = teamsToSort.OrderByDescending(x => x.AvgTeamAge).ToArray();
                    }
                    break;
                case 3:
                    if (teamsToSort[0].AvgTeamHeight >= teamsToSort[1].AvgTeamHeight)
                    {
                        arr = teamsToSort.OrderBy(x => x.AvgTeamHeight).ToArray();
                    }
                    if (teamsToSort[0].AvgTeamHeight <= teamsToSort[1].AvgTeamHeight)
                    {
                        arr = teamsToSort.OrderByDescending(x => x.AvgTeamHeight).ToArray();
                    }
                    break;
            }
            foreach (var item in arr)
            {
                teamsOutput.Add(item);
            }
            return teamsOutput;
        }
        private double GetAvgTeamHeight(BindingList< BasketballPlayers> basketballPlayers)
        {
            double num = 0;
            double avgToReturn = 0;
            foreach (var item in basketballPlayers)
            {
                num += item.Height;
            }
            avgToReturn = num / basketballPlayers.Count;
            var avg = Math.Round(avgToReturn, 2);
            return avg;
        }
        private double GetAvgTeamAge(BindingList<BasketballPlayers> basketballPlayers)
        {
            double num = 0;
            double avgToReturn = 0;
            foreach (var item in basketballPlayers)
            {
                num += item.Age;
            }
            avgToReturn = num / basketballPlayers.Count;
            var avg = Math.Round(avgToReturn, 2);
            return avg;
        }
        public void CheckTeamPicture(Teams team)
        {
            if (string.IsNullOrEmpty(team.Logo))
            {
                team.Logo = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\NullPicture.png";
            }
            if(!File.Exists(team.Logo))
            {
                team.Logo = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\NullPicture.png";
            }
        }
        public bool CompleteTeamNullCheck(Teams team)
        {
            bool check = false;
            if(string.IsNullOrEmpty(team.TeamName) || string.IsNullOrEmpty(team.City) || string.IsNullOrEmpty(team.Logo))
            {
                check = true;
            }
            return check;
        }
        protected void OnPropertyChanged(string propertyname = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
