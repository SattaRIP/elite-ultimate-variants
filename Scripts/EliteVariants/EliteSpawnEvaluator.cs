using System;
using System.Collections.Generic;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Evaluates whether a creature should be transformed into an elite variant
    /// </summary>
    public static class EliteSpawnEvaluator
    {
        private static Dictionary<string, bool> _eligibilityCache = new Dictionary<string, bool>();

        /// <summary>
        /// Main decision function for elite transformation
        /// </summary>
        public static bool ShouldTransformToElite(
            GameObject creature,
            Zone zone,
            out EliteTier tier,
            Dictionary<string, int> elitesPerZone,
            int lastSpawnTurn)
        {
            tier = EliteTier.Elite;

            // Early exit: feature disabled
            if (!EliteVariantSettings.EnableNaturalSpawning)
            {
                EliteVariantDebug.LogSpawnPrevented("Natural spawning disabled");
                return false;
            }

            // Check creature eligibility
            if (!IsEligibleForEliteTransform(creature))
            {
                EliteVariantDebug.LogSpawnPrevented($"Creature not eligible: {creature?.Blueprint ?? "null"}");
                return false;
            }

            // Check zone tier requirements (must meet at least the elite threshold)
            int zoneTier = zone?.NewTier ?? 0;
            if (zoneTier < EliteVariantSettings.MinZoneTierForElites)
            {
                EliteVariantDebug.LogSpawnPrevented($"Zone tier {zoneTier} < minimum {EliteVariantSettings.MinZoneTierForElites}");
                return false;
            }

            // Check player level requirements (must meet at least the elite threshold)
            int playerLevel = The.Player?.Statistics?["Level"]?.Value ?? 1;
            if (playerLevel < EliteVariantSettings.MinPlayerLevelForElites)
            {
                EliteVariantDebug.LogSpawnPrevented($"Player level {playerLevel} < minimum {EliteVariantSettings.MinPlayerLevelForElites}");
                return false;
            }

            // Check per-zone density limit
            if (!CanSpawnMoreElitesInZone(zone, zoneTier, elitesPerZone))
            {
                int currentCount = elitesPerZone.GetValueOrDefault(zone?.ZoneID ?? "", 0);
                int maxAllowed = EliteSpawnSafety.MaxElitesPerZone.GetValueOrDefault(zoneTier, 0);
                EliteVariantDebug.LogZoneLimitReached(zone?.DisplayName ?? "Unknown", currentCount, maxAllowed);
                return false;
            }

            // Check cooldown
            if (IsOnCooldown(lastSpawnTurn))
            {
                int currentTurn = (int)(The.Game?.Turns ?? 0);
                int turnsSince = currentTurn - lastSpawnTurn;
                int cooldown = EliteSpawnSafety.GlobalCooldownTurns;
                EliteVariantDebug.LogCooldown(cooldown - turnsSince);
                return false;
            }

            // Roll for elite chance
            var rng = new Random();
            float ultimateChance = CalculateEliteChance(zoneTier, playerLevel, EliteTier.Ultimate);
            float eliteChance = CalculateEliteChance(zoneTier, playerLevel, EliteTier.Elite);

            float roll = (float)rng.NextDouble();

            // Try ultimate first (if player level and zone tier sufficient)
            if (playerLevel >= EliteVariantSettings.MinPlayerLevelForUltimates
                && zoneTier >= EliteVariantSettings.MinZoneTierForUltimates
                && EliteVariantSettings.EnableUltimateSpawning
                && roll < ultimateChance)
            {
                tier = EliteTier.Ultimate;
                return true;
            }
            // Then try elite
            else if (roll < eliteChance + ultimateChance)
            {
                tier = EliteTier.Elite;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Calculate spawn chance based on zone tier and player level
        /// </summary>
        private static float CalculateEliteChance(int zoneTier, int playerLevel, EliteTier targetTier)
        {
            // Early game protection - use settings values
            if (zoneTier < EliteVariantSettings.MinZoneTierForElites)
                return 0f;
            if (playerLevel < EliteVariantSettings.MinPlayerLevelForElites)
                return 0f;
            if (targetTier == EliteTier.Ultimate && (
                playerLevel < EliteVariantSettings.MinPlayerLevelForUltimates ||
                zoneTier < EliteVariantSettings.MinZoneTierForUltimates))
                return 0f;

            // Get base chance from tier table
            float baseChance = 0f;
            if (targetTier == EliteTier.Elite)
            {
                baseChance = EliteSpawnSafety.EliteBaseChance.GetValueOrDefault(zoneTier, 0f);
            }
            else
            {
                baseChance = EliteSpawnSafety.UltimateBaseChance.GetValueOrDefault(zoneTier, 0f);
            }

            // Apply user multiplier (0.5x - 2.0x) - tier specific
            baseChance *= (targetTier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateSpawnRate
                : EliteVariantSettings.EliteSpawnRate;

            // Hard cap at 50%
            return Math.Clamp(baseChance, 0f, 0.50f);
        }

        /// <summary>
        /// Check if more elites can spawn in this zone
        /// </summary>
        private static bool CanSpawnMoreElitesInZone(Zone zone, int zoneTier, Dictionary<string, int> elitesPerZone)
        {
            if (zone == null)
                return false;

            int maxAllowed = EliteSpawnSafety.MaxElitesPerZone.GetValueOrDefault(zoneTier, 0);
            int currentCount = elitesPerZone.GetValueOrDefault(zone.ZoneID, 0);

            // Check zone-specific limit
            if (currentCount >= maxAllowed)
                return false;

            // Check absolute safety cap
            if (currentCount >= EliteSpawnSafety.AbsoluteMaxPerZone)
                return false;

            return true;
        }

        /// <summary>
        /// Check if global cooldown is active
        /// </summary>
        private static bool IsOnCooldown(int lastSpawnTurn)
        {
            int currentTurn = (int)(The.Game?.Turns ?? 0);
            int turnsSince = currentTurn - lastSpawnTurn;

            return turnsSince < EliteSpawnSafety.GlobalCooldownTurns;
        }

        /// <summary>
        /// Check if blueprint is eligible for elite transformation (public overload)
        /// </summary>
        public static bool IsEligibleForEliteTransform(string blueprint)
        {
            if (string.IsNullOrEmpty(blueprint))
                return false;

            // Check cache first
            if (_eligibilityCache.TryGetValue(blueprint, out bool cachedResult))
                return cachedResult;

            // Evaluate eligibility
            bool eligible = CheckBlueprintEligibility(blueprint);
            _eligibilityCache[blueprint] = eligible;

            return eligible;
        }

        /// <summary>
        /// Check if creature is eligible for elite transformation
        /// Reuses CreaturePool exclusion logic
        /// </summary>
        private static bool IsEligibleForEliteTransform(GameObject creature)
        {
            if (creature == null)
                return false;

            // Must be a creature
            if (!creature.HasTag("Creature"))
                return false;

            // Don't transform already-elite creatures
            if (creature.GetIntProperty("IsEliteVariant") == 1)
                return false;

            string blueprint = creature.Blueprint;
            if (string.IsNullOrEmpty(blueprint))
                return false;

            // Check cache first
            if (_eligibilityCache.TryGetValue(blueprint, out bool cachedResult))
                return cachedResult;

            // Evaluate eligibility
            bool eligible = CheckBlueprintEligibility(blueprint);
            _eligibilityCache[blueprint] = eligible;

            return eligible;
        }

        /// <summary>
        /// Check blueprint eligibility (cached)
        /// </summary>
        private static bool CheckBlueprintEligibility(string blueprint)
        {
            // Exclude same patterns as CreaturePool
            if (blueprint.Contains("EliteVariant") ||
                blueprint.Contains("Merchant") ||
                blueprint.Contains("Dromad") ||
                blueprint.Contains("Trader") ||
                blueprint.Contains("Vendor") ||
                blueprint.Contains("Seller") ||
                blueprint.Contains("Warden") ||
                blueprint.Contains("Spawn") ||
                blueprint.Contains("Cherub") ||
                blueprint.Contains("Sightless") ||
                blueprint.Contains("Corpse") ||
                blueprint.Contains("Bite") ||
                blueprint.Contains("Claw") ||
                blueprint.Contains("Fist") ||
                blueprint.Contains("Tendril") ||
                blueprint.Contains("Pseudopod") ||
                blueprint.Contains("Weep") ||
                blueprint.Contains("weep") ||
                blueprint.Contains("Fluid Carrier") ||
                blueprint.Contains("Mayor"))
                return false;

            // Exclude non-hostile plants and fungus
            if (blueprint.Contains("Puff") ||
                blueprint.Contains("Puffer") ||
                blueprint.Contains("Watervine") ||
                blueprint.Contains("Aloe Volta") ||
                blueprint.Contains("Aloe Pyra") ||
                blueprint.Contains("Thirst thistle") ||
                blueprint.Contains("Yurl") ||
                blueprint.Contains("Asphodel"))
                return false;

            // Check blueprint tags
            var bp = GameObjectFactory.Factory.Blueprints.GetValueOrDefault(blueprint);
            if (bp == null)
                return false;

            if (bp.HasTag("BaseObject") ||
                bp.HasTag("ExcludeFromDynamicEncounters") ||
                bp.HasTag("IgnoresPhase") ||
                bp.HasTag("IgnoresBeguiling") ||
                bp.HasTag("CannotBeUnique") ||
                bp.HasTag("NoThreatTracking"))
                return false;

            // Check if robot (they don't have biological mutations)
            if (bp.InheritsFrom("BaseRobot"))
                return false;

            return true;
        }

        /// <summary>
        /// Clear eligibility cache (call on game load)
        /// </summary>
        public static void ClearCache()
        {
            _eligibilityCache.Clear();
        }
    }
}
