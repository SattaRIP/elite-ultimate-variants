using System;
using System.Collections.Generic;
using XRL.Core;
using XRL.Rules;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Spawns elite army encounters (groups of elites/ultimates)
    /// Used in population tables to create special elite encounter groups
    /// </summary>
    [Serializable]
    public class EliteArmySpawner : IPart
    {
        // Set via blueprint XML
        public string ArmyTypeString = "SmallSquad";
        public int MinPlayerLevel = 15;
        public int MinZoneTier = 5;

        // Runtime state
        private bool HasSpawned = false;

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            if (HasSpawned)
            {
                return base.HandleEvent(E);
            }

            try
            {
                // Parse army type from string
                ArmyType armyType;
                if (!EliteArmyConfig.TryParseArmyType(ArmyTypeString, out armyType))
                {
                    // Invalid type, default to SmallSquad
                    armyType = ArmyType.SmallSquad;
                }

                // Check if army can spawn
                if (!CanSpawnArmy(armyType))
                {
                    // Conditions not met, destroy spawner and exit
                    ParentObject.Obliterate();
                    return false;
                }

                // Select creature type for army
                string baseCreature = SelectCreatureForArmy();
                if (string.IsNullOrEmpty(baseCreature))
                {
                    EliteVariantDebug.LogSpawnPrevented("No eligible creature found for army");
                    // No valid creature found
                    ParentObject.Obliterate();
                    return false;
                }

                // Spawn the army
                int leaderCount = 0;
                int goonCount = 0;
                SpawnArmy(armyType, baseCreature, out leaderCount, out goonCount);

                // Mark zone as having army
                Zone zone = ParentObject.CurrentZone;
                if (zone != null)
                {
                    EliteArmyTracker.RecordArmySpawn(zone, armyType);

                    // Debug notification
                    EliteVariantDebug.LogArmySpawn(armyType, baseCreature, leaderCount, goonCount,
                                                   zone.DisplayName, zone.NewTier);
                }

                HasSpawned = true;
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                MetricsManager.LogError("EliteArmySpawner error: " + ex.Message);
            }

            // Destroy spawner object
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        /// <summary>
        /// Check if army can spawn based on level, tier, zone limits, and settings
        /// </summary>
        private bool CanSpawnArmy(ArmyType type)
        {
            // Check if army spawning is enabled
            if (!EliteVariantSettings.EnableArmySpawning)
            {
                EliteVariantDebug.LogSpawnPrevented("Army spawning disabled in settings");
                return false;
            }

            // Get player level
            int playerLevel = 0;
            try
            {
                var player = XRL.Core.XRLCore.Core?.Game?.Player?.Body;
                if (player != null)
                {
                    playerLevel = player.Level;
                }
            }
            catch
            {
                playerLevel = 1;
            }

            // Check global minimum player level for armies
            if (playerLevel < EliteVariantSettings.MinPlayerLevelForArmies)
            {
                EliteVariantDebug.LogSpawnPrevented($"Player level {playerLevel} < army minimum {EliteVariantSettings.MinPlayerLevelForArmies}");
                return false;
            }

            // Get composition for this army type
            var composition = EliteArmyConfig.GetComposition(type);

            // Check type-specific level requirement
            if (playerLevel < composition.MinLevel)
            {
                EliteVariantDebug.LogSpawnPrevented($"Player level {playerLevel} < {type} minimum {composition.MinLevel}");
                return false;
            }

            // Get zone tier
            Zone zone = ParentObject.CurrentZone;
            if (zone == null)
            {
                EliteVariantDebug.LogSpawnPrevented("Zone is null");
                return false;
            }

            int zoneTier = zone.NewTier;

            // Check global minimum zone tier for armies
            if (zoneTier < EliteVariantSettings.MinZoneTierForArmies)
            {
                EliteVariantDebug.LogSpawnPrevented($"Zone tier {zoneTier} < army minimum {EliteVariantSettings.MinZoneTierForArmies}");
                return false;
            }

            // Check type-specific tier requirement
            if (zoneTier < composition.MinTier)
            {
                EliteVariantDebug.LogSpawnPrevented($"Zone tier {zoneTier} < {type} minimum {composition.MinTier}");
                return false;
            }

            // Check if zone already has an army (max 1 per zone)
            if (EliteArmyTracker.HasArmy(zone))
            {
                EliteVariantDebug.LogZoneLimitReached(zone.DisplayName, 1, 1, "army");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Select a creature blueprint for the army
        /// Prioritizes creatures already in the zone for thematic coherence
        /// </summary>
        private string SelectCreatureForArmy()
        {
            Zone zone = ParentObject.CurrentZone;
            if (zone == null)
            {
                return null;
            }

            // Try to find eligible creatures already in this zone
            var eligibleCreatures = new List<string>();

            foreach (var obj in zone.GetObjects())
            {
                if (obj == null || obj == ParentObject)
                    continue;

                // Must be a creature
                if (!obj.HasTag("Creature"))
                    continue;

                // Skip players
                if (obj.IsPlayer())
                    continue;

                // Check if eligible for elite transformation
                if (!EliteSpawnEvaluator.IsEligibleForEliteTransform(obj.Blueprint))
                    continue;

                // Add to candidate list
                if (!eligibleCreatures.Contains(obj.Blueprint))
                {
                    eligibleCreatures.Add(obj.Blueprint);
                }
            }

            // If we found creatures in zone, pick random one
            if (eligibleCreatures.Count > 0)
            {
                return eligibleCreatures.GetRandomElement();
            }

            // Fallback: use creature pool
            return CreaturePool.SelectRandomCreature(new Random());
        }

        /// <summary>
        /// Spawn the elite army
        /// </summary>
        private void SpawnArmy(ArmyType type, string baseCreature, out int actualLeaders, out int actualGoons)
        {
            actualLeaders = 0;
            actualGoons = 0;

            var composition = EliteArmyConfig.GetComposition(type);
            var rng = new Random();

            Zone zone = ParentObject.CurrentZone;
            if (zone == null)
            {
                return;
            }

            // Get spawner location
            string spawnCell = ParentObject.CurrentCell?.ParentZone?.ZoneID;
            int spawnX = ParentObject.CurrentCell?.X ?? 40;
            int spawnY = ParentObject.CurrentCell?.Y ?? 25;

            var leaders = new List<GameObject>();
            var goons = new List<GameObject>();

            // Determine actual counts (random within min/max range)
            int leaderCount = rng.Next(composition.MinLeaders, composition.MaxLeaders + 1);
            int goonCount = rng.Next(composition.MinGoons, composition.MaxGoons + 1);

            // Spawn leaders (ultimates)
            for (int i = 0; i < leaderCount; i++)
            {
                var leader = SpawnArmyMember(baseCreature, EliteTier.Ultimate, false, zone, spawnX, spawnY);
                if (leader != null)
                {
                    leaders.Add(leader);
                }
            }

            // Spawn goons (elites)
            for (int i = 0; i < goonCount; i++)
            {
                var goon = SpawnArmyMember(baseCreature, EliteTier.Elite, true, zone, spawnX, spawnY);
                if (goon != null)
                {
                    goons.Add(goon);

                    // Assign goon to a random leader
                    if (leaders.Count > 0)
                    {
                        var leader = leaders.GetRandomElement(rng);
                        SetPartyLeader(goon, leader);
                    }
                }
            }

            // Return actual spawned counts
            actualLeaders = leaders.Count;
            actualGoons = goons.Count;
        }

        /// <summary>
        /// Spawn a single army member (elite or ultimate)
        /// </summary>
        private GameObject SpawnArmyMember(string baseCreature, EliteTier tier, bool isGoon,
                                          Zone zone, int centerX, int centerY)
        {
            try
            {
                // Create elite variant
                var elite = EliteVariantGenerator.CreateEliteVariant(
                    baseCreature,
                    new Random(),
                    isGoon,
                    customLevel: null,
                    customTier: tier
                );

                if (elite == null)
                {
                    return null;
                }

                // Find nearby spawn location (within 3 cells of center)
                Cell spawnCell = FindNearbySpawnCell(zone, centerX, centerY, radius: 3);

                if (spawnCell != null)
                {
                    // Add to zone at spawn location
                    spawnCell.AddObject(elite);
                }
                else
                {
                    // Fallback: try to add at center
                    zone.GetCell(centerX, centerY)?.AddObject(elite);
                }

                return elite;
            }
            catch (Exception ex)
            {
                MetricsManager.LogError("Error spawning army member: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Find a nearby empty cell for spawning
        /// </summary>
        private Cell FindNearbySpawnCell(Zone zone, int centerX, int centerY, int radius)
        {
            if (zone == null)
                return null;

            var rng = new Random();
            var candidates = new List<Cell>();

            // Search in radius around center
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    int x = centerX + dx;
                    int y = centerY + dy;

                    // Bounds check
                    if (x < 0 || x >= 80 || y < 0 || y >= 25)
                        continue;

                    Cell cell = zone.GetCell(x, y);
                    if (cell == null)
                        continue;

                    // Check if cell is suitable for spawning
                    if (cell.IsEmpty() && !cell.IsSolid())
                    {
                        candidates.Add(cell);
                    }
                }
            }

            // Return random candidate
            if (candidates.Count > 0)
            {
                return candidates.GetRandomElement(rng);
            }

            return null;
        }

        /// <summary>
        /// Set party leader relationship for goon
        /// </summary>
        private void SetPartyLeader(GameObject goon, GameObject leader)
        {
            if (goon == null || leader == null)
                return;

            try
            {
                Brain goonBrain = goon.GetPart<Brain>();
                if (goonBrain != null)
                {
                    goonBrain.PartyLeader = leader;

                    // PartyLeader relationship handles following behavior
                    // No need to explicitly add follow goal
                }
            }
            catch (Exception ex)
            {
                // Non-critical error, continue
                MetricsManager.LogError("Error setting party leader: " + ex.Message);
            }
        }
    }
}
