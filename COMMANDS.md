# Elite & Ultimate Variants - Command Reference

Complete reference for all 40+ wish commands included in Elite & Ultimate Variants.

## How to Use Wish Commands

1. Press ` (backtick key) to open the wish menu
2. Type the command name
3. Press Enter
4. Follow any prompts that appear

---

## Essential Commands (Use These First!)

### `elitepreset`
**Purpose**: Instantly apply a difficulty preset to all settings.

**What it does**:
- Opens selection menu with 5 presets (Easy/Normal/Hard/Brutal/Nightmare)
- Immediately updates all 25+ settings to balanced values
- Shows confirmation message in both popup and message log
- Switches preset dropdown to match your selection

**When to use**:
- First time setup
- Quick difficulty adjustments mid-game
- Testing different difficulty levels
- After manually tweaking settings (to return to preset)

**Example workflow**:
```
` → elitepreset → Select "Hard" → Settings instantly updated
```

---

### `eliteautopreset`
**Purpose**: Enable automatic preset application when changing dropdown in options menu.

**What it does**:
- Attaches EliteVariantPresetMonitor to your character
- Monitors preset dropdown for changes every 10 turns
- Automatically applies new preset when detected
- Shows confirmation message when changes apply

**When to use**:
- Run ONCE per character at game start
- Enables seamless difficulty changes via options menu
- No more console commands needed after this!

