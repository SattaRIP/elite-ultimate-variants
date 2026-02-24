using System;
using System.Collections.Generic;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command: spawnelite
    /// Spawns a random Elite-tier creature near the player.
    /// Elite = white/silver color (&Y), moderate power.
    /// </summary>
    [Serializable]
    public class SpawnEliteCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            SpawnCreatureWithTier(EliteTier.Elite);
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        private void SpawnCreatureWithTier(EliteTier tier)
        {
            var rng = new Random();

            // Select a random creature
            string baseCreature = CreaturePool.SelectRandomCreature(rng);

            // Calculate level based on player's zone
            Zone zone = The.Player?.CurrentZone;
            int level = EliteVariantGenerator.CalculateEliteLevel(zone, isGoon: false, tier);

            // Create the elite variant with explicit tier
            GameObject elite = EliteVariantGenerator.CreateEliteVariant(
                baseCreature,
                rng,
                isGoon: false,
                customLevel: level,
                customTier: tier
            );

            if (elite == null)
            {
                XRL.Messages.MessageQueue.AddPlayerMessage("{{R|[Elite Variants] Failed to create elite creature.}}");
                return;
            }

            // Place near player
            Cell playerCell = The.Player?.CurrentCell;
            if (playerCell != null)
            {
                List<Cell> adjacent = playerCell.GetEmptyAdjacentCells();
                Cell targetCell = (adjacent != null && adjacent.Count > 0) ? adjacent[0] : playerCell;
                targetCell.AddObject(elite);

                string tierName = tier == EliteTier.Ultimate ? "Ultimate" : "Elite";
                string color = tier == EliteTier.Ultimate ? "W" : "Y";
                XRL.Messages.MessageQueue.AddPlayerMessage(
                    $"{{{{{color}|[Elite Variants] Spawned {tierName}: {elite.DisplayName} (Level {level})}}}}");
            }
            else
            {
                XRL.Messages.MessageQueue.AddPlayerMessage("{{R|[Elite Variants] No valid spawn location found.}}");
            }
        }
    }

    /// <summary>
    /// Wish command: spawnultimate
    /// Spawns a random Ultimate-tier creature near the player.
    /// Ultimate = gold color (&W), high power.
    /// </summary>
    [Serializable]
    public class SpawnUltimateCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            SpawnCreatureWithTier(EliteTier.Ultimate);
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        private void SpawnCreatureWithTier(EliteTier tier)
        {
            var rng = new Random();

            // Select a random creature
            string baseCreature = CreaturePool.SelectRandomCreature(rng);

            // Calculate level based on player's zone
            Zone zone = The.Player?.CurrentZone;
            int level = EliteVariantGenerator.CalculateEliteLevel(zone, isGoon: false, tier);

            // Create the elite variant with explicit tier
            GameObject elite = EliteVariantGenerator.CreateEliteVariant(
                baseCreature,
                rng,
                isGoon: false,
                customLevel: level,
                customTier: tier
            );

            if (elite == null)
            {
                XRL.Messages.MessageQueue.AddPlayerMessage("{{R|[Elite Variants] Failed to create ultimate creature.}}");
                return;
            }

            // Place near player
            Cell playerCell = The.Player?.CurrentCell;
            if (playerCell != null)
            {
                List<Cell> adjacent = playerCell.GetEmptyAdjacentCells();
                Cell targetCell = (adjacent != null && adjacent.Count > 0) ? adjacent[0] : playerCell;
                targetCell.AddObject(elite);

                string tierName = tier == EliteTier.Ultimate ? "Ultimate" : "Elite";
                string color = tier == EliteTier.Ultimate ? "W" : "Y";
                XRL.Messages.MessageQueue.AddPlayerMessage(
                    $"{{{{{color}|[Elite Variants] Spawned {tierName}: {elite.DisplayName} (Level {level})}}}}");
            }
            else
            {
                XRL.Messages.MessageQueue.AddPlayerMessage("{{R|[Elite Variants] No valid spawn location found.}}");
            }
        }
    }
}
