using System;
using System.Collections.Generic;
using System.Linq;
using XRL.World;
using XRL.World.Parts.Mutation;

namespace XRL.World.Parts
{
    /// <summary>
    /// Test result tracking for automated test system
    /// </summary>
    public class TestResult
    {
        public string TestName { get; set; }
        public bool Passed { get; set; }
        public string Details { get; set; }
        public List<string> SubTests { get; set; } = new List<string>();

        public TestResult(string name)
        {
            TestName = name;
            Passed = true;
            Details = "";
        }

        public void AddSubTest(string name, bool passed, string details = "")
        {
            string status = passed ? "{{G|PASS}}" : "{{R|FAIL}}";
            SubTests.Add($"  {status} {name}{(string.IsNullOrEmpty(details) ? "" : $": {details}")}");
            if (!passed) Passed = false;
        }

        public string GetReport()
        {
            string header = Passed ? "{{G|PASS}}" : "{{R|FAIL}}";
            string report = $"{header} {{W|{TestName}}}\n";
            foreach (var sub in SubTests)
            {
                report += sub + "\n";
            }
            return report;
        }
    }

    /// <summary>
    /// Test framework for Elite Variants mod
    /// </summary>
    public static class EliteVariantTestFramework
    {
        private static List<TestResult> _results = new List<TestResult>();
        private static int _totalTests = 0;
        private static int _passedTests = 0;
        private static List<GameObject> _spawnedObjects = new List<GameObject>();

        /// <summary>
        /// Reset test state
        /// </summary>
        public static void Reset()
        {
            _results.Clear();
            _totalTests = 0;
            _passedTests = 0;
            CleanupSpawnedObjects();
        }

        /// <summary>
        /// Add a test result
        /// </summary>
        public static void AddResult(TestResult result)
        {
            _results.Add(result);
            _totalTests++;
            if (result.Passed) _passedTests++;
        }

        /// <summary>
        /// Track spawned object for cleanup
        /// </summary>
        public static void TrackObject(GameObject obj)
        {
            if (obj != null)
                _spawnedObjects.Add(obj);
        }

        /// <summary>
        /// Cleanup all spawned test objects
        /// </summary>
        public static void CleanupSpawnedObjects()
        {
            foreach (var obj in _spawnedObjects)
            {
                try
                {
                    if (obj != null && obj.IsValid())
                    {
                        obj.Obliterate();
                    }
                }
                catch { }
            }
            _spawnedObjects.Clear();
        }

        /// <summary>
        /// Get summary report
        /// </summary>
        public static string GetSummary()
        {
            string color = _passedTests == _totalTests ? "G" : "R";
            return $"{{{{{color}|=== TEST SUMMARY: {_passedTests}/{_totalTests} PASSED ===}}}}";
        }

        /// <summary>
        /// Get full report
        /// </summary>
        public static string GetFullReport()
        {
            string report = "{{c|========================================}}\n";
            report += "{{W|       ELITE VARIANTS TEST REPORT}}\n";
            report += "{{c|========================================}}\n\n";

            foreach (var result in _results)
            {
                report += result.GetReport() + "\n";
            }

            report += GetSummary();
            return report;
        }

        /// <summary>
        /// Log message to message queue
        /// </summary>
        public static void Log(string message)
        {
            XRL.Messages.MessageQueue.AddPlayerMessage(message);
        }
    }

    // ========================================
    // EQUIPMENT TESTS
    // ========================================

