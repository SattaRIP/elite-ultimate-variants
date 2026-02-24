# Steam Workshop Upload Guide - Elite & Ultimate Variants

This guide walks you through uploading Elite & Ultimate Variants to the Steam Workshop for Caves of Qud.

## Prerequisites

1. **Steam Account** with Caves of Qud in your library
2. **Caves of Qud** must be installed
3. **Mod files** ready (you have them in this directory)

## Important Files

Before uploading, you'll need:

- ✅ **Mod files** - All files in this directory
- ✅ **Description** - Already prepared in `STEAM_DESCRIPTION.txt`
- ⏸️ **Preview image** - See "Creating Preview Image" section below

## Method 1: In-Game Upload (Easiest)

### Step 1: Prepare Your Mod

1. Ensure all mod files are in:
   ```
   Linux: ~/.config/unity3d/Freehold Games/CavesOfQud/Mods/EliteVariants/
   Windows: %USERPROFILE%\AppData\LocalLow\Freehold Games\CavesOfQud\Mods\EliteVariants\
   Mac: ~/Library/Application Support/Freehold Games/CavesOfQud/Mods/EliteVariants/
   ```

2. Verify these key files exist:
   - `manifest.json`
   - `ModOptions.xml`
   - `ObjectBlueprints.xml`
   - `Scripts/` directory with all .cs files
   - `README.md`

### Step 2: Launch Caves of Qud

1. Launch Caves of Qud
2. From main menu, click **Mods**
3. Find **Elite & Ultimate Variants** in the mod list
4. Ensure it's **enabled** (checkmark)

### Step 3: Upload to Workshop

1. With Elite & Ultimate Variants selected, click **Upload to Workshop**
2. Steam overlay will appear with upload form

### Step 4: Fill Workshop Details

**Title**: `Elite & Ultimate Variants`

**Description**: Copy/paste from `STEAM_DESCRIPTION.txt` (already BBCode formatted)

**Visibility**:
- Choose **Public** (recommended for community)
- Or **Friends Only** / **Private** for testing

**Tags**: Select appropriate tags:
- Gameplay
- Creatures
- Difficulty
- (Add others as appropriate)

**Change Notes**: `Initial release v1.0.0`

**Preview Image**: Upload your preview image (see section below)

### Step 5: Accept Agreement

1. Read Steam Workshop Agreement
2. Check "I agree" if you agree
3. Click **Upload**

### Step 6: Verify Upload

1. After upload completes, click **View Item**
2. Verify description formatting is correct
3. Check preview image displays
4. Test subscribe/unsubscribe

---

## Method 2: Manual Upload with SteamCMD (Advanced)

This method is more complex but gives you more control.

### Prerequisites

1. Install SteamCMD:
   - Windows: https://developer.valvesoftware.com/wiki/SteamCMD#Windows
   - Linux: `sudo apt install steamcmd` (or download from Valve)
   - Mac: https://developer.valvesoftware.com/wiki/SteamCMD#macOS

### Step 1: Create Workshop VDF File

Create `workshop_upload.vdf`:

```vdf
"workshopitem"
{
  "appid" "333640"
  "publishedfileid" "0"
  "contentfolder" "/path/to/EliteVariants"
  "previewfile" "/path/to/preview.png"
  "visibility" "0"
  "title" "Elite & Ultimate Variants"
  "description" "See STEAM_DESCRIPTION.txt"
  "changenote" "Initial release v1.0.0"
}
```

Replace `/path/to/EliteVariants` with actual path.
`publishedfileid` of "0" creates new item. After first upload, use the Workshop ID.

### Step 2: Run SteamCMD

```bash
steamcmd +login <your_steam_username> +workshop_build_item /path/to/workshop_upload.vdf +quit
```

Enter password when prompted.

### Step 3: Note Workshop ID

After upload completes, SteamCMD will display the Workshop ID. Save this for future updates.

---

## Creating a Preview Image

The preview image is the first thing users see on Steam Workshop. Recommended specs:

**Image Requirements**:
- Dimensions: **512x512 pixels** (or 16:9 ratio like 1920x1080)
- Format: PNG or JPG
- Size: Under 1MB

### Preview Image Ideas

1. **Screenshots**:
   - Elite variant with silver glow in combat
   - Ultimate variant with gold glow
   - Army formation
   - Side-by-side comparison (normal vs elite vs ultimate)

2. **Text Overlay**:
   - Mod title: "Elite & Ultimate Variants"
   - Key features: "5 Difficulty Presets • 40+ Commands • Elite Armies"
   - Version: "v1.0.0"

