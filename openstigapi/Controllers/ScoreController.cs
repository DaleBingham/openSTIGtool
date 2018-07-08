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

namespace openstigapi.Controllers
{
    [Route("api/[controller]")]
    public class ScoreController : Controller
    {
            private int notReviewed = 0; 
            private int notApplicable = 0;
            private int open = 0;
            private int notAFinding = 0;

        const string exampleSTIG = "\\examples\\asd-example.ckl";

        // GET api/values
        [HttpGet]
        public string Get()
        {
            // open the web path/examples/ckl file
            string filename = Directory.GetCurrentDirectory() + exampleSTIG;
            string returnedXML = string.Empty;

            CHECKLIST asdChecklist = new CHECKLIST();

            XmlSerializer serializer = new XmlSerializer(typeof(CHECKLIST));
            StreamReader reader = new StreamReader(filename);
            asdChecklist = (CHECKLIST)serializer.Deserialize(reader);
            reader.Close();
            //notReviewed = asdChecklist.Items[1]
            // now see what score you can get
            if (asdChecklist.Items.Length == 2 && asdChecklist.Items[1] != null) {
                CHECKLISTSTIGS objSTIG = (CHECKLISTSTIGS)asdChecklist.Items[1];
                CHECKLISTSTIGSISTIG[] iSTIG = objSTIG.iSTIG;
                if (iSTIG.Length == 1 && iSTIG[0] != null){
                    CHECKLISTSTIGSISTIG asdSTIG = (CHECKLISTSTIGSISTIG)iSTIG[0];
                    if (asdSTIG.VULN != null && asdSTIG.VULN.Length > 0){
                        CHECKLISTSTIGSISTIGVULN[] asdVulnerabilities = asdSTIG.VULN;
                        notReviewed = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "not_reviewed").Count();
                        returnedXML += "Not Reviewed Count: " + notReviewed.ToString() + "<br />";
                        notApplicable = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "not_applicable").Count();
                        returnedXML += "Not Applicable Count: " + notApplicable.ToString() + "<br />";
                        open = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "open").Count();
                        returnedXML += "Open Count: " + open.ToString() + "<br />";
                        notAFinding = asdVulnerabilities.Where(x => x.STATUS.ToLower() == "notafinding").Count();
                        returnedXML += "Not a Finding Count: " + notAFinding.ToString() + "<br />";
                    }
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
