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
        public Teams()
        {
            
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
