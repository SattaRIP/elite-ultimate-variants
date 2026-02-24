using System;
using XRL.Messages;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Debug utilities for Elite Variants mod
    /// Provides verbose logging and notifications when debug mode is enabled
    /// </summary>
    public static class EliteVariantDebug
    {
        /// <summary>
        /// Log a debug message to the message log (only if debug mode is on)
        /// </summary>
        public static void Log(string message, string color = "c")
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            MessageQueue.AddPlayerMessage($"{{{color}|[Elite Debug]}} {message}");
        }

        /// <summary>
        /// Log an elite spawn event
        /// </summary>
        public static void LogEliteSpawn(GameObject creature, EliteTier tier, string zoneName, int zoneTier)
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            string tierColor = (tier == EliteTier.Ultimate) ? "M" : "Y";
            string tierName = (tier == EliteTier.Ultimate) ? "ULTIMATE" : "ELITE";

            MessageQueue.AddPlayerMessage(
                $"{{c|[Elite Debug]}} {{{tierColor}|{tierName}}} spawned: " +
                $"{{W|{creature.DisplayName}}} " +
                $"(Lvl {creature.Statistics["Level"].BaseValue}) " +
                $"in {{Y|{zoneName}}} (Tier {zoneTier})"
            );
        }

        /// <summary>
        /// Log elite army spawn event
        /// </summary>
        public static void LogArmySpawn(ArmyType armyType, string baseCreature, int leaderCount, int goonCount, string zoneName, int zoneTier)
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            var composition = EliteArmyConfig.GetComposition(armyType);

            MessageQueue.AddPlayerMessage(
                $"{{c|[Army Debug]}} {{M|{armyType}}} spawned: " +
                $"{{W|{baseCreature}}} " +
                $"({leaderCount} leaders, {goonCount} goons) " +
                $"in {{Y|{zoneName}}} (Tier {zoneTier})"
            );
        }

        /// <summary>
        /// Log spawn prevention reason
        /// </summary>
        public static void LogSpawnPrevented(string reason)
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            MessageQueue.AddPlayerMessage($"{{c|[Elite Debug]}} {{R|Spawn prevented:}} {reason}");
        }

        /// <summary>
        /// Log enhancement application
        /// </summary>
        public static void LogEnhancement(GameObject creature, string enhancementName, string enhancementType)
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            MessageQueue.AddPlayerMessage(
                $"{{c|[Elite Debug]}} Applied {{{GetEnhancementColor(enhancementType)}|{enhancementType}}}: " +
                $"{{Y|{enhancementName}}} to {{W|{creature.DisplayName}}}"
            );
        }

        /// <summary>
        /// Log equipment grant
        /// </summary>
        public static void LogEquipment(GameObject creature, GameObject item, int itemTier)
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            MessageQueue.AddPlayerMessage(
                $"{{c|[Elite Debug]}} Granted {{C|T{itemTier}}} equipment: " +
                $"{{Y|{item.DisplayName}}} to {{W|{creature.DisplayName}}}"
            );
        }

        /// <summary>
        /// Log zone limit reached
        /// </summary>
        public static void LogZoneLimitReached(string zoneName, int currentCount, int maxCount, string limitType = "elite")
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            MessageQueue.AddPlayerMessage(
                $"{{c|[Elite Debug]}} {{R|Zone limit reached:}} " +
                $"{{W|{zoneName}}} has {currentCount}/{maxCount} {limitType}s"
            );
        }

        /// <summary>
        /// Log cooldown information
        /// </summary>
        public static void LogCooldown(int turnsRemaining)
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            MessageQueue.AddPlayerMessage(
                $"{{c|[Elite Debug]}} {{Y|Global cooldown:}} {turnsRemaining} turns remaining"
            );
        }

        /// <summary>
        /// Log player level/tier gate check
        /// </summary>
        public static void LogGateCheck(int playerLevel, int minLevel, int zoneTier, int minTier, bool passed)
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            string status = passed ? "{{G|PASS}}" : "{{R|FAIL}}";
            MessageQueue.AddPlayerMessage(
                $"{{c|[Elite Debug]}} {{Y|Gate check}} {status}: " +
                $"Player Lvl {playerLevel}/{minLevel}, Zone Tier {zoneTier}/{minTier}"
            );
        }

        /// <summary>
        /// Get color code for enhancement type
        /// </summary>
        private static string GetEnhancementColor(string type)
        {
            switch (type)
            {
                case "Physical Mutation": return "G";
                case "Mental Mutation": return "C";
                case "Cybernetic": return "W";
                case "Equipment": return "Y";
                default: return "y";
            }
        }

        /// <summary>
        /// Log detailed statistics
        /// </summary>
        public static void LogStats(string category, string stats)
        {
            if (!EliteVariantSettings.EnableDebugMode)
                return;

            MessageQueue.AddPlayerMessage($"{{c|[Elite Debug]}} {{Y|{category}}}:\n{stats}");
        }

        /// <summary>
        /// Show verbose creature info
        /// </summary>
        public static string GetCreatureDebugInfo(GameObject creature)
        {
            if (creature == null)
                return "null";

            return $"{creature.DisplayName} " +
                   $"(Lvl {creature.Statistics["Level"].BaseValue}, " +
                   $"HP {creature.hitpoints}/{creature.baseHitpoints}, " +
                   $"AV {creature.Statistics["AV"].Value}, " +
                   $"DV {creature.Statistics["DV"].Value})";
        }
    }
}
