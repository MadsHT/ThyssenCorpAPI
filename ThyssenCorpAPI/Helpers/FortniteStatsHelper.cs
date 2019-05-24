using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<JToken> GetPlayerStatsFromUID(string username)
        {
            var uid = GetUIdFromUsername(username).Result;
            JObject jo = null;
            HttpResponseMessage response = await http.GetAsync(StatsPath + uid);
            if (response.IsSuccessStatusCode)
            {
                jo = JObject.Parse(await response.Content.ReadAsStringAsync());
            }
            else
            {
                jo = new JObject("There was a problem with getting " + username);
            }

            return jo;
        }

        public async Task<JArray> GetPlayerComparedStats(string compareTO, string username, string username2)
        {
            JObject user1Token = null;
            JObject user2Token = null;
            string uid2 = null;

            var uid = GetUIdFromUsername(username).Result;
            if (uid.Contains("problem"))
            {
                return new JArray(new JObject(uid));
            }

            HttpResponseMessage response = await http.GetAsync(StatsPath + uid);
            if (response.IsSuccessStatusCode)
            {
                user1Token = JObject.Parse(await response.Content.ReadAsStringAsync());
            }
            else
            {
                return new JArray(response.Content);
            }

            if (username2 != String.Empty)
            {
                uid2 = GetUIdFromUsername(username2).Result;
                if (uid.Contains("problem"))
                {
                    return new JArray(new JObject(uid));
                }
                
                response = await http.GetAsync(StatsPath + uid2);
                if (response.IsSuccessStatusCode)
                {
                    user2Token = JObject.Parse(await response.Content.ReadAsStringAsync());
                }
            }
            else
            {
                return new JArray(response.Content);
            }

            return new JArray(user1Token, user2Token);
        }
    }
}