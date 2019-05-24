using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
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

        public async Task<JToken> GetPlayerStatsFromUID(string username)
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

        public async Task<JToken> GetPlayerComparedStats(string username, string username2)
        {
            JObject jo = null;
            string uid2 = null;

            if (username2 != "")
            {
                uid2 = GetUIdFromUsername(username2).Result;
            }

            var uid = GetUIdFromUsername(username).Result;

            HttpResponseMessage response = await http.GetAsync(StatsPath + uid);
            if (response.IsSuccessStatusCode)
            {
                jo = JObject.Parse(await response.Content.ReadAsStringAsync());
            }

            response = await http.GetAsync(StatsPath + uid2);
            if (response.IsSuccessStatusCode)
            {
                jo.Add(JObject.Parse(await response.Content.ReadAsStringAsync()));
            }

            return jo;
        }
    }
}