using System;
using System.Collections.Generic;

namespace openstigapi.Models
{
    [Serializable]
    public class Artifact
    {
        public Artifact () {
            id= Guid.NewGuid();
            Checklist = new CHECKLIST();
        }

        public DateTime created { get; set; }
        public string title { get; set; }
        public CHECKLIST Checklist { get; set; }
        public Guid id { get; set; }
        public string filePath { get; set; }
        public STIGtype type { get; set; }
    }

    public enum STIGtype{
        ASD = 0,
        DBInstance = 10,
        DBServer = 20,
        DOTNET = 30,
        JAVA = 40
    }

}