using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    public static class SkillToStatModifierMappings
    {
        //CLASS Constants
        private const string CLASS_BERSERKER    = "Berserker";
        private const string CLASS_TEMPLARR     = "Templar";
        private const string CLASS_WARDEN       = "Warden";
        private const string CLASS_WARLOCK      = "Warlock";

        //TREE Constants
        private const string TREE_GUARDIAN = "Guardian";

        //SKILL PROPERTY Constants
        private const string SKILL_COOLDOWN = "COOLDOWN";
        private const string SKILL_DAMAGE = "DAMAGE";
        private const string SKILL_DURATION = "DURATION";
        private const string SKILL_EFFECT = "EFFECT";
        private const string SKILL_PROC = "PROC";
        private const string SKILL_VALUE = "VALUE";

        public static readonly List<SkillToStatModifierMapping> AllSkillToStatModifierMappings = new List<SkillToStatModifierMapping>()
        {
            //BERSERKER //TODO RESUME AND FINISH
            new SkillToStatModifierMapping(CLASS_BERSERKER, TREE_GUARDIAN, "Claw", StatModifiers.DAMAGE_PHYSICAL, SkillToStatModifierMapping.SkillProperties.DAMAGE)

            //TEMPLAR

            //WARDEN

            //WARLOCK
        };
    }
}
