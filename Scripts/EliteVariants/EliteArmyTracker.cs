using System;
using System.Collections.Generic;
using System.Linq;
using XRL.Core;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Tracks elite army spawns per zone
    /// </summary>
    [HasGameBasedStaticCache]
    public static class EliteArmyTracker
    {
        /// <summary>
        /// Zones that currently have armies (max 1 per zone)
        /// </summary>
        [GameBasedStaticCache]
        private static HashSet<string> _zonesWithArmies = new HashSet<string>();

        /// <summary>
        /// Total armies spawned this game session
        /// </summary>
        [GameBasedStaticCache]
        private static int _totalArmiesSpawned = 0;

        /// <summary>
        /// Breakdown of armies by type
        /// </summary>
        [GameBasedStaticCache]
        private static Dictionary<ArmyType, int> _armiesByType = new Dictionary<ArmyType, int>();

        /// <summary>
        /// Check if a zone already has an army
        /// </summary>
        public static bool HasArmy(Zone zone)
        {
            if (zone == null)
                return false;

            return _zonesWithArmies.Contains(zone.ZoneID);
        }

        /// <summary>
        /// Check if a zone already has an army (by zone ID)
        /// </summary>
        public static bool HasArmy(string zoneID)
        {
            if (string.IsNullOrEmpty(zoneID))
                return false;

            return _zonesWithArmies.Contains(zoneID);
        }

        /// <summary>
        /// Record that an army has spawned in a zone
        /// </summary>
        public static void RecordArmySpawn(Zone zone, ArmyType type)
        {
            if (zone == null)
                return;

            RecordArmySpawn(zone.ZoneID, type);
        }

        /// <summary>
        /// Record that an army has spawned in a zone (by zone ID)
        /// </summary>
        public static void RecordArmySpawn(string zoneID, ArmyType type)
        {
            if (string.IsNullOrEmpty(zoneID))
                return;

            // Mark zone as having an army
            _zonesWithArmies.Add(zoneID);

            // Increment total counter
            _totalArmiesSpawned++;

            // Track by type
            if (!_armiesByType.ContainsKey(type))
            {
                _armiesByType[type] = 0;
            }
            _armiesByType[type]++;
        }

        /// <summary>
        /// Get total number of armies spawned this session
        /// </summary>
        public static int GetTotalArmiesSpawned()
        {
            return _totalArmiesSpawned;
        }

        /// <summary>
        /// Get number of zones with armies
        /// </summary>
        public static int GetZonesWithArmiesCount()
        {
            return _zonesWithArmies.Count;
        }

        /// <summary>
        /// Get count for specific army type
        /// </summary>
        public static int GetCountByType(ArmyType type)
        {
            if (_armiesByType.ContainsKey(type))
            {
                return _armiesByType[type];
            }
            return 0;
        }

        /// <summary>
        /// Get formatted statistics string
        /// </summary>
        public static string GetStatsString()
        {
            var lines = new List<string>();

            lines.Add($"{{Y|Total Armies Spawned:}} {_totalArmiesSpawned}");
            lines.Add($"{{Y|Zones with Armies:}} {_zonesWithArmies.Count}");

            if (_armiesByType.Count > 0)
            {
                lines.Add("{{Y|Breakdown by Type:}}");
                foreach (var kvp in _armiesByType.OrderBy(x => x.Key))
                {
                    string typeName = kvp.Key.ToString();
                    int count = kvp.Value;
                    lines.Add($"  {{W|{typeName}:}} {count}");
                }
            }

            // Check current zone
            try
            {
                var player = XRL.Core.XRLCore.Core?.Game?.Player?.Body;
                if (player != null && player.CurrentZone != null)
                {
                    bool currentZoneHasArmy = HasArmy(player.CurrentZone);
                    string status = currentZoneHasArmy ? "{{G|Yes}}" : "{{r|No}}";
                    lines.Add($"{{Y|Current Zone Has Army:}} {status}");
                }
            }
            catch
            {
                // Ignore errors getting current zone
            }

            return string.Join("\n", lines);
        }

        /// <summary>
        /// Reset all tracking data
        /// </summary>
        public static void Reset()
        {
            _zonesWithArmies.Clear();
            _totalArmiesSpawned = 0;
            _armiesByType.Clear();
        }

        /// <summary>
        /// Remove army tracking for a specific zone
        /// Useful for testing/debugging
        /// </summary>
        public static void RemoveZone(string zoneID)
        {
            if (!string.IsNullOrEmpty(zoneID))
            {
                _zonesWithArmies.Remove(zoneID);
            }
        }

        /// <summary>
        /// Remove army tracking for current zone
        /// </summary>
        public static void RemoveCurrentZone()
        {
            try
            {
                var player = XRL.Core.XRLCore.Core?.Game?.Player?.Body;
                if (player != null && player.CurrentZone != null)
                {
                    RemoveZone(player.CurrentZone.ZoneID);
                }
            }
            catch
            {
                // Ignore errors
            }
        }
    }
}
