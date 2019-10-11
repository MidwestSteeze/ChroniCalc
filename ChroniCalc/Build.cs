using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    [Serializable]
    public class Build
    {
        public int level;

        public int masteryLevel;

        //[JsonProperty("name")]
        public string name;

        public CharacterClass characterClass;

        public Build()
        {

        }

        public Build(string aName, CharacterClass aCharacterClass, int aLevel, int aMasteryLevel)
        {
            level = aLevel;
            masteryLevel = aMasteryLevel;
            name = aName;
            characterClass = aCharacterClass;
        }
    }
}
