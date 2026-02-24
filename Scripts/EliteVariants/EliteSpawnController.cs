using System;
using System.Collections.Generic;
using XRL.Core;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    /// <summary>
    /// Global controller for natural elite variant spawning
    /// Intercepts creature creation and transforms eligible creatures into elites
    /// </summary>
    [Serializable]
    [HasGameBasedStaticCache]
    public class EliteSpawnController : IPart
    {
        // Zone tracking (per-zone elite counts)
        [GameBasedStaticCache]
        private static Dictionary<string, int> _elitesPerZone = new Dictionary<string, int>();

        // Global cooldown tracking
        [GameBasedStaticCache]
        private static int _lastEliteSpawnTurn = 0;

        // Total spawned counter for stats
        [GameBasedStaticCache]
        private static int _totalElitesSpawned = 0;

        // Prevent infinite loop during transformation
        [NonSerialized]
        private static bool _isTransforming = false;

        /// <summary>
        /// Initialize controller on game load
        /// </summary>
        [CallAfterGameLoaded]
        public static new void Initialize()
        {
            if (!EliteVariantSettings.EnableNaturalSpawning)
                return;

            // Clear caches
            EliteSpawnEvaluator.ClearCache();
            _elitesPerZone.Clear();
            _lastEliteSpawnTurn = 0;
            _totalElitesSpawned = 0;

            // Create persistent controller object
            try
            {
                var controller = GameObjectFactory.Factory.CreateObject("EliteSpawnController");
                if (controller != null)
                {
                    controller.MakeActive(); // Keep in memory across zones
                    XRL.Messages.MessageQueue.AddPlayerMessage("{{c|[EliteVariants] Natural spawning enabled}}");
                }
            }
            catch (Exception ex)
            {
                XRL.Messages.MessageQueue.AddPlayerMessage($"{{R|[EliteVariants] Failed to initialize: {ex.Message}}}");
            }
        }

        public override bool AllowStaticRegistration()
        {
            return true;
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("ObjectCreated");
            Registrar.Register("ZoneActivated");
            base.Register(Object, Registrar);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == ObjectCreatedEvent.ID
                || ID == ZoneActivatedEvent.ID;
        }

        /// <summary>
        /// Handle creature creation - primary transformation mechanism
        /// </summary>
        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            // CRITICAL: Prevent infinite loops
            if (_isTransforming)
                return base.HandleEvent(E);

            if (E.ReplacementObject != null)
                return base.HandleEvent(E);

            if (E.Object == null)
                return base.HandleEvent(E);

            if (E.Object.GetIntProperty("IsEliteVariant") == 1)
                return base.HandleEvent(E);

            // Only process creatures
            if (!E.Object.HasTag("Creature"))
                return base.HandleEvent(E);

            // Check if we should transform this creature
            _isTransforming = true;
            try
            {
                Zone zone = E.Object.CurrentZone;

                if (EliteSpawnEvaluator.ShouldTransformToElite(
                    E.Object,
                    zone,
                    out EliteTier tier,
                    _elitesPerZone,
                    _lastEliteSpawnTurn))
                {
                    // Create elite variant
                    var rng = new Random();
                    int eliteLevel = EliteVariantGenerator.CalculateEliteLevel(zone, false);

                    GameObject elite = EliteVariantGenerator.CreateEliteVariant(
                        E.Object.Blueprint,
                        rng,
                        false,
                        eliteLevel,
                        tier
                    );

                    if (elite != null)
                    {
                        // Replace original creature with elite
                        E.ReplacementObject = elite;

                        // Update tracking
                        RecordEliteSpawn(zone);

                        // Debug notification (verbose)
                        if (EliteVariantSettings.EnableDebugMode)
                        {
                            string zoneName = zone?.DisplayName ?? "Unknown Zone";
                            int zoneTier = zone?.NewTier ?? 0;
                            EliteVariantDebug.LogEliteSpawn(elite, tier, zoneName, zoneTier);
                        }
                        else
                        {
                            // Normal message
                            string tierName = tier == EliteTier.Ultimate ? "ultimate" : "elite";
                            XRL.Messages.MessageQueue.AddPlayerMessage(
                                $"{{c|[EliteVariants] Spawned {tierName} {elite.GetDisplayName(0, null, null, AsIfKnown: true, Single: true)}}}",
                                'c'
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XRL.Messages.MessageQueue.AddPlayerMessage($"{{R|[EliteVariants] Transform error: {ex.Message}}}");
            }
            finally
            {
                _isTransforming = false;
            }

            return base.HandleEvent(E);
        }

        /// <summary>
        /// Handle zone activation - batch processing opportunity
        /// </summary>
        public override bool HandleEvent(ZoneActivatedEvent E)
        {
            // Optional: Could implement batch zone processing here
            // For now, ObjectCreatedEvent handles everything
            return base.HandleEvent(E);
        }

        /// <summary>
        /// Record elite spawn in zone tracking
        /// </summary>
        private void RecordEliteSpawn(Zone zone)
        {
            if (zone == null)
                return;

            string zoneId = zone.ZoneID;
            if (!_elitesPerZone.ContainsKey(zoneId))
                _elitesPerZone[zoneId] = 0;

            _elitesPerZone[zoneId]++;
            _lastEliteSpawnTurn = (int)(The.Game?.Turns ?? 0);
            _totalElitesSpawned++;
        }

        /// <summary>
        /// Get spawn statistics (for wish commands)
        /// </summary>
        public static SpawnStats GetStats()
        {
            Zone currentZone = The.Player?.CurrentZone;
            int currentZoneCount = 0;
            if (currentZone != null)
            {
                currentZoneCount = _elitesPerZone.GetValueOrDefault(currentZone.ZoneID, 0);
            }

            int currentTurn = (int)(The.Game?.Turns ?? 0);
            int turnsSince = currentTurn - _lastEliteSpawnTurn;

            return new SpawnStats
            {
                TotalSpawned = _totalElitesSpawned,
                CurrentZoneCount = currentZoneCount,
                TurnsSinceLastSpawn = turnsSince
            };
        }

        /// <summary>
        /// Reset counters (for wish commands)
        /// </summary>
        public static void ResetCounters()
        {
            _elitesPerZone.Clear();
            _lastEliteSpawnTurn = 0;
            _totalElitesSpawned = 0;
            EliteSpawnEvaluator.ClearCache();
        }
    }

    /// <summary>
    /// Spawn statistics structure
    /// </summary>
    public struct SpawnStats
    {
        public int TotalSpawned;
        public int CurrentZoneCount;
        public int TurnsSinceLastSpawn;
    }
}
