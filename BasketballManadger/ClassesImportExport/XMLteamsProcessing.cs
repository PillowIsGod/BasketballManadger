using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace BasketballManadger
{
    public class XMLTeamsProcessing : FileTypesProcessing
    {
        public XMLTeamsProcessing(string filePath) : base(filePath)
        {

        }

        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            throw new NotImplementedException();
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(FileProcessingPath);
            XmlElement root = xdoc.DocumentElement;
            BindingList<Teams> teamsToReturn = new BindingList<Teams>();

            foreach (XmlNode item in root)
            {
                Teams team = new Teams();
                foreach (XmlNode node in item.ChildNodes)
                {
                   
                    if (node.Name == "teamName")
                    {
                        team.TeamName = node.InnerText;
                    }
                    if (node.Name == "city")
                    {
                        team.City = node.InnerText;
                    }
                    if (node.Name == "logo")
                    {
                        team.Logo = node.InnerText;
                    }

                    
                }
                teamsToReturn.Add(team);
            }
            return teamsToReturn;
        }

        public override void ImportPlayersData(params BasketballPlayers[] playersToImport)
        {
            throw new NotImplementedException();
        }

        public override void ImportTeamData(params Teams [] teamsToImport)
        {
            XmlDocument xdoc = new XmlDocument();
            XmlElement xmlTeam = xdoc.CreateElement("teams");
            xdoc.AppendChild(xmlTeam);
            foreach (var item in teamsToImport)
            {
                var xmlTeamGot = GetXMLTeam(item, xdoc);
                xmlTeam.AppendChild(xmlTeamGot);
            }
            xdoc.Save(FileProcessingPath);
        }

        public XmlElement GetXMLTeam(Teams team, XmlDocument xdoc)
        {
            XmlElement xmlTeam = xdoc.CreateElement("team");


            XmlElement cityElem = xdoc.CreateElement("city");
            xmlTeam.AppendChild(cityElem);
            XmlElement nameElem = xdoc.CreateElement("teamName");
            xmlTeam.AppendChild(nameElem);

            XmlElement logoElem = xdoc.CreateElement("logo");
            xmlTeam.AppendChild(logoElem);
          


            XmlText cityText = xdoc.CreateTextNode(team.City);
            XmlText nameText = xdoc.CreateTextNode(team.TeamName);
            XmlText logoText = xdoc.CreateTextNode(team.Logo);

            cityElem.AppendChild(cityText);
            nameElem.AppendChild(nameText);
            logoElem.AppendChild(logoText);

            return xmlTeam;
        }
    }
}
