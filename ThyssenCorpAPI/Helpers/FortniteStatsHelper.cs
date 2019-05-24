using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace ThyssenCorpAPI.Helpers
{
    public class FortniteStatsHelper
    {
        private static HttpClient http = new HttpClient();
        private static string UIDpath = "https://fortnite-public-api.theapinetwork.com/prod09/users/id?username=";

        private static string StatsPath =
            "https://fortnite-public-api.theapinetwork.com/prod09/users/public/br_stats_v2?user_id=";

        private async Task<String> GetUIdFromUsername(string name)
        {
            JObject jo = null;

            HttpResponseMessage response = await http.GetAsync(UIDpath + name);
            if (response.IsSuccessStatusCode)
            {
                jo = JObject.Parse(await response.Content.ReadAsStringAsync());
            }

            return jo["uid"].ToString();
        }

        public async Task<JToken> GetPlayerStatsFromUsername(string username)
        {
            var uid = GetUIdFromUsername(username).Result;
            JObject jo = null;
            HttpResponseMessage response = await http.GetAsync(StatsPath + uid);
            if (response.IsSuccessStatusCode)
            {
                jo = JObject.Parse(await response.Content.ReadAsStringAsync());
            }

            return jo;
        }

        public JArray GetSoloStat(string compareTo, string username)
        {
            JArray returnList = new JArray();

            compareTo = CompareTo(compareTo);

            var userStats = GetPlayerStatsFromUsername(username).Result;

            var controllers = userStats["devices"];

            foreach (JToken controller in controllers)
            {
                JToken data = null;

                if (userStats["data"][controller.ToString()]["defaultsolo"] != null)
                {
                    data = userStats["data"][controller.ToString()]["defaultsolo"]["default"][compareTo];
                }

                var soloStat = new JObject();
                soloStat.Add(controller.ToString(),
                    data ?? $"{username} has no kills with {ConvertController(controller.ToString())}");
                returnList.Add(soloStat);
            }

            return returnList;
        }

        public JArray GetPlayerComparedStats(string compareTo, string username, string username2)
        {
            compareTo = CompareTo(compareTo);

            
            
            JArray returnList = new JArray();
            ArrayList usernames = new ArrayList();
            usernames.Add(username);
            usernames.Add(username2);

            foreach (string name in usernames)
            {
                JToken userToken = GetPlayerStatsFromUsername(name).Result;

                JObject comparedJObject = new JObject();

                var comparedToken = userToken["overallData"]["defaultModes"][compareTo];

                JObject nameAndFilter = new JObject();
                nameAndFilter.Add("name", name);
                nameAndFilter.Add("filter", compareTo);
                
                returnList.Add(nameAndFilter);
                
                comparedJObject.Add("result",
                    comparedToken ?? $"There was a problem getting the {compareTo} from {name}");

                returnList.Add(comparedJObject);
            }

            return returnList;
        }

        private static string ConvertController(string controller)
        {
            switch (controller)
            {
                case "touch":
                    controller = "a phone or the switch";
                    break;
                case "gamepad":
                    controller = "a game controller";
                    break;
                case "keyboardmouse":
                    controller = "keyboard and mouse";
                    break;
            }

            return controller;
        }

        private static string CompareTo(string compareTo)
        {
            switch (compareTo)
            {
                case "wins":
                    compareTo = "placetop1";
                    break;
                case "matches":
                    compareTo = "matchesplayed";
                    break;
            }

            return compareTo;
        }
    }
}