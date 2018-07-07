﻿using System;
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
    public class ExamplesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {
            // open the web path/examples/ckl file
            string filename = Directory.GetCurrentDirectory() + "\\examples\\asd-example.xml";
            string checklistXML = string.Empty;
            CHECKLIST asdChecklist = new CHECKLIST();

            // put that into a class and deserialize that
            if (System.IO.File.Exists(filename)) {
                 checklistXML = System.IO.File.ReadAllText(filename);
            }
            XmlSerializer serializer = new XmlSerializer(typeof(CHECKLIST));
            StreamReader reader = new StreamReader(filename);
            asdChecklist = (CHECKLIST)serializer.Deserialize(reader);
            reader.Close();
            // serialize into a string to return
            return checklistXML;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

    }
}
