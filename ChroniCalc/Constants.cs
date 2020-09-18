using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    public static class Constants
    {
        // SKILL IDS OF PASSIVE BONUS SKILLS (e.g. The skill button that keeps a running total of the number of points spent on a given Tree)
        public static class SkillIDs
        {
            // CLASS TREES
            public const int BERSERKER_GUARDIAN = 380;
            public const int BERSERKER_SKY_LORD = 478;
            public const int BERSERKER_DRAGONKIN = 82;
            public const int BERSERKER_FROSTBORN = 93;

            public const int TEMPLAR_VENGEANCE = 241;
            public const int TEMPLAR_WRATH = 242;
            public const int TEMPLAR_CONVICTION = 273;
            public const int TEMPLAR_REDEMPTION = 274;

            public const int WARDEN_WIND_RANGER = 512;
            public const int WARDEN_DRUID = 545;
            public const int WARDEN_STORM_CALLER = 124;
            public const int WARDEN_WINTER_HERALD = 609;

            public const int WARLOCK_CORRUPTOR = 642;
            public const int WARLOCK_LICH = 748;
            public const int WARLOCK_DEMONOLOGIST = 707;
            public const int WARLOCK_REAPER = 182;

            // MASTERY TREE
            public const int MASTERY_BERSERKER_GUARDIAN = 100011;
            public const int MASTERY_BERSERKER_SKY_LORD = 100013;
            public const int MASTERY_BERSERKER_DRAGONKIN = 100012;
            public const int MASTERY_BERSERKER_FROSTBORN = 100014;

            public const int MASTERY_TEMPLAR_VENGEANCE = 100007;
            public const int MASTERY_TEMPLAR_WRATH = 100009;
            public const int MASTERY_TEMPLAR_CONVICTION = 100008;
            public const int MASTERY_TEMPLAR_REDEMPTION = 100010;

            public const int MASTERY_WARDEN_WIND_RANGER = 100003;
            public const int MASTERY_WARDEN_DRUID = 100004;
            public const int MASTERY_WARDEN_STORM_CALLER = 100005;
            public const int MASTERY_WARDEN_WINTER_HERALD = 100006;

            public const int MASTERY_WARLOCK_CORRUPTOR = 100015;
            public const int MASTERY_WARLOCK_LICH = 100017;
            public const int MASTERY_WARLOCK_DEMONOLOGIST = 100016;
            public const int MASTERY_WARLOCK_REAPER = 100018;

            /// <summary>
            /// A list of all Passive Bonus Skill IDs that aren't actually selectable Skills, but instead hold the total level of a particular Tree
            /// </summary>
            public static List<int> PassiveBonusSkillIDs = new List<int> { BERSERKER_GUARDIAN, BERSERKER_SKY_LORD, BERSERKER_DRAGONKIN, BERSERKER_FROSTBORN,
                                                                           TEMPLAR_VENGEANCE, TEMPLAR_WRATH, TEMPLAR_CONVICTION, TEMPLAR_REDEMPTION,
                                                                           WARDEN_WIND_RANGER, WARDEN_DRUID, WARDEN_STORM_CALLER, WARDEN_WINTER_HERALD,
                                                                           WARLOCK_CORRUPTOR, WARLOCK_LICH, WARLOCK_DEMONOLOGIST, WARLOCK_REAPER,
                                                                           MASTERY_BERSERKER_GUARDIAN, MASTERY_BERSERKER_SKY_LORD, MASTERY_BERSERKER_DRAGONKIN, MASTERY_BERSERKER_FROSTBORN,
                                                                           MASTERY_TEMPLAR_VENGEANCE, MASTERY_TEMPLAR_WRATH, MASTERY_TEMPLAR_CONVICTION, MASTERY_TEMPLAR_REDEMPTION,
                                                                           MASTERY_WARDEN_WIND_RANGER, MASTERY_WARDEN_DRUID, MASTERY_WARDEN_STORM_CALLER, MASTERY_WARDEN_WINTER_HERALD,
                                                                           MASTERY_WARLOCK_CORRUPTOR, MASTERY_WARLOCK_LICH, MASTERY_WARLOCK_DEMONOLOGIST, MASTERY_WARLOCK_REAPER };
        }
    }
}
