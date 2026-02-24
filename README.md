# Elite & Ultimate Variants

A comprehensive Caves of Qud mod that introduces powerful elite and ultimate variants of creatures, adding challenging encounters and exciting rewards to your adventures.

## Features

### Elite Variants
- **Powerful Enhanced Creatures**: Elite variants with 2.0x-3.0x stat multipliers (configurable)
- **Multiple Enhancement Types**: Physical mutations, mental mutations, cybernetics, and tier-appropriate equipment
- **Visual Distinction**: Silver glow and "elite" prefix for easy identification
- **Natural Spawning**: Elites spawn naturally during gameplay based on zone tier and player level

### Ultimate Variants
- **Legendary Boss Encounters**: Ultimate variants with 3.5x-6.0x stat multipliers (configurable)
- **Maximum Enhancements**: More mutations, better cybernetics, and superior equipment
- **Visual Distinction**: Gold glow and "ultimate" prefix
- **Rare Spawns**: Lower spawn rates but higher requirements for balanced difficulty

### Elite Army Spawning
- **Coordinated Groups**: Five army types ranging from small squads to titan pairs
- **Leader-Goon Dynamics**: Ultimates lead elites in organized formations
- **Zone Limitations**: Maximum 1 army per zone to prevent overwhelming encounters

### Difficulty Presets
Five carefully balanced presets scaling from 25% to 100% power:
- **Easy (25%)**: Relaxed difficulty for casual play
- **Normal (50%)**: Balanced experience (default)
- **Hard (75%)**: Challenging encounters
- **Brutal (87.5%)**: Very hard difficulty
- **Nightmare (100%)**: Maximum challenge

### Smart Preset Application
- **Manual Application**: Use `elitepreset` wish command for instant preset changes
- **Automatic Application**: Use `eliteautopreset` to enable auto-apply when changing presets in options menu
- **Seamless Integration**: Change difficulty mid-game without console commands

## Installation

1. Download the mod files
2. Extract to: `[Caves of Qud Directory]/Mods/EliteVariants/`
3. Enable the mod in-game: Main Menu → Mods → Enable "Elite & Ultimate Variants"
4. Start a new game or load an existing save

## Quick Start

### Recommended First Steps
1. Open Options → Mods: Elite Variants
2. Select a difficulty preset (Normal recommended for first playthrough)
3. In-game, use wish command `eliteautopreset` to enable automatic preset changes
4. Adjust individual settings in "Mods: Elite Variants (Custom)" category if desired

### Important Wish Commands

#### `elitepreset` - Quick Preset Application
**Most Important Command**: Instantly apply a difficulty preset to all settings.
- Opens selection menu with all 5 presets
- Immediately updates all multipliers and spawn settings
- Shows confirmation message when complete
- Perfect for quick difficulty adjustments mid-game

**How to use:**
1. Press ` (backtick) to open wish menu
2. Type `elitepreset`
3. Select desired preset from menu
4. Settings apply instantly

#### `eliteautopreset` - Enable Automatic Preset Changes
**Most Important Command**: Enable automatic preset application for seamless difficulty changes.
- Attaches monitor to your character
- Automatically applies preset changes from options menu
- Updates occur every ~10 turns after changing preset
- Shows confirmation message when changes apply

**How to use:**
1. Press ` (backtick) to open wish menu
2. Type `eliteautopreset`
3. Confirmation message appears
4. Now changing presets in options menu will auto-apply after ~10 turns

**Why use this?** Skip the console entirely - just change the dropdown in options and continue playing!

## Settings Guide

### Main Category: Mods: Elite Variants

#### Difficulty Preset
Master control for all settings. Select from 5 balanced presets or choose Custom to manually configure.

**Preset Breakdown:**

| Setting | Easy | Normal | Hard | Brutal | Nightmare |
|---------|------|--------|------|--------|-----------|
| Elite Power Mult. | 1.5x | 2.0x | 2.5x | 2.75x | 3.0x |
| Elite HP Mult. | 2.0x | 3.0x | 4.0x | 4.5x | 5.0x |
| Ultimate Power Mult. | 2.5x | 3.5x | 4.5x | 5.25x | 6.0x |
| Ultimate HP Mult. | 4.0x | 5.5x | 7.0x | 7.75x | 8.5x |
| Elite Spawn Rate | 50% | 100% | 150% | 175% | 200% |
| Ultimate Spawn Rate | 50% | 100% | 150% | 175% | 200% |
| Min Player Level | 10 | 16 | 20 | 23 | 25 |
| Min Zone Tier | 3 | 5 | 6 | 6 | 7 |

**Note:** Changing any individual setting in the Custom category will switch preset to "Custom"

#### Enable Elite Variants
Toggle natural spawning of elite variants (silver glow, moderate power boost).

#### Enable Ultimate Variants
Toggle natural spawning of ultimate variants (gold glow, maximum power boost).