**Example workflow**:
```
` → eliteautopreset → Done!
Now: Options → Change preset → Wait ~10 turns → Auto-applies
```

**Note**: This is persistent per character. You don't need to run it again unless you start a new character.

---

## Preset & Settings Commands

### `EliteVariantsSettings`
**Purpose**: Display all current settings in message log.

**What it does**:
- Shows all multipliers, spawn rates, requirements
- Displays current preset
- Lists toggle states (elite/ultimate/army spawning)

**When to use**:
- Verify settings after applying preset
- Check current configuration without opening options
- Debugging why spawns aren't working

---

## Spawning Commands

### `spawnelite`
**Purpose**: Manually spawn a single elite variant at your location.

**What it does**:
- Opens creature selection menu
- Spawns elite version (silver glow, 2.0x-3.0x power)
- Bypasses natural spawning requirements
- Useful for testing or creating specific encounters

**When to use**:
- Testing elite enhancements
- Creating custom encounters
- Inspecting specific creature as elite

---

### `spawnultimate`
**Purpose**: Manually spawn a single ultimate variant at your location.

**What it does**:
- Opens creature selection menu
- Spawns ultimate version (gold glow, 3.5x-6.0x power)
- Bypasses natural spawning requirements
- Maximum power variant

**When to use**:
- Testing ultimate enhancements
- Boss-tier custom encounters
- Endgame challenges

---

### `spawnarmy`
**Purpose**: Force spawn an elite army at your location.

**What it does**:
- Opens army type selection menu
- Spawns chosen army composition:
  - Small Squad (2-3 elites)
  - Elite Patrol (4-5 elites)
  - Elite Warband (1 ultimate + 4-5 goons)
  - Ultimate Host (2 ultimates + 6-8 goons)
  - Titan Pair (2-3 ultimates)
- Bypasses player level and zone tier requirements
- Still respects 1-army-per-zone limit

**When to use**:
- Testing army compositions
- Epic battle scenarios
- Stress testing

---

## Natural Spawning Control

### `elitespawn:status`
**Purpose**: Display comprehensive spawn statistics and current settings.

**What it does**:
- Shows if natural spawning is enabled
- Displays spawn attempt counters
- Lists successful/failed spawn counts
- Shows current spawn rates and requirements

**When to use**:
- Troubleshooting why elites aren't spawning
- Monitoring spawn frequency
- Verifying configuration

**Example output**:
```
Natural Elite Spawning: ENABLED
Elites Spawned: 47
Ultimates Spawned: 12
Spawn Attempts: 234
Last Spawn: 15 turns ago
```

---

### `elitespawn:toggle`
**Purpose**: Quick toggle natural spawning on/off.

**What it does**:
- Toggles EnableNaturalSpawning setting
- Shows current state
- Faster than opening options menu

**When to use**:
- Temporarily disable spawning for specific areas
- Quick on/off without menu navigation
- Testing with/without natural spawning

---

### `elitespawn:reset`
**Purpose**: Reset all spawn tracking and cooldowns.

**What it does**:
- Clears spawn attempt counters
- Resets zone density tracking
- Clears global cooldown
- Does NOT destroy existing elites

**When to use**:
- After changing settings (fresh start)
- Clearing stale tracking data
- Testing spawn mechanics

---

## Army System Commands

### `elitearmy:status`
**Purpose**: Display army spawning statistics.

**What it does**:
- Shows total armies spawned
- Breaks down by army type
- Lists zones with armies
- Shows army composition stats

**When to use**:
- Checking army spawn frequency
- Verifying army types are working
- Troubleshooting army spawning

---

### `elitearmy:reset`
**Purpose**: Reset army tracking and zone limits.

**What it does**:
- Clears army spawn counters
- Resets zone army limits
- Does NOT destroy existing armies

**When to use**:
- Allow armies to spawn again in zones
- Clear tracking data
- Fresh start for army testing

---

## Debug Commands

### `elitedebug:toggle`
**Purpose**: Toggle detailed debug logging.

**What it does**:
- Enables/disables verbose spawn logs
- Shows eligibility checks in message log
- Displays failure reasons for spawn attempts
- Performance impact when enabled

**When to use**:
- Troubleshooting spawn failures
- Understanding spawn decision process
- Mod development

**Warning**: Generates lots of messages. Use temporarily for debugging only.

---

### `elitedebug:status`
**Purpose**: Show current debug mode state.

**What it does**:
- Displays if debug mode is on/off
- Shows debug-related settings

**When to use**:
- Checking if debug mode is active
- Quick status check

---

### `elitedebug:stats`
**Purpose**: Display detailed spawn statistics with failure reasons.

**What it does**:
- Shows spawn attempts broken down by reason
- Lists failure categories:
  - Spawning disabled
  - Ineligible creatures
  - Zone tier too low
  - Player level too low
  - Zone density limit
  - Cooldown active
  - Roll failed
- Percentages and counts for each

**When to use**:
- Understanding why spawns are failing
- Identifying configuration issues
- Optimization

**Example output**:
```
Spawn Attempts: 500
Successful: 75 (15%)
Failed - Zone Tier: 200 (40%)
Failed - Cooldown: 150 (30%)
Failed - Roll: 75 (15%)
```

---

### `elitedebug:bypass`
**Purpose**: Show help for bypassing spawn restrictions during testing.

**What it does**:
- Displays list of bypass techniques
- Shows relevant commands
- Explains testing workflows

**When to use**:
- Mod development
- Testing spawn mechanics
- Learning advanced features

---

## Testing Commands (Automated)

### `elitetest:full`
**Purpose**: Run complete automated test suite.

**What it does**:
- Tests equipment system
- Tests mutation caps
- Tests stat multipliers
- Tests army compositions
- Tests settings system
- Shows pass/fail for each test
- Generates detailed report

**When to use**:
- Verifying mod installation
- After updating settings
- Mod development
- Troubleshooting

**Duration**: ~30 seconds

---

### `elitetest:equipment`
**Purpose**: Test equipment system only.

**What it does**:
- Spawns test elites with different equipment tiers
- Verifies tier-appropriate gear
- Checks equipment slot population
- Reports results

**When to use**:
- Verifying equipment settings
- Troubleshooting loot
- Testing equipment tier bonuses

---

### `elitetest:mutations`
**Purpose**: Test mutation cap system.

**What it does**:
- Spawns elites with varying mutation caps
- Verifies mental mutation limits
- Verifies physical mutation limits
- Reports if caps are respected

**When to use**:
- Verifying mutation cap settings
- Testing mutation distribution
- Troubleshooting mutation issues

---

### `elitetest:multipliers`
**Purpose**: Test stat multiplier system.

**What it does**:
- Spawns elites at different tiers
- Verifies power multipliers
- Verifies HP multipliers
- Checks enhancement multipliers
- Reports multiplier application

**When to use**:
- Verifying difficulty settings
- Testing custom multipliers
- Troubleshooting power scaling

---

### `elitetest:armies`
**Purpose**: Test all army compositions.

**What it does**:
- Spawns each of the 5 army types
- Verifies leader counts
- Verifies goon counts
- Checks party relationships
- Reports composition accuracy

**When to use**:
- Verifying army spawning
- Testing army settings
- Checking leader-goon dynamics

---

### `elitetest:settings`
**Purpose**: Test settings persistence and application.

**What it does**:
- Tests preset application
- Verifies setting persistence
- Checks option value ranges
- Tests custom vs preset modes

**When to use**:
- Verifying preset system
- Testing settings changes
- Troubleshooting options menu

---

### `elitetest:spawn`
**Purpose**: Quick spawn test with detailed stats report.

**What it does**:
- Spawns 1 elite and 1 ultimate
- Shows detailed stat comparison vs base creature
- Reports all applied enhancements
- Verifies tier differences

**When to use**:
- Quick verification
- Visual inspection
- Stat comparison

---

## Quick Visual Testing

### `elitequick:equipment`
**Purpose**: Spawn 5 elites with different equipment for visual inspection.

**What it does**:
- Spawns 5 elites side-by-side
- Each has different equipment tier bonus
- Allows visual comparison
- No cleanup (use elitecleanup after)

**When to use**:
- Visual verification of equipment
- Screenshot/demonstration
- Manual inspection

---

### `elitequick:mutations`
**Purpose**: Spawn 5 elites with different mutation cap configurations.

**What it does**:
- Spawns 5 elites side-by-side
- Each has different mutation caps
- Shows mutation cap effects
- No cleanup

**When to use**:
- Visual verification of mutation caps
- Comparing mutation diversity
- Manual testing

---

### `elitequick:tiers`
**Purpose**: Spawn 3 elites/ultimates showing tier differences.

**What it does**:
- Spawns 1 base creature, 1 elite, 1 ultimate
- Side-by-side comparison
- Visual tier differences
- No cleanup

**When to use**:
- Demonstrating tier differences
- Visual comparison
- Screenshots

---

### `elitequick:armies`
**Purpose**: Spawn all 5 army types in sequence.

**What it does**:
- Spawns each army type at your location
- Staggered spacing to prevent overlap
- Shows all compositions
- WARNING: Spawns 20-30 creatures total

**When to use**:
- Demonstrating army system
- Visual inspection of all types
- Epic battles (if you can handle it!)

**Note**: Use in open area. May cause lag. Use `elitecleanup` after.

---

### `elitequick:stress`
**Purpose**: Spawn 10 random elites/ultimates for stress testing.

**What it does**:
- Spawns 10 random elite or ultimate variants
- Random creature types
- Random elite/ultimate tier
- Stress test for performance

**When to use**:
- Performance testing
- Testing with many elites
- Chaos mode

**Warning**: Can cause lag on lower-end systems.

---

### `elitequick:reset`
**Purpose**: Reset all settings to defaults and clear all tracking.

**What it does**:
- Resets all settings to Normal preset
- Clears spawn tracking
- Clears army tracking
- Does NOT destroy existing elites
- Fresh start

**When to use**:
- Starting over with clean slate
- Undoing custom configuration
- Troubleshooting settings issues

---

## Utility Commands

### `elitecleanup`
**Purpose**: Destroy all elite creatures and clear tracking (fixes lag).

**What it does**:
- Destroys ALL elite variants in current zone
- Clears spawn tracking
- Clears army tracking
- Frees memory
- Shows count of destroyed elites

**When to use**:
- Game lagging from too many elites
- After stress testing
- Cleaning up after testing commands
- Starting fresh in a zone

**Warning**: Destroys ALL elites, including those from natural spawns. This is permanent!

**Example output**:
```
Destroyed 47 elite creatures
Tracking cleared
Zone reset
```

---

## Command Categories Quick Reference

**Most Important**: `elitepreset`, `eliteautopreset`

**Spawning**: `spawnelite`, `spawnultimate`, `spawnarmy`

**Status**: `elitespawn:status`, `elitearmy:status`, `elitedebug:stats`

**Control**: `elitespawn:toggle`, `elitespawn:reset`, `elitearmy:reset`

**Debug**: `elitedebug:toggle`, `elitedebug:status`, `elitedebug:bypass`

**Testing**: `elitetest:*` (7 commands)

**Quick Tests**: `elitequick:*` (5 commands)

**Utility**: `elitecleanup`, `EliteVariantsSettings`

---

## Recommended Command Workflow

### First Time Setup
```
1. eliteautopreset          - Enable auto-apply
2. elitepreset              - Select difficulty
3. elitespawn:status        - Verify configuration
```

### Adjusting Difficulty Mid-Game
```
Option A (Instant):
  elitepreset → Select new preset

