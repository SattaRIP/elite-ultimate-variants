using System;
using XRL.UI;

namespace XRL.World.Parts
{
    public static class EliteVariantSettings
    {
        // ===== ELITE OPTION IDS =====
        private const string OPTION_ELITE_POWER      = "Option_EliteVariants_ElitePowerMultiplier";
        private const string OPTION_ELITE_HP         = "Option_EliteVariants_EliteHPMultiplier";
        private const string OPTION_ELITE_LEVEL      = "Option_EliteVariants_EliteLevelOffset";
        private const string OPTION_ELITE_CHANCE     = "Option_EliteVariants_EliteChance";
        private const string OPTION_ELITE_ENHANCE    = "Option_EliteVariants_EliteEnhancementMultiplier";
        private const string OPTION_ELITE_ENABLE     = "Option_EliteVariants_EnableEliteSpawning";
        private const string OPTION_ELITE_RATE       = "Option_EliteVariants_EliteSpawnRate";
        private const string OPTION_ELITE_MIN_LEVEL  = "Option_EliteVariants_MinPlayerLevel";
        private const string OPTION_ELITE_MIN_TIER   = "Option_EliteVariants_MinEliteZoneTier";
        private const string OPTION_ELITE_PHYS_CAP   = "Option_EliteVariants_ElitePhysicalMutationCap";
        private const string OPTION_ELITE_MENT_CAP   = "Option_EliteVariants_EliteMentalMutationCap";
        private const string OPTION_ELITE_MIN_ITEMS  = "Option_EliteVariants_EliteMinItemCount";
        private const string OPTION_ELITE_MAX_ITEMS  = "Option_EliteVariants_EliteMaxItemCount";
        private const string OPTION_ELITE_TIER8_PCT  = "Option_EliteVariants_EliteTier8Chance";
        private const string OPTION_ELITE_TIER7_PCT  = "Option_EliteVariants_EliteTier7Chance";

        // ===== ULTIMATE OPTION IDS =====
        private const string OPTION_ULT_POWER        = "Option_EliteVariants_UltimatePowerMultiplier";
        private const string OPTION_ULT_HP           = "Option_EliteVariants_UltimateHPMultiplier";
        private const string OPTION_ULT_LEVEL        = "Option_EliteVariants_UltimateLevelOffset";
        private const string OPTION_ULT_CHANCE       = "Option_EliteVariants_UltimateChance";
        private const string OPTION_ULT_ENHANCE      = "Option_EliteVariants_UltimateEnhancementMultiplier";
        private const string OPTION_ULT_ENABLE       = "Option_EliteVariants_EnableUltimateSpawning";
        private const string OPTION_ULT_RATE         = "Option_EliteVariants_UltimateSpawnRate";
        private const string OPTION_ULT_MIN_LEVEL    = "Option_EliteVariants_MinUltimateLevel";
        private const string OPTION_ULT_MIN_TIER     = "Option_EliteVariants_MinUltimateZoneTier";
        private const string OPTION_ULT_PHYS_CAP     = "Option_EliteVariants_UltimatePhysicalMutationCap";
        private const string OPTION_ULT_MENT_CAP     = "Option_EliteVariants_UltimateMentalMutationCap";
        private const string OPTION_ULT_MIN_ITEMS    = "Option_EliteVariants_UltimateMinItemCount";
        private const string OPTION_ULT_MAX_ITEMS    = "Option_EliteVariants_UltimateMaxItemCount";
        private const string OPTION_ULT_TIER8_PCT    = "Option_EliteVariants_UltimateTier8Chance";
        private const string OPTION_ULT_TIER7_PCT    = "Option_EliteVariants_UltimateTier7Chance";

        // ===== ARMY OPTION IDS =====
        private const string OPTION_ARMY_ENABLE      = "Option_EliteVariants_EnableArmySpawning";
        private const string OPTION_ARMY_WEIGHT      = "Option_EliteVariants_ArmySpawnWeight";
        private const string OPTION_ARMY_MIN_LEVEL   = "Option_EliteVariants_MinPlayerLevelForArmies";
        private const string OPTION_ARMY_MIN_TIER    = "Option_EliteVariants_MinZoneTierForArmies";

        // ===== DEBUG OPTION IDS =====
        private const string OPTION_DEBUG_ENABLE     = "Option_EliteVariants_EnableDebugMode";

