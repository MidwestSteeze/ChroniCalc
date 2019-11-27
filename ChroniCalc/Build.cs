using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ChroniCalc
{
    [Serializable]
    public class Build
    {
        public enum BuildStatus
        {
            None,
            Opening,
            Saved
        }

        private int _level;
        private int _masteryLevel;

        [XmlIgnoreAttribute]
        public BuildStatus buildStatus;

        public CharacterClass characterClass;
        public DateTime lastModified;
        public DateTime lastSaved;
        public int Level { get { return _level; } set { _level = value; SetAsModified(); } }
        public int MasteryLevel { get { return _masteryLevel; } set { _masteryLevel = value; SetAsModified(); } }
        public string ApplicationVersion;
        //[JsonProperty("name")]
        public string name;

        public Build()
        {
            // Blank constructors are for deserialization into objects; so default buildStatus to Opening since that's what we'll be doing when using this constructor
            buildStatus = BuildStatus.Opening;
        }

        public Build(string aName, CharacterClass aCharacterClass, int aLevel, int aMasteryLevel)
        {
            ApplicationVersion = Application.ProductVersion;
            Level = aLevel;
            MasteryLevel = aMasteryLevel;
            name = aName;
            characterClass = aCharacterClass;
        }

        public bool IsModified()
        {
            if (lastModified > lastSaved && !(characterClass is null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetAsModified()
        {
            // Don't flag the Build as modified if we're opening one (because loading up skill buttons and assigning their level will trigger this procedure)
            if (buildStatus != BuildStatus.Opening)
            {
                this.lastModified = DateTime.Now;
            }
        }
    }
}
