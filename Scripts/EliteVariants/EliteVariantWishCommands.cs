using System;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command to show current settings
    /// </summary>
    [Serializable]
    public class EliteVariantsShowSettings : IPart
    {
        public override bool AllowStaticRegistration() => true;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            XRL.Messages.MessageQueue.AddPlayerMessage("{{W|=== Elite Variants Settings ===}}");
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{Y|Power:}} {EliteVariantSettings.ElitePowerMultiplier:F1}x / {EliteVariantSettings.UltimatePowerMultiplier:F1}x");
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{Y|Level:}} +{EliteVariantSettings.EliteLevelOffset} / +{EliteVariantSettings.UltimateLevelOffset}");
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{Y|Ultimate:}} {EliteVariantSettings.UltimateChance}%");
            XRL.Messages.MessageQueue.AddPlayerMessage($"{{Y|Enhancements:}} {EliteVariantSettings.EliteEnhancementMultiplier:F1}x / {EliteVariantSettings.UltimateEnhancementMultiplier:F1}x");
            XRL.Messages.MessageQueue.AddPlayerMessage("{{W|================================}}");
            ParentObject.Destroy();
            return base.HandleEvent(E);
        }
    }

    /// <summary>
    /// Set power to 1.5x for both tiers
    /// </summary>
    [Serializable]
    public class EliteVariantsPower15 : IPart
    {
        public override bool AllowStaticRegistration() => true;
        public override bool WantEvent(int ID, int cascade) => base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteVariantSettings.ElitePowerMultiplier = 1.5f;
            EliteVariantSettings.UltimatePowerMultiplier = 1.5f;
            XRL.Messages.MessageQueue.AddPlayerMessage("{{G|Power set to 1.5x (both tiers)}}");
            ParentObject.Destroy();
            return base.HandleEvent(E);
        }
    }

    [Serializable]
    public class EliteVariantsPower20 : IPart
    {
        public override bool AllowStaticRegistration() => true;
        public override bool WantEvent(int ID, int cascade) => base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteVariantSettings.ElitePowerMultiplier = 2.0f;
            EliteVariantSettings.UltimatePowerMultiplier = 2.0f;
            XRL.Messages.MessageQueue.AddPlayerMessage("{{G|Power set to 2.0x (both tiers)}}");
            ParentObject.Destroy();
            return base.HandleEvent(E);
        }
    }

    [Serializable]
    public class EliteVariantsLevel10 : IPart
    {
        public override bool AllowStaticRegistration() => true;
        public override bool WantEvent(int ID, int cascade) => base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteVariantSettings.EliteLevelOffset = 10;
            EliteVariantSettings.UltimateLevelOffset = 10;
            XRL.Messages.MessageQueue.AddPlayerMessage("{{G|Level offset set to +10 (both tiers)}}");
            ParentObject.Destroy();
            return base.HandleEvent(E);
        }
    }

    [Serializable]
    public class EliteVariantsUltimate100 : IPart
    {
        public override bool AllowStaticRegistration() => true;
        public override bool WantEvent(int ID, int cascade) => base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteVariantSettings.UltimateChance = 100;
            XRL.Messages.MessageQueue.AddPlayerMessage("{{G|Ultimate chance set to 100%}}");
            ParentObject.Destroy();
            return base.HandleEvent(E);
        }
    }

    [Serializable]
    public class EliteVariantsReset : IPart
    {
        public override bool AllowStaticRegistration() => true;
        public override bool WantEvent(int ID, int cascade) => base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            EliteVariantSettings.ResetToDefaults();
            XRL.Messages.MessageQueue.AddPlayerMessage("{{G|Settings reset to defaults}}");
            ParentObject.Destroy();
            return base.HandleEvent(E);
        }
    }
}
