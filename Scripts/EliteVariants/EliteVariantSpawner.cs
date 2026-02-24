using System;
using XRL.World;
using XRL.World.Parts;
using XRL.World.ZoneBuilders;

namespace XRL.World.Parts
{
    /// <summary>
    /// IPart that spawns elite variant encounters when triggered.
    /// Creates a single elite creature from any creature in the game/mods.
    /// </summary>
    [Serializable]
    public class EliteVariantSpawner : IPart
    {
        public bool bDynamic = true;  // Allow dynamic spawning
        public bool ForceUltimate = false;  // Force Ultimate tier instead of random
        private bool HasSpawned = false;

        public override bool AllowStaticRegistration()
        {
            return true;
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            XRL.Messages.MessageQueue.AddPlayerMessage("{{M|[DEBUG] EliteVariantSpawner.Register called for " + Object.Blueprint + "}}");
            base.Register(Object, Registrar);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) ||
                   ID == ZoneBuiltEvent.ID ||
                   ID == EnteredCellEvent.ID ||
                   ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ZoneBuiltEvent E)
        {
            XRL.Messages.MessageQueue.AddPlayerMessage("{{Y|[DEBUG] ZoneBuiltEvent fired!}}");
            // Spawn when zone is built (natural generation)
            if (!HasSpawned)
            {
                TrySpawnEncounter();
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EnteredCellEvent E)
        {
            XRL.Messages.MessageQueue.AddPlayerMessage("{{Y|[DEBUG] EnteredCellEvent fired!}}");
            // Spawn when player enters the cell
            if (!HasSpawned && ParentObject.CurrentCell != null)
            {
                TrySpawnEncounter();
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            XRL.Messages.MessageQueue.AddPlayerMessage("{{Y|[DEBUG] ObjectCreatedEvent fired!}}");
            // Try to spawn immediately when object is created
            // Will retry via EnteredCell if cell isn't available yet
            if (!HasSpawned)
            {
                TrySpawnEncounter();
            }
            return base.HandleEvent(E);
        }

        /// <summary>
        /// Attempts to spawn an elite variant encounter
        /// </summary>
        private void TrySpawnEncounter()
        {
            XRL.Messages.MessageQueue.AddPlayerMessage("{{c|[DEBUG] TrySpawnEncounter called}}");

            if (!bDynamic || HasSpawned)
                return;

            Cell cell = ParentObject.CurrentCell;
            if (cell == null)
                return; // Will retry when EnteredCellEvent fires

            HasSpawned = true;

            var rng = new Random();
            Zone zone = cell.ParentZone;

            // Select encounter composition
            var (encounterType, count, useGoons) = EliteVariantConfig.SelectEncounterComposition(rng);

            // Select base creature from pool
            string baseCreature = CreaturePool.SelectRandomCreature(rng);

            XRL.Messages.MessageQueue.AddPlayerMessage($"{{c|[DEBUG] Encounter: {encounterType}, count: {count}}}");

            // Determine tier (use forced tier if specified, otherwise random based on zone)
            EliteTier tier = ForceUltimate
                ? EliteTier.Ultimate
                : EliteVariantConfig.SelectTier(rng, zone?.NewTier ?? 5);
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{c|[DEBUG] Tier: {tier}}}");

            // Calculate appropriate elite level for this zone
            int eliteLevel = EliteVariantGenerator.CalculateEliteLevel(zone, isGoon: false, tier);
            int goonLevel = EliteVariantGenerator.CalculateEliteLevel(zone, isGoon: true, tier);
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{c|[DEBUG] Elite level: {eliteLevel}, Goon level: {goonLevel}}}");

            if (encounterType == EncounterType.MixedElites)
            {
                // Spawn diverse elite team - different creatures, same level
                XRL.Messages.MessageQueue.AddPlayerMessage("{{G|[DEBUG] Spawning mixed elite team}}");
                for (int i = 0; i < count; i++)
                {
                    string creature = CreaturePool.SelectRandomCreature(rng);
                    SpawnEliteCreature(creature, zone, cell, rng, false, eliteLevel, tier, null);
                }
            }
            else if (encounterType == EncounterType.ElitePlusGoons)
            {
                // Spawn 1 leader + goons (use pre-selected baseCreature for solo-only types)
                XRL.Messages.MessageQueue.AddPlayerMessage($"{{G|[DEBUG] Spawning elite + {count} goons}}");
                GameObject leader = SpawnEliteCreature(baseCreature, zone, cell, rng, false, eliteLevel, tier, null);

                for (int i = 0; i < count; i++)
                {
                    GameObject goon = SpawnEliteCreature(baseCreature, zone, cell, rng, true, goonLevel, tier, null);
                    if (goon != null && leader != null)
                    {
                        Brain goonBrain = goon.GetPart<Brain>();
                        if (goonBrain != null)
                        {
                            goonBrain.PartyLeader = leader;
                        }
                    }
                }
            }
            else
            {
                // Solo, Duo, Trio, Pack - identical creatures (use pre-selected baseCreature)
                XRL.Messages.MessageQueue.AddPlayerMessage($"{{G|[DEBUG] Spawning {encounterType} ({count} creatures)}}");
                for (int i = 0; i < count; i++)
                {
                    SpawnEliteCreature(baseCreature, zone, cell, rng, false, eliteLevel, tier, null);
                }
            }

            XRL.Messages.MessageQueue.AddPlayerMessage("{{G|[DEBUG] Spawning complete!}}");
            ParentObject.Destroy();
        }

        /// <summary>
        /// Spawns a single elite creature
        /// </summary>
        private GameObject SpawnEliteCreature(
            string baseCreature,
            Zone zone,
            Cell spawnCell,
            Random rng,
            bool isGoon,
            int level,
            EliteTier tier,
            GameObject leader)
        {
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{c|[DEBUG] Creating {baseCreature} (level {level}, tier {tier})}}");

            GameObject elite = EliteVariantGenerator.CreateEliteVariant(
                baseCreature,
                rng,
                isGoon,
                level,
                tier
            );

            if (elite == null)
            {
                XRL.Messages.MessageQueue.AddPlayerMessage("{{R|[DEBUG] Creation failed!}}");
                return null;
            }

            XRL.Messages.MessageQueue.AddPlayerMessage($"{{G|[DEBUG] Created: {elite.DisplayName}}}");

            // Place creature
            Cell targetCell = spawnCell;
            var adjacent = spawnCell.GetEmptyAdjacentCells();
            if (adjacent != null && adjacent.Count > 0)
            {
                targetCell = adjacent.GetRandomElement(rng);
            }

            targetCell.AddObject(elite);
            return elite;
        }
    }
}
