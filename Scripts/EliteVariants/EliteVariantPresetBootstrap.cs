using System;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Bootstrap to add preset monitor to player
    /// </summary>
    [Serializable]
    public class EliteVariantPresetBootstrap : IPlayerMutator
    {
        public void mutate(GameObject player)
        {
            // Add the preset monitor to the player
            player.RequirePart<EliteVariantPresetMonitor>();
        }
    }
}