#### Enable Elite Army Spawning
Toggle spawning of organized elite groups with leader-goon dynamics.

#### Enable Debug Mode
Show detailed spawn information and statistics in message log (for testing/development).

### Custom Category: Mods: Elite Variants (Custom)

All settings available as dropdowns with predefined values for stability.

#### Elite Settings
- **Power Multiplier (1.0x - 3.0x)**: Stat/AV/DV scaling for elites
- **HP Multiplier (1.0x - 5.0x)**: Health pool scaling
- **Enhancement Multiplier (1.0x - 3.0x)**: Number of mutations/upgrades
- **Level Offset (0 - 30)**: Additional levels added to elite variants
- **Spawn Rate (50% - 200%)**: Probability multiplier for elite spawns
- **Equipment Tier Bonus (0 - 4)**: Equipment quality boost above zone tier

#### Ultimate Settings
- **Power Multiplier (1.0x - 6.0x)**: Stat/AV/DV scaling for ultimates
- **HP Multiplier (1.0x - 8.5x)**: Health pool scaling
- **Enhancement Multiplier (1.0x - 4.0x)**: Number of mutations/upgrades
- **Level Offset (0 - 30)**: Additional levels added to ultimates
- **Spawn Rate (50% - 200%)**: Probability multiplier for ultimate spawns
- **Equipment Tier Bonus (0 - 6)**: Equipment quality boost

#### Spawn Requirements
**Note:** These use INVERSE scaling - lower values = harder (enemies appear earlier)

- **Min Player Level for Elites (5 - 25)**: Player must reach this level before elites spawn
- **Min Zone Tier for Elites (1 - 7)**: Zone must be this tier or higher
- **Min Player Level for Ultimates (15 - 35)**: Player level requirement for ultimates
- **Min Zone Tier for Ultimates (3 - 8)**: Zone tier requirement for ultimates
- **Min Player Level for Armies (10 - 30)**: Player level requirement for army spawns
- **Min Zone Tier for Armies (2 - 7)**: Zone tier requirement for armies

#### Army Settings
- **Army Spawn Rate (50% - 200%)**: Probability multiplier for army encounters
- **Leader Stat Bonus (100% - 200%)**: Additional power for army leaders
- **Goon Stat Bonus (50% - 150%)**: Power level for army goons

#### Advanced Settings
- **Max Elites Per Zone (1 - 8)**: Density limit for elite spawns per zone
- **Spawn Cooldown Turns (10 - 100)**: Minimum turns between elite spawns
- **Mental Mutation Cap (1 - 5)**: Maximum mental mutations per elite
- **Physical Mutation Cap (1 - 5)**: Maximum physical mutations per elite

## All Wish Commands

### Preset & Settings Commands

#### `elitepreset`
Instantly apply a difficulty preset. Opens selection menu with 5 presets.

#### `eliteautopreset`
Enable automatic preset application when changing dropdown in options menu.

#### `EliteVariantsSettings`
Display current settings in message log (legacy command, prefer options menu).

### Spawning Commands

#### `spawnelite`
Spawn a single elite variant at your location. Opens creature selection menu.

#### `spawnultimate`
Spawn a single ultimate variant at your location. Opens creature selection menu.

#### `spawnarmy`
Force spawn an elite army. Opens army type selection menu (SmallSquad, ElitePatrol, etc.).

### Natural Spawning Control

#### `elitespawn:status`
Display current natural spawning status, statistics, and settings.

#### `elitespawn:toggle`
Toggle natural spawning on/off without opening options menu.

#### `elitespawn:reset`
Reset natural spawning statistics and cooldowns.

### Army System Commands

#### `elitearmy:status`
Display army spawning statistics (armies spawned, composition breakdown).

#### `elitearmy:reset`
Reset army tracking data and zone limits.

### Debug Commands

#### `elitedebug:toggle`
Toggle debug mode (detailed spawn logs in message log).

#### `elitedebug:status`
Show debug mode status and current debug settings.

#### `elitedebug:stats`
Display detailed spawn statistics (attempts, successes, failures, reasons).

#### `elitedebug:bypass`
Show help for bypassing spawn restrictions during testing.

### Testing Commands

#### `elitetest:full`
Run complete automated test suite (all systems).

#### `elitetest:equipment`
Test equipment system (verify tier-appropriate gear).

#### `elitetest:mutations`
Test mutation caps (verify mental/physical limits).

#### `elitetest:multipliers`
Test stat multipliers (verify power scaling).

#### `elitetest:armies`
Test army compositions (verify all 5 army types).

#### `elitetest:settings`
Test settings system (verify preset application).

#### `elitetest:spawn`
Quick spawn test with detailed stat report.

### Quick Visual Testing

#### `elitequick:equipment`
Spawn 5 elites with different equipment configurations for visual inspection.

#### `elitequick:mutations`
Spawn 5 elites with different mutation cap configurations.

