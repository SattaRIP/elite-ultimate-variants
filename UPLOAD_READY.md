# Elite & Ultimate Variants - Ready for Upload!

All files have been prepared for uploading to both GitHub and Steam Workshop.

## What Has Been Created

### Documentation Files
✅ **README.md** - Comprehensive GitHub documentation with:
- Complete feature overview
- Installation instructions
- All 40+ wish commands documented
- **elitepreset** and **eliteautopreset** highlighted as most important
- Difficulty preset breakdown table
- Spawn mechanics explained
- Troubleshooting guide
- Legendary creature interactions
- Version history

✅ **STEAM_DESCRIPTION.txt** - BBCode formatted Steam Workshop description with:
- Feature highlights
- **elitepreset** and **eliteautopreset** prominently featured
- Quick start guide
- Preset comparison table
- All commands listed
- Troubleshooting section
- Formatted for Steam Workshop BBCode

✅ **COMMANDS.md** - Complete command reference guide:
- All 40+ commands documented in detail
- Usage examples for each
- Recommended workflows
- Command combinations
- Tips and tricks

✅ **STEAM_UPLOAD_GUIDE.md** - Step-by-step workshop upload instructions:
- In-game upload method (easiest)
- SteamCMD method (advanced)
- Preview image creation guide
- Post-upload checklist
- Troubleshooting

### Upload Tools
✅ **upload_to_github.sh** - Automated GitHub upload script:
- Creates repository
- Commits all files
- Pushes to GitHub
- Creates release (optional)
- Works with GitHub CLI or manual token

✅ **.gitignore** - Git configuration:
- Excludes build artifacts
- Ignores user-specific files
- Configured for C# projects

✅ **.gitattributes** - Git line ending configuration

---

## What You Need to Do

### For GitHub Upload

#### Option 1: Using the Script (Recommended)

1. Open terminal in the mod directory:
   ```bash
   cd ~/.config/unity3d/Freehold\ Games/CavesOfQud/Mods/EliteVariants/
   ```

2. Run the upload script:
   ```bash
   ./upload_to_github.sh
   ```

3. Follow the prompts:
   - Enter your GitHub username
   - Enter repository name (suggested: `elite-ultimate-variants`)
   - Choose public/private
   - Authenticate when prompted

4. The script will:
   - Initialize git repository
   - Commit all files
   - Create GitHub repository
   - Push code
   - Optionally create v1.0.0 release

#### Option 2: Manual Upload

If you prefer manual control:

1. Create repository at https://github.com/new
   - Name: `elite-ultimate-variants`
   - Visibility: Public (recommended)
   - **Do NOT** initialize with README

2. In terminal:
   ```bash
   cd ~/.config/unity3d/Freehold\ Games/CavesOfQud/Mods/EliteVariants/
   git init
   git add .
   git commit -m "Initial commit - Elite & Ultimate Variants v1.0.0"
   git branch -M main
   git remote add origin https://github.com/YOUR_USERNAME/elite-ultimate-variants.git
   git push -u origin main
   ```

3. Create release at https://github.com/YOUR_USERNAME/elite-ultimate-variants/releases/new
   - Tag: `v1.0.0`
   - Title: `Elite & Ultimate Variants v1.0.0`
   - Description: Copy from README.md features section

---

### For Steam Workshop Upload

#### Step 1: Create Preview Image

You need a 512x512 (or 1920x1080) preview image. Options:

**Quick Option**: In-game screenshot
1. Launch Caves of Qud
2. Use wish command: `spawnultimate`
3. Take screenshot (F12 for Steam, F9 in-game)
4. Crop to 512x512 using image editor
5. Optional: Add text overlay "Elite & Ultimate Variants v1.0.0"

