using System;
using System.Collections.Generic;

namespace XRL.World.Parts
{
    /// <summary>
    /// Army types for elite encounter groups
    /// </summary>
    public enum ArmyType
    {
        SmallSquad,      // 2-3 elites, tier 5+
        ElitePatrol,     // 4-5 elites, tier 6+
        EliteWarband,    // 1 ultimate + 4-5 elite goons, tier 7+
        UltimateHost,    // 2 ultimates + 6-8 elite goons, tier 8+
        TitanPair        // 2-3 ultimates, no goons, tier 8+
    }

    /// <summary>
    /// Army composition definition
    /// </summary>
    public struct ArmyComposition
    {
        public int MinLeaders;       // Minimum number of ultimates
        public int MaxLeaders;       // Maximum number of ultimates
        public int MinGoons;         // Minimum number of elite goons
        public int MaxGoons;         // Maximum number of elite goons
        public int MinLevel;         // Player level requirement
        public int MinTier;          // Zone tier requirement
        public float WeightMultiplier; // Spawn weight multiplier

        public ArmyComposition(int minLeaders, int maxLeaders, int minGoons, int maxGoons,
                              int minLevel, int minTier, float weightMultiplier)
        {
            MinLeaders = minLeaders;
            MaxLeaders = maxLeaders;
            MinGoons = minGoons;
            MaxGoons = maxGoons;
            MinLevel = minLevel;
            MinTier = minTier;
            WeightMultiplier = weightMultiplier;
        }
    }

    /// <summary>
    /// Configuration for elite army encounters
    /// </summary>
    public static class EliteArmyConfig
    {
        /// <summary>
        /// Army composition definitions
        /// </summary>
        public static readonly Dictionary<ArmyType, ArmyComposition> Compositions =
            new Dictionary<ArmyType, ArmyComposition>
        {
            {
                ArmyType.SmallSquad,
                new ArmyComposition(
                    minLeaders: 0,
                    maxLeaders: 0,
                    minGoons: 2,
                    maxGoons: 3,
                    minLevel: 15,
                    minTier: 5,
                    weightMultiplier: 1.0f
                )
            },
            {
                ArmyType.ElitePatrol,
                new ArmyComposition(
                    minLeaders: 0,
                    maxLeaders: 0,
                    minGoons: 4,
                    maxGoons: 5,
                    minLevel: 20,
                    minTier: 6,
                    weightMultiplier: 0.8f
                )
            },
            {
                ArmyType.EliteWarband,
                new ArmyComposition(
                    minLeaders: 1,
                    maxLeaders: 1,
                    minGoons: 4,
                    maxGoons: 5,
                    minLevel: 25,
                    minTier: 7,
                    weightMultiplier: 0.6f
                )
            },
            {
                ArmyType.UltimateHost,
                new ArmyComposition(
                    minLeaders: 2,
                    maxLeaders: 2,
                    minGoons: 6,
                    maxGoons: 8,
                    minLevel: 30,
                    minTier: 8,
                    weightMultiplier: 0.4f
                )
            },
            {
                ArmyType.TitanPair,
                new ArmyComposition(
                    minLeaders: 2,
                    maxLeaders: 3,
                    minGoons: 0,
                    maxGoons: 0,
                    minLevel: 35,
                    minTier: 8,
                    weightMultiplier: 0.3f
                )
            }
        };

        /// <summary>
        /// Get composition for army type
        /// </summary>
        public static ArmyComposition GetComposition(ArmyType type)
        {
            if (Compositions.ContainsKey(type))
            {
                return Compositions[type];
            }

            // Default to SmallSquad if type not found
            return Compositions[ArmyType.SmallSquad];
        }

        /// <summary>
        /// Get army type from string (case-insensitive)
        /// </summary>
        public static bool TryParseArmyType(string typeString, out ArmyType result)
        {
            try
            {
                result = (ArmyType)Enum.Parse(typeof(ArmyType), typeString, ignoreCase: true);
                return true;
            }
            catch
            {
                result = ArmyType.SmallSquad;
                return false;
            }
        }

        /// <summary>
        /// Check if player meets level requirement for army type
        /// </summary>
        public static bool MeetsLevelRequirement(ArmyType type, int playerLevel)
        {
            var composition = GetComposition(type);
            return playerLevel >= composition.MinLevel;
        }

        /// <summary>
        /// Check if zone meets tier requirement for army type
        /// </summary>
        public static bool MeetsTierRequirement(ArmyType type, int zoneTier)
        {
            var composition = GetComposition(type);
            return zoneTier >= composition.MinTier;
        }

        /// <summary>
        /// Get description of army type for display
        /// </summary>
        public static string GetDescription(ArmyType type)
        {
            var comp = GetComposition(type);

            if (comp.MaxLeaders > 0 && comp.MaxGoons > 0)
            {
                return $"{comp.MinLeaders}-{comp.MaxLeaders} ultimate(s) + {comp.MinGoons}-{comp.MaxGoons} elite goons";
            }
            else if (comp.MaxLeaders > 0)
            {
                return $"{comp.MinLeaders}-{comp.MaxLeaders} ultimate(s)";
            }
            else
            {
                return $"{comp.MinGoons}-{comp.MaxGoons} elites";
            }
        }
    }
}
