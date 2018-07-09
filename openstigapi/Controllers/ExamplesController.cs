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

namespace openstigapi.Controllers
{
    [Route("api/[controller]")]
    public class ExamplesController : Controller
    {
        const string exampleSTIG = "\\examples\\asd-example.ckl";

        // GET api/values
        [HttpGet]
        public string Get()
        {
            // open the web path/examples/ckl file
            string filename = Directory.GetCurrentDirectory() + exampleSTIG;
            string checklistXML = string.Empty;
            string returnedXML = string.Empty;

            CHECKLIST asdChecklist = new CHECKLIST();

            // put that into a class and deserialize that
            asdChecklist = ChecklistLoader.LoadASDChecklist(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(CHECKLIST));

            // serialize into a string to return
            using(var sww = new StringWriter())
            {
                using(XmlWriter writer = XmlWriter.Create(sww))
                {
                    serializer.Serialize(writer, asdChecklist);
                    returnedXML = sww.ToString(); // Your XML
                }
            }

            return returnedXML;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

    }
}
