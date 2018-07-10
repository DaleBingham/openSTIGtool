using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using openstigapi.Classes;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Score cklScore = new Score();
            string checklist = await _cache.GetStringAsync(id.ToString());
            if (!string.IsNullOrEmpty(checklist)) {
                Artifact asdSTIGChecklist = JsonConvert.DeserializeObject<Artifact>(checklist);
                if (asdSTIGChecklist.Checklist == null || asdSTIGChecklist.Checklist.Items == null){
                    // load the checklist
                    asdSTIGChecklist.Checklist = ChecklistLoader.LoadASDChecklist(Directory.GetCurrentDirectory() + 
                        "\\wwwroot\\data" + asdSTIGChecklist.filePath);
                        // save it to the cache for next time           
                    _cache.SetString(asdSTIGChecklist.id.ToString(),JsonConvert.SerializeObject(asdSTIGChecklist));
                }
                if (asdSTIGChecklist != null && asdSTIGChecklist.Checklist.Items != null && 
                    asdSTIGChecklist.Checklist.Items.Length == 2 && asdSTIGChecklist.Checklist.Items[1] != null) {
                    // now see what score you can get
                    CHECKLISTSTIGS objSTIG = (CHECKLISTSTIGS)asdSTIGChecklist.Checklist.Items[1];
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
            }
            return Json(cklScore);
        }
    }
}
