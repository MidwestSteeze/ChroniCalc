using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    class SampleRoot
    {
        [JsonProperty("Class")]
        public string classHeader { get; set; }

        [JsonProperty("Content")]
        public SampleCharacterClass className { get; set; }
    }
}
