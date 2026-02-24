using System;
using XRL.UI;

namespace XRL.World.Parts
{
    /// <summary>
    /// Handles difficulty preset application for Elite Variants
    /// Presets are applied via the 'elitepreset' wish command only
    /// </summary>
    public static class EliteVariantPresets
    {

        /// <summary>
        /// Apply a difficulty preset
        /// </summary>
        private static void ApplyPreset(string preset)
        {
            switch (preset)
            {
                case "Easy":
                    ApplyEasyPreset();
                    break;
                case "Normal":
                    ApplyNormalPreset();
                    break;
                case "Hard":
                    ApplyHardPreset();
                    break;
                case "Brutal":
                    ApplyBrutalPreset();
                    break;
                case "Nightmare":
                    ApplyNightmarePreset();
                    break;
            }
        }

        private static void ApplyEasyPreset()
        {
            // Spawn Chances (25% of max)
            Options.SetOption("Option_EliteVariants_EliteChance", "30");
            Options.SetOption("Option_EliteVariants_UltimateChance", "25");

            // Elite: 1.5x/2.0x/1.5x (25% of range)
            Options.SetOption("Option_EliteVariants_ElitePowerMultiplier", "15");
            Options.SetOption("Option_EliteVariants_EliteHPMultiplier", "20");
            Options.SetOption("Option_EliteVariants_EliteEnhancementMultiplier", "15");
            Options.SetOption("Option_EliteVariants_EliteLevelOffset", "8");

            // Ultimate: 2.2x/3.0x/2.2x (25% of range)
            Options.SetOption("Option_EliteVariants_UltimatePowerMultiplier", "22");
            Options.SetOption("Option_EliteVariants_UltimateHPMultiplier", "30");
            Options.SetOption("Option_EliteVariants_UltimateEnhancementMultiplier", "22");
            Options.SetOption("Option_EliteVariants_UltimateLevelOffset", "8");

            // Spawn rates (25% of range)
            Options.SetOption("Option_EliteVariants_EliteSpawnRate", "9");
            Options.SetOption("Option_EliteVariants_UltimateSpawnRate", "9");

            // Mutation caps (25% of max)
            Options.SetOption("Option_EliteVariants_ElitePhysicalMutationCap", "5");
            Options.SetOption("Option_EliteVariants_EliteMentalMutationCap", "5");
            Options.SetOption("Option_EliteVariants_UltimatePhysicalMutationCap", "5");
            Options.SetOption("Option_EliteVariants_UltimateMentalMutationCap", "5");

            // Equipment (fewer items, lower tiers)
            Options.SetOption("Option_EliteVariants_EliteMinItemCount", "0");
            Options.SetOption("Option_EliteVariants_EliteMaxItemCount", "2");
            Options.SetOption("Option_EliteVariants_EliteTier8Chance", "10");
            Options.SetOption("Option_EliteVariants_EliteTier7Chance", "30");
            Options.SetOption("Option_EliteVariants_UltimateMinItemCount", "1");
            Options.SetOption("Option_EliteVariants_UltimateMaxItemCount", "2");
            Options.SetOption("Option_EliteVariants_UltimateTier8Chance", "20");
            Options.SetOption("Option_EliteVariants_UltimateTier7Chance", "40");

            // Requirements (INVERSE - higher values = easier, spawn later)
            Options.SetOption("Option_EliteVariants_MinPlayerLevel", "23");
            Options.SetOption("Option_EliteVariants_MinEliteZoneTier", "6");
            Options.SetOption("Option_EliteVariants_MinUltimateLevel", "32");
            Options.SetOption("Option_EliteVariants_MinUltimateZoneTier", "6");

            // Army settings
            Options.SetOption("Option_EliteVariants_ArmySpawnWeight", "9");
            Options.SetOption("Option_EliteVariants_MinPlayerLevelForArmies", "32");
            Options.SetOption("Option_EliteVariants_MinZoneTierForArmies", "7");
        }

