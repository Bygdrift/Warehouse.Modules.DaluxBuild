using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Module.Services.Models
{
    public class Project
    {
        public string Address { get; set; }
        public string BoxType { get; set; }
        //public object ClosingDateTime { get; set; }  //Not relevant. Will always be null because closed projects are not visible
        public DateTime CreationDateTime { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public GenericId ProjectID { get; set; }
        //public string Status { get; set; }  Irelevant because you only can acces active projects
        public Projectmetadata[] ProjectMetaData { get; set; }
    }

    public class Projectmetadata
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueDescription { get; set; }
    }

    public class ProjectShort
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public GenericId ProjectID { get; set; }
    }
}