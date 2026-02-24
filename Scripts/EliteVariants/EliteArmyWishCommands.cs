using System;
using XRL.UI;
using XRL.World;
using XRL.Rules;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command: elitearmy:status
    /// Shows army spawn statistics
    /// </summary>
    [Serializable]
    public class EliteArmyStatusCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            string stats = EliteArmyTracker.GetStatsString();
            Popup.Show(stats);
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }

    /// <summary>
    /// Wish command: elitearmy:reset
    /// Resets army tracking data
    /// </summary>
    [Serializable]
    public class EliteArmyResetCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteArmyTracker.Reset();
            Popup.Show("{{G|Elite army tracking data reset.}}");
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }

    /// <summary>
    /// Wish command: spawnarmy [type]
    /// Force spawns an elite army of specified type
    /// Usage: spawnarmy small, spawnarmy medium, spawnarmy large, spawnarmy epic, spawnarmy titans
    /// </summary>
    [Serializable]
    public class SpawnArmyCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            try
            {
                // Get player
                var player = XRL.Core.XRLCore.Core?.Game?.Player?.Body;
                if (player == null || player.CurrentCell == null)
                {
                    Popup.Show("{{r|Error: Player not found}}");
                    ParentObject.Obliterate();
                    return base.HandleEvent(E);
                }

                // Prompt for army type
                string[] options = new string[]
                {
                    "SmallSquad (2-3 elites)",
                    "ElitePatrol (4-5 elites)",
                    "EliteWarband (1 ult + 4-5 elites)",
                    "UltimateHost (2 ults + 6-8 elites)",
                    "TitanPair (2-3 ultimates)"
                };

                int choice = Popup.ShowOptionList(
                    Title: "Select Army Type",
                    Options: options,
                    AllowEscape: true
                );

                if (choice < 0)
                {
                    Popup.Show("{{y|Cancelled}}");
                    ParentObject.Obliterate();
                    return base.HandleEvent(E);
                }

                // Map choice to army type
                ArmyType armyType = ArmyType.SmallSquad;
                switch (choice)
                {
                    case 0: armyType = ArmyType.SmallSquad; break;
                    case 1: armyType = ArmyType.ElitePatrol; break;
                    case 2: armyType = ArmyType.EliteWarband; break;
                    case 3: armyType = ArmyType.UltimateHost; break;
                    case 4: armyType = ArmyType.TitanPair; break;
                }

                // Select creature type
                string baseCreature = CreaturePool.SelectRandomCreature(new Random());
                if (string.IsNullOrEmpty(baseCreature))
                {
                    Popup.Show("{{r|Error: Failed to select creature type}}");
                    ParentObject.Obliterate();
                    return base.HandleEvent(E);
                }

                // Get composition
                var composition = EliteArmyConfig.GetComposition(armyType);
                var rng = new Random();

                // Spawn army near player
                Zone zone = player.CurrentZone;
                int spawnX = player.CurrentCell.X;
                int spawnY = player.CurrentCell.Y;

                var leaders = new System.Collections.Generic.List<GameObject>();
                var goons = new System.Collections.Generic.List<GameObject>();

                // Determine counts
                int leaderCount = rng.Next(composition.MinLeaders, composition.MaxLeaders + 1);
                int goonCount = rng.Next(composition.MinGoons, composition.MaxGoons + 1);

                // Spawn leaders
                for (int i = 0; i < leaderCount; i++)
                {
                    var leader = EliteVariantGenerator.CreateEliteVariant(
                        baseCreature,
                        rng,
                        isGoon: false,
                        customTier: EliteTier.Ultimate
                    );

                    if (leader != null)
                    {
                        Cell spawnCell = FindNearbyCell(zone, spawnX, spawnY, 5);
                        if (spawnCell != null)
                        {
                            spawnCell.AddObject(leader);
                            leaders.Add(leader);
                        }
                    }
                }

                // Spawn goons
                for (int i = 0; i < goonCount; i++)
                {
                    var goon = EliteVariantGenerator.CreateEliteVariant(
                        baseCreature,
                        rng,
                        isGoon: true,
                        customTier: EliteTier.Elite
                    );

                    if (goon != null)
                    {
                        Cell spawnCell = FindNearbyCell(zone, spawnX, spawnY, 5);
                        if (spawnCell != null)
                        {
                            spawnCell.AddObject(goon);
                            goons.Add(goon);

                            // Assign to random leader
                            if (leaders.Count > 0)
                            {
                                var leader = leaders.GetRandomElement(rng);
                                Brain goonBrain = goon.GetPart<Brain>();
                                if (goonBrain != null)
                                {
                                    goonBrain.PartyLeader = leader;
                                }
                            }
                        }
                    }
                }

                // Mark zone
                EliteArmyTracker.RecordArmySpawn(zone, armyType);

                // Show success message
                string desc = EliteArmyConfig.GetDescription(armyType);
                Popup.Show($"{{G|Spawned {armyType}:}} {desc}\n{{Y|Creature:}} {baseCreature}");
            }
            catch (Exception ex)
            {
                Popup.Show($"{{r|Error:}} {ex.Message}");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        /// <summary>
        /// Find nearby empty cell for spawning
        /// </summary>
        private Cell FindNearbyCell(Zone zone, int centerX, int centerY, int radius)
        {
            if (zone == null)
                return null;

            var rng = new Random();

            for (int attempt = 0; attempt < 20; attempt++)
            {
                int x = centerX + rng.Next(-radius, radius + 1);
                int y = centerY + rng.Next(-radius, radius + 1);

                if (x < 0 || x >= 80 || y < 0 || y >= 25)
                    continue;

                Cell cell = zone.GetCell(x, y);
                if (cell != null && cell.IsEmpty() && !cell.IsSolid())
                {
                    return cell;
                }
            }

            // Fallback: just return center cell
            return zone.GetCell(centerX, centerY);
        }
    }
}
