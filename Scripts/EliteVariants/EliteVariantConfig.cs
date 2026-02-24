using System;
using System.Collections.Generic;
using System.Linq;

namespace XRL.World.Parts
{
    /// <summary>
    /// Encounter composition types for elite spawns
    /// </summary>
    public enum EncounterType
    {
        Solo,              // 1 elite, max enhancements
        Duo,               // 2 identical elites
        Trio,              // 3 identical elites
        ElitePlusGoons,    // 1 elite + 3-5 goons
        Pack,              // 5-8 identical elites
        MixedElites        // 1 elite + 1-3 different elite creatures
    }

    /// <summary>
    /// Power tier for elite variants
    /// </summary>
    public enum EliteTier
    {
        Elite,      // White, moderate power, 1-3 mental mutations
        Ultimate    // Golden, high power, 4-6 mental mutations
    }

    public static class EliteVariantConfig
    {
        // Level Scaling Configuration
        public const int MinEliteLevel = 15;
        public const int MaxEliteLevel = 50;
        public const int ZoneTierLevelBonus = 5;    // Bonus per zone tier
        public const int PlayerLevelBonus = 5;       // Bonus relative to player
        public const int GoonLevelPenalty = 10;      // How much weaker goons are

        // Base Stats Configuration
        public const int LeaderBaseAV = 12;
        public const int LeaderBaseDV = 6;
        public const int GoonBaseAV = 8;
        public const int GoonBaseDV = 4;
        public const int StrengthBoost = 1;
        public const int EgoBoost = 2;
        public const int WillpowerBoost = 1;

        // Enhancement Configuration
        public const int MinEnhancements = 2;
        public const int MaxEnhancements = 4;
        public const int GoonEnhancements = 1;

        // Mutation Level Ranges
        public const int LeaderMutationLevelMin = 8;
        public const int LeaderMutationLevelMax = 10;
        public const int GoonMutationLevelMin = 4;
        public const int GoonMutationLevelMax = 6;

        // Cybernetics Configuration
        public const string LeaderCyberTable = "Cybernetics6";
        public const string GoonCyberTable = "Cybernetics4";
        public const int LeaderCyberChance = 85;
        public const int GoonCyberChance = 50;
        public const int LeaderLicensesAtLeast = 10;
        public const int GoonLicensesAtLeast = 6;

        // Visual Theme (Golden/Ultimate)
        public const string DisplayColor = "&Y";    // Yellow
        public const string DetailColor = "y";       // lowercase yellow detail
        public const int GlowRadius = 2;

        // Naming Prefixes (elite theme)
        public static readonly string[] NamePrefixes = new[]
        {
            "elite",
            "champion",
            "empowered",
            "awakened",
            "ascended",
            "golden"
        };

        // Enhancement Type Weights
        public const int PhysicalMutationWeight = 30;
        public const int MentalMutationWeight = 30;
        public const int CyberneticsWeight = 20;
        public const int EquipmentWeight = 20;

        // Encounter Composition Weights (relative probability)
        public static readonly Dictionary<EncounterType, int> EncounterWeights = new Dictionary<EncounterType, int>
        {
            { EncounterType.Solo, 20 },
            { EncounterType.Duo, 15 },
            { EncounterType.Trio, 10 },
            { EncounterType.ElitePlusGoons, 25 },
            { EncounterType.Pack, 15 },
            { EncounterType.MixedElites, 15 }
        };

        /// <summary>
        /// Selects a random encounter composition based on weights
        /// </summary>
        public static (EncounterType type, int count, bool useGoons) SelectEncounterComposition(Random rng)
        {
            // Weighted random selection
            int totalWeight = EncounterWeights.Values.Sum();
            int roll = rng.Next(totalWeight);
            int cumulative = 0;

            foreach (var kvp in EncounterWeights)
            {
                cumulative += kvp.Value;
                if (roll < cumulative)
                {
                    return kvp.Key switch
                    {
                        EncounterType.Solo => (kvp.Key, 1, false),
                        EncounterType.Duo => (kvp.Key, 2, false),
                        EncounterType.Trio => (kvp.Key, 3, false),
                        EncounterType.ElitePlusGoons => (kvp.Key, rng.Next(3, 6), true),
                        EncounterType.Pack => (kvp.Key, rng.Next(5, 9), false),
                        EncounterType.MixedElites => (kvp.Key, rng.Next(2, 4), false),
                        _ => (EncounterType.Solo, 1, false)
                    };
                }
            }

            return (EncounterType.Solo, 1, false);
        }

