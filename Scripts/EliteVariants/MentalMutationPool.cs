using System;
using System.Collections.Generic;

namespace XRL.World.Parts
{
    public static class MentalMutationPool
    {
        // All mental mutations available for psychic variants
        // Based on Mutations.xml category="Mental"
        private static readonly string[] MentalMutations = new[]
        {
            "Beguiling",
            "Burgeoning",
            "Clairvoyance",
            "Confusion",
            "Cryokinesis",
            "Disintegration",
            "Domination",
            "WillForce",          // Ego Projection
            "ForceBubble",
            "ForceWall",
            "Kindle",
            "LightManipulation",
            "MassMind",
            "MentalMirror",
            "Precognition",
            "Psychometry",
            "Pyrokinesis",
            "SensePsychic",
            "SpacetimeVortex",
            "StunningForce",
            "SunderMind",
            "LifeDrain",          // Syphon Vim
            "Telepathy",
            "Teleportation",
            "TeleportOther",
            "TimeDilation",
            "TemporalFugue"
        };

        // Mutation weights (default 100, time distortion reduced to 25)
        private static readonly Dictionary<string, int> MutationWeights = new Dictionary<string, int>
        {
            // Time distortion mutations (reduced weight)
            { "TimeDilation", 25 },
            { "TemporalFugue", 25 },
            { "SpacetimeVortex", 25 },

            // All other mutations default to 100 (not listed here)
        };

        private const int DefaultMutationWeight = 100;

        // High-tier offensive mutations for leaders
        private static readonly string[] OffensiveMutations = new[]
        {
            "SunderMind",
            "Disintegration",
            "Pyrokinesis",
            "Cryokinesis",
            "StunningForce",
            "TemporalFugue"
        };

        /// <summary>
        /// Get weight for a specific mutation (25 for time distortion, 100 for others)
        /// </summary>
        private static int GetMutationWeight(string mutationName)
        {
            return MutationWeights.ContainsKey(mutationName)
                ? MutationWeights[mutationName]
                : DefaultMutationWeight;
        }

        /// <summary>
        /// Select a mutation from available list using weighted random selection
        /// </summary>
        private static string SelectWeightedMutation(List<string> available, Random rng)
        {
            int totalWeight = 0;
            foreach (var mutation in available)
                totalWeight += GetMutationWeight(mutation);

            int roll = rng.Next(totalWeight);
            int cumulative = 0;

            foreach (var mutation in available)
            {
                cumulative += GetMutationWeight(mutation);
                if (roll < cumulative)
                    return mutation;
            }

            // Fallback (should never happen)
            return available[rng.Next(available.Count)];
        }

        /// <summary>
        /// Select N unique random mutations.
        /// </summary>
        /// <param name="count">Number of mutations to select</param>
        /// <param name="rng">Random number generator</param>
        /// <param name="isGoon">If false (leader), use higher mutation levels and ensure offensive</param>
        /// <returns>List of (MutationName, Level) tuples</returns>
        public static List<(string Name, int Level)> SelectMutations(
            int count,
            Random rng,
            bool isGoon = false)
        {
            var selected = new List<(string, int)>();
            var available = new List<string>(MentalMutations);

            int minLevel = isGoon ? EliteVariantConfig.GoonMutationLevelMin : EliteVariantConfig.LeaderMutationLevelMin;
            int maxLevel = isGoon ? EliteVariantConfig.GoonMutationLevelMax : EliteVariantConfig.LeaderMutationLevelMax;

            // Ensure at least one offensive mutation for leaders
            if (!isGoon && count > 0)
            {
                int offIdx = rng.Next(OffensiveMutations.Length);
                string offMutation = OffensiveMutations[offIdx];
                int level = rng.Next(minLevel, maxLevel + 1);
                selected.Add((offMutation, level));
                available.Remove(offMutation);
                count--;
            }

            // Fill remaining slots using weighted random selection
            while (count > 0 && available.Count > 0)
            {
                string mutation = SelectWeightedMutation(available, rng);
                int level = rng.Next(minLevel, maxLevel + 1);
                selected.Add((mutation, level));
                available.Remove(mutation);
                count--;
            }

            return selected;
        }
    }
}
