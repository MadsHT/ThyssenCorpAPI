using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ThyssenCorpAPI.Helpers;

namespace ThyssenCorpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FortniteController : ControllerBase
    {
        private FortniteStatsHelper FSH = new FortniteStatsHelper();

        // GET api/fortnite/stats/username
        [HttpGet]
        [Route("stats/{username}")]
        public ActionResult<JToken> Get(string username)
        {
            return FSH.GetPlayerStatsFromUsername(username).Result;
        }

        [HttpGet]
        [Route("compare/{inputString}")]
        public ActionResult<JArray> GetStatsCompare(string inputString)
        {
            string[] inputStringSplit = inputString.Split('-');

            string username = inputStringSplit[1];
            string compareTo = inputStringSplit[0];
            string username2 = "IAmCBJ";


            if (inputStringSplit.Length > 2)
            {
                username2 = inputStringSplit[2];
            }

            return FSH.GetPlayerComparedStats(compareTo, username, username2);
        }

        [HttpGet]
        [Route("soloStats/{inputString}")]
        public ActionResult<JArray> GetSoloStats(string inputString)
        {
            string[] inputStringSplit = inputString.Split('-');

            string username = "IAmCBJ";
            
            string compareTo = inputStringSplit[0];

            if (inputStringSplit.Length > 1)
            {
                username = inputStringSplit[1];
            }
            
            return FSH.GetSoloStat(compareTo, username);
        }
    }
}