        /// <summary>
        /// Selects tier based on zone difficulty and random chance
        /// Uses difficulty settings (ultimate chance) as base, then adjusts by zone
        /// </summary>
        public static EliteTier SelectTier(Random rng, int zoneTier)
        {
            // Get base ultimate chance from settings (0-100)
            int baseUltimateChance = EliteVariantSettings.UltimateChance;

            // Adjust by zone tier (scale the base value)
            float zoneMultiplier = 1.0f;
            if (zoneTier <= 4)
            {
                zoneMultiplier = 0.67f;  // Lower tier = less ultimate (30% → 20%)
            }
            else if (zoneTier <= 6)
            {
                zoneMultiplier = 1.33f;  // Mid tier = more ultimate (30% → 40%)
            }
            else // Tier 7-8
            {
                zoneMultiplier = 2.0f;   // High tier = much more ultimate (30% → 60%)
            }

            int ultimateChance = (int)Math.Clamp(baseUltimateChance * zoneMultiplier, 0, 100);
            int eliteChance = 100 - ultimateChance;

            int roll = rng.Next(100);
            return roll < eliteChance ? EliteTier.Elite : EliteTier.Ultimate;
        }

        /// <summary>
        /// Get mental mutation cap based on tier
        /// Applies difficulty settings (enhancement multiplier)
        /// </summary>
        public static int GetMentalMutationCap(int level, EliteTier tier)
        {
            // Check if user has set a custom cap
            int settingsCap = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateMentalMutationCap
                : EliteVariantSettings.EliteMentalMutationCap;

            // If settings cap is 0, return unlimited (int.MaxValue)
            if (settingsCap == 0)
                return int.MaxValue;

            // Apply enhancement multiplier to the settings cap
            float enhMult = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateEnhancementMultiplier
                : EliteVariantSettings.EliteEnhancementMultiplier;
            int scaledCap = (int)Math.Ceiling(settingsCap * enhMult);

            // Ensure at least 1 mental mutation possible
            return Math.Max(1, scaledCap);
        }

        /// <summary>
        /// Get physical mutation cap based on tier
        /// Applies difficulty settings (enhancement multiplier)
        /// </summary>
        public static int GetPhysicalMutationCap(int level, EliteTier tier)
        {
            // Check if user has set a custom cap
            int settingsCap = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimatePhysicalMutationCap
                : EliteVariantSettings.ElitePhysicalMutationCap;

            // If settings cap is 0, return unlimited (int.MaxValue)
            if (settingsCap == 0)
                return int.MaxValue;

            // Apply enhancement multiplier to the settings cap
            float enhMult = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateEnhancementMultiplier
                : EliteVariantSettings.EliteEnhancementMultiplier;
            int scaledCap = (int)Math.Ceiling(settingsCap * enhMult);

            return Math.Max(1, scaledCap);
        }

        /// <summary>
        /// Get total enhancement count based on level, tier, and goon status
        /// Applies difficulty settings (enhancement multiplier)
        /// </summary>
        public static int GetEnhancementCount(Random rng, int level, EliteTier tier, bool isGoon)
        {
            int baseCount;

            if (isGoon)
            {
                baseCount = tier == EliteTier.Ultimate ? rng.Next(1, 3) : 1;
            }
            else
            {
                if (tier == EliteTier.Elite)
                {
                    baseCount = rng.Next(2, 6); // 2-5
                }
                else // Ultimate
                {
                    baseCount = rng.Next(4, 9); // 4-8
                }
            }

            // Apply tier-specific enhancement multiplier from settings
            float enhMult = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateEnhancementMultiplier
                : EliteVariantSettings.EliteEnhancementMultiplier;
            int scaledCount = (int)Math.Ceiling(baseCount * enhMult);

            // Ensure at least 1 enhancement
            return Math.Max(1, scaledCount);
        }
    }
}
