using System;
using XRL.UI;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command: elitecleanup
    /// Destroys all elite/ultimate creatures and clears all tracking to fix lag
    /// </summary>
    [Serializable]
    public class EliteCleanupCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            try
            {
                int destroyed = 0;

                // Find all zones and destroy elite variants
                foreach (var zoneEntry in The.ZoneManager.CachedZones)
                {
                    var zone = zoneEntry.Value;
                    if (zone == null) continue;

                    var objectsToDestroy = new System.Collections.Generic.List<GameObject>();

                    foreach (var obj in zone.GetObjects())
                    {
                        if (obj == null) continue;

                        // Check if it's an elite variant
                        if (obj.GetIntProperty("IsEliteVariant") == 1)
                        {
                            objectsToDestroy.Add(obj);
                        }
                    }

                    // Destroy them
                    foreach (var obj in objectsToDestroy)
                    {
                        obj.Obliterate();
                        destroyed++;
                    }
                }

                // Clear all tracking
                EliteSpawnController.ResetCounters();
                EliteArmyTracker.Reset();

                // Force garbage collection
                System.GC.Collect();

                Popup.Show($"{{c|Elite Cleanup Complete}}\n\n" +
                          $"Destroyed {{W|{destroyed}}} elite/ultimate creatures\n" +
                          $"Cleared all spawn tracking\n" +
                          $"Forced garbage collection\n\n" +
                          $"Performance should improve now.");
            }
            catch (Exception ex)
            {
                Popup.Show($"{{R|Error during cleanup: {ex.Message}}}");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }
}
