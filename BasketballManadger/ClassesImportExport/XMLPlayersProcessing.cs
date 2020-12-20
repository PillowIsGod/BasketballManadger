using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace BasketballManadger
{
    public class XMLPlayersProcessing : FileTypesProcessing
    {
        public XMLPlayersProcessing(string filePath) : base(filePath)
        {

        }

        public override BindingList<BasketballPlayers> GetPlayersFromFile()
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(FileProcessingPath);
            XmlElement root = xdoc.DocumentElement;
            BindingList<BasketballPlayers> playerToReturn = new BindingList<BasketballPlayers>();

            foreach (XmlNode item in root)
            {
                BasketballPlayers player = new BasketballPlayers();
                foreach (XmlNode node in item.ChildNodes)
                {
                    
                    if (node.Name == "name")
                    {
                        player.Name = node.InnerText;
                    }
                    if (node.Name == "position")
                    {
                        player.Position = node.InnerText;
                    }
                    if (node.Name == "picture")
                    {
                        player.Picture = node.InnerText;
                    }
                    if (node.Name == "current_team")
                    {
                        player.Current_team = node.InnerText;
                    }
                    if (node.Name == "age")
                    { player.Age = Convert.ToInt32(node.InnerText); }
                    if (node.Name == "career_age")
                    { player.Career_age = Convert.ToInt32(node.InnerText); }
                    if (node.Name == "height")
                    { player.Height = Convert.ToDouble(node.InnerText); }
                    if (node.Name == "weight")
                    { player.Weight = Convert.ToInt32(node.InnerText); }



                   
                }
                playerToReturn.Add(player);
            }
            return playerToReturn;
        }

        public override BindingList<Teams> GetTeamFromFIle()
        {
            throw new NotImplementedException();
        }

        public override void ImportPlayersData(BindingList<BasketballPlayers> playersToImport)
        {
            XmlDocument xdoc = new XmlDocument();
            XmlElement xmlPlayer = xdoc.CreateElement("players");
            xdoc.AppendChild(xmlPlayer);
            foreach (var item in playersToImport)
            {
                var xmlPlayerGot = GetXMLPlayer(item, xdoc);
                xmlPlayer.AppendChild(xmlPlayerGot);
            }
            xdoc.Save(FileProcessingPath);
        }

        public override void ImportTeamData(BindingList<Teams> teamsToImport)
        {
            throw new NotImplementedException();
        }

        public XmlElement GetXMLPlayer(BasketballPlayers player, XmlDocument xdoc)
        {
            XmlElement xmlPlayer = xdoc.CreateElement("player");

            XmlElement positionElem = xdoc.CreateElement("position");
            xmlPlayer.AppendChild(positionElem);

            XmlElement nameElem = xdoc.CreateElement("teamName");
            xmlPlayer.AppendChild(nameElem);

            XmlElement teamElem = xdoc.CreateElement("current_team");
            xmlPlayer.AppendChild(teamElem);

            XmlElement ageElem = xdoc.CreateElement("age");
            xmlPlayer.AppendChild(ageElem);

            XmlElement careerElem = xdoc.CreateElement("career_age");
            xmlPlayer.AppendChild(careerElem);

            XmlElement heightElem = xdoc.CreateElement("height");
            xmlPlayer.AppendChild(heightElem);

            XmlElement weightElem = xdoc.CreateElement("weight");
            xmlPlayer.AppendChild(weightElem);

            XmlElement pictureElem = xdoc.CreateElement("picture");
            xmlPlayer.AppendChild(pictureElem);


            XmlText positionText = xdoc.CreateTextNode(player.Position);
            XmlText nameText = xdoc.CreateTextNode(player.Name);
            XmlText teamText = xdoc.CreateTextNode(player.Current_team);
            XmlText ageText = xdoc.CreateTextNode(player.Age.ToString());
            XmlText careerText = xdoc.CreateTextNode(player.Career_age.ToString());
            XmlText heightText = xdoc.CreateTextNode(player.Height.ToString());
            XmlText weightText = xdoc.CreateTextNode(player.Weight.ToString());
            XmlText pictureText = xdoc.CreateTextNode(player.Picture);


            positionElem.AppendChild(positionText);
            nameElem.AppendChild(nameText);
            teamElem.AppendChild(teamText);
            ageElem.AppendChild(ageText);
            careerElem.AppendChild(careerText);
            heightElem.AppendChild(heightText);
            weightElem.AppendChild(weightText);
            pictureElem.AppendChild(pictureText);

            return xmlPlayer;
        }
    }
}