        private static void ApplyNormalPreset()
        {
            // Spawn Chances (50% of max)
            Options.SetOption("Option_EliteVariants_EliteChance", "50");
            Options.SetOption("Option_EliteVariants_UltimateChance", "50");

            // Elite: 2.0x/3.0x/2.0x (50% of range)
            Options.SetOption("Option_EliteVariants_ElitePowerMultiplier", "20");
            Options.SetOption("Option_EliteVariants_EliteHPMultiplier", "30");
            Options.SetOption("Option_EliteVariants_EliteEnhancementMultiplier", "20");
            Options.SetOption("Option_EliteVariants_EliteLevelOffset", "15");

            // Ultimate: 3.5x/5.5x/3.5x (50% of range)
            Options.SetOption("Option_EliteVariants_UltimatePowerMultiplier", "35");
            Options.SetOption("Option_EliteVariants_UltimateHPMultiplier", "55");
            Options.SetOption("Option_EliteVariants_UltimateEnhancementMultiplier", "35");
            Options.SetOption("Option_EliteVariants_UltimateLevelOffset", "15");

            // Spawn rates (50% of range)
            Options.SetOption("Option_EliteVariants_EliteSpawnRate", "12");
            Options.SetOption("Option_EliteVariants_UltimateSpawnRate", "12");

            // Mutation caps (50% of max)
            Options.SetOption("Option_EliteVariants_ElitePhysicalMutationCap", "10");
            Options.SetOption("Option_EliteVariants_EliteMentalMutationCap", "10");
            Options.SetOption("Option_EliteVariants_UltimatePhysicalMutationCap", "10");
            Options.SetOption("Option_EliteVariants_UltimateMentalMutationCap", "10");

            // Equipment (balanced)
            Options.SetOption("Option_EliteVariants_EliteMinItemCount", "1");
            Options.SetOption("Option_EliteVariants_EliteMaxItemCount", "3");
            Options.SetOption("Option_EliteVariants_EliteTier8Chance", "30");
            Options.SetOption("Option_EliteVariants_EliteTier7Chance", "40");
            Options.SetOption("Option_EliteVariants_UltimateMinItemCount", "2");
            Options.SetOption("Option_EliteVariants_UltimateMaxItemCount", "3");
            Options.SetOption("Option_EliteVariants_UltimateTier8Chance", "50");
            Options.SetOption("Option_EliteVariants_UltimateTier7Chance", "35");

            // Requirements (INVERSE - mid values)
            Options.SetOption("Option_EliteVariants_MinPlayerLevel", "16");
            Options.SetOption("Option_EliteVariants_MinEliteZoneTier", "5");
            Options.SetOption("Option_EliteVariants_MinUltimateLevel", "25");
            Options.SetOption("Option_EliteVariants_MinUltimateZoneTier", "5");

            // Army settings
            Options.SetOption("Option_EliteVariants_ArmySpawnWeight", "12");
            Options.SetOption("Option_EliteVariants_MinPlayerLevelForArmies", "25");
            Options.SetOption("Option_EliteVariants_MinZoneTierForArmies", "6");
        }

