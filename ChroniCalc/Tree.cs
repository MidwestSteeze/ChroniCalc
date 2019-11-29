﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChroniCalc
{
    [Serializable]
    public class Tree
    {
        [XmlIgnoreAttribute]
        //[JsonProperty("level")]
        public int level;

        public string name;

        public List<Skill> skills;
    }
}
