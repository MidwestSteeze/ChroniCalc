using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    public static class StatModifiers
    {
        // List of available Modifiers that will be used as Stat names within ChroniCalc
        // Stats Page 1/5
        public const string HEALTH_FLAT = "HEALTH_FLAT";
        public const string MANA_FLAT = "MANA_FLAT";
        public const string DAMAGE_FLAT = "DAMAGE_FLAT";
        public const string ATTACK_SPEED = "ATTACK_SPEED";
        public const string CRIT_CHANCE = "CRIT_CHANCE";
        public const string CRIT_DAMAGE = "CRIT_DAMAGE";
        public const string OVERPOWER = "OVERPOWER";
        // Damage MULTIPLIERS
        public const string DAMAGE_PHYSICAL = "DAMAGE_PHYSICAL";
        public const string DAMAGE_FIRE = "DAMAGE_FIRE";
        public const string DAMAGE_LIGHTNING = "DAMAGE_LIGHTNING";
        public const string DAMAGE_HOLY = "DAMAGE_HOLY";
        public const string DAMAGE_SWORD = "DAMAGE_SWORD";
        public const string DAMAGE_SHIELD = "DAMAGE_SHIELD";
        public const string DAMAGE_MAGIC = "DAMAGE_MAGIC";
        // Extras/Unlisted Alternatives
        public const string HEALTH_INCREASED = "HEALTH_INCREASED";

        // Stats Page 2/5
        public const string THORNS_FLAT = "THORNS_FLAT";
        public const string REGEN_HEALTH_FLAT = "REGEN_HEALTH_FLAT";
        public const string REGEN_MANA_FLAT = "REGEN_MANA_FLAT";
        public const string HEALTH_ON_HIT = "HEALTH_ON_HIT";
        public const string MANA_ON_HIT = "MANA_ON_HIT";
        public const string EVASION = "EVASION";
        public const string DAMAGE_REDUCTION = "DAMAGE_REDUCTION";

        public const string RESISTANCE_PHYSICAL = "RESISTANCE_PHYSICAL";
        public const string RESISTANCE_FIRE = "RESISTANCE_FIRE";
        public const string RESISTANCE_FROST = "RESISTANCE_FROST";
        public const string RESISTANCE_LIGHTNING = "RESISTANCE_LIGHTNING";
        public const string RESISTANCE_POISON = "RESISTANCE_POISON";
        public const string RESISTANCE_HOLY = "RESISTANCE_HOLY";
        public const string RESISTANCE_SHADOW = "RESISTANCE_SHADOW";
        // Extras/Unlisted Alternatives
        public const string RESISTANCE_ALL = "RESISTANCE_ALL"; // TODO may not need, it's not on the page, would apply the value to all resists individually on Skill level change
        public const string THORNS_INCREASED = "THORNS_INCREASED";

        // Stats Page 3/5
        public const string COOLDOWN_REDUCTION = "COOLDOWN_REDUCTION";
        public const string MANA_COST_REDUCTION = "MANA_COST_REDUCTION";
        public const string EFFECT_DURATION = "EFFECT_DURATION";
        public const string BONUS_CRYSTALS = "BONUS_CRYSTALS";
        public const string MAGIC_FIND = "MAGIC_FIND";
        public const string BONUS_EXPERIENCE = "BONUS_EXPERIENCE";
        public const string POTION_EFFECTIVENESS = "POTION_EFFECTIVENESS";

        public const string REACH = "REACH";
        public const string MOVEMENT_SPEED = "MOVEMENT_SPEED";
        public const string BLOCK_CHANCE = "BLOCK_CHANCE";
        public const string DAMAGE_BLOCKED = "DAMAGE_BLOCKED";
        public const string AURA_EFFECT = "AURA_EFFECT";
        public const string AURA_RANGE = "AURA_RANGE";
        public const string DAMAGE_STAGGERED = "DAMAGE_STAGGERED";

        // Stats Page 4/5
        public const string DAMAGE_BLEEDING_FLAT = "DAMAGE_BLEEDING_FLAT";
        public const string DAMAGE_BURN_FLAT = "DAMAGE_BURN_FLAT";
        public const string DAMAGE_FROST_FLAT = "DAMAGE_FROST_FLAT";
        public const string DAMAGE_POISON_FLAT = "DAMAGE_POISON_FLAT";
        public const string DAMAGE_CHAIN_LIGHTNING_FLAT = "DAMAGE_CHAIN_LIGHTNING_FLAT";
        public const string DAMAGE_SMITE_FLAT = "DAMAGE_SMITE_FLAT";
        public const string DAMAGE_BLIGHT_FLAT = "DAMAGE_BLIGHT_FLAT";

        public const string DAMAGE_BLEED = "DAMAGE_BLEED";
        public const string HEALING_TAKEN = "HEALING_TAKEN";
        public const string WILLPOWER = "WILLPOWER";
        public const string MANA_SHIELD = "MANA_SHIELD";
        public const string DAMAGE_POISON = "DAMAGE_POISON";
        public const string DAMAGE_FROST = "DAMAGE_FROST";
        public const string DAMAGE_SHADOW = "DAMAGE_SHADOW";

        // Stats Page 5/5
        public const string GEM_STRENGTH = "GEM_STRENGTH";
        public const string DAMAGE_SATELLITE = "DAMAGE_SATELLITE";
        public const string LIGHT_RADIUS = "LIGHT_RADIUS";
        public const string PICKUP_STRENGTH = "PICKUP_STRENGTH";
        public const string PICKUP_RADIUS = "PICKUP_RADIUS";

        public const string COMPANION_HEALTH = "COMPANION_HEALTH";
        public const string COMPANION_DAMAGE = "COMPANION_DAMAGE";
        public const string COMPANION_DAMAGE_REDUCTION = "COMPANION_DAMAGE_REDUCTION";
        public const string COMPANION_PASSIVE_AND_USE_EFFECT = "COMPANION_PASSIVE_AND_USE_EFFECT";

        //TODO think i can delete, maybe don't need
        // List of available SKill Properties that the value can be pulled from on the SKill Data
        //public const string COOLDOWN = "COOLDOWN";
        //public const string DAMAGE = "DAMAGE";
        //public const string DURATION = "DURATION";
        //public const string EFFECT = "EFFECT";
        //public const string PROC = "PROC";
        //public const string RANGE = "RANGE";
        //public const string RANGE2 = "RANGE2";
        //public const string VALUE = "VALUE";

        public static readonly List<string> StatModifierNames = new List<string>()
        {
            // List of available Modifiers that will be used as Stat names within ChroniCalc
            // Stats Page 1/5
            HEALTH_FLAT,
            MANA_FLAT,
            DAMAGE_FLAT,
            ATTACK_SPEED,
            CRIT_CHANCE,
            CRIT_DAMAGE,
            OVERPOWER,
            // Damage MULTIPLIERS
            DAMAGE_PHYSICAL,
            DAMAGE_FIRE,
            DAMAGE_LIGHTNING,
            DAMAGE_HOLY,
            DAMAGE_SWORD,
            DAMAGE_SHIELD,
            DAMAGE_MAGIC,
            // Extras/Unlisted Alternatives
            HEALTH_INCREASED,

            // Stats Page 2/5
            THORNS_FLAT,
            REGEN_HEALTH_FLAT,
            REGEN_MANA_FLAT,
            HEALTH_ON_HIT,
            MANA_ON_HIT,
            EVASION,
            DAMAGE_REDUCTION,

            RESISTANCE_PHYSICAL,
            RESISTANCE_FIRE,
            RESISTANCE_FROST,
            RESISTANCE_LIGHTNING,
            RESISTANCE_POISON,
            RESISTANCE_HOLY,
            RESISTANCE_SHADOW,
            // Extras/Unlisted Alternatives
            RESISTANCE_ALL, // TODO may not need, it's not on the page, would apply the value to all resists individually on Skill level change
            THORNS_INCREASED,

            // Stats Page 3/5
            COOLDOWN_REDUCTION,
            MANA_COST_REDUCTION,
            EFFECT_DURATION,
            BONUS_CRYSTALS,
            MAGIC_FIND,
            BONUS_EXPERIENCE,
            POTION_EFFECTIVENESS,

            REACH,
            MOVEMENT_SPEED,
            BLOCK_CHANCE,
            DAMAGE_BLOCKED,
            AURA_EFFECT,
            AURA_RANGE,
            DAMAGE_STAGGERED,

            // Stats Page 4/5
            DAMAGE_BLEEDING_FLAT,
            DAMAGE_BURN_FLAT,
            DAMAGE_FROST_FLAT,
            DAMAGE_POISON_FLAT,
            DAMAGE_CHAIN_LIGHTNING_FLAT,
            DAMAGE_SMITE_FLAT,
            DAMAGE_BLIGHT_FLAT,

            DAMAGE_BLEED,
            HEALING_TAKEN,
            WILLPOWER,
            MANA_SHIELD,
            DAMAGE_POISON,
            DAMAGE_FROST,
            DAMAGE_SHADOW,

            // Stats Page 5/5
            GEM_STRENGTH,
            DAMAGE_SATELLITE,
            LIGHT_RADIUS,
            PICKUP_STRENGTH,
            PICKUP_RADIUS,

            COMPANION_HEALTH,
            COMPANION_DAMAGE,
            COMPANION_DAMAGE_REDUCTION,
            COMPANION_PASSIVE_AND_USE_EFFECT
        };
    }
}
