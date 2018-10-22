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
using Microsoft.Extensions.Logging;

namespace openstigapi.Controllers
{
    [Route("api/[controller]")]
    public class ChecklistController : Controller
    {
	    private readonly IDistributedCache _cache;
         
        private readonly ILogger<ExamplesController> _logger;

        public ChecklistController(IDistributedCache cache, ILogger<ExamplesController> logger)
        {
            _cache = cache;
            LoadInitialChecklist();
            _logger = logger;
        }

        // GET api/checklist
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Artifact> listing = new List<Artifact>();
            string lstChecklist = await _cache.GetStringAsync("stigListing");
            if (!string.IsNullOrEmpty(lstChecklist)) {
                listing = JsonConvert.DeserializeObject<List<Artifact>>(lstChecklist);
                _logger.LogInformation("/checklist: STIG artifact listing is not empty so returning the list.");
            }
            return Json(listing);
        }

        // GET api/checklist/33434343-3333-3333-3333-919191919191
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation("/checklist/" + id.ToString()+ ": Getting STIG artifact.");
            var checklist = await _cache.GetStringAsync(id.ToString());
            return Json(checklist);
        }

        /// <summary>
        /// POST api/checklist/
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Artifact value)
        {
            value.id = Guid.NewGuid();
            _logger.LogInformation("/checklist/: POSTing a new STIG artifact.");
            await _cache.SetStringAsync(value.id.ToString(),JsonConvert.SerializeObject(value));
            _logger.LogInformation("/checklist/: POSTing STIG artifact successful.");
            return CreatedAtRoute(new {id = value.id}, value);
        }

        private void LoadInitialChecklist()
        {
            if (string.IsNullOrEmpty(_cache.GetString("dbfbf6a2-929a-4c13-ab00-d5266107b9f2"))){
                _logger.LogInformation("/checklist/: Loading initial checklist.");
                Artifact initial = new Artifact();
                initial.created = DateTime.Now;
                initial.filePath = "/dbfbf6a2-929a-4c13-ab00-d5266107b9f2/asd-real-world.ckl";
                initial.id = Guid.Parse("dbfbf6a2-929a-4c13-ab00-d5266107b9f2");
                initial.title = "Initial example ASD STIG Checklist";
                initial.type = STIGtype.ASD;

                _cache.SetString(initial.id.ToString(),JsonConvert.SerializeObject(initial));
                List<Artifact> listing = new List<Artifact>();
                listing.Add(initial);
                _cache.SetString("stigListing",JsonConvert.SerializeObject(listing));
            }
        }
    }
}
