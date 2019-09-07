using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    class SampleCharacterClass
    {
        //[JsonProperty("name")]
        public string name;

        //[JsonProperty("tree")]
        public SampleTree tree;
    }
}