3. **Colors**:
   - Silver (#C0C0C0) for elite theme
   - Gold (#FFD700) for ultimate theme
   - Dark background to make creatures pop

### How to Create Preview Image

**Option 1: In-Game Screenshot**
1. Use `spawnultimate` to spawn a gold ultimate variant
2. Take screenshot with F12 (Steam) or F9 (in-game)
3. Edit with image editor to add text overlay
4. Crop to 512x512 or 1920x1080

**Option 2: Image Editor**
1. Use GIMP, Photoshop, or online tool (Canva, Photopea)
2. Create 512x512 canvas
3. Dark background (black or dark gray)
4. Add text: "Elite & Ultimate Variants"
5. Add subtitle: "Challenging Enhanced Enemies for Caves of Qud"
6. Optional: Add Caves of Qud logo/elements
7. Export as PNG

**Option 3: Template**
Use a template generator like:
- Canva (free templates)
- Steam Grid DB (for Steam artwork style)

---

## Updating Your Workshop Item

After initial upload, to update:

### In-Game Method
1. Make changes to mod files
2. Launch Caves of Qud → Mods
3. Select Elite & Ultimate Variants
4. Click **Upload to Workshop**
5. Fill **Change Notes** with what changed
6. Click **Update**

### SteamCMD Method
1. Update `workshop_upload.vdf` with actual Workshop ID:
   ```vdf
   "publishedfileid" "1234567890"  # Your actual Workshop ID
   ```
2. Run SteamCMD command again
3. Update change notes

---

## Workshop Description Tips

Your description is already prepared in `STEAM_DESCRIPTION.txt`. Tips for best results:

1. **BBCode Formatting**:
   - Use `[h1]`, `[h2]` for headers
   - Use `[b]` for bold, `[i]` for italics
   - Use `[code]` for commands
   - Use `[list]` for bullet points
   - Use `[table]` for data tables

2. **Structure**:
   - ✅ Clear feature list at top (done)
   - ✅ Important commands highlighted (done)
   - ✅ Quick start guide (done)
   - ✅ Detailed documentation (done)
   - ✅ Troubleshooting section (done)

3. **Length**:
   - Keep first section concise (hook users)
   - Detailed info below for interested users
   - Table of contents for long descriptions

---

## After Publishing

### Promote Your Mod

1. **GitHub Link**: Add Workshop link to GitHub README
2. **Reddit**: Post to r/cavesofqud
3. **Discord**: Share in Caves of Qud Discord #modding channel
4. **Twitter**: Tweet with #CavesOfQud hashtag

### Monitor Feedback

1. Check Workshop comments regularly
2. Respond to bug reports
3. Consider feature requests
4. Update based on user feedback

### Version Updates

When releasing updates:

1. Update version in `manifest.json`
2. Update README.md version history
3. Re-upload to Workshop with detailed change notes
4. Create GitHub release tag (v1.0.1, v1.0.2, etc.)
5. Announce update in Workshop announcement section

---

## Troubleshooting Upload Issues

### "Upload Failed" Error
- Verify you own Caves of Qud on Steam
- Check mod files are complete
- Ensure manifest.json is valid
- Try restarting Steam

### Preview Image Not Showing
- Check image is under 1MB
- Verify format is PNG or JPG
- Try re-uploading image separately
- Clear Steam cache

### Description Formatting Broken
- Verify BBCode syntax (closing tags)
- Test in Steam Workshop BBCode tester
- Check for special characters
- Copy from STEAM_DESCRIPTION.txt exactly

### Can't Find Mod in Game
- Verify mod files are in correct directory
- Check manifest.json has correct metadata
- Restart Caves of Qud
- Enable mod in mod manager

---

## Workshop Metadata Reference

**Title**: Elite & Ultimate Variants

**Short Description** (for workshop card):
```
Add challenging elite and ultimate creature variants with 5 difficulty presets, army spawning, and 40+ commands. Silver and gold glowing enemies with enhanced abilities!
```

**Tags**:
- Gameplay
- Creatures
- Difficulty
- Content
- QoL (Quality of Life)

**Language**: English

**Visibility**: Public (recommended)

---

## Required Files Checklist

Before uploading, verify these files exist:

Core Files:
- [ ] `manifest.json` - Mod metadata
- [ ] `ModOptions.xml` - Options menu configuration
- [ ] `ObjectBlueprints.xml` - Game object definitions
- [ ] `README.md` - GitHub documentation
- [ ] `STEAM_DESCRIPTION.txt` - Workshop description
- [ ] `COMMANDS.md` - Command reference
- [ ] `.gitignore` - Git configuration

Scripts (all .cs files in `Scripts/EliteVariants/`):
- [ ] `EliteVariantGenerator.cs`
- [ ] `EliteVariantPresets.cs`
- [ ] `EliteVariantPresetMonitor.cs`
- [ ] `EliteVariantPresetBootstrap.cs`
- [ ] `EliteVariantPresetCommands.cs`
- [ ] `EliteVariantEnableAutoPresets.cs`
- [ ] `EliteSpawnEvaluator.cs`
- [ ] `EliteArmySpawner.cs`
- [ ] All other .cs files...

Optional (but recommended):
- [ ] Preview image (512x512 or 1920x1080)
- [ ] Screenshots for Workshop gallery
- [ ] `LICENSE` file (if open source)
- [ ] `CHANGELOG.md` (version history)

---

## Post-Upload Checklist

After publishing to Workshop:

- [ ] Verify description displays correctly
- [ ] Check preview image shows
- [ ] Test subscribe/download
- [ ] Enable mod and verify it works
- [ ] Add Workshop URL to GitHub README
- [ ] Create GitHub release pointing to Workshop
- [ ] Post announcement on Reddit/Discord
- [ ] Monitor initial comments/bug reports

---

## Useful Links

- **Steam Workshop Agreement**: https://steamcommunity.com/sharedfiles/workshoplegalagreement
- **BBCode Guide**: https://steamcommunity.com/comment/Guide/formattinghelp
- **Caves of Qud Modding Wiki**: https://wiki.cavesofqud.com/wiki/Modding
- **SteamCMD Documentation**: https://developer.valvesoftware.com/wiki/SteamCMD

---

## Questions?

If you encounter issues:
1. Check this guide's troubleshooting section
2. Consult Caves of Qud modding Discord
3. Review Workshop upload tutorial videos
4. Contact Steam Support for account/technical issues

---

Good luck with your upload! Live and drink, friend.
