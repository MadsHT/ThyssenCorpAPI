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
        
        public async Task<String> GetUIdFromUsername(string name)
        {
            JObject jo = null;
            
            HttpResponseMessage response = await http.GetAsync(UIDpath + name);
            if (response.IsSuccessStatusCode)
            {
             jo = JObject.Parse(await response.Content.ReadAsStringAsync());
            }
            return jo["uid"].ToString();
        }
    }
}