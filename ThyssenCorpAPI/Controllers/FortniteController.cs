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

        // GET api/fortnite/username
        [HttpGet("/stats/{username}")]
        public ActionResult<JToken> Get(string username)
        {
            return FSH.GetPlayerStatsFromUID(username).Result;
        }

        [HttpGet()]
        [Route("/compare/{username}")]
        public ActionResult<JToken> Get(String username = "IAmCBJ", [FromQuery] string username2 = "")
        {
            return FSH.GetPlayerComparedStats(username, username2).Result;
        }
    }
}