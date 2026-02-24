using System;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Handles initialization and configuration of Elite Variants settings
    /// Wish for this object to configure difficulty settings
    /// </summary>
    [Serializable]
    public class EliteVariantSettingsManager : IPart
    {
        public override bool AllowStaticRegistration()
        {
            return true;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            // Show current settings
            XRL.Messages.MessageQueue.AddPlayerMessage("{{W|=== Elite Variants - Current Settings ===}}");
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{Y|Power Multiplier:}} {EliteVariantSettings.ElitePowerMultiplier:F1}x / {EliteVariantSettings.UltimatePowerMultiplier:F1}x (Elite / Ultimate)");
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{Y|Level Offset:}} +{EliteVariantSettings.EliteLevelOffset} / +{EliteVariantSettings.UltimateLevelOffset} (Elite / Ultimate)");
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{Y|Ultimate Chance:}} {EliteVariantSettings.UltimateChance}%");
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{Y|Enhancement Multiplier:}} {EliteVariantSettings.EliteEnhancementMultiplier:F1}x / {EliteVariantSettings.UltimateEnhancementMultiplier:F1}x (Elite / Ultimate)");
            XRL.Messages.MessageQueue.AddPlayerMessage("{{W|=========================================}}");
            XRL.Messages.MessageQueue.AddPlayerMessage("");
            XRL.Messages.MessageQueue.AddPlayerMessage("{{c|To change settings: Open the in-game Options menu}}");
            XRL.Messages.MessageQueue.AddPlayerMessage("{{c|Navigate to: Options > Mods: Elite Variants}}");

            // Destroy this manager object after showing info
            ParentObject.Destroy();

            return base.HandleEvent(E);
        }
    }

}
