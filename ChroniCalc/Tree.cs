using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    class Tree
    {
        //[JsonProperty("inside")]
        public string name;

        //[JsonProperty("again")]
        public List<Skill> skills;
    }
}
