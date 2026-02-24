using System;
using System.Collections.Generic;
using System.Linq;

namespace XRL.World.Parts
{
    /// <summary>
    /// Pool of physical mutations for elite variants
    /// Handles selection with exclusion rules
    /// </summary>
    public static class PhysicalMutationPool
    {
        // Standard physical mutations (33 available)
        private static readonly string[] PhysicalMutations = new[]
        {
            "Adrenal Control",
            "Beak",
            "Burrowing Claws",
            "Carapace",
            "Corrosive Gas Generation",
            "Double-muscled",
            "Electrical Generation",
            "Electromagnetic Pulse",
            "Flaming Ray",
            "Freezing Ray",
            "Heightened Hearing",
            "Heightened Quickness",
            "Horns",
            "Multiple Arms",
            "Multiple Legs",
            "Night Vision",
            "Phasing",
            "Photosynthetic Skin",
            "Quills",
            "Regeneration",
            "Sleep Gas Generation",
            "Slime Glands",
            "Spinnerets",
            "Stinger (Confusing Venom)",
            "Stinger (Paralyzing Venom)",
            "Stinger (Poisoning Venom)",
            "Thick Fur",
            "Triple-jointed",
            "Two-headed",
            "Two-hearted",
            "Wings"
        };

        // Elite-tier offensive mutations (preferred for leaders)
        private static readonly string[] OffensiveMutations = new[]
        {
            "Electrical Generation",
            "Flaming Ray",
            "Freezing Ray",
            "Corrosive Gas Generation",
            "Quills",
            "Horns",
            "Multiple Arms"
        };

        // Mutation exclusions (pairs that cannot coexist)
        private static readonly Dictionary<string, string[]> Exclusions = new Dictionary<string, string[]>
        {
            { "Carapace", new[] { "Quills" } },
            { "Quills", new[] { "Carapace" } },
            { "Flaming Ray", new[] { "Freezing Ray" } },
            { "Freezing Ray", new[] { "Flaming Ray" } },
            { "Photosynthetic Skin", new[] { "Albino", "Carnivorous" } }
        };

        // Horns variants for variety
        private static readonly string[] HornsVariants = new[]
        {
            null,               // Default
            "Horns Antlers",
            "Horns Spiral",
            "Horns Oxen"
        };

        /// <summary>
        /// Select random physical mutations ensuring no exclusions
        /// </summary>
        public static List<(string Name, int Level)> SelectMutations(
            int count,
            Random rng,
            bool ensureOffensive,
            HashSet<string> existingMutations)
        {
            var selected = new List<(string Name, int Level)>();
            var available = new List<string>(PhysicalMutations);

            // Remove already-existing mutations
            available.RemoveAll(m => existingMutations.Contains(m));

            // Remove excluded mutations based on existing ones
            foreach (var existing in existingMutations)
            {
                if (Exclusions.ContainsKey(existing))
                {
                    foreach (var excluded in Exclusions[existing])
                    {
                        available.Remove(excluded);
                    }
                }
            }

            if (available.Count == 0)
                return selected;

            // Ensure at least one offensive mutation for leaders
            if (ensureOffensive && count > 0)
            {
                var availableOffensive = OffensiveMutations
                    .Where(m => available.Contains(m))
                    .ToList();

                if (availableOffensive.Count > 0)
                {
                    string offensive = availableOffensive[rng.Next(availableOffensive.Count)];
                    int level = rng.Next(
                        EliteVariantConfig.LeaderMutationLevelMin,
                        EliteVariantConfig.LeaderMutationLevelMax + 1
                    );

                    selected.Add((offensive, level));
                    available.Remove(offensive);
                    count--;

                    // Apply exclusions
                    if (Exclusions.ContainsKey(offensive))
                    {
                        foreach (var excluded in Exclusions[offensive])
                        {
                            available.Remove(excluded);
                        }
                    }
                }
            }

            // Select remaining mutations
            for (int i = 0; i < count && available.Count > 0; i++)
            {
                string mutation = available[rng.Next(available.Count)];
                int level = rng.Next(
                    EliteVariantConfig.LeaderMutationLevelMin,
                    EliteVariantConfig.LeaderMutationLevelMax + 1
                );

                selected.Add((mutation, level));
                available.Remove(mutation);

                // Apply exclusions
                if (Exclusions.ContainsKey(mutation))
                {
                    foreach (var excluded in Exclusions[mutation])
                    {
                        available.Remove(excluded);
                    }
                }
            }

            return selected;
        }

        /// <summary>
        /// Get random variant for Horns mutation
        /// </summary>
        public static string GetHornsVariant(Random rng)
        {
            return HornsVariants[rng.Next(HornsVariants.Length)];
        }

        /// <summary>
        /// Check if a mutation has exclusions with existing mutations
        /// </summary>
        public static bool IsExcluded(string mutation, HashSet<string> existing)
        {
            if (!Exclusions.ContainsKey(mutation))
                return false;

            foreach (var excluded in Exclusions[mutation])
            {
                if (existing.Contains(excluded))
                    return true;
            }

            return false;
        }
    }
}