**Polished Option**: Use image editor (GIMP, Photoshop, Canva)
1. Create 512x512 canvas
2. Dark background
3. Add title "Elite & Ultimate Variants"
4. Add subtitle "Enhanced Enemies for Caves of Qud"
5. Use silver (#C0C0C0) and gold (#FFD700) theme colors
6. Export as PNG

Save as: `preview.png` in the mod directory

#### Step 2: Upload to Workshop

**Method 1 - In-Game (Easiest):**

1. Launch Caves of Qud
2. Main Menu → **Mods**
3. Find **Elite & Ultimate Variants**
4. Enable it (checkmark)
5. Click **Upload to Workshop**
6. Fill in the form:
   - **Title**: Elite & Ultimate Variants
   - **Description**: Copy/paste from `STEAM_DESCRIPTION.txt`
   - **Preview Image**: Upload your `preview.png`
   - **Visibility**: Public
   - **Tags**: Gameplay, Creatures, Difficulty
   - **Change Notes**: Initial release v1.0.0
7. Click **Upload**
8. Click **View Item** to verify

**Method 2 - SteamCMD (Advanced):**

See `STEAM_UPLOAD_GUIDE.md` for detailed instructions.

---

## After Uploading

### GitHub
1. Add topics to repository:
   - caves-of-qud
   - mod
   - roguelike
   - csharp
   - game-mod

2. Add Steam Workshop link to README
3. Enable GitHub Issues for bug reports

### Steam Workshop
1. Verify description formatting
2. Test subscribe/download
3. Add screenshots to gallery (optional):
   - Elite variant in combat
   - Ultimate variant
   - Elite army formation
   - Options menu

4. Make announcement post with key features

### Promotion
1. Post to r/cavesofqud on Reddit
2. Share in Caves of Qud Discord (#modding channel)
3. Tweet with #CavesOfQud
4. Add Workshop link to GitHub README

---

## Credentials Needed

### For GitHub:
- GitHub username
- **Either**:
  - GitHub Personal Access Token (with `repo` permission)
    - Create at: https://github.com/settings/tokens/new
  - **OR** GitHub CLI (gh) installed and authenticated
    - Install: https://cli.github.com/

### For Steam Workshop:
- Steam account with Caves of Qud
- Preview image file (512x512 PNG/JPG)
- No special credentials needed for in-game upload!

---

## Quick Reference: What Files Are Where

```
EliteVariants/
├── README.md                          # GitHub documentation
├── STEAM_DESCRIPTION.txt              # Workshop description (BBCode)
├── COMMANDS.md                        # Complete command reference
├── STEAM_UPLOAD_GUIDE.md             # Workshop upload tutorial
├── UPLOAD_READY.md                   # This file
├── upload_to_github.sh               # GitHub upload script
├── .gitignore                        # Git configuration
├── .gitattributes                    # Git line endings
├── manifest.json                     # Mod metadata
├── ModOptions.xml                    # Options menu
├── ObjectBlueprints.xml              # Game objects
├── Scripts/EliteVariants/            # C# source code
│   ├── EliteVariantGenerator.cs
│   ├── EliteVariantPresets.cs
│   ├── EliteVariantPresetMonitor.cs
│   ├── EliteVariantPresetBootstrap.cs
│   ├── EliteVariantPresetCommands.cs
│   ├── EliteVariantEnableAutoPresets.cs
│   └── ... (all other .cs files)
└── preview.png                       # YOU NEED TO CREATE THIS
```

---

## Checklist Before Upload

### GitHub
- [ ] Review README.md (looks good?)
- [ ] Verify all .cs files are present
- [ ] Check manifest.json version is 1.0.0
- [ ] Have GitHub credentials ready

### Steam Workshop
- [ ] Create preview.png (512x512 or 1920x1080)
- [ ] Review STEAM_DESCRIPTION.txt
- [ ] Have Steam account logged in
- [ ] Mod enabled in-game
- [ ] Tested mod works on current save

---

## Support After Upload

After uploading, monitor for:
- Bug reports (GitHub Issues / Workshop comments)
- Feature requests
- Compatibility issues
- Balance feedback

Respond to feedback and update as needed. Use version numbers:
- `v1.0.1`, `v1.0.2` for bug fixes
- `v1.1.0`, `v1.2.0` for new features
- `v2.0.0` for major changes

---

## You're All Set!

Everything is ready for upload. Just need:
1. Your GitHub credentials
2. A preview image for Steam Workshop

Run `./upload_to_github.sh` when ready for GitHub.
Follow `STEAM_UPLOAD_GUIDE.md` when ready for Steam Workshop.

Good luck! Live and drink, friend.
