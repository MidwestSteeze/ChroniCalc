using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChroniCalc
{
    [Serializable]
    public class Skill
    {
        private string _description;
        private string _element;
        private string _family;
        private string _name;
        private string _type;

        // START Skill Json Properties

        [XmlIgnoreAttribute]
        [JsonProperty("effect")]
        public double[] effect;

        [XmlIgnoreAttribute]
        [JsonProperty("cooldown")]
        public int cooldown;

        [XmlIgnoreAttribute]
        [JsonProperty("duration")]
        public double[] duration;

        [XmlIgnoreAttribute]
        [JsonProperty("cost100")]
        public int cost100;

        //The id of other skills that are pre-reqs in order to allocate this one
        [XmlIgnoreAttribute]
        [JsonProperty("skill_requirement")]
        public int[] skill_requirement;

        //The horizontal position on the tree
        [JsonProperty("x")]
        public int x;

        //The multiplier increase/decrease
        [XmlIgnoreAttribute]
        [JsonProperty("damage")]
        public double[] damage;

        [XmlIgnoreAttribute]
        [JsonProperty("range2")]
        public double[] range2;

        //For creating links between skills (ie. Passive A affects all X family type skills)
        [XmlIgnoreAttribute]
        [JsonProperty("family")]
        public string family //TODOSSG change to Enum?
        {
            get
            {
                return _family;
            }
            set
            {
                _family = value;
            }
        }

        [XmlIgnoreAttribute]
        [JsonProperty("min_level")]
        public int min_level;

        //The id used for determining pre-req skills
        [JsonProperty("id")]
        public int id;

        [XmlIgnoreAttribute]
        [JsonProperty("range")]
        public double[] range;

        //The damage type (ie. Fire, Cold, Lightning, etc)
        [XmlIgnoreAttribute]
        [JsonProperty("element")]
        public string element //TODOSSG change to Enum type
        {
            get
            {
                return _element;
            }
            set
            {
                _element = value;
            }
        }

        [XmlIgnoreAttribute]
        [JsonProperty("value")]
        public double[] value;

        [XmlIgnoreAttribute]
        [JsonProperty("proc")]
        public int[] proc;

        //The status (ie. Active or Passive)
        [XmlIgnoreAttribute]
        [JsonProperty("type")]
        public string type //TODOSSG change to Enum
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        [XmlIgnoreAttribute]
        [JsonProperty("description_next")]
        public string description_next; // TODOSSG enable this is a property so it can be searched within the Tree search function? Is it used in game anywhere though, that it'd be applicable for a search?

        //The full description
        [XmlIgnoreAttribute]
        [JsonProperty("description")]
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        //The max rank alloted
        [XmlIgnoreAttribute]
        [JsonProperty("max_rank")]
        public int max_rank;

        //The vertical position on the tree
        [JsonProperty("y")]
        public int y;

        [XmlIgnoreAttribute]
        [JsonProperty("cost1")]
        public int cost1;

        //The name as seen on the tree
        [XmlIgnoreAttribute]
        [JsonProperty("name")]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        // END Skill Json Properties

        [JsonProperty("level")]
        public int level;

        // Used to identify the location in the Mastery Tree for .build imports/exports since x,y isn't being used by squarebit
        [JsonProperty("slotID")]
        public int slotID;

        public Skill Duplicate()
        {
            return (Skill)this.MemberwiseClone();
        }
    }
}
