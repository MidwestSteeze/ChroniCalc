using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    class SampleTree
    {
        //[JsonProperty("name")]
        public string name;

        //[JsonProperty("skill")]
        public SampleSkill skill;
    }
}