        // ===== ELITE PROPERTIES =====

        public static float ElitePowerMultiplier
        {
            get
            {
                try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_POWER, "10")) / 10.0f, 1.0f, 3.0f); } catch { return 1.0f; }
            }
            set { Options.SetOption(OPTION_ELITE_POWER, ((int)Math.Clamp(value * 10f, 10f, 30f)).ToString()); }
        }

        public static float EliteHPMultiplier
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_HP, "15")) / 10.0f, 1.0f, 5.0f); } catch { return 1.5f; } }
            set { Options.SetOption(OPTION_ELITE_HP, ((int)Math.Clamp(value * 10f, 10f, 50f)).ToString()); }
        }

        public static int EliteLevelOffset
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_LEVEL, "0")), 0, 30); } catch { return 0; } }
            set { Options.SetOption(OPTION_ELITE_LEVEL, Math.Clamp(value, 0, 30).ToString()); }
        }

        public static int EliteChance
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_CHANCE, "20")), 0, 100); } catch { return 20; } }
            set { Options.SetOption(OPTION_ELITE_CHANCE, Math.Clamp(value, 0, 100).ToString()); }
        }

        public static float EliteEnhancementMultiplier
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_ENHANCE, "10")) / 10.0f, 1.0f, 3.0f); } catch { return 1.0f; } }
            set { Options.SetOption(OPTION_ELITE_ENHANCE, ((int)Math.Clamp(value * 10f, 10f, 30f)).ToString()); }
        }

        public static bool EnableEliteSpawning
        {
            get { try { return Options.GetOption(OPTION_ELITE_ENABLE, "Yes") == "Yes"; } catch { return true; } }
            set { Options.SetOption(OPTION_ELITE_ENABLE, value ? "Yes" : "No"); }
        }

        public static float EliteSpawnRate
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_RATE, "10")) / 10.0f, 0.5f, 2.0f); } catch { return 1.0f; } }
        }

        public static int MinPlayerLevelForElites
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_MIN_LEVEL, "10")), 1, 30); } catch { return 10; } }
        }

        public static int MinZoneTierForElites
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_MIN_TIER, "4")), 1, 8); } catch { return 4; } }
        }

        public static int ElitePhysicalMutationCap
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_PHYS_CAP, "5")), 0, 20); } catch { return 5; } }
            set { Options.SetOption(OPTION_ELITE_PHYS_CAP, Math.Clamp(value, 0, 20).ToString()); }
        }

        public static int EliteMentalMutationCap
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_MENT_CAP, "3")), 0, 20); } catch { return 3; } }
            set { Options.SetOption(OPTION_ELITE_MENT_CAP, Math.Clamp(value, 0, 20).ToString()); }
        }

        public static int EliteMinItemCount
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_MIN_ITEMS, "2")), 0, 10); } catch { return 2; } }
            set { Options.SetOption(OPTION_ELITE_MIN_ITEMS, Math.Clamp(value, 0, 10).ToString()); }
        }

        public static int EliteMaxItemCount
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_MAX_ITEMS, "4")), 0, 10); } catch { return 4; } }
            set { Options.SetOption(OPTION_ELITE_MAX_ITEMS, Math.Clamp(value, 0, 10).ToString()); }
        }

        public static int EliteTier8Chance
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_TIER8_PCT, "40")), 0, 100); } catch { return 40; } }
            set { Options.SetOption(OPTION_ELITE_TIER8_PCT, Math.Clamp(value, 0, 100).ToString()); }
        }

        public static int EliteTier7Chance
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ELITE_TIER7_PCT, "40")), 0, 100); } catch { return 40; } }
            set { Options.SetOption(OPTION_ELITE_TIER7_PCT, Math.Clamp(value, 0, 100).ToString()); }
        }

        // ===== ULTIMATE PROPERTIES =====

        public static float UltimatePowerMultiplier
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_POWER, "15")) / 10.0f, 1.0f, 6.0f); } catch { return 1.5f; } }
            set { Options.SetOption(OPTION_ULT_POWER, ((int)Math.Clamp(value * 10f, 10f, 60f)).ToString()); }
        }

        public static float UltimateHPMultiplier
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_HP, "20")) / 10.0f, 1.0f, 10.0f); } catch { return 2.0f; } }
            set { Options.SetOption(OPTION_ULT_HP, ((int)Math.Clamp(value * 10f, 10f, 100f)).ToString()); }
        }

        public static int UltimateLevelOffset
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_LEVEL, "5")), 0, 30); } catch { return 5; } }
            set { Options.SetOption(OPTION_ULT_LEVEL, Math.Clamp(value, 0, 30).ToString()); }
        }

        public static int UltimateChance
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_CHANCE, "30")), 0, 100); } catch { return 30; } }
            set { Options.SetOption(OPTION_ULT_CHANCE, Math.Clamp(value, 0, 100).ToString()); }
        }

        public static float UltimateEnhancementMultiplier
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_ENHANCE, "15")) / 10.0f, 1.0f, 6.0f); } catch { return 1.5f; } }
            set { Options.SetOption(OPTION_ULT_ENHANCE, ((int)Math.Clamp(value * 10f, 10f, 60f)).ToString()); }
        }

        public static bool EnableUltimateSpawning
        {
            get { try { return Options.GetOption(OPTION_ULT_ENABLE, "Yes") == "Yes"; } catch { return true; } }
            set { Options.SetOption(OPTION_ULT_ENABLE, value ? "Yes" : "No"); }
        }

        public static float UltimateSpawnRate
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_RATE, "10")) / 10.0f, 0.5f, 2.0f); } catch { return 1.0f; } }
        }

        public static int MinPlayerLevelForUltimates
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_MIN_LEVEL, "20")), 10, 40); } catch { return 20; } }
        }

        public static int MinZoneTierForUltimates
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_MIN_TIER, "6")), 1, 8); } catch { return 6; } }
        }

        public static int UltimatePhysicalMutationCap
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_PHYS_CAP, "7")), 0, 20); } catch { return 7; } }
            set { Options.SetOption(OPTION_ULT_PHYS_CAP, Math.Clamp(value, 0, 20).ToString()); }
        }

        public static int UltimateMentalMutationCap
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_MENT_CAP, "6")), 0, 20); } catch { return 6; } }
            set { Options.SetOption(OPTION_ULT_MENT_CAP, Math.Clamp(value, 0, 20).ToString()); }
        }

        public static int UltimateMinItemCount
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_MIN_ITEMS, "3")), 0, 10); } catch { return 3; } }
            set { Options.SetOption(OPTION_ULT_MIN_ITEMS, Math.Clamp(value, 0, 10).ToString()); }
        }

        public static int UltimateMaxItemCount
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_MAX_ITEMS, "4")), 0, 10); } catch { return 4; } }
            set { Options.SetOption(OPTION_ULT_MAX_ITEMS, Math.Clamp(value, 0, 10).ToString()); }
        }

        public static int UltimateTier8Chance
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_TIER8_PCT, "60")), 0, 100); } catch { return 60; } }
            set { Options.SetOption(OPTION_ULT_TIER8_PCT, Math.Clamp(value, 0, 100).ToString()); }
        }

        public static int UltimateTier7Chance
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ULT_TIER7_PCT, "30")), 0, 100); } catch { return 30; } }
            set { Options.SetOption(OPTION_ULT_TIER7_PCT, Math.Clamp(value, 0, 100).ToString()); }
        }

        // ===== ARMY PROPERTIES =====

        public static bool EnableArmySpawning
        {
            get { return Options.GetOption(OPTION_ARMY_ENABLE, "Yes") == "Yes"; }
            set { Options.SetOption(OPTION_ARMY_ENABLE, value ? "Yes" : "No"); }
        }

        public static float ArmySpawnWeight
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ARMY_WEIGHT, "10")) / 10.0f, 0.5f, 2.0f); } catch { return 1.0f; } }
            set { Options.SetOption(OPTION_ARMY_WEIGHT, ((int)Math.Clamp(value * 10f, 5f, 20f)).ToString()); }
        }

        public static int MinPlayerLevelForArmies
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ARMY_MIN_LEVEL, "15")), 10, 40); } catch { return 15; } }
            set { Options.SetOption(OPTION_ARMY_MIN_LEVEL, Math.Clamp(value, 10, 40).ToString()); }
        }

        public static int MinZoneTierForArmies
        {
            get { try { return Math.Clamp(int.Parse(Options.GetOption(OPTION_ARMY_MIN_TIER, "5")), 4, 8); } catch { return 5; } }
            set { Options.SetOption(OPTION_ARMY_MIN_TIER, Math.Clamp(value, 4, 8).ToString()); }
        }

        // ===== DEBUG PROPERTIES =====

        public static bool EnableDebugMode
        {
            get { return Options.GetOption(OPTION_DEBUG_ENABLE, "No") == "Yes"; }
            set { Options.SetOption(OPTION_DEBUG_ENABLE, value ? "Yes" : "No"); }
        }

        // ===== CONVENIENCE: shared natural spawning check =====

        public static bool EnableNaturalSpawning => EnableEliteSpawning || EnableUltimateSpawning;

        // ===== RESET & DEBUG =====

        public static void ResetToDefaults()
        {
            ElitePowerMultiplier = 1.0f;
            EliteHPMultiplier = 1.5f;
            EliteLevelOffset = 0;
            EliteChance = 20;
            EliteEnhancementMultiplier = 1.0f;
            EnableEliteSpawning = true;

            UltimatePowerMultiplier = 1.5f;
            UltimateHPMultiplier = 2.0f;
            UltimateLevelOffset = 5;
            UltimateChance = 30;
            UltimateEnhancementMultiplier = 1.5f;
            EnableUltimateSpawning = true;
        }

        public static string GetDebugInfo()
        {
            return $"=== Elite Variants Settings ===\n" +
                   $"{{Y|Power:}} {ElitePowerMultiplier:F1}x / {UltimatePowerMultiplier:F1}x\n" +
                   $"{{Y|HP:}} {EliteHPMultiplier:F1}x / {UltimateHPMultiplier:F1}x\n" +
                   $"{{Y|Level:}} +{EliteLevelOffset} / +{UltimateLevelOffset}\n" +
                   $"{{Y|Chance:}} {EliteChance}% / {UltimateChance}%\n" +
                   $"{{Y|Enhancements:}} {EliteEnhancementMultiplier:F1}x / {UltimateEnhancementMultiplier:F1}x\n" +
                   $"{{Y|Spawning:}} {(EnableEliteSpawning ? "{{G|ON}}" : "{{R|OFF}}")} / {(EnableUltimateSpawning ? "{{G|ON}}" : "{{R|OFF}}")}\n" +
                   $"{{Y|Spawn Rate:}} {EliteSpawnRate:F1}x / {UltimateSpawnRate:F1}x\n" +
                   $"{{Y|Min Level:}} {MinPlayerLevelForElites} / {MinPlayerLevelForUltimates}\n" +
                   $"{{Y|Min Tier:}} {MinZoneTierForElites} / {MinZoneTierForUltimates}\n" +
                   $"(Elite / Ultimate)";
        }

        public static bool HandleWishCommand(string command)
        {
            if (command == "elitespawn:status")
            {
                var stats = EliteSpawnController.GetStats();
                string message = $"{{c|Elite Spawn Statistics:}}\n" +
                               $"Total Spawned: {stats.TotalSpawned}\n" +
                               $"Elites in Current Zone: {stats.CurrentZoneCount}\n" +
                               $"Last Spawn: {stats.TurnsSinceLastSpawn} turns ago\n" +
                               $"Elite Spawning: {(EnableEliteSpawning ? "{{G|ON}}" : "{{R|OFF}}")}\n" +
                               $"Ultimate Spawning: {(EnableUltimateSpawning ? "{{G|ON}}" : "{{R|OFF}}")}\n" +
                               $"Elite Rate: {EliteSpawnRate:F1}x | Ultimate Rate: {UltimateSpawnRate:F1}x";
                Popup.Show(message);
                return true;
            }

            if (command == "elitespawn:toggle")
            {
                bool newState = !EnableEliteSpawning;
                EnableEliteSpawning = newState;
                EnableUltimateSpawning = newState;
                Popup.Show($"Natural spawning: {(newState ? "{{G|ENABLED}}" : "{{R|DISABLED}}")}");
                return true;
            }

            if (command == "elitespawn:reset")
            {
                EliteSpawnController.ResetCounters();
                Popup.Show("{{c|Elite spawn counters reset!}}");
                return true;
            }

            return false;
        }
    }
}