        private static void ApplyHardPreset()
        {
            // Spawn Chances (75% of max)
            Options.SetOption("Option_EliteVariants_EliteChance", "75");
            Options.SetOption("Option_EliteVariants_UltimateChance", "75");

            // Elite: 2.5x/4.0x/2.5x (75% of range)
            Options.SetOption("Option_EliteVariants_ElitePowerMultiplier", "25");
            Options.SetOption("Option_EliteVariants_EliteHPMultiplier", "40");
            Options.SetOption("Option_EliteVariants_EliteEnhancementMultiplier", "25");
            Options.SetOption("Option_EliteVariants_EliteLevelOffset", "22");

            // Ultimate: 4.8x/8.0x/4.8x (75% of range)
            Options.SetOption("Option_EliteVariants_UltimatePowerMultiplier", "48");
            Options.SetOption("Option_EliteVariants_UltimateHPMultiplier", "80");
            Options.SetOption("Option_EliteVariants_UltimateEnhancementMultiplier", "48");
            Options.SetOption("Option_EliteVariants_UltimateLevelOffset", "22");

            // Spawn rates (75% of range)
            Options.SetOption("Option_EliteVariants_EliteSpawnRate", "16");
            Options.SetOption("Option_EliteVariants_UltimateSpawnRate", "16");

            // Mutation caps (75% of max)
            Options.SetOption("Option_EliteVariants_ElitePhysicalMutationCap", "15");
            Options.SetOption("Option_EliteVariants_EliteMentalMutationCap", "15");
            Options.SetOption("Option_EliteVariants_UltimatePhysicalMutationCap", "15");
            Options.SetOption("Option_EliteVariants_UltimateMentalMutationCap", "15");

            // Equipment (more items, higher tiers)
            Options.SetOption("Option_EliteVariants_EliteMinItemCount", "2");
            Options.SetOption("Option_EliteVariants_EliteMaxItemCount", "4");
            Options.SetOption("Option_EliteVariants_EliteTier8Chance", "50");
            Options.SetOption("Option_EliteVariants_EliteTier7Chance", "35");
            Options.SetOption("Option_EliteVariants_UltimateMinItemCount", "3");
            Options.SetOption("Option_EliteVariants_UltimateMaxItemCount", "4");
            Options.SetOption("Option_EliteVariants_UltimateTier8Chance", "70");
            Options.SetOption("Option_EliteVariants_UltimateTier7Chance", "25");

            // Requirements (INVERSE - lower values = earlier spawns = harder)
            Options.SetOption("Option_EliteVariants_MinPlayerLevel", "8");
            Options.SetOption("Option_EliteVariants_MinEliteZoneTier", "3");
            Options.SetOption("Option_EliteVariants_MinUltimateLevel", "18");
            Options.SetOption("Option_EliteVariants_MinUltimateZoneTier", "3");

            // Army settings
            Options.SetOption("Option_EliteVariants_ArmySpawnWeight", "16");
            Options.SetOption("Option_EliteVariants_MinPlayerLevelForArmies", "18");
            Options.SetOption("Option_EliteVariants_MinZoneTierForArmies", "5");
        }

        private static void ApplyBrutalPreset()
        {
            // Spawn Chances (87.5% of max)
            Options.SetOption("Option_EliteVariants_EliteChance", "90");
            Options.SetOption("Option_EliteVariants_UltimateChance", "90");

            // Elite: 2.8x/4.5x/2.8x (87.5% of range)
            Options.SetOption("Option_EliteVariants_ElitePowerMultiplier", "28");
            Options.SetOption("Option_EliteVariants_EliteHPMultiplier", "45");
            Options.SetOption("Option_EliteVariants_EliteEnhancementMultiplier", "28");
            Options.SetOption("Option_EliteVariants_EliteLevelOffset", "26");

            // Ultimate: 5.4x/9.0x/5.4x (87.5% of range)
            Options.SetOption("Option_EliteVariants_UltimatePowerMultiplier", "54");
            Options.SetOption("Option_EliteVariants_UltimateHPMultiplier", "90");
            Options.SetOption("Option_EliteVariants_UltimateEnhancementMultiplier", "54");
            Options.SetOption("Option_EliteVariants_UltimateLevelOffset", "26");

            // Spawn rates (87.5% of range)
            Options.SetOption("Option_EliteVariants_EliteSpawnRate", "18");
            Options.SetOption("Option_EliteVariants_UltimateSpawnRate", "18");

            // Mutation caps (87.5% of max)
            Options.SetOption("Option_EliteVariants_ElitePhysicalMutationCap", "18");
            Options.SetOption("Option_EliteVariants_EliteMentalMutationCap", "18");
            Options.SetOption("Option_EliteVariants_UltimatePhysicalMutationCap", "18");
            Options.SetOption("Option_EliteVariants_UltimateMentalMutationCap", "18");

            // Equipment (near maximum)
            Options.SetOption("Option_EliteVariants_EliteMinItemCount", "3");
            Options.SetOption("Option_EliteVariants_EliteMaxItemCount", "4");
            Options.SetOption("Option_EliteVariants_EliteTier8Chance", "70");
            Options.SetOption("Option_EliteVariants_EliteTier7Chance", "25");
            Options.SetOption("Option_EliteVariants_UltimateMinItemCount", "4");
            Options.SetOption("Option_EliteVariants_UltimateMaxItemCount", "4");
            Options.SetOption("Option_EliteVariants_UltimateTier8Chance", "85");
            Options.SetOption("Option_EliteVariants_UltimateTier7Chance", "15");

            // Requirements (INVERSE - very low values = very early spawns)
            Options.SetOption("Option_EliteVariants_MinPlayerLevel", "5");
            Options.SetOption("Option_EliteVariants_MinEliteZoneTier", "2");
            Options.SetOption("Option_EliteVariants_MinUltimateLevel", "14");
            Options.SetOption("Option_EliteVariants_MinUltimateZoneTier", "2");

            // Army settings
            Options.SetOption("Option_EliteVariants_ArmySpawnWeight", "18");
            Options.SetOption("Option_EliteVariants_MinPlayerLevelForArmies", "14");
            Options.SetOption("Option_EliteVariants_MinZoneTierForArmies", "5");
        }

