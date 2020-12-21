using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public string FullTeamName { get
            {
                return this.City + " " + this.TeamName;
            } }


        public double AvgTeamHeight { get
            {
                return GetAvgTeamHeight(this.BasketballPlayers);
            } }
        public double AvgTeamAge { get
            {
                return GetAvgTeamAge(this.BasketballPlayers);
            }
            }
        
        public int AmountOfTeamMembers { get {
                return this.BasketballPlayers.Count;
            } }
        public Teams()
        {
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
                team.Logo = @"C:\Users\Zhenya\source\repos\BasketballManadger\BasketballManadger\Logos\NullPicture.png";
            }
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
