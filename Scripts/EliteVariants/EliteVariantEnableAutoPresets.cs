using System;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command: eliteautopreset
    /// Enables automatic preset application for current character
    /// </summary>
    [Serializable]
    public class EliteAutoPresetCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            var player = XRL.Core.XRLCore.Core?.Game?.Player?.Body;
            if (player != null)
            {
                player.RequirePart<EliteVariantPresetMonitor>();
                XRL.Messages.MessageQueue.AddPlayerMessage(
                    "{{G|[Elite Variants] Command activated: eliteautopreset}}\n" +
                    "{{W|Automatic preset application has been enabled successfully.}}\n" +
                    "{{c|How it works:}}\n" +
                    "  1. Open Options > Mods: Elite Variants\n" +
                    "  2. Change the Difficulty Preset dropdown\n" +
                    "  3. Close options and wait ~10 turns\n" +
                    "  4. Settings will auto-apply and you'll see a confirmation message."
                );
            }
            else
            {
                XRL.Messages.MessageQueue.AddPlayerMessage("{{R|[Elite Variants] Error:}} Could not find player.");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }
}