        private static void ApplyNightmarePreset()
        {
            // Spawn Chances (100% - maximum)
            Options.SetOption("Option_EliteVariants_EliteChance", "100");
            Options.SetOption("Option_EliteVariants_UltimateChance", "100");

            // Elite: 3.0x/5.0x/3.0x (maxed)
            Options.SetOption("Option_EliteVariants_ElitePowerMultiplier", "30");
            Options.SetOption("Option_EliteVariants_EliteHPMultiplier", "50");
            Options.SetOption("Option_EliteVariants_EliteEnhancementMultiplier", "30");
            Options.SetOption("Option_EliteVariants_EliteLevelOffset", "30");

            // Ultimate: 6.0x/10.0x/6.0x (maxed)
            Options.SetOption("Option_EliteVariants_UltimatePowerMultiplier", "60");
            Options.SetOption("Option_EliteVariants_UltimateHPMultiplier", "100");
            Options.SetOption("Option_EliteVariants_UltimateEnhancementMultiplier", "60");
            Options.SetOption("Option_EliteVariants_UltimateLevelOffset", "30");

            // Spawn rates (maximum)
            Options.SetOption("Option_EliteVariants_EliteSpawnRate", "20");
            Options.SetOption("Option_EliteVariants_UltimateSpawnRate", "20");

            // Mutation caps (unlimited)
            Options.SetOption("Option_EliteVariants_ElitePhysicalMutationCap", "0");
            Options.SetOption("Option_EliteVariants_EliteMentalMutationCap", "0");
            Options.SetOption("Option_EliteVariants_UltimatePhysicalMutationCap", "0");
            Options.SetOption("Option_EliteVariants_UltimateMentalMutationCap", "0");

            // Equipment (maximum - all items, all Tier 8)
            Options.SetOption("Option_EliteVariants_EliteMinItemCount", "4");
            Options.SetOption("Option_EliteVariants_EliteMaxItemCount", "4");
            Options.SetOption("Option_EliteVariants_EliteTier8Chance", "100");
            Options.SetOption("Option_EliteVariants_EliteTier7Chance", "0");
            Options.SetOption("Option_EliteVariants_UltimateMinItemCount", "4");
            Options.SetOption("Option_EliteVariants_UltimateMaxItemCount", "4");
            Options.SetOption("Option_EliteVariants_UltimateTier8Chance", "100");
            Options.SetOption("Option_EliteVariants_UltimateTier7Chance", "0");

            // Requirements (INVERSE - minimum values = earliest possible spawns)
            Options.SetOption("Option_EliteVariants_MinPlayerLevel", "1");
            Options.SetOption("Option_EliteVariants_MinEliteZoneTier", "1");
            Options.SetOption("Option_EliteVariants_MinUltimateLevel", "10");
            Options.SetOption("Option_EliteVariants_MinUltimateZoneTier", "1");

            // Army settings (maximum frequency, earliest spawns)
            Options.SetOption("Option_EliteVariants_ArmySpawnWeight", "20");
            Options.SetOption("Option_EliteVariants_MinPlayerLevelForArmies", "10");
            Options.SetOption("Option_EliteVariants_MinZoneTierForArmies", "4");
        }

        /// <summary>
        /// Force apply a preset immediately (called by elitepreset wish command)
        /// </summary>
        public static void ForceApplyPreset(string preset)
        {
            ApplyPreset(preset);
        }
    }
}
