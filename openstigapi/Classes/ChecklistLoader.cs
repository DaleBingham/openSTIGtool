using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using openstigapi.Models;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace openstigapi.Classes
{
    public static class ChecklistLoader
    {        
        public static CHECKLIST LoadASDChecklist(string filepath) {
            CHECKLIST asdChecklist = new CHECKLIST();
            if (System.IO.File.Exists(filepath)) {
                XmlSerializer serializer = new XmlSerializer(typeof(CHECKLIST));
                StreamReader reader = new StreamReader(filepath);
                asdChecklist = (CHECKLIST)serializer.Deserialize(reader);
                reader.Close();
            }
            return asdChecklist;
        }
    }
}