    /// <summary>
    /// Wish command: elitetest:equipment
    /// Tests equipment system for elite variants
    /// </summary>
    [Serializable]
    public class EliteTestEquipmentCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            RunEquipmentTests();
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        public static void RunEquipmentTests()
        {
            EliteVariantTestFramework.Reset();

            var result = new TestResult("Equipment System");
            var rng = new Random(12345); // Fixed seed for reproducibility

            // Test 1: Elite equipment counts
            int eliteMinExpected = EliteVariantSettings.EliteMinItemCount;
            int eliteMaxExpected = EliteVariantSettings.EliteMaxItemCount;

            int eliteMinFound = int.MaxValue;
            int eliteMaxFound = 0;

            for (int i = 0; i < 10; i++)
            {
                var elite = EliteVariantGenerator.CreateEliteVariant(
                    "Snapjaw Scavenger",
                    new Random(rng.Next()),
                    isGoon: false,
                    customTier: EliteTier.Elite
                );

                if (elite != null)
                {
                    int itemCount = CountEquipment(elite);
                    eliteMinFound = Math.Min(eliteMinFound, itemCount);
                    eliteMaxFound = Math.Max(eliteMaxFound, itemCount);
                    EliteVariantTestFramework.TrackObject(elite);
                }
            }

            bool eliteCountsValid = eliteMinFound >= eliteMinExpected && eliteMaxFound <= eliteMaxExpected;
            result.AddSubTest(
                "Elite item count range",
                eliteCountsValid,
                $"Expected {eliteMinExpected}-{eliteMaxExpected}, Got {eliteMinFound}-{eliteMaxFound}"
            );

            // Test 2: Ultimate equipment counts
            int ultMinExpected = EliteVariantSettings.UltimateMinItemCount;
            int ultMaxExpected = EliteVariantSettings.UltimateMaxItemCount;

            int ultMinFound = int.MaxValue;
            int ultMaxFound = 0;

            for (int i = 0; i < 10; i++)
            {
                var ultimate = EliteVariantGenerator.CreateEliteVariant(
                    "Snapjaw Scavenger",
                    new Random(rng.Next()),
                    isGoon: false,
                    customTier: EliteTier.Ultimate
                );

                if (ultimate != null)
                {
                    int itemCount = CountEquipment(ultimate);
                    ultMinFound = Math.Min(ultMinFound, itemCount);
                    ultMaxFound = Math.Max(ultMaxFound, itemCount);
                    EliteVariantTestFramework.TrackObject(ultimate);
                }
            }

            bool ultCountsValid = ultMinFound >= ultMinExpected && ultMaxFound <= ultMaxExpected;
            result.AddSubTest(
                "Ultimate item count range",
                ultCountsValid,
                $"Expected {ultMinExpected}-{ultMaxExpected}, Got {ultMinFound}-{ultMaxFound}"
            );

            // Test 3: Goons should NOT get equipment
            var goon = EliteVariantGenerator.CreateEliteVariant(
                "Snapjaw Scavenger",
                rng,
                isGoon: true,
                customTier: EliteTier.Elite
            );

            if (goon != null)
            {
                int goonItems = CountEquipment(goon);
                // Goons shouldn't have elite equipment (they may have natural equipment)
                result.AddSubTest(
                    "Goons receive no elite equipment",
                    goonItems == 0,
                    $"Goon has {goonItems} elite items (expected 0)"
                );
                EliteVariantTestFramework.TrackObject(goon);
            }

            // Test 4: Equipment tier distribution
            int tier8Count = 0;
            int tier7Count = 0;
            int tier6Count = 0;
            int totalItems = 0;

            for (int i = 0; i < 20; i++)
            {
                var elite = EliteVariantGenerator.CreateEliteVariant(
                    "Snapjaw Scavenger",
                    new Random(rng.Next()),
                    isGoon: false,
                    customTier: EliteTier.Elite
                );

                if (elite != null)
                {
                    CountEquipmentTiers(elite, ref tier8Count, ref tier7Count, ref tier6Count, ref totalItems);
                    EliteVariantTestFramework.TrackObject(elite);
                }
            }

            // Check that tier distribution roughly matches settings
            int expectedTier8Pct = EliteVariantSettings.EliteTier8Chance;
            int expectedTier7Pct = EliteVariantSettings.EliteTier7Chance;
            int actualTier8Pct = totalItems > 0 ? (tier8Count * 100 / totalItems) : 0;
            int actualTier7Pct = totalItems > 0 ? (tier7Count * 100 / totalItems) : 0;

            // Allow 25% variance for statistical sampling
            bool tierDistValid = Math.Abs(actualTier8Pct - expectedTier8Pct) < 25 &&
                                 Math.Abs(actualTier7Pct - expectedTier7Pct) < 25;

            result.AddSubTest(
                "Equipment tier distribution",
                tierDistValid,
                $"T8: {actualTier8Pct}% (exp ~{expectedTier8Pct}%), T7: {actualTier7Pct}% (exp ~{expectedTier7Pct}%)"
            );

            EliteVariantTestFramework.AddResult(result);
            EliteVariantTestFramework.CleanupSpawnedObjects();

            // Output report
            EliteVariantTestFramework.Log("{{c|=== EQUIPMENT TEST RESULTS ===}}");
            EliteVariantTestFramework.Log(result.GetReport());
        }

