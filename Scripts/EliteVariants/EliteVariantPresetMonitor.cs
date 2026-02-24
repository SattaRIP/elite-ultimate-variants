using System;
using XRL.UI;

namespace XRL.World.Parts
{
    /// <summary>
    /// Monitors preset option changes and auto-applies them
    /// Attached to player to run during normal gameplay, not during UI rendering
    /// </summary>
    [Serializable]
    public class EliteVariantPresetMonitor : IPart
    {
        private const string PRESET_OPTION = "Option_EliteVariants_DifficultyPreset";
        private string _lastCheckedPreset = null;
        private int _tickCounter = 0;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == EndTurnEvent.ID
                || ID == AfterPlayerBodyChangeEvent.ID;
        }

        public override bool HandleEvent(AfterPlayerBodyChangeEvent E)
        {
            // Re-attach to new player body
            if (E.NewBody != null && E.NewBody != ParentObject)
            {
                E.NewBody.RequirePart<EliteVariantPresetMonitor>();
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            // Only check every 10 turns to avoid performance impact
            _tickCounter++;
            if (_tickCounter < 10)
            {
                return base.HandleEvent(E);
            }
            _tickCounter = 0;

            try
            {
                string currentPreset = Options.GetOption(PRESET_OPTION, "Normal");

                // Initialize on first check
                if (_lastCheckedPreset == null)
                {
                    _lastCheckedPreset = currentPreset;
                    return base.HandleEvent(E);
                }

                // If preset changed and it's not Custom, apply it
                if (currentPreset != _lastCheckedPreset && currentPreset != "Custom")
                {
                    EliteVariantPresets.ForceApplyPreset(currentPreset);
                    _lastCheckedPreset = currentPreset;

                    // Show notification
                    if (ParentObject.IsPlayer())
                    {
                        XRL.Messages.MessageQueue.AddPlayerMessage(
                            "{{G|[Elite Variants] Automatic preset application successful!}}\n" +
                            "{{W|Difficulty preset '{{Y|" + currentPreset + "}}' has been applied.}}\n" +
                            "{{c|All Elite Variants settings have been updated.}}"
                        );
                    }
                }
                else if (currentPreset != _lastCheckedPreset)
                {
                    _lastCheckedPreset = currentPreset;
                }
            }
            catch (Exception)
            {
                // Silently fail to avoid breaking the game
            }

            return base.HandleEvent(E);
        }

        public override bool AllowStaticRegistration()
        {
            return true;
        }
    }
}
