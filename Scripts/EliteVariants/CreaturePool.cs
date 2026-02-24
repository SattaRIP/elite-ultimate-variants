using System;
using System.Collections.Generic;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Manages pool of valid creatures for elite variant generation
    /// Scans ALL creatures from base game + mods
    /// </summary>
    public static class CreaturePool
    {
        private static List<string> _cachedCreatures = null;

        /// <summary>
        /// Get all valid creature blueprints for elite variant generation
        /// Results are cached for performance
        /// </summary>
        public static List<string> GetAllValidCreatures()
        {
            if (_cachedCreatures != null)
                return _cachedCreatures;

            _cachedCreatures = new List<string>();

            foreach (var kvp in GameObjectFactory.Factory.Blueprints)
            {
                var blueprint = kvp.Value;
                string name = kvp.Key;

                // Must be a creature
                if (!blueprint.HasTag("Creature"))
                    continue;

                // Skip base objects (templates)
                if (blueprint.HasTag("BaseObject"))
                    continue;

                // Skip excluded creatures
                if (blueprint.HasTag("ExcludeFromDynamicEncounters"))
                    continue;

                // Skip bosses and unique creatures
                if (blueprint.HasTag("IgnoresPhase") ||
                    blueprint.HasTag("IgnoresBeguiling") ||
                    blueprint.HasTag("CannotBeUnique"))
                    continue;

                // Skip existing special types
                if (name.Contains("Cherub") ||
                    name.Contains("Sightless") ||
                    name.Contains("Corpse") ||
                    name.Contains("Bite") ||
                    name.Contains("Claw") ||
                    name.Contains("Fist") ||
                    name.Contains("Tendril") ||
                    name.Contains("Pseudopod") ||
                    name.Contains("Weep") ||          // Skip any weep-named variants
                    name.Contains("weep") ||          // Skip any weep-named variants (lowercase)
                    name.Contains("Fluid Carrier") || // Skip fluid carriers
                    name.Contains("Mayor") ||         // Skip mayors
                    name.Contains("EliteVariant") ||  // Don't create elites of elites!
                    name.Contains("Spawn"))
                    continue;

                // Skip anything with a shop/restock part (covers all merchant types)
                if (blueprint.HasPart("Restocker"))
                    continue;

                // Skip merchants, traders, sellers, wardens, and service NPCs by name
                if (name.Contains("Dromad") ||
                    name.Contains("Merchant") ||
                    name.Contains("Trader") ||
                    name.Contains("Vendor") ||
                    name.Contains("Seller") ||
                    name.Contains("Warden") ||
                    name.Contains("Chef") ||
                    name.Contains("Oven") ||
                    name.Contains("Cook") ||
                    name.Contains("Tinker") ||
                    name.Contains("Apothecary") ||
                    name.Contains("Gunsmith") ||
                    name.Contains("Armorer") ||
                    name.Contains("Armourer") ||
                    name.Contains("Barber") ||
                    name.Contains("Farmer") ||
                    name.Contains("Bard") ||
                    name.Contains("Scribe") ||
                    name.Contains("Guard") ||
                    name.Contains("Militia"))
                    continue;

                // Skip non-hostile plants and fungus
                if (name.Contains("Puff") ||
                    name.Contains("Puffer") ||
                    name.Contains("Watervine") ||
                    name.Contains("Aloe Volta") ||
                    name.Contains("Aloe Pyra") ||
                    name.Contains("Thirst thistle") ||
                    name.Contains("Yurl") ||
                    name.Contains("Asphodel"))
                    continue;

                // Skip robots (they don't have biological mutations)
                if (blueprint.InheritsFrom("BaseRobot"))
                    continue;

                // Skip all liquid weeps - blueprint names are "[liquid]Lichen" / "[liquid]Lichen Minor",
                // not "Weep", so name filters above never catch them
                if (blueprint.InheritsFrom("LiquidLichen") || blueprint.InheritsFrom("LiquidLichen Minor"))
                    continue;

                // Skip immobile/non-threatening creatures
                if (blueprint.HasTag("NoThreatTracking"))
                    continue;

                _cachedCreatures.Add(name);
            }

            // Log creature count for debugging
            XRL.Messages.MessageQueue.AddPlayerMessage(
                $"{{c|[EliteVariants] Scanned {_cachedCreatures.Count} valid creatures}}"
            );

            return _cachedCreatures;
        }

        /// <summary>
        /// Select random creature from valid pool
        /// </summary>
        public static string SelectRandomCreature(Random rng)
        {
            var creatures = GetAllValidCreatures();

            if (creatures.Count == 0)
            {
                // Fallback to some default creatures if pool is empty
                string[] fallbacks = new[]
                {
                    "Snapjaw Scavenger",
                    "Goatfolk Shaman",
                    "Young Ivoryaut",
                    "Troll Foal"
                };
                return fallbacks[rng.Next(fallbacks.Length)];
            }

            return creatures[rng.Next(creatures.Count)];
        }

        /// <summary>
        /// Check if creature should only spawn as solo/leader (traders, wardens, etc.)
        /// </summary>
        public static bool IsSoloOnlyCreature(string creatureName)
        {
            return creatureName.Contains("Trader") ||
                   creatureName.Contains("Warden") ||
                   creatureName.Contains("Merchant") ||
                   creatureName.Contains("Vendor") ||
                   creatureName.Contains("Dromad");
        }

        /// <summary>
        /// Force rebuild of creature cache (useful if mods change)
        /// </summary>
        public static void RebuildCache()
        {
            _cachedCreatures = null;
        }
    }
}