#### `elitequick:tiers`
Spawn 3 elites/ultimates showing tier differences.

#### `elitequick:armies`
Spawn all 5 army types in sequence.

#### `elitequick:stress`
Spawn 10 random elites/ultimates for stress testing.

#### `elitequick:reset`
Reset all settings to defaults and clear tracking.

### Utility Commands

#### `elitecleanup`
Destroy all elite creatures and clear tracking (fixes lag from too many spawns).

## How Spawning Works

### Individual Elite/Ultimate Spawning (Natural)

When a creature spawns naturally, the mod evaluates whether to transform it:

**7-Step Decision Process:**
1. **Natural spawning enabled?** Check if feature is turned on
2. **Creature eligible?** Must be hostile creature, not merchant/vendor/legendary
3. **Zone tier requirement met?** Zone must be high enough tier
4. **Player level requirement met?** Player must be high enough level
5. **Zone density limit OK?** Zone can't exceed max elites per zone
6. **Global cooldown expired?** Enough turns since last spawn
7. **Random roll succeeds?** Roll against spawn chance percentage

**Ultimate vs Elite Priority:**
- If player meets ultimate requirements, ultimate is tried first
- If ultimate roll fails, elite roll is attempted
- At high levels/tiers, more ultimates spawn naturally

### Army Spawning

Armies spawn from spawner objects (placed manually or via wish commands):

**Requirements:**
- Army spawning enabled
- Player level ≥ global minimum for armies
- Player level ≥ army type minimum (15-35 depending on type)
- Zone tier ≥ global minimum for armies
- Zone tier ≥ army type minimum (tier 5-8 depending on type)
- Zone doesn't already have an army (hard limit: 1 per zone)

**Army Types:**

| Type | Leaders | Goons | Min Level | Min Tier |
|------|---------|-------|-----------|----------|
| Small Squad | 0 | 2-3 | 15 | 5 |
| Elite Patrol | 0 | 4-5 | 20 | 6 |
| Elite Warband | 1 | 4-5 | 25 | 7 |
| Ultimate Host | 2 | 6-8 | 30 | 8 |
| Titan Pair | 2-3 | 0 | 35 | 8 |

**Key Differences:**
- Individual spawning is probabilistic and continuous
- Army spawning is deterministic (if conditions met, army spawns)
- Armies have leader-goon party dynamics
- Individual elites are independent

## Interactions with Vanilla Content

### Legendary Creatures
**Most legendaries are excluded** from transformation due to:
- `ExcludeFromDynamicEncounters` tag
- `IgnoresBeguiling` or `IgnoresPhase` tags
- Manual placement (not natural spawning)

**Force spawning an elite legendary** (via `spawnelite`/`spawnultimate` wish commands):
- Preserves all original mutations and abilities
- Adds new mutations they don't already have
- Stacks elite bonuses on top of legendary power
- Results in extremely powerful super-boss encounters

### Faction Relationships
Elite variants **preserve their original faction**:
- Elite snapjaw is still snapjaw faction
- Elite Putus Templar is still Putus Templar faction
- Enables elite vs elite faction warfare
- Player faction relationships apply normally

### Equipment & Loot
- Elites drop their equipped items when killed
- Equipment is tier-appropriate and often powerful
- Higher tier zones = better elite equipment
- Ultimate variants have best-in-tier gear

## Troubleshooting

### Elites Not Spawning
1. Check `elitespawn:status` - is natural spawning enabled?
2. Verify player level meets minimum (default: 16 for elites, 25 for ultimates)
3. Verify zone tier meets minimum (default: tier 5+ for elites, tier 5+ for ultimates)
4. Check `elitedebug:stats` to see why spawns are failing

### Game Lag After Many Spawns
Use `elitecleanup` wish command to destroy all elite creatures and clear tracking.

### Preset Not Applying
1. If using options menu, wait ~10 turns for auto-apply
2. Or use `elitepreset` wish command for instant application
3. Verify you ran `eliteautopreset` at least once per character

### Armies Not Spawning
1. Check army spawning is enabled in options
2. Verify minimum requirements (default: player level 20+, zone tier 4+)
3. Check zone doesn't already have an army (limit: 1 per zone)
4. Use `elitearmy:status` to see statistics

## Performance Notes

- Elite spawning checks run every 10 turns (minimal performance impact)
- Zone density limits prevent overcrowding
- Global cooldown prevents spawn spam
- Eligibility checks are cached for performance

## Credits

Created by mythraps for Caves of Qud

## Version History

### v1.0.0 (Initial Release)
- Elite and Ultimate variant system
- 5 difficulty presets with smart scaling
- Elite army spawning with 5 army types
- Automatic and manual preset application
- 40+ wish commands for customization and testing
- Comprehensive options menu with combo box interface
- Natural spawning with safety limits
- Debug and testing tools

## License

This mod is provided as-is for Caves of Qud. Feel free to modify for personal use.