Option B (Seamless):
  Options menu → Change dropdown → Wait ~10 turns
```

### Troubleshooting Spawns
```
1. elitespawn:status        - Check if enabled
2. elitedebug:toggle        - Enable debug mode
3. Play for a bit
4. elitedebug:stats         - See failure reasons
5. elitedebug:toggle        - Disable debug mode
```

### Testing Configuration
```
1. elitetest:spawn          - Quick verification
2. spawnelite               - Manual test
3. spawnultimate            - Ultimate test
4. elitecleanup             - Clean up
```

### Cleaning Up After Testing
```
1. elitecleanup             - Destroy all elites
2. elitespawn:reset         - Clear tracking
3. elitearmy:reset          - Clear army tracking
4. elitequick:reset         - Reset to defaults (optional)
```

---

## Tips

- Use `eliteautopreset` once at game start, then just use options menu
- Enable debug mode temporarily when troubleshooting
- Use `elitecleanup` after testing commands to prevent lag
- Check `elitespawn:status` before debugging spawn issues
- `elitequick:*` commands don't auto-cleanup - remember to use `elitecleanup`
- Automated tests (`elitetest:*`) show detailed reports
- Visual tests (`elitequick:*`) allow manual inspection

---

## Frequently Used Command Combinations

**Quick difficulty change**:
```
elitepreset → Select → Done
```

**Thorough spawn troubleshooting**:
```
elitespawn:status → elitedebug:toggle → [play] → elitedebug:stats
```

**Testing new settings**:
```
elitetest:spawn → [inspect] → elitecleanup
```

**Stress test**:
```
elitequick:stress → [battle] → elitecleanup
```

**Complete reset**:
```
elitecleanup → elitespawn:reset → elitearmy:reset → elitequick:reset
```

---

For more information, see README.md or the Steam Workshop description.
