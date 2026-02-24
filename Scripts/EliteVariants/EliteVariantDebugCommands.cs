using System;
using XRL.UI;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command: elitedebug:toggle
    /// Toggles debug mode on/off
    /// </summary>
    [Serializable]
    public class EliteDebugToggleCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            bool newState = !EliteVariantSettings.EnableDebugMode;
            EliteVariantSettings.EnableDebugMode = newState;

            string statusMsg = newState ? "{{G|ENABLED}}" : "{{R|DISABLED}}";
            Popup.Show($"{{c|Elite Variants Debug Mode:}} {statusMsg}");

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }

    /// <summary>
    /// Wish command: elitedebug:status
    /// Shows debug mode status and current settings
    /// </summary>
    [Serializable]
    public class EliteDebugStatusCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            bool debugEnabled = EliteVariantSettings.EnableDebugMode;
            string statusColor = debugEnabled ? "G" : "R";
            string statusText = debugEnabled ? "ENABLED" : "DISABLED";

            string message = $"{{c|=== Elite Variants Debug Status ===}}\n\n" +
                           $"{{Y|Debug Mode:}} {{{statusColor}|{statusText}}}\n\n" +
                           $"{{c|Elite Settings:}}\n" +
                           $"  Power: {{W|{EliteVariantSettings.ElitePowerMultiplier:F1}x}}\n" +
                           $"  HP: {{W|{EliteVariantSettings.EliteHPMultiplier:F1}x}}\n" +
                           $"  Enhancement: {{W|{EliteVariantSettings.EliteEnhancementMultiplier:F1}x}}\n" +
                           $"  Spawn Rate: {{W|{EliteVariantSettings.EliteSpawnRate:F1}x}}\n" +
                           $"  Min Level: {{W|{EliteVariantSettings.MinPlayerLevelForElites}}}\n" +
                           $"  Min Tier: {{W|{EliteVariantSettings.MinZoneTierForElites}}}\n\n" +
                           $"{{c|Ultimate Settings:}}\n" +
                           $"  Power: {{W|{EliteVariantSettings.UltimatePowerMultiplier:F1}x}}\n" +
                           $"  HP: {{W|{EliteVariantSettings.UltimateHPMultiplier:F1}x}}\n" +
                           $"  Enhancement: {{W|{EliteVariantSettings.UltimateEnhancementMultiplier:F1}x}}\n" +
                           $"  Spawn Rate: {{W|{EliteVariantSettings.UltimateSpawnRate:F1}x}}\n" +
                           $"  Min Level: {{W|{EliteVariantSettings.MinPlayerLevelForUltimates}}}\n" +
                           $"  Min Tier: {{W|{EliteVariantSettings.MinZoneTierForUltimates}}}\n\n" +
                           $"{{c|Army Settings:}}\n" +
                           $"  Enabled: {{{(EliteVariantSettings.EnableArmySpawning ? "G|YES" : "R|NO")}}}\n" +
                           $"  Spawn Weight: {{W|{EliteVariantSettings.ArmySpawnWeight:F1}x}}\n" +
                           $"  Min Level: {{W|{EliteVariantSettings.MinPlayerLevelForArmies}}}\n" +
                           $"  Min Tier: {{W|{EliteVariantSettings.MinZoneTierForArmies}}}";

            Popup.Show(message);

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }

    /// <summary>
    /// Wish command: elitedebug:stats
    /// Shows comprehensive spawn statistics
    /// </summary>
    [Serializable]
    public class EliteDebugStatsCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            // Get elite spawn stats
            var eliteStats = EliteSpawnController.GetStats();

            // Get army stats
            string armyStats = EliteArmyTracker.GetStatsString();

            // Get current zone info
            Zone currentZone = The.Player?.CurrentZone;
            string zoneName = currentZone?.DisplayName ?? "Unknown";
            int zoneTier = currentZone?.NewTier ?? 0;
            int playerLevel = The.Player?.Level ?? 1;

            string message = $"{{c|=== Elite Variants Statistics ===}}\n\n" +
                           $"{{Y|Current Zone:}} {zoneName} (Tier {zoneTier})\n" +
                           $"{{Y|Player Level:}} {playerLevel}\n\n" +
                           $"{{c|Elite Spawns:}}\n" +
                           $"  Total Spawned: {{W|{eliteStats.TotalSpawned}}}\n" +
                           $"  In Current Zone: {{W|{eliteStats.CurrentZoneCount}}}\n" +
                           $"  Turns Since Last: {{W|{eliteStats.TurnsSinceLastSpawn}}}\n\n" +
                           $"{{c|Army Spawns:}}\n{armyStats}";

            Popup.Show(message);

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }

    /// <summary>
    /// Wish command: elitedebug:bypass
    /// Bypasses level/tier requirements for testing (toggles on/off)
    /// Note: This would require additional implementation in spawn evaluators
    /// For now, just shows a message about using spawnarmy/spawnelite commands
    /// </summary>
    [Serializable]
    public class EliteDebugBypassCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            Popup.Show(
                "{{c|Debug Bypass Help:}}\n\n" +
                "{{Y|To force spawn elites/armies for testing:}}\n\n" +
                "{{W|spawnelite}} - Spawn an elite variant\n" +
                "{{W|spawnultimate}} - Spawn an ultimate variant\n" +
                "{{W|spawnarmy}} - Spawn an elite army\n\n" +
                "{{y|These commands bypass all level/tier requirements.}}"
            );

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }
}
