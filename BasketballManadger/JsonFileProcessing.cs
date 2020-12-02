using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace BasketballManadger
{
    public class JsonFileProcessing
    {
        public readonly string JsonPath;
        public JsonFileProcessing(string jsonPath)
        {
            JsonPath = jsonPath;
        }
        
        private BindingList<T> GetDataArray<T>() where T : new()
        {
            BindingList<T> result = new BindingList<T>();


            string content = File.ReadAllText(JsonPath);

                if (string.IsNullOrEmpty(content))
            {
                return result;
            }
            

            var text = JsonConvert.DeserializeObject<JsonRootModel>(content);


            if(typeof(T) == typeof(BasketballPlayers))
            {
                foreach (var item in text.Players)
                {
                    var tType = (T)Convert.ChangeType(item, typeof(BasketballPlayers));
                    result.Add(tType);
                }

            }
            if (typeof(T) == typeof(Teams))
            {
                foreach (var item in text.Teams)
                {
                    var tType = (T)Convert.ChangeType(item, typeof(Teams));
                    result.Add(tType);
                }
            }

            if (typeof(T) == typeof(Positions))
            {
                foreach (var item in text.Positions)
                {
                    var tType = (T)Convert.ChangeType(item, typeof(Positions));
                    result.Add(tType);
                }

            }
            return result;
        }


        public BindingList<BasketballPlayers> GetBasketballPlayers()
        {
            
            var result = GetDataArray<BasketballPlayers>();

            return result;

        }
        public BindingList<Teams> GetTeams()
        {
            var result = GetDataArray<Teams>();

            return result;

        }
        public BindingList<Positions> GetPositions()
        {
            var result = GetDataArray<Positions>();

            return result;
        }
        //public void SaveData(BindingList<Teams> listTosave)
        //{
        //    var str = JObject.FromObject(new { Teams = listTosave, Players = listPlayers, Positions = listPositions}).ToString();
        //    using (StreamWriter sw = File.CreateText(JsonPath))
        //    {
        //        string output = JsonConvert.SerializeObject(listTosave);
        //        sw.Write(output);
        //    }
        //}

    }
}
