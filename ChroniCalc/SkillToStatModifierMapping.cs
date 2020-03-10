using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    public class SkillToStatModifierMapping
    {
        public enum SkillProperties
        {
            COOLDOWN,
            DAMAGE,
            DURATION,
            EFFECT,
            PROC,
            VALUE
        }

        public string CharacterClass;
        public string Tree;
        public string Skill;
        public string Name;
        public SkillProperties SkillPropertyToPullValueFrom;

        public SkillToStatModifierMapping(string inCharacterClass, string inTree, string inSkill, string inModifierName, SkillProperties inSkillProperty)
        {
            this.CharacterClass = inCharacterClass;
            this.Tree = inTree;
            this.Skill = inSkill;
            this.Name = inModifierName;
            this.SkillPropertyToPullValueFrom = inSkillProperty;
        }
    }
}