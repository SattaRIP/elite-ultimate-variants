using System;
using System.Collections.Generic;
using System.Linq;
using XRL.World;
using XRL.World.AI.GoalHandlers;
using XRL.World.Parts;
using XRL.World.Parts.Mutation;

namespace XRL.World.Parts
{
    /// <summary>
    /// Enhancement types for elite variants
    /// </summary>
    public enum EnhancementType
    {
        PhysicalMutation,
        MentalMutation,
        Cybernetics,
        Equipment
    }

    public static class EliteVariantGenerator
    {
        /// <summary>
        /// Calculates appropriate elite level based on zone tier and player level
        /// Applies difficulty settings (level offset)
        /// </summary>
        public static int CalculateEliteLevel(Zone zone, bool isGoon = false, EliteTier tier = EliteTier.Elite)
        {
            // Get zone tier (1-8)
            int zoneTier = zone?.NewTier ?? 5;

            // Get player level (fallback to 1 if player not found)
            int playerLevel = The.Player?.Statistics?["Level"]?.Value ?? 1;

            // Base level from zone tier (Tier * 5)
            int zoneBasedLevel = zoneTier * 5;

            // Calculate elite level: higher of zone-appropriate or player-appropriate
            int eliteLevel = Math.Max(
                zoneBasedLevel + EliteVariantConfig.ZoneTierLevelBonus,
                playerLevel + EliteVariantConfig.PlayerLevelBonus
            );

            // Apply difficulty setting: tier-specific level offset
            eliteLevel += (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateLevelOffset
                : EliteVariantSettings.EliteLevelOffset;

            // Apply goon penalty if needed
            if (isGoon)
            {
                eliteLevel = Math.Max(eliteLevel - EliteVariantConfig.GoonLevelPenalty, EliteVariantConfig.MinEliteLevel);
            }

            // Clamp to min/max
            return Math.Clamp(eliteLevel, EliteVariantConfig.MinEliteLevel, EliteVariantConfig.MaxEliteLevel);
        }

        /// <summary>
        /// Creates an elite variant from a base creature blueprint.
        /// Elite variants get mixed enhancements: physical mutations, mental mutations,
        /// cybernetics, and powerful equipment.
        /// </summary>
        public static GameObject CreateEliteVariant(
            string baseBlueprint,
            Random rng,
            bool isGoon = false,
            int? customLevel = null,
            EliteTier? customTier = null)
        {
            // Create base creature
            GameObject creature = GameObjectFactory.Factory.CreateObject(baseBlueprint);
            if (creature == null) return null;

            // Get original display name for naming
            string originalName = creature.GetDisplayName(
                int.MaxValue,
                AsIfKnown: true,
                NoColor: true
            );

            // Determine level (use custom if provided, otherwise use defaults)
            int eliteLevel = customLevel ?? (isGoon ? 30 : 40);

            // Determine tier (use custom if provided, otherwise default to Elite)
            EliteTier tier = customTier ?? EliteTier.Elite;

            // Analyze creature strengths BEFORE applying bonuses
            var strengths = AnalyzeCreatureStrengths(creature);

            // Apply adaptive stats
            ApplyBaseStats(creature, isGoon, eliteLevel, tier, strengths);

            // Enhancement selection with mutation caps
            int enhancementCount = EliteVariantConfig.GetEnhancementCount(rng, eliteLevel, tier, isGoon);
            int mentalMutationCap = EliteVariantConfig.GetMentalMutationCap(eliteLevel, tier);
            int physicalMutationCap = EliteVariantConfig.GetPhysicalMutationCap(eliteLevel, tier);

            var enhancements = SelectEnhancements(rng, enhancementCount, isGoon,
                                                 mentalMutationCap, physicalMutationCap);

            foreach (var enhancement in enhancements)
            {
                ApplyEnhancement(creature, enhancement, rng, isGoon, tier);
            }

            // Grant equipment separately (no longer part of enhancement system)
            EquipmentPool.GrantEquipment(creature, rng, tier, !isGoon);

            // Apply tier-specific visual theme
            ApplyVisualTheme(creature, originalName, rng, tier);

            // Keep vanilla faction - creatures use their original faction relationships
            // This allows for dynamic faction vs faction battles

            // Ensure Brain is ready for combat
            Brain brain = creature.GetPart<Brain>();
            if (brain != null)
            {
                brain.Mobile = true;
                brain.Hibernating = false;
                brain.PerformReequip();
            }

            // Tag as elite variant
            creature.SetIntProperty("IsEliteVariant", 1);
            if (!isGoon)
            {
                creature.SetIntProperty("IsEliteLeader", 1);
            }

            return creature;
        }

        /// <summary>
        /// Analyze creature's base stats to identify strengths and weaknesses
        /// Returns scaling factors: 3 = High (>16), 2 = Medium (12-16), 1 = Low (<12)
        /// </summary>
        private static Dictionary<string, int> AnalyzeCreatureStrengths(GameObject creature)
        {
            var strengths = new Dictionary<string, int>();
            var stats = new[] { "Strength", "Agility", "Toughness", "Intelligence", "Willpower", "Ego" };

            foreach (var statName in stats)
            {
                if (creature.Statistics.ContainsKey(statName))
                {
                    int baseValue = creature.Statistics[statName].BaseValue;

                    // Categorize: High (>16) = 3x, Medium (12-16) = 2x, Low (<12) = 1x
                    if (baseValue > 16)
                        strengths[statName] = 3;
                    else if (baseValue >= 12)
                        strengths[statName] = 2;
                    else
                        strengths[statName] = 1;
                }
            }

            return strengths;
        }

        /// <summary>
        /// Apply base stats scaled by level using adaptive scaling
        /// Buffs creature's strengths significantly, weaknesses minimally
        /// Applies difficulty settings (power multiplier)
        /// </summary>
        private static void ApplyBaseStats(GameObject creature, bool isGoon, int level, EliteTier tier, Dictionary<string, int> strengths)
        {
            // Set level
            if (creature.Statistics.ContainsKey("Level"))
            {
                creature.Statistics["Level"].BaseValue = level;
            }

            // Tier multiplier for all bonuses
            float tierMult = tier == EliteTier.Ultimate ? 1.5f : 1.0f;

            // Apply difficulty setting: tier-specific power multiplier (affects all scaling)
            float powerMult = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimatePowerMultiplier
                : EliteVariantSettings.ElitePowerMultiplier;

            // Base stat bonus from level
            int baseBonus = level / 10;

            // Apply adaptive scaling to each stat (buff strengths, not weaknesses)
            foreach (var kvp in strengths)
            {
                string statName = kvp.Key;
                int scalingFactor = kvp.Value; // 1, 2, or 3

                if (creature.Statistics.ContainsKey(statName))
                {
                    // Stronger stats get bigger boosts, scaled by power multiplier
                    int bonus = (int)(baseBonus * scalingFactor * tierMult * powerMult);
                    creature.Statistics[statName].Boost += bonus;
                }
            }

            // HP scaling: settings multiplier applied directly to creature's base HP.
            // 1.0x = same as the unmodified creature. Default: Elite 1.5x, Ultimate 2.0x.
            int originalHP = 50; // Fallback
            if (creature.Statistics.ContainsKey("Hitpoints"))
            {
                originalHP = creature.Statistics["Hitpoints"].BaseValue;
            }

            float hpSettingsMult = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateHPMultiplier
                : EliteVariantSettings.EliteHPMultiplier;

            int totalHP = (int)(originalHP * hpSettingsMult);

            if (creature.Statistics.ContainsKey("Hitpoints"))
            {
                creature.Statistics["Hitpoints"].BaseValue = Math.Max(1, totalHP);
            }

            // AV based on Toughness, DV based on Agility
            int toughnessFactor = strengths.ContainsKey("Toughness") ? strengths["Toughness"] : 1;
            int agilityFactor = strengths.ContainsKey("Agility") ? strengths["Agility"] : 1;

            int avBonus = (int)((level / 5) * toughnessFactor * powerMult);
            int dvBonus = (int)((level / 8) * agilityFactor * powerMult);

            if (tier == EliteTier.Ultimate)
            {
                avBonus = (int)(avBonus * 1.5f);
                dvBonus = (int)(dvBonus * 1.5f);
            }

            if (creature.Statistics.ContainsKey("AV"))
            {
                int baseAV = isGoon ? 6 : 10;
                creature.Statistics["AV"].BaseValue = baseAV + avBonus;
            }

            if (creature.Statistics.ContainsKey("DV"))
            {
                int baseDV = isGoon ? 4 : 6;
                creature.Statistics["DV"].BaseValue = baseDV + dvBonus;
            }

            // Set tier based on level
            int itemTier = Math.Max(1, level / 5);
            creature.SetStringProperty("Tier", itemTier.ToString());
        }

        /// <summary>
        /// Select random enhancements with weighted distribution and mutation caps
        /// </summary>
        private static List<EnhancementType> SelectEnhancements(
            Random rng,
            int count,
            bool isGoon,
            int mentalMutationCap,
            int physicalMutationCap)
        {
            var selected = new List<EnhancementType>();
            int mentalMutationsAdded = 0;
            int physicalMutationsAdded = 0;

            var weights = new Dictionary<EnhancementType, int>
            {
                { EnhancementType.PhysicalMutation, EliteVariantConfig.PhysicalMutationWeight },
                { EnhancementType.MentalMutation, EliteVariantConfig.MentalMutationWeight },
                { EnhancementType.Cybernetics, EliteVariantConfig.CyberneticsWeight }
            };

            // Weighted random selection with caps
            for (int i = 0; i < count; i++)
            {
                // Apply caps by setting weights to 0
                if (mentalMutationsAdded >= mentalMutationCap)
                {
                    weights[EnhancementType.MentalMutation] = 0;
                }
                if (physicalMutationsAdded >= physicalMutationCap)
                {
                    weights[EnhancementType.PhysicalMutation] = 0;
                }

                // Safety check: If all weights are 0, break early
                if (weights.Values.All(w => w == 0))
                {
                    break;
                }

                var type = WeightedRandomSelect(weights, rng);
                selected.Add(type);

                // Increment counters
                if (type == EnhancementType.MentalMutation)
                {
                    mentalMutationsAdded++;
                }
                else if (type == EnhancementType.PhysicalMutation)
                {
                    physicalMutationsAdded++;
                }

                // Reduce weight after selection to encourage variety
                weights[type] = Math.Max(5, weights[type] / 2);
            }

            return selected;
        }

        /// <summary>
        /// Weighted random selection from dictionary
        /// </summary>
        private static EnhancementType WeightedRandomSelect(Dictionary<EnhancementType, int> weights, Random rng)
        {
            int totalWeight = weights.Values.Sum();
            int randomValue = rng.Next(totalWeight);

            int cumulativeWeight = 0;
            foreach (var kvp in weights)
            {
                cumulativeWeight += kvp.Value;
                if (randomValue < cumulativeWeight)
                {
                    return kvp.Key;
                }
            }

            return weights.Keys.First();
        }

        /// <summary>
        /// Apply selected enhancement to creature
        /// </summary>
        private static void ApplyEnhancement(GameObject creature, EnhancementType type, Random rng, bool isGoon, EliteTier tier)
        {
            switch (type)
            {
                case EnhancementType.PhysicalMutation:
                    ApplyPhysicalMutation(creature, rng, isGoon);
                    break;
                case EnhancementType.MentalMutation:
                    ApplyMentalMutation(creature, rng, isGoon);
                    break;
                case EnhancementType.Cybernetics:
                    ApplyCybernetics(creature, isGoon, tier);
                    break;
                // Equipment is now handled separately, not as an enhancement
            }
        }

        /// <summary>
        /// Apply physical mutation to creature
        /// </summary>
        private static void ApplyPhysicalMutation(GameObject creature, Random rng, bool isGoon)
        {
            Mutations mutations = creature.RequirePart<Mutations>();

            // Get existing mutations to avoid duplicates and exclusions
            var existing = new HashSet<string>();
            foreach (var entry in mutations.MutationList)
            {
                if (entry != null && !string.IsNullOrEmpty(entry.Name))
                {
                    existing.Add(entry.Name);
                }
            }

            // Select 1 physical mutation
            var selectedMutations = PhysicalMutationPool.SelectMutations(1, rng, !isGoon, existing);

            foreach (var (mutationName, level) in selectedMutations)
            {
                if (mutations.GetMutation(mutationName) != null)
                    continue;

                // Handle Horns variant
                if (mutationName == "Horns")
                {
                    string variant = PhysicalMutationPool.GetHornsVariant(rng);
                    if (variant != null)
                    {
                        mutations.AddMutation(mutationName, level);
                        // Variant handling is complex - let default behavior handle it
                    }
                    else
                    {
                        mutations.AddMutation(mutationName, level);
                    }
                }
                else
                {
                    mutations.AddMutation(mutationName, level);
                }
            }
        }

        /// <summary>
        /// Apply mental mutation to creature
        /// </summary>
        private static void ApplyMentalMutation(GameObject creature, Random rng, bool isGoon)
        {
            Mutations mutations = creature.RequirePart<Mutations>();

            // Select 1 mental mutation
            var selectedMutations = MentalMutationPool.SelectMutations(1, rng, isGoon);

            foreach (var (mutationName, level) in selectedMutations)
            {
                if (mutations.GetMutation(mutationName) != null)
                    continue;

                mutations.AddMutation(mutationName, level);
            }
        }

        /// <summary>
        /// Apply tier-specific cybernetics to creature
        /// </summary>
        private static void ApplyCybernetics(GameObject creature, bool isGoon, EliteTier tier)
        {
            try
            {
                var cyber = creature.RequirePart<CyberneticsHasRandomImplants>();

                if (tier == EliteTier.Ultimate)
                {
                    // Ultimate gets better cybernetics
                    cyber.ImplantChance = isGoon ? "75" : "95";
                    cyber.ImplantTable = isGoon ? "Cybernetics6" : "Cybernetics8";
                    cyber.LicensesAtLeast = isGoon ? "8" : "15";
                }
                else // Elite
                {
                    cyber.ImplantChance = isGoon ? "50" : "85";
                    cyber.ImplantTable = isGoon ? "Cybernetics4" : "Cybernetics6";
                    cyber.LicensesAtLeast = isGoon ? "6" : "10";
                }
            }
            catch (Exception)
            {
                // CyberneticsHasRandomImplants part might not be available - skip silently
            }
        }

        /// <summary>
        /// Apply tier-specific visual theme (Elite = white/silver, Ultimate = gold)
        /// </summary>
        private static void ApplyVisualTheme(GameObject creature, string originalName, Random rng, EliteTier tier)
        {
            string prefix = tier == EliteTier.Ultimate ? "ultimate" : "elite";
            string newName = $"{prefix} {originalName}";

            Render render = creature.GetPart<Render>();
            if (render != null)
            {
                render.DisplayName = newName;
                // In CoQ's color system: W = gold, Y = white/silver
                render.ColorString = tier == EliteTier.Ultimate ? "&W" : "&Y";
                render.DetailColor = tier == EliteTier.Ultimate ? "W" : "Y";
            }

            // Add glow effect
            var light = creature.RequirePart<LightSource>();
            light.Lit = true;
            light.Radius = tier == EliteTier.Ultimate ? 3 : 1;
        }

        /// <summary>
        /// Initialize the Brain part for active combat AI.
        /// This is CRITICAL - without proper Brain initialization, elite variants
        /// will be passive and won't attack even when hostile.
        /// </summary>
    }
}
