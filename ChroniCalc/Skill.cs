using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    public class Skill
    {
        // START Skill Json Properties

        [JsonProperty("effect")]
        public string[] effect;

        [JsonProperty("cooldown")]
        public int cooldown;

        [JsonProperty("duration")]
        public double[] duration;

        [JsonProperty("cost100")]
        public int cost100;

        //The id of other skills that are pre-reqs in order to allocate this one
        [JsonProperty("skill_requirement")]
        public int[] skill_requirement;

        //The horizontal position on the tree
        [JsonProperty("x")]
        public int x;

        //The multiplier increase/decrease
        [JsonProperty("damage")]
        public string[] damage;

        [JsonProperty("range2")]
        public double[] range2;

        //For creating links between skills (ie. Passive A affects all X family type skills)
        [JsonProperty("family")]
        public string family; //TODOSSG change to Enum?

        [JsonProperty("min_level")]
        public int min_level;

        //The id used for determining pre-req skills
        [JsonProperty("id")]
        public int id;

        [JsonProperty("range")]
        public double[] range;

        //The damage type (ie. Fire, Cold, Lightning, etc)
        [JsonProperty("element")]
        public string element; //TODOSSG change to Enum type

        [JsonProperty("value")]
        public double[] value;

        [JsonProperty("proc")]
        public int[] proc;

        //The status (ie. Active or Passive)
        [JsonProperty("type")]
        public string type; //TODOSSG change to Enum

        [JsonProperty("description_next")]
        public string description_next;

        //The full description
        [JsonProperty("description")]
        public string description;

        //The max rank alloted
        [JsonProperty("max_rank")]
        public int max_rank;

        //The vertical position on the tree
        [JsonProperty("y")]
        public int y;

        [JsonProperty("cost1")]
        public int cost1;

        //The name as seen on the tree
        [JsonProperty("name")]
        public string name;

        // END Skill Json Properties

        [JsonProperty("level")]
        public int level;

        public Skill Duplicate()
        {
            return (Skill)this.MemberwiseClone();
        }
    }
}
