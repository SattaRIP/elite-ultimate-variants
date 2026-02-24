using System;
using System.Collections.Generic;
using System.Text;
using XRL.UI;
using XRL.World;
using XRL.Rules;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command: elitequick:equipment
    /// Spawns 5 elites with different equipment configurations
    /// </summary>
    [Serializable]
    public class EliteQuickEquipmentCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            try
            {
                var player = The.Player;
                if (player == null || player.CurrentCell == null)
                {
                    Popup.Show("{{r|Error: Player not found}}");
                    ParentObject.Obliterate();
                    return base.HandleEvent(E);
                }

                var rng = new Random();
                string baseCreature = CreaturePool.SelectRandomCreature(rng);
                Zone zone = player.CurrentZone;
                int level = EliteVariantGenerator.CalculateEliteLevel(zone, false, EliteTier.Elite);

                // Save current settings
                int oldMin = EliteVariantSettings.EliteMinItemCount;
                int oldMax = EliteVariantSettings.EliteMaxItemCount;

                var configs = new List<(int min, int max, string desc)>
                {
                    (0, 0, "No equipment (0/0)"),
                    (2, 2, "Exactly 2 items (2/2)"),
                    (4, 4, "Exactly 4 items (4/4)"),
                    (1, 3, "Variable 1-3 items (1/3)"),
                    (3, 6, "Variable 3-6 items (3/6)")
                };

                StringBuilder popup = new StringBuilder();
                popup.AppendLine("{{W|Equipment Test Spawned:}}");
                popup.AppendLine($"{{c|Base Creature:}} {baseCreature}");
                popup.AppendLine();
                popup.AppendLine("{{Y|Expected Equipment:}}");

                int spawnCount = 0;
                foreach (var config in configs)
                {
                    // Set configuration
                    EliteVariantSettings.EliteMinItemCount = config.min;
                    EliteVariantSettings.EliteMaxItemCount = config.max;

                    // Create elite
                    GameObject elite = EliteVariantGenerator.CreateEliteVariant(
                        baseCreature,
                        rng,
                        isGoon: false,
                        customLevel: level,
                        customTier: EliteTier.Elite
                    );

                    if (elite != null)
                    {
                        Cell spawnCell = FindNearbyCell(player.CurrentCell, 3 + spawnCount);
                        if (spawnCell != null)
                        {
                            spawnCell.AddObject(elite);
                            popup.AppendLine($"  {{Y|{spawnCount + 1}.}} {config.desc}");
                            spawnCount++;
                        }
                    }
                }

                // Restore settings
                EliteVariantSettings.EliteMinItemCount = oldMin;
                EliteVariantSettings.EliteMaxItemCount = oldMax;

                popup.AppendLine();
                popup.AppendLine("{{G|Examine each elite to verify equipment count.}}");
                Popup.Show(popup.ToString());
            }
            catch (Exception ex)
            {
                Popup.Show($"{{r|Error:}} {ex.Message}");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        private Cell FindNearbyCell(Cell center, int offset)
        {
            if (center == null || center.ParentZone == null)
                return null;

            Zone zone = center.ParentZone;
            var rng = new Random();

            for (int attempt = 0; attempt < 20; attempt++)
            {
                int x = center.X + rng.Next(-offset, offset + 1);
                int y = center.Y + rng.Next(-offset, offset + 1);

                if (x < 0 || x >= 80 || y < 0 || y >= 25)
                    continue;

                Cell cell = zone.GetCell(x, y);
                if (cell != null && cell.IsEmpty() && !cell.IsSolid())
                {
                    return cell;
                }
            }

            return center;
        }
    }

    /// <summary>
    /// Wish command: elitequick:mutations
    /// Spawns 5 elites with different mutation cap configurations
    /// </summary>
    [Serializable]
    public class EliteQuickMutationsCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            try
            {
                var player = The.Player;
                if (player == null || player.CurrentCell == null)
                {
                    Popup.Show("{{r|Error: Player not found}}");
                    ParentObject.Obliterate();
                    return base.HandleEvent(E);
                }

                var rng = new Random();
                string baseCreature = CreaturePool.SelectRandomCreature(rng);
                Zone zone = player.CurrentZone;
                int level = EliteVariantGenerator.CalculateEliteLevel(zone, false, EliteTier.Elite);

                // Save current settings
                int oldPhys = EliteVariantSettings.ElitePhysicalMutationCap;
                int oldMent = EliteVariantSettings.EliteMentalMutationCap;

                var configs = new List<(int phys, int ment, string desc)>
                {
                    (0, 0, "No mutations (0 phys / 0 mental)"),
                    (3, 3, "Light mutations (3 phys / 3 mental)"),
                    (10, 10, "Heavy mutations (10 phys / 10 mental)"),
                    (20, 20, "Max mutations (20 phys / 20 mental)"),
                    (0, 20, "Mental only (0 phys / 20 mental)")
                };

                StringBuilder popup = new StringBuilder();
                popup.AppendLine("{{W|Mutation Caps Test Spawned:}}");
                popup.AppendLine($"{{c|Base Creature:}} {baseCreature}");
                popup.AppendLine();
                popup.AppendLine("{{Y|Expected Mutation Caps:}}");

                int spawnCount = 0;
                foreach (var config in configs)
                {
                    // Set configuration
                    EliteVariantSettings.ElitePhysicalMutationCap = config.phys;
                    EliteVariantSettings.EliteMentalMutationCap = config.ment;

                    // Create elite
                    GameObject elite = EliteVariantGenerator.CreateEliteVariant(
                        baseCreature,
                        rng,
                        isGoon: false,
                        customLevel: level,
                        customTier: EliteTier.Elite
                    );

                    if (elite != null)
                    {
                        Cell spawnCell = FindNearbyCell(player.CurrentCell, 3 + spawnCount);
                        if (spawnCell != null)
                        {
                            spawnCell.AddObject(elite);
                            popup.AppendLine($"  {{Y|{spawnCount + 1}.}} {config.desc}");
                            spawnCount++;
                        }
                    }
                }

                // Restore settings
                EliteVariantSettings.ElitePhysicalMutationCap = oldPhys;
                EliteVariantSettings.EliteMentalMutationCap = oldMent;

                popup.AppendLine();
                popup.AppendLine("{{G|Examine each elite to verify mutation counts.}}");
                Popup.Show(popup.ToString());
            }
            catch (Exception ex)
            {
                Popup.Show($"{{r|Error:}} {ex.Message}");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        private Cell FindNearbyCell(Cell center, int offset)
        {
            if (center == null || center.ParentZone == null)
                return null;

            Zone zone = center.ParentZone;
            var rng = new Random();

            for (int attempt = 0; attempt < 20; attempt++)
            {
                int x = center.X + rng.Next(-offset, offset + 1);
                int y = center.Y + rng.Next(-offset, offset + 1);

                if (x < 0 || x >= 80 || y < 0 || y >= 25)
                    continue;

                Cell cell = zone.GetCell(x, y);
                if (cell != null && cell.IsEmpty() && !cell.IsSolid())
                {
                    return cell;
                }
            }

            return center;
        }
    }

    /// <summary>
    /// Wish command: elitequick:tiers
    /// Spawns 3 elites/ultimates showing tier differences
    /// </summary>
    [Serializable]
    public class EliteQuickTiersCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            try
            {
                var player = The.Player;
                if (player == null || player.CurrentCell == null)
                {
                    Popup.Show("{{r|Error: Player not found}}");
                    ParentObject.Obliterate();
                    return base.HandleEvent(E);
                }

                var rng = new Random();
                string baseCreature = CreaturePool.SelectRandomCreature(rng);
                Zone zone = player.CurrentZone;

                // Save current settings
                float oldElitePower = EliteVariantSettings.ElitePowerMultiplier;
                float oldEliteHP = EliteVariantSettings.EliteHPMultiplier;
                float oldEliteEnhance = EliteVariantSettings.EliteEnhancementMultiplier;
                float oldUltPower = EliteVariantSettings.UltimatePowerMultiplier;
                float oldUltHP = EliteVariantSettings.UltimateHPMultiplier;
                float oldUltEnhance = EliteVariantSettings.UltimateEnhancementMultiplier;

                var configs = new List<(EliteTier tier, float power, float hp, float enhance, string desc)>
                {
                    (EliteTier.Elite, 1.0f, 1.5f, 1.0f, "Elite (default: 1.0x power / 1.5x HP / 1.0x enhance)"),
                    (EliteTier.Ultimate, 1.5f, 2.0f, 1.5f, "Ultimate (default: 1.5x power / 2.0x HP / 1.5x enhance)"),
                    (EliteTier.Ultimate, 6.0f, 10.0f, 6.0f, "Ultimate (max: 6.0x power / 10.0x HP / 6.0x enhance)")
                };

                StringBuilder popup = new StringBuilder();
                popup.AppendLine("{{W|Tier Comparison Test Spawned:}}");
                popup.AppendLine($"{{c|Base Creature:}} {baseCreature}");
                popup.AppendLine();
                popup.AppendLine("{{Y|Expected Stats:}}");

                int spawnCount = 0;
                foreach (var config in configs)
                {
                    // Set configuration
                    if (config.tier == EliteTier.Elite)
                    {
                        EliteVariantSettings.ElitePowerMultiplier = config.power;
                        EliteVariantSettings.EliteHPMultiplier = config.hp;
                        EliteVariantSettings.EliteEnhancementMultiplier = config.enhance;
                    }
                    else
                    {
                        EliteVariantSettings.UltimatePowerMultiplier = config.power;
                        EliteVariantSettings.UltimateHPMultiplier = config.hp;
                        EliteVariantSettings.UltimateEnhancementMultiplier = config.enhance;
                    }

                    int level = EliteVariantGenerator.CalculateEliteLevel(zone, false, config.tier);

                    // Create elite
                    GameObject elite = EliteVariantGenerator.CreateEliteVariant(
                        baseCreature,
                        rng,
                        isGoon: false,
                        customLevel: level,
                        customTier: config.tier
                    );

                    if (elite != null)
                    {
                        Cell spawnCell = FindNearbyCell(player.CurrentCell, 3 + spawnCount);
                        if (spawnCell != null)
                        {
                            spawnCell.AddObject(elite);
                            popup.AppendLine($"  {{Y|{spawnCount + 1}.}} {config.desc}");
                            spawnCount++;
                        }
                    }
                }

                // Restore settings
                EliteVariantSettings.ElitePowerMultiplier = oldElitePower;
                EliteVariantSettings.EliteHPMultiplier = oldEliteHP;
                EliteVariantSettings.EliteEnhancementMultiplier = oldEliteEnhance;
                EliteVariantSettings.UltimatePowerMultiplier = oldUltPower;
                EliteVariantSettings.UltimateHPMultiplier = oldUltHP;
                EliteVariantSettings.UltimateEnhancementMultiplier = oldUltEnhance;

                popup.AppendLine();
                popup.AppendLine("{{G|Examine stats to verify multipliers are applied.}}");
                Popup.Show(popup.ToString());
            }
            catch (Exception ex)
            {
                Popup.Show($"{{r|Error:}} {ex.Message}");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        private Cell FindNearbyCell(Cell center, int offset)
        {
            if (center == null || center.ParentZone == null)
                return null;

            Zone zone = center.ParentZone;
            var rng = new Random();

            for (int attempt = 0; attempt < 20; attempt++)
            {
                int x = center.X + rng.Next(-offset, offset + 1);
                int y = center.Y + rng.Next(-offset, offset + 1);

                if (x < 0 || x >= 80 || y < 0 || y >= 25)
                    continue;

                Cell cell = zone.GetCell(x, y);
                if (cell != null && cell.IsEmpty() && !cell.IsSolid())
                {
                    return cell;
                }
            }

            return center;
        }
    }

    /// <summary>
    /// Wish command: elitequick:armies
    /// Spawns all 5 army types in sequence
    /// </summary>
    [Serializable]
    public class EliteQuickArmiesCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            try
            {
                var player = The.Player;
                if (player == null || player.CurrentCell == null)
                {
                    Popup.Show("{{r|Error: Player not found}}");
                    ParentObject.Obliterate();
                    return base.HandleEvent(E);
                }

                var rng = new Random();
                string baseCreature = CreaturePool.SelectRandomCreature(rng);
                Zone zone = player.CurrentZone;

                var armyTypes = new ArmyType[]
                {
                    ArmyType.SmallSquad,
                    ArmyType.ElitePatrol,
                    ArmyType.EliteWarband,
                    ArmyType.UltimateHost,
                    ArmyType.TitanPair
                };

                StringBuilder popup = new StringBuilder();
                popup.AppendLine("{{W|Army Types Test Spawned:}}");
                popup.AppendLine($"{{c|Base Creature:}} {baseCreature}");
                popup.AppendLine();
                popup.AppendLine("{{Y|Expected Compositions:}}");

                int offsetMultiplier = 0;
                foreach (var armyType in armyTypes)
                {
                    var composition = EliteArmyConfig.GetComposition(armyType);
                    string desc = EliteArmyConfig.GetDescription(armyType);

                    // Determine counts
                    int leaderCount = rng.Next(composition.MinLeaders, composition.MaxLeaders + 1);
                    int goonCount = rng.Next(composition.MinGoons, composition.MaxGoons + 1);

                    // Spawn leaders
                    for (int i = 0; i < leaderCount; i++)
                    {
                        GameObject leader = EliteVariantGenerator.CreateEliteVariant(
                            baseCreature,
                            rng,
                            isGoon: false,
                            customTier: EliteTier.Ultimate
                        );

                        if (leader != null)
                        {
                            Cell spawnCell = FindNearbyCell(player.CurrentCell, 5 + offsetMultiplier * 3);
                            if (spawnCell != null)
                            {
                                spawnCell.AddObject(leader);
                            }
                        }
                    }

                    // Spawn goons
                    for (int i = 0; i < goonCount; i++)
                    {
                        GameObject goon = EliteVariantGenerator.CreateEliteVariant(
                            baseCreature,
                            rng,
                            isGoon: true,
                            customTier: EliteTier.Elite
                        );

                        if (goon != null)
                        {
                            Cell spawnCell = FindNearbyCell(player.CurrentCell, 5 + offsetMultiplier * 3);
                            if (spawnCell != null)
                            {
                                spawnCell.AddObject(goon);
                            }
                        }
                    }

                    popup.AppendLine($"  {{Y|{armyType}:}} {desc}");
                    popup.AppendLine($"    {{c|Spawned:}} {leaderCount} ultimate(s), {goonCount} elite(s)");
                    offsetMultiplier++;
                }

                popup.AppendLine();
                popup.AppendLine("{{G|Look around to see all army types.}}");
                Popup.Show(popup.ToString());
            }
            catch (Exception ex)
            {
                Popup.Show($"{{r|Error:}} {ex.Message}");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        private Cell FindNearbyCell(Cell center, int offset)
        {
            if (center == null || center.ParentZone == null)
                return null;

            Zone zone = center.ParentZone;
            var rng = new Random();

            for (int attempt = 0; attempt < 20; attempt++)
            {
                int x = center.X + rng.Next(-offset, offset + 1);
                int y = center.Y + rng.Next(-offset, offset + 1);

                if (x < 0 || x >= 80 || y < 0 || y >= 25)
                    continue;

                Cell cell = zone.GetCell(x, y);
                if (cell != null && cell.IsEmpty() && !cell.IsSolid())
                {
                    return cell;
                }
            }

            return center;
        }
    }

    /// <summary>
    /// Wish command: elitequick:stress
    /// Spawn 10 random elites/ultimates to stress test
    /// </summary>
    [Serializable]
    public class EliteQuickStressCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            try
            {
                var player = The.Player;
                if (player == null || player.CurrentCell == null)
                {
                    Popup.Show("{{r|Error: Player not found}}");
                    ParentObject.Obliterate();
                    return base.HandleEvent(E);
                }

                var rng = new Random();
                Zone zone = player.CurrentZone;

                StringBuilder popup = new StringBuilder();
                popup.AppendLine("{{W|Stress Test: Spawning 10 Random Elites/Ultimates}}");
                popup.AppendLine();
                popup.AppendLine("{{Y|Spawned:}}");

                int eliteCount = 0;
                int ultimateCount = 0;

                for (int i = 0; i < 10; i++)
                {
                    // Random creature
                    string baseCreature = CreaturePool.SelectRandomCreature(rng);

                    // Random tier
                    EliteTier tier = (rng.Next(2) == 0) ? EliteTier.Elite : EliteTier.Ultimate;
                    if (tier == EliteTier.Elite)
                        eliteCount++;
                    else
                        ultimateCount++;

                    // Random settings variations
                    if (rng.Next(3) == 0)
                    {
                        EliteVariantSettings.EliteMinItemCount = rng.Next(0, 6);
                        EliteVariantSettings.EliteMaxItemCount = rng.Next(EliteVariantSettings.EliteMinItemCount, 10);
                    }

                    if (rng.Next(3) == 0)
                    {
                        EliteVariantSettings.ElitePhysicalMutationCap = rng.Next(0, 15);
                        EliteVariantSettings.EliteMentalMutationCap = rng.Next(0, 15);
                    }

                    int level = EliteVariantGenerator.CalculateEliteLevel(zone, false, tier);

                    // Create elite
                    GameObject elite = EliteVariantGenerator.CreateEliteVariant(
                        baseCreature,
                        rng,
                        isGoon: false,
                        customLevel: level,
                        customTier: tier
                    );

                    if (elite != null)
                    {
                        Cell spawnCell = FindNearbyCell(player.CurrentCell, 3 + i);
                        if (spawnCell != null)
                        {
                            spawnCell.AddObject(elite);
                        }
                    }
                }

                popup.AppendLine($"  {{Y|Elites:}} {eliteCount}");
                popup.AppendLine($"  {{W|Ultimates:}} {ultimateCount}");
                popup.AppendLine();
                popup.AppendLine("{{G|Random settings were applied. Check creatures for variety.}}");
                Popup.Show(popup.ToString());
            }
            catch (Exception ex)
            {
                Popup.Show($"{{r|Error:}} {ex.Message}");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        private Cell FindNearbyCell(Cell center, int offset)
        {
            if (center == null || center.ParentZone == null)
                return null;

            Zone zone = center.ParentZone;
            var rng = new Random();

            for (int attempt = 0; attempt < 20; attempt++)
            {
                int x = center.X + rng.Next(-offset, offset + 1);
                int y = center.Y + rng.Next(-offset, offset + 1);

                if (x < 0 || x >= 80 || y < 0 || y >= 25)
                    continue;

                Cell cell = zone.GetCell(x, y);
                if (cell != null && cell.IsEmpty() && !cell.IsSolid())
                {
                    return cell;
                }
            }

            return center;
        }
    }

    /// <summary>
    /// Wish command: elitequick:reset
    /// Reset all settings to defaults and clear all tracking
    /// </summary>
    [Serializable]
    public class EliteQuickResetCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            try
            {
                // Reset settings
                EliteVariantSettings.ResetToDefaults();

                // Clear spawn controller counters
                EliteSpawnController.ResetCounters();

                // Clear army tracker counters
                EliteArmyTracker.Reset();

                StringBuilder popup = new StringBuilder();
                popup.AppendLine("{{G|Elite Variants Reset Complete:}}");
                popup.AppendLine();
                popup.AppendLine("{{Y|Settings:}} Reset to defaults");
                popup.AppendLine("{{Y|Spawn Counters:}} Cleared");
                popup.AppendLine("{{Y|Army Tracker:}} Cleared");
                popup.AppendLine();
                popup.AppendLine("{{c|All systems restored to default state.}}");

                Popup.Show(popup.ToString());
            }
            catch (Exception ex)
            {
                Popup.Show($"{{r|Error:}} {ex.Message}");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }
}
