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
        [HttpGet("{username}")]
        public ActionResult<JToken> Get(string username)
        {
            Console.WriteLine(FSH.GetUIdFromUsername(username).Result["uid"]);
            return FSH.GetUIdFromUsername(username).Result;
        }

        // GET api/fortnite/uid/username
        [HttpGet()]
        public ActionResult<string> Get()
        {
            return "hellow";
        }
    }
}