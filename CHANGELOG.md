# Changelog

All notable changes to the Elite Variants mod will be documented in this file.

## [1.0.0] - 2026-02-22

### Added

#### Core Features
- **Elite Variants**: Enhanced creatures with white/silver visual theme
- **Ultimate Variants**: Golden supreme variants with significantly higher power
- **Natural Spawning System**: Variants spawn naturally based on player level and zone tier
- **Mixed Enhancement System**: Variants receive random mix of mutations, cybernetics, and equipment

#### Enhancement Types
- **Physical Mutations**: 33 mutations with smart exclusion rules (e.g., incompatible mutations)
- **Mental Mutations**: 27 psychic powers with tier-appropriate levels
- **Cybernetics**: Tier 4-8 implants via CyberneticsHasRandomImplants
- **Equipment**: Tier 6-8 weapons and armor (zetachrome, crysteel, flawless crysteel)

#### Elite Army System
- **Small Squad**: 2-3 elites (Tier 5+ zones, Level 15+)
- **Elite Patrol**: 4-5 elites (Tier 6+ zones, Level 20+)
- **Elite Warband**: 1 ultimate + 4-5 goons (Tier 7+ zones, Level 25+)
- **Ultimate Host**: 2 ultimates + 6-8 goons (Tier 8+ zones, Level 30+)
- **Titan Pair**: 2-3 ultimates, no goons (Tier 8+ zones, Level 35+)

#### Configuration System
- **Difficulty Presets**: Normal, Hard, Brutal, Nightmare, Custom
- **Full Mod Options Integration**: All settings configurable via Options > Mods: Elite Variants
- **Per-Tier Settings**: Separate configuration for Elite vs Ultimate variants

#### Configurable Settings
- Power Multiplier (1.0x - 3.0x Elite, 1.0x - 6.0x Ultimate)
- HP Multiplier (1.0x - 5.0x Elite, 1.0x - 10.0x Ultimate)
- Enhancement Multiplier (affects mutations, cybernetics, equipment)
- Level Offset (+0 to +30 bonus levels)
- Spawn Rate (0.5x - 2.0x frequency)
- Spawn Chance % (0-100%)
- Min Player Level (1-30 for Elite, 10-40 for Ultimate)
- Min Zone Tier (1-8)
- Physical/Mental Mutation Caps (0 = unlimited)
- Equipment Item Counts (0-4)
- Tier 7/8 Equipment Chances (0-100%)

#### Wish Commands
- Core: `spawnelite`, `spawnultimate`, `spawnarmy`, `elitepreset`
- Status: `elitespawn:status`, `elitearmy:status`
- Control: `elitespawn:toggle`, `elitespawn:reset`, `elitearmy:reset`
- Debug: `elitedebug:toggle`, `elitedebug:status`, `elitedebug:stats`
- Testing: `elitetest:full`, `elitetest:equipment`, `elitetest:mutations`, `elitetest:armies`
- Quick Tests: `elitequick:equipment`, `elitequick:mutations`, `elitequick:tiers`, `elitequick:armies`, `elitequick:stress`
- Utility: `elitecleanup`, `elitequick:reset`

#### Population Table Integration
- Tier 5-8 Cave populations
- Tier 5-8 Ruins populations
- TierXCaveEncounters

#### Technical Features
- Adaptive stat scaling (buffs creature's existing strengths)
- Creature pool exclusion rules (no bosses, merchants, immobile creatures)
- Zone-based elite limits (prevents overcrowding)
- Cooldown system between spawns
- Safe infinite loop prevention

### Fixed
- Fixed crash when selecting "Custom" difficulty preset (removed CheckAndApplyPreset call from property getters)

### Notes
- Compatible with vanilla Caves of Qud and most creature mods
- Variants can be created from any eligible creature in the game
- All settings persist across game sessions
