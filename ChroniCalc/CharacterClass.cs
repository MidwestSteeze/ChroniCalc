using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    [Serializable]
    public class CharacterClass
    {
        [JsonProperty("name")]
        public string name;

        [JsonProperty("tree")]
        public List<Tree> trees;

        public CharacterClass()
        {

        }

        public CharacterClass(string aName, List<Tree> aTrees)
        {
            name = aName;
            trees = aTrees;
        }
    }
}