        private static int CountEquipment(GameObject creature)
        {
            if (creature == null) return 0;

            int count = 0;
            var body = creature.GetPart<Body>();
            if (body != null)
            {
                foreach (var part in body.GetParts())
                {
                    if (part.Equipped != null)
                    {
                        // Check if it's elite-tier equipment
                        string name = part.Equipped.Blueprint ?? "";
                        if (name.Contains("Crysteel") || name.Contains("Zetachrome") ||
                            name.Contains("Carbide") || name.Contains("Fullerite") ||
                            name.Contains("Cudgel") || name.Contains("Long Sword") ||
                            name.Contains("Battle Axe") || name.Contains("Dagger"))
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        private static void CountEquipmentTiers(GameObject creature, ref int tier8, ref int tier7, ref int tier6, ref int total)
        {
            if (creature == null) return;

            var body = creature.GetPart<Body>();
            if (body != null)
            {
                foreach (var part in body.GetParts())
                {
                    if (part.Equipped != null)
                    {
                        string name = part.Equipped.Blueprint ?? "";
                        if (name.Contains("Zetachrome") || name.Contains("8"))
                        {
                            tier8++;
                            total++;
                        }
                        else if (name.Contains("Flawless Crysteel") || name.Contains("7"))
                        {
                            tier7++;
                            total++;
                        }
                        else if (name.Contains("Carbide") || name.Contains("Crysteel") || name.Contains("6"))
                        {
                            tier6++;
                            total++;
                        }
                    }
                }
            }
        }
    }

    // ========================================
    // MUTATION TESTS
    // ========================================

    /// <summary>
    /// Wish command: elitetest:mutations
    /// Tests mutation caps for elite variants
    /// </summary>
    [Serializable]
    public class EliteTestMutationsCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            RunMutationTests();
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        public static void RunMutationTests()
        {
            EliteVariantTestFramework.Reset();

            var result = new TestResult("Mutation System");
            var rng = new Random(12345);

            // Get expected caps from settings
            int eliteMentalCap = EliteVariantConfig.GetMentalMutationCap(30, EliteTier.Elite);
            int elitePhysCap = EliteVariantConfig.GetPhysicalMutationCap(30, EliteTier.Elite);
            int ultMentalCap = EliteVariantConfig.GetMentalMutationCap(40, EliteTier.Ultimate);
            int ultPhysCap = EliteVariantConfig.GetPhysicalMutationCap(40, EliteTier.Ultimate);

            // Test 1: Elite mental mutation cap
            int maxEliteMental = 0;
            for (int i = 0; i < 15; i++)
            {
                var elite = EliteVariantGenerator.CreateEliteVariant(
                    "Snapjaw Scavenger",
                    new Random(rng.Next()),
                    isGoon: false,
                    customTier: EliteTier.Elite
                );

                if (elite != null)
                {
                    int mentalCount = CountMentalMutations(elite);
                    maxEliteMental = Math.Max(maxEliteMental, mentalCount);
                    EliteVariantTestFramework.TrackObject(elite);
                }
            }

            bool eliteMentalValid = maxEliteMental <= eliteMentalCap;
            result.AddSubTest(
                "Elite mental mutation cap",
                eliteMentalValid,
                $"Max found: {maxEliteMental}, Cap: {eliteMentalCap}"
            );

            // Test 2: Elite physical mutation cap
            int maxElitePhys = 0;
            for (int i = 0; i < 15; i++)
            {
                var elite = EliteVariantGenerator.CreateEliteVariant(
                    "Snapjaw Scavenger",
                    new Random(rng.Next()),
                    isGoon: false,
                    customTier: EliteTier.Elite
                );

                if (elite != null)
                {
                    int physCount = CountPhysicalMutations(elite);
                    maxElitePhys = Math.Max(maxElitePhys, physCount);
                    EliteVariantTestFramework.TrackObject(elite);
                }
            }

            bool elitePhysValid = maxElitePhys <= elitePhysCap;
            result.AddSubTest(
                "Elite physical mutation cap",
                elitePhysValid,
                $"Max found: {maxElitePhys}, Cap: {elitePhysCap}"
            );

            // Test 3: Ultimate mental mutation cap
            int maxUltMental = 0;
            for (int i = 0; i < 15; i++)
            {
                var ultimate = EliteVariantGenerator.CreateEliteVariant(
                    "Snapjaw Scavenger",
                    new Random(rng.Next()),
                    isGoon: false,
                    customTier: EliteTier.Ultimate
                );

                if (ultimate != null)
                {
                    int mentalCount = CountMentalMutations(ultimate);
                    maxUltMental = Math.Max(maxUltMental, mentalCount);
                    EliteVariantTestFramework.TrackObject(ultimate);
                }
            }

            bool ultMentalValid = maxUltMental <= ultMentalCap;
            result.AddSubTest(
                "Ultimate mental mutation cap",
                ultMentalValid,
                $"Max found: {maxUltMental}, Cap: {ultMentalCap}"
            );

            // Test 4: Ultimate physical mutation cap
            int maxUltPhys = 0;
            for (int i = 0; i < 15; i++)
            {
                var ultimate = EliteVariantGenerator.CreateEliteVariant(
                    "Snapjaw Scavenger",
                    new Random(rng.Next()),
                    isGoon: false,
                    customTier: EliteTier.Ultimate
                );

                if (ultimate != null)
                {
                    int physCount = CountPhysicalMutations(ultimate);
                    maxUltPhys = Math.Max(maxUltPhys, physCount);
                    EliteVariantTestFramework.TrackObject(ultimate);
                }
            }

            bool ultPhysValid = maxUltPhys <= ultPhysCap;
            result.AddSubTest(
                "Ultimate physical mutation cap",
                ultPhysValid,
                $"Max found: {maxUltPhys}, Cap: {ultPhysCap}"
            );

            // Test 5: Mutations have proper levels
            var testElite = EliteVariantGenerator.CreateEliteVariant(
                "Snapjaw Scavenger",
                rng,
                isGoon: false,
                customTier: EliteTier.Elite
            );

            if (testElite != null)
            {
                bool levelsValid = CheckMutationLevels(testElite,
                    EliteVariantConfig.LeaderMutationLevelMin,
                    EliteVariantConfig.LeaderMutationLevelMax);
                result.AddSubTest(
                    "Mutation levels in range",
                    levelsValid,
                    $"Expected {EliteVariantConfig.LeaderMutationLevelMin}-{EliteVariantConfig.LeaderMutationLevelMax}"
                );
                EliteVariantTestFramework.TrackObject(testElite);
            }

            EliteVariantTestFramework.AddResult(result);
            EliteVariantTestFramework.CleanupSpawnedObjects();

            // Output report
            EliteVariantTestFramework.Log("{{c|=== MUTATION TEST RESULTS ===}}");
            EliteVariantTestFramework.Log(result.GetReport());
        }

        private static int CountMentalMutations(GameObject creature)
        {
            if (creature == null) return 0;

            var mutations = creature.GetPart<Mutations>();
            if (mutations == null) return 0;

            int count = 0;
            foreach (var entry in mutations.MutationList)
            {
                if (entry != null)
                {
                    var mutEntry = entry.GetMutationEntry();
                    if (mutEntry != null && mutEntry.Category.ToString() == "Mental")
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private static int CountPhysicalMutations(GameObject creature)
        {
            if (creature == null) return 0;

            var mutations = creature.GetPart<Mutations>();
            if (mutations == null) return 0;

            int count = 0;
            foreach (var entry in mutations.MutationList)
            {
                if (entry != null)
                {
                    var mutEntry = entry.GetMutationEntry();
                    if (mutEntry != null && mutEntry.Category.ToString() == "Physical")
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private static bool CheckMutationLevels(GameObject creature, int minLevel, int maxLevel)
        {
            if (creature == null) return true;

            var mutations = creature.GetPart<Mutations>();
            if (mutations == null) return true;

            foreach (var entry in mutations.MutationList)
            {
                if (entry != null)
                {
                    int level = entry.Level;
                    if (level < minLevel || level > maxLevel)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    // ========================================
    // MULTIPLIER TESTS
    // ========================================

    /// <summary>
    /// Wish command: elitetest:multipliers
    /// Tests stat multipliers for elite variants
    /// </summary>
    [Serializable]
    public class EliteTestMultipliersCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            RunMultiplierTests();
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        public static void RunMultiplierTests()
        {
            EliteVariantTestFramework.Reset();

            var result = new TestResult("Stat Multipliers");
            var rng = new Random(12345);

            // Create base creature for comparison
            var baseCreature = GameObjectFactory.Factory.CreateObject("Snapjaw Scavenger");
            if (baseCreature == null)
            {
                result.AddSubTest("Base creature creation", false, "Failed to create base creature");
                EliteVariantTestFramework.AddResult(result);
                return;
            }

            int baseHP = baseCreature.Statistics.ContainsKey("Hitpoints")
                ? baseCreature.Statistics["Hitpoints"].BaseValue : 0;

            EliteVariantTestFramework.TrackObject(baseCreature);

            // Test 1: Elite HP multiplier
            float eliteHPMult = EliteVariantSettings.EliteHPMultiplier;
            var elite = EliteVariantGenerator.CreateEliteVariant(
                "Snapjaw Scavenger",
                rng,
                isGoon: false,
                customTier: EliteTier.Elite
            );

            if (elite != null)
            {
                int eliteHP = elite.Statistics.ContainsKey("Hitpoints")
                    ? elite.Statistics["Hitpoints"].BaseValue : 0;
                int expectedHP = (int)(baseHP * eliteHPMult);

                // Allow 10% variance due to other stat modifications
                bool hpValid = Math.Abs(eliteHP - expectedHP) <= expectedHP * 0.1f;
                result.AddSubTest(
                    "Elite HP multiplier",
                    hpValid,
                    $"Base: {baseHP}, Elite: {eliteHP}, Expected: ~{expectedHP} ({eliteHPMult:F1}x)"
                );
                EliteVariantTestFramework.TrackObject(elite);
            }

            // Test 2: Ultimate HP multiplier
            float ultHPMult = EliteVariantSettings.UltimateHPMultiplier;
            var ultimate = EliteVariantGenerator.CreateEliteVariant(
                "Snapjaw Scavenger",
                rng,
                isGoon: false,
                customTier: EliteTier.Ultimate
            );

            if (ultimate != null)
            {
                int ultHP = ultimate.Statistics.ContainsKey("Hitpoints")
                    ? ultimate.Statistics["Hitpoints"].BaseValue : 0;
                int expectedHP = (int)(baseHP * ultHPMult);

                bool hpValid = Math.Abs(ultHP - expectedHP) <= expectedHP * 0.1f;
                result.AddSubTest(
                    "Ultimate HP multiplier",
                    hpValid,
                    $"Base: {baseHP}, Ultimate: {ultHP}, Expected: ~{expectedHP} ({ultHPMult:F1}x)"
                );
                EliteVariantTestFramework.TrackObject(ultimate);
            }

            // Test 3: Elite has IsEliteVariant property
            if (elite != null)
            {
                bool hasTag = elite.GetIntProperty("IsEliteVariant") == 1;
                result.AddSubTest("Elite has IsEliteVariant tag", hasTag);
            }

            // Test 4: Leader has IsEliteLeader property
            if (elite != null)
            {
                bool hasLeaderTag = elite.GetIntProperty("IsEliteLeader") == 1;
                result.AddSubTest("Leader has IsEliteLeader tag", hasLeaderTag);
            }

            // Test 5: Goon does NOT have IsEliteLeader property
            var goon = EliteVariantGenerator.CreateEliteVariant(
                "Snapjaw Scavenger",
                rng,
                isGoon: true,
                customTier: EliteTier.Elite
            );

            if (goon != null)
            {
                bool noLeaderTag = goon.GetIntProperty("IsEliteLeader") != 1;
                result.AddSubTest("Goon lacks IsEliteLeader tag", noLeaderTag);
                EliteVariantTestFramework.TrackObject(goon);
            }

            // Test 6: Elite level matches expected
            Zone zone = The.Player?.CurrentZone;
            int expectedLevel = EliteVariantGenerator.CalculateEliteLevel(zone, false, EliteTier.Elite);
            if (elite != null && elite.Statistics.ContainsKey("Level"))
            {
                int actualLevel = elite.Statistics["Level"].BaseValue;
                bool levelValid = actualLevel == expectedLevel;
                result.AddSubTest(
                    "Elite level calculation",
                    levelValid,
                    $"Expected: {expectedLevel}, Got: {actualLevel}"
                );
            }

            EliteVariantTestFramework.AddResult(result);
            EliteVariantTestFramework.CleanupSpawnedObjects();

            // Output report
            EliteVariantTestFramework.Log("{{c|=== MULTIPLIER TEST RESULTS ===}}");
            EliteVariantTestFramework.Log(result.GetReport());
        }
    }

    // ========================================
    // ARMY TESTS
    // ========================================

    /// <summary>
    /// Wish command: elitetest:armies
    /// Tests army composition system
    /// </summary>
    [Serializable]
    public class EliteTestArmiesCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            RunArmyTests();
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        public static void RunArmyTests()
        {
            EliteVariantTestFramework.Reset();

            var result = new TestResult("Army System");

            // Test each army type configuration
            foreach (ArmyType armyType in Enum.GetValues(typeof(ArmyType)))
            {
                var comp = EliteArmyConfig.GetComposition(armyType);

                // Verify composition values are valid
                bool compositionValid =
                    comp.MinLeaders >= 0 &&
                    comp.MaxLeaders >= comp.MinLeaders &&
                    comp.MinGoons >= 0 &&
                    comp.MaxGoons >= comp.MinGoons &&
                    comp.MinLevel > 0 &&
                    comp.MinTier >= 1 && comp.MinTier <= 8;

                result.AddSubTest(
                    $"{armyType} composition",
                    compositionValid,
                    $"Leaders: {comp.MinLeaders}-{comp.MaxLeaders}, Goons: {comp.MinGoons}-{comp.MaxGoons}"
                );
            }

            // Test SmallSquad specifically (most common)
            var smallSquad = EliteArmyConfig.GetComposition(ArmyType.SmallSquad);
            result.AddSubTest(
                "SmallSquad is goons-only",
                smallSquad.MinLeaders == 0 && smallSquad.MaxLeaders == 0,
                $"Leaders: {smallSquad.MinLeaders}-{smallSquad.MaxLeaders}"
            );

            // Test TitanPair specifically (ultimates only)
            var titanPair = EliteArmyConfig.GetComposition(ArmyType.TitanPair);
            result.AddSubTest(
                "TitanPair is leaders-only",
                titanPair.MinGoons == 0 && titanPair.MaxGoons == 0,
                $"Goons: {titanPair.MinGoons}-{titanPair.MaxGoons}"
            );

            // Test army type parsing
            ArmyType parsed;
            bool parseSuccess = EliteArmyConfig.TryParseArmyType("SmallSquad", out parsed);
            result.AddSubTest(
                "Army type parsing",
                parseSuccess && parsed == ArmyType.SmallSquad
            );

            // Test case-insensitive parsing
            bool caseInsensitive = EliteArmyConfig.TryParseArmyType("smallsquad", out parsed);
            result.AddSubTest(
                "Case-insensitive parsing",
                caseInsensitive && parsed == ArmyType.SmallSquad
            );

            // Test invalid parsing falls back gracefully
            bool invalidParseFails = !EliteArmyConfig.TryParseArmyType("InvalidType", out parsed);
            result.AddSubTest("Invalid type handling", invalidParseFails);

            // Test level requirement check
            bool meetsLevel = EliteArmyConfig.MeetsLevelRequirement(ArmyType.SmallSquad, 15);
            bool failsLevel = !EliteArmyConfig.MeetsLevelRequirement(ArmyType.SmallSquad, 10);
            result.AddSubTest(
                "Level requirement check",
                meetsLevel && failsLevel,
                "SmallSquad requires level 15"
            );

            // Test tier requirement check
            bool meetsTier = EliteArmyConfig.MeetsTierRequirement(ArmyType.SmallSquad, 5);
            bool failsTier = !EliteArmyConfig.MeetsTierRequirement(ArmyType.SmallSquad, 4);
            result.AddSubTest(
                "Tier requirement check",
                meetsTier && failsTier,
                "SmallSquad requires tier 5"
            );

            // Test description generation
            string desc = EliteArmyConfig.GetDescription(ArmyType.EliteWarband);
            bool hasDesc = !string.IsNullOrEmpty(desc) && desc.Contains("ultimate") && desc.Contains("goon");
            result.AddSubTest(
                "Army description generation",
                hasDesc,
                desc
            );

            EliteVariantTestFramework.AddResult(result);

            // Output report
            EliteVariantTestFramework.Log("{{c|=== ARMY TEST RESULTS ===}}");
            EliteVariantTestFramework.Log(result.GetReport());
        }
    }

    // ========================================
    // SETTINGS TESTS
    // ========================================

    /// <summary>
    /// Wish command: elitetest:settings
    /// Tests that settings affect spawns correctly
    /// </summary>
    [Serializable]
    public class EliteTestSettingsCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            RunSettingsTests();
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        public static void RunSettingsTests()
        {
            EliteVariantTestFramework.Reset();

            var result = new TestResult("Settings System");

            // Test 1: Settings are readable
            try
            {
                float elitePower = EliteVariantSettings.ElitePowerMultiplier;
                float eliteHP = EliteVariantSettings.EliteHPMultiplier;
                float ultPower = EliteVariantSettings.UltimatePowerMultiplier;
                float ultHP = EliteVariantSettings.UltimateHPMultiplier;

                result.AddSubTest(
                    "Settings readable",
                    true,
                    $"Elite: {elitePower:F1}x/{eliteHP:F1}x, Ult: {ultPower:F1}x/{ultHP:F1}x"
                );
            }
            catch (Exception ex)
            {
                result.AddSubTest("Settings readable", false, ex.Message);
            }

            // Test 2: Mutation caps are readable
            try
            {
                int eliteMentalCap = EliteVariantSettings.EliteMentalMutationCap;
                int elitePhysCap = EliteVariantSettings.ElitePhysicalMutationCap;
                int ultMentalCap = EliteVariantSettings.UltimateMentalMutationCap;
                int ultPhysCap = EliteVariantSettings.UltimatePhysicalMutationCap;

                result.AddSubTest(
                    "Mutation caps readable",
                    true,
                    $"Elite M/P: {eliteMentalCap}/{elitePhysCap}, Ult M/P: {ultMentalCap}/{ultPhysCap}"
                );
            }
            catch (Exception ex)
            {
                result.AddSubTest("Mutation caps readable", false, ex.Message);
            }

            // Test 3: Item settings are readable
            try
            {
                int eliteMin = EliteVariantSettings.EliteMinItemCount;
                int eliteMax = EliteVariantSettings.EliteMaxItemCount;
                int ultMin = EliteVariantSettings.UltimateMinItemCount;
                int ultMax = EliteVariantSettings.UltimateMaxItemCount;

                bool rangesValid = eliteMin <= eliteMax && ultMin <= ultMax;
                result.AddSubTest(
                    "Item count settings valid",
                    rangesValid,
                    $"Elite: {eliteMin}-{eliteMax}, Ult: {ultMin}-{ultMax}"
                );
            }
            catch (Exception ex)
            {
                result.AddSubTest("Item count settings valid", false, ex.Message);
            }

            // Test 4: Tier chance settings are readable
            try
            {
                int eliteT8 = EliteVariantSettings.EliteTier8Chance;
                int eliteT7 = EliteVariantSettings.EliteTier7Chance;
                int ultT8 = EliteVariantSettings.UltimateTier8Chance;
                int ultT7 = EliteVariantSettings.UltimateTier7Chance;

                bool chancesValid = eliteT8 + eliteT7 <= 100 && ultT8 + ultT7 <= 100;
                result.AddSubTest(
                    "Tier chance settings valid",
                    chancesValid,
                    $"Elite T8/T7: {eliteT8}%/{eliteT7}%, Ult T8/T7: {ultT8}%/{ultT7}%"
                );
            }
            catch (Exception ex)
            {
                result.AddSubTest("Tier chance settings valid", false, ex.Message);
            }

            // Test 5: Army settings are readable
            try
            {
                bool armyEnabled = EliteVariantSettings.EnableArmySpawning;
                float armyWeight = EliteVariantSettings.ArmySpawnWeight;
                int armyMinLevel = EliteVariantSettings.MinPlayerLevelForArmies;
                int armyMinTier = EliteVariantSettings.MinZoneTierForArmies;

                result.AddSubTest(
                    "Army settings readable",
                    true,
                    $"Enabled: {armyEnabled}, Weight: {armyWeight:F1}x, MinLvl: {armyMinLevel}, MinTier: {armyMinTier}"
                );
            }
            catch (Exception ex)
            {
                result.AddSubTest("Army settings readable", false, ex.Message);
            }

            // Test 6: Debug mode can be toggled
            try
            {
                bool originalDebug = EliteVariantSettings.EnableDebugMode;
                EliteVariantSettings.EnableDebugMode = !originalDebug;
                bool toggledDebug = EliteVariantSettings.EnableDebugMode;
                EliteVariantSettings.EnableDebugMode = originalDebug; // Restore

                result.AddSubTest(
                    "Debug mode toggleable",
                    toggledDebug != originalDebug
                );
            }
            catch (Exception ex)
            {
                result.AddSubTest("Debug mode toggleable", false, ex.Message);
            }

            // Test 7: GetDebugInfo returns valid string
            try
            {
                string debugInfo = EliteVariantSettings.GetDebugInfo();
                bool hasContent = !string.IsNullOrEmpty(debugInfo) && debugInfo.Contains("Power");
                result.AddSubTest("Debug info generation", hasContent);
            }
            catch (Exception ex)
            {
                result.AddSubTest("Debug info generation", false, ex.Message);
            }

            EliteVariantTestFramework.AddResult(result);

            // Output report
            EliteVariantTestFramework.Log("{{c|=== SETTINGS TEST RESULTS ===}}");
            EliteVariantTestFramework.Log(result.GetReport());
        }
    }

    // ========================================
    // FULL TEST SUITE
    // ========================================

    /// <summary>
    /// Wish command: elitetest:full
    /// Runs all tests and reports comprehensive results
    /// </summary>
    [Serializable]
    public class EliteTestFullCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            RunFullTestSuite();
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        public static void RunFullTestSuite()
        {
            EliteVariantTestFramework.Reset();

            EliteVariantTestFramework.Log("{{W|==============================================}}");
            EliteVariantTestFramework.Log("{{W|   ELITE VARIANTS - AUTOMATED TEST SUITE}}");
            EliteVariantTestFramework.Log("{{W|==============================================}}");
            EliteVariantTestFramework.Log("");

            // Run all test categories
            EliteVariantTestFramework.Log("{{c|Running Settings Tests...}}");
            EliteTestSettingsCommand.RunSettingsTests();
            EliteVariantTestFramework.Log("");

            EliteVariantTestFramework.Log("{{c|Running Equipment Tests...}}");
            EliteTestEquipmentCommand.RunEquipmentTests();
            EliteVariantTestFramework.Log("");

            EliteVariantTestFramework.Log("{{c|Running Mutation Tests...}}");
            EliteTestMutationsCommand.RunMutationTests();
            EliteVariantTestFramework.Log("");

            EliteVariantTestFramework.Log("{{c|Running Multiplier Tests...}}");
            EliteTestMultipliersCommand.RunMultiplierTests();
            EliteVariantTestFramework.Log("");

            EliteVariantTestFramework.Log("{{c|Running Army Tests...}}");
            EliteTestArmiesCommand.RunArmyTests();
            EliteVariantTestFramework.Log("");

            // Final summary
            EliteVariantTestFramework.Log("{{W|==============================================}}");
            EliteVariantTestFramework.Log(EliteVariantTestFramework.GetSummary());
            EliteVariantTestFramework.Log("{{W|==============================================}}");
        }
    }

    // ========================================
    // QUICK SPAWN TEST
    // ========================================

    /// <summary>
    /// Wish command: elitetest:spawn
    /// Quick test that spawns one of each type and reports stats
    /// </summary>
    [Serializable]
    public class EliteTestSpawnCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            RunSpawnTest();
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }

        public static void RunSpawnTest()
        {
            var rng = new Random();

            EliteVariantTestFramework.Log("{{W|=== SPAWN TEST ===}}");

            // Spawn Elite
            var elite = EliteVariantGenerator.CreateEliteVariant(
                CreaturePool.SelectRandomCreature(rng),
                rng,
                isGoon: false,
                customTier: EliteTier.Elite
            );

            if (elite != null)
            {
                ReportCreatureStats("Elite", elite);

                // Place near player
                Cell playerCell = The.Player?.CurrentCell;
                if (playerCell != null)
                {
                    var adjacent = playerCell.GetEmptyAdjacentCells();
                    if (adjacent != null && adjacent.Count > 0)
                    {
                        adjacent[0].AddObject(elite);
                    }
                }
            }

            // Spawn Ultimate
            var ultimate = EliteVariantGenerator.CreateEliteVariant(
                CreaturePool.SelectRandomCreature(rng),
                rng,
                isGoon: false,
                customTier: EliteTier.Ultimate
            );

            if (ultimate != null)
            {
                ReportCreatureStats("Ultimate", ultimate);

                // Place near player
                Cell playerCell = The.Player?.CurrentCell;
                if (playerCell != null)
                {
                    var adjacent = playerCell.GetEmptyAdjacentCells();
                    if (adjacent != null && adjacent.Count > 0)
                    {
                        adjacent[0].AddObject(ultimate);
                    }
                }
            }

            EliteVariantTestFramework.Log("{{G|Spawn test complete - check adjacent cells}}");
        }

        private static void ReportCreatureStats(string tier, GameObject creature)
        {
            if (creature == null) return;

            string name = creature.GetDisplayName(int.MaxValue, AsIfKnown: true, NoColor: true);
            int level = creature.Statistics.ContainsKey("Level") ? creature.Statistics["Level"].BaseValue : 0;
            int hp = creature.Statistics.ContainsKey("Hitpoints") ? creature.Statistics["Hitpoints"].BaseValue : 0;
            int av = creature.Statistics.ContainsKey("AV") ? creature.Statistics["AV"].BaseValue : 0;
            int dv = creature.Statistics.ContainsKey("DV") ? creature.Statistics["DV"].BaseValue : 0;

            // Count mutations
            int mutations = 0;
            var mutPart = creature.GetPart<Mutations>();
            if (mutPart != null)
            {
                mutations = mutPart.MutationList.Count;
            }

            // Count equipment
            int items = 0;
            var body = creature.GetPart<Body>();
            if (body != null)
            {
                foreach (var part in body.GetParts())
                {
                    if (part.Equipped != null) items++;
                }
            }

            string color = tier == "Ultimate" ? "W" : "Y";
            EliteVariantTestFramework.Log($"{{{{{color}|{tier}:}}}} {name}");
            EliteVariantTestFramework.Log($"  Level: {{W|{level}}} | HP: {{W|{hp}}} | AV: {{W|{av}}} | DV: {{W|{dv}}}");
            EliteVariantTestFramework.Log($"  Mutations: {{W|{mutations}}} | Equipment: {{W|{items}}}");
        }
    }
}
