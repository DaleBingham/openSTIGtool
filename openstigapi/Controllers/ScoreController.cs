using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using openstigapi.Models;
using System.IO;
using System.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace openstigapi.Controllers
{
    [Route("api/[controller]")]
    public class ScoreController : Controller
    {
        const string exampleSTIG = "\\examples\\asd-example.ckl";

	    private readonly IDistributedCache  _cache;
 
		// _distributedCache.GetString(cacheKey);
		// _distributedCache.SetString(cacheKey, existingTime);
        
        public ScoreController(IDistributedCache  cache)
        {
            _cache = cache;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // open the web path/examples/ckl file
            string filename = Directory.GetCurrentDirectory() + exampleSTIG;
            Score cklScore = new Score();

            CHECKLIST asdChecklist = new CHECKLIST();

            XmlSerializer serializer = new XmlSerializer(typeof(CHECKLIST));
            StreamReader reader = new StreamReader(filename);
            asdChecklist = (CHECKLIST)serializer.Deserialize(reader);
            reader.Close();
            
            // now see what score you can get
            if (asdChecklist.Items.Length == 2 && asdChecklist.Items[1] != null) {
                CHECKLISTSTIGS objSTIG = (CHECKLISTSTIGS)asdChecklist.Items[1];
                CHECKLISTSTIGSISTIG[] iSTIG = objSTIG.iSTIG;
                if (iSTIG.Length == 1 && iSTIG[0] != null){
                    CHECKLISTSTIGSISTIG asdSTIG = (CHECKLISTSTIGSISTIG)iSTIG[0];
                    if (asdSTIG.VULN != null && asdSTIG.VULN.Length > 0){
                        CHECKLISTSTIGSISTIGVULN[] asdVulnerabilities = asdSTIG.VULN;
                        cklScore.NotReviewed = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "not_reviewed").Count();
                        cklScore.NotApplicable = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "not_applicable").Count();
                        cklScore.Open = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "open").Count();
                        cklScore.NotAFinding = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "notafinding").Count();
                    }
                }
            }
            return Json(cklScore);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

    }
}
