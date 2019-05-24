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
        [Route("compare/{compareTo}")]
        public ActionResult<JArray> GetStatsCompare(string compareTo, [FromQuery] String username, [FromQuery] string username2 = "IAmCBJ")
        {
            return FSH.GetPlayerComparedStats(compareTo, username, username2);
        }
        
        [HttpGet]
        [Route("soloStats/{compareTo}")]
        public ActionResult<JArray> GetSoloStats(string compareTo, [FromQuery] String username)
        {
            return FSH.GetSoloStat(compareTo, username);
        }
    }
}