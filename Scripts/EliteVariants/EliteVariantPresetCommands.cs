using System;
using XRL.UI;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Wish command: elitepreset
    /// Quickly apply a difficulty preset
    /// </summary>
    [Serializable]
    public class ElitePresetCommand : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == ObjectCreatedEvent.ID;
        }

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            var options = new System.Collections.Generic.List<string>
            {
                "Easy - Relaxed (25%)",
                "Normal - Balanced (50%)",
                "Hard - Challenging (75%)",
                "Brutal - Very Hard (87.5%)",
                "Nightmare - Maximum (100%)",
                "Cancel"
            };

            int choice = Popup.ShowOptionList(
                "Select Difficulty Preset",
                options.ToArray(),
                null,
                0,
                "Choose a preset to apply to all Elite Variants settings. Percentages represent power scaling from minimum to maximum values."
            );

            if (choice >= 0 && choice < 5)
            {
                string[] presets = { "Easy", "Normal", "Hard", "Brutal", "Nightmare" };
                string preset = presets[choice];

                // Force apply it immediately
                EliteVariantPresets.ForceApplyPreset(preset);

                // Message log confirmation
                XRL.Messages.MessageQueue.AddPlayerMessage(
                    $"{{G|[Elite Variants] Command activated: elitepreset}}\n" +
                    $"{{W|Difficulty preset '{{Y|{preset}}}' has been applied successfully.}}\n" +
                    $"{{c|All Elite Variants settings have been updated. Check the mod options menu to verify.}}"
                );

                // Also show popup
                Popup.Show($"{{c|Difficulty Preset Applied: {preset}}}\n\n" +
                          "All multiplier settings have been updated.\n" +
                          "Check the mod options menu to see the changes.\n\n" +
                          "{{y|Note:}} Changing individual settings will switch preset to 'Custom'.");
            }

            ParentObject.Obliterate();
            return base.HandleEvent(E);
        }
    }
}
