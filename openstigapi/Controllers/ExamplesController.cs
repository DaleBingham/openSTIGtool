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
using Microsoft.Extensions.Logging;

namespace openstigapi.Controllers
{
    [Route("api/[controller]")]
    public class ExamplesController : Controller
    {
        const string exampleSTIG = "/examples/asd-example.ckl";
        private readonly ILogger<ExamplesController> _logger;

        public ExamplesController(ILogger<ExamplesController> logger)
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public string Get()
        {
            // open the web path/examples/ckl file
            string filename = Directory.GetCurrentDirectory() + exampleSTIG;
            string checklistXML = string.Empty;
            string returnedXML = string.Empty;
            
            if (System.IO.File.Exists(filename)) {
                CHECKLIST asdChecklist = new CHECKLIST();
                _logger.LogInformation("/example/: Example file active so returning an example ASD STIG.");

                // put that into a class and deserialize that
                asdChecklist = ChecklistLoader.LoadASDChecklist(filename);
                XmlSerializer serializer = new XmlSerializer(typeof(CHECKLIST));
                _logger.LogInformation("Serialized ASD example checklist");

                // serialize into a string to return
                using(var sww = new StringWriter())
                {
                    using(XmlWriter writer = XmlWriter.Create(sww))
                    {
                        serializer.Serialize(writer, asdChecklist);
                        _logger.LogInformation("/example/: Returning XML string of ASD example checklist");
                        returnedXML = sww.ToString(); // Your XML
                    }
                }
            }

            return returnedXML;
        }

    }
}
