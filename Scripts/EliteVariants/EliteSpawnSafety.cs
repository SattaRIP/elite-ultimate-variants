using System;
using System.Collections.Generic;

namespace XRL.World.Parts
{
    /// <summary>
    /// Safety constants and limits for elite variant natural spawning
    /// </summary>
    public static class EliteSpawnSafety
    {
        // Absolute maximum elites per zone (failsafe)
        public const int AbsoluteMaxPerZone = 10;

        // Maximum elite percentage of zone creatures
        public const float MaxElitePercentage = 0.25f; // 25%

        // Minimum turns between ANY elite spawn (global cooldown)
        public const int GlobalCooldownTurns = 50;

        // Minimum cooldown even with max multiplier
        public const int MinimumCooldownTurns = 25;

        // Base spawn chances by tier (Elite)
        public static readonly Dictionary<int, float> EliteBaseChance = new Dictionary<int, float>
        {
            { 1, 0f },
            { 2, 0f },
            { 3, 0f },
            { 4, 0.02f },  // 2%
            { 5, 0.05f },  // 5%
            { 6, 0.08f },  // 8%
            { 7, 0.12f },  // 12%
            { 8, 0.15f }   // 15%
        };

        // Base spawn chances by tier (Ultimate)
        public static readonly Dictionary<int, float> UltimateBaseChance = new Dictionary<int, float>
        {
            { 1, 0f },
            { 2, 0f },
            { 3, 0f },
            { 4, 0f },
            { 5, 0.01f },  // 1%
            { 6, 0.02f },  // 2%
            { 7, 0.04f },  // 4%
            { 8, 0.06f }   // 6%
        };

        // Max elites per zone by tier
        public static readonly Dictionary<int, int> MaxElitesPerZone = new Dictionary<int, int>
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 1 },
            { 5, 2 },
            { 6, 3 },
            { 7, 4 },
            { 8, 5 }
        };
    }
}
