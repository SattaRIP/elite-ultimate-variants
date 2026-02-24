using System;
using XRL.UI;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command: elitespawn:status
    /// Shows spawn statistics
    /// </summary>
    [Serializable]
    public class EliteSpawnStatusCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteVariantSettings.HandleWishCommand("elitespawn:status");
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }

    /// <summary>
    /// Wish command: elitespawn:toggle
    /// Toggles natural spawning on/off
    /// </summary>
    [Serializable]
    public class EliteSpawnToggleCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteVariantSettings.HandleWishCommand("elitespawn:toggle");
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }

    /// <summary>
    /// Wish command: elitespawn:reset
    /// Resets spawn counters
    /// </summary>
    [Serializable]
    public class EliteSpawnResetCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteVariantSettings.HandleWishCommand("elitespawn:reset");
            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }
}
