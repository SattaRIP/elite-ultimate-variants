# Elite Variants - Difficulty Settings

## Overview

Elite Variants is designed to provide challenging encounters for high-level characters. The difficulty configuration system allows you to scale the challenge to match your character's power level.

**Note:** This mod does not include "easy mode" options. All settings scale from baseline (1.0x) to more challenging. The mod's purpose is to add variety and difficulty, not to make the game easier.

## Viewing Current Settings

Use the wish command:
```
EliteVariantsSettings
```

This will display your current settings and available configuration commands.

## Configuration Options

### 1. Power Multiplier (1.0x to 3.0x)
**Available Commands:**
- `EliteVariantsPower:1.0` - Default difficulty
- `EliteVariantsPower:1.25` - 25% stronger
- `EliteVariantsPower:1.5` - 50% stronger
- `EliteVariantsPower:2.0` - 2x stronger
- `EliteVariantsPower:2.5` - 2.5x stronger
- `EliteVariantsPower:3.0` - 3x stronger (brutal)

Controls the overall strength of elite variants. Scales HP, stats, AV, and DV globally.

**Effect:**
- HP: Scaled directly by multiplier
- Stats: All stat bonuses scaled by multiplier
- AV/DV: Defensive bonuses scaled by multiplier

### 2. Level Offset (0 to +30)
**Available Commands:**
- `EliteVariantsLevel:0` - Match zone/player level (default)
- `EliteVariantsLevel:+5` - 5 levels above you
- `EliteVariantsLevel:+10` - 10 levels above you
- `EliteVariantsLevel:+15` - 15 levels above you
- `EliteVariantsLevel:+20` - 20 levels above you
- `EliteVariantsLevel:+30` - 30 levels above you (extreme)

Adjusts how many levels above your character/zone the elites spawn.

**Effect:**
- Higher level = more HP, better stats, stronger mutations
- Each +5 levels significantly increases challenge

### 3. Ultimate Chance (0% to 100%)
**Available Commands:**
- `EliteVariantsUltimate:0` - Only Elite tier (white), never Ultimate
- `EliteVariantsUltimate:30` - Default (30% base chance)
- `EliteVariantsUltimate:50` - Equal chance of Elite/Ultimate
- `EliteVariantsUltimate:75` - Mostly Ultimate
- `EliteVariantsUltimate:100` - Only Ultimate tier (golden)

Sets the base chance for ultimate tier spawns before zone adjustments.

**Zone Tier Adjustments:**
- Tier 1-4: Chance × 0.67 (e.g., 30% → 20%)
- Tier 5-6: Chance × 1.33 (e.g., 30% → 40%)
- Tier 7-8: Chance × 2.0 (e.g., 30% → 60%)

**Effect:**
- Elite tier: 1-3 mental mutations, moderate power, white appearance
- Ultimate tier: 4-6 mental mutations, high power, golden appearance, better cybernetics (Tier 6-8)

### 4. Enhancement Multiplier (1.0x to 3.0x)
**Available Commands:**
- `EliteVariantsEnhancements:1.0` - Default enhancement count
- `EliteVariantsEnhancements:1.25` - 25% more enhancements
- `EliteVariantsEnhancements:1.5` - 50% more enhancements
- `EliteVariantsEnhancements:2.0` - Double enhancements
- `EliteVariantsEnhancements:2.5` - 2.5x enhancements
- `EliteVariantsEnhancements:3.0` - Triple enhancements (overwhelming)

Scales the number of mutations, cybernetics, and equipment granted to elites.

**Effect:**
- Mental mutation cap scaled (e.g., 3 → 9 at 3.0x)
- Total enhancement count scaled (e.g., 2-5 → 6-15 at 3.0x)
- More mutations = more abilities and threats

## Reset to Defaults

**Command:** `EliteVariantsReset`

Resets all settings to default values:
- Power: 1.0x
- Level Offset: +0
- Ultimate Chance: 30%
- Enhancements: 1.0x

## Recommended Configurations

### Standard Challenge (Default)
```
EliteVariantsReset
```
Balanced for challenging but fair encounters at your level.

### Hard Mode (For experienced high-level characters)
```
EliteVariantsPower:1.5
EliteVariantsLevel:+10
EliteVariantsUltimate:60
EliteVariantsEnhancements:1.5
```
Significantly tougher elites with more abilities, mostly ultimate tier.

### Brutal Mode (For overpowered builds)
```
EliteVariantsPower:2.0
EliteVariantsLevel:+20
EliteVariantsUltimate:100
EliteVariantsEnhancements:2.0
```
Only golden ultimates, massively overleveled with double enhancements.

### Nightmare Mode (Only for the truly prepared)
```
EliteVariantsPower:3.0
EliteVariantsLevel:+30
EliteVariantsUltimate:100
EliteVariantsEnhancements:3.0
```
Triple power, 30 levels above you, all ultimates with triple enhancements. Expect to face elites with 9+ mental mutations, tier 8 cybernetics, and stats that vastly exceed yours.

### Testing/Spectacle Mode (Visual variety without overwhelming challenge)
```
EliteVariantsPower:1.0
EliteVariantsLevel:0
EliteVariantsUltimate:100
EliteVariantsEnhancements:2.0
```
All golden ultimates with many abilities at your level - flashy and varied without being impossibly hard.

## Tips for High-Level Play

1. **Start at default:** Even default settings provide a significant challenge. Test the waters before increasing difficulty.

2. **Level offset matters most:** Each +5 levels dramatically increases the threat level. A level 50 elite at +20 offset becomes level 70.

3. **Enhancements scale exponentially:** At 3.0x, an ultimate can have 15+ total enhancements including 9 mental mutations. This is overwhelming even for optimized builds.

4. **Zone tier affects ultimate chance:** In high-tier zones (7-8), ultimate spawns are already common. Setting 100% ultimate guarantees golden elites everywhere.

5. **Settings persist:** Your configuration is saved between sessions. Adjust as your character grows in power.

6. **Adaptive scaling means variety:** High-Strength creatures become brutal melee monsters, high-Ego creatures become devastating psychics. Each encounter is unique.

## Power Level Reference

**Level 30 player, default settings (1.0x power, +0 level, 30% ultimate):**
- Elite: Level 30, ~500 HP, 2-4 enhancements
- Ultimate: Level 30, ~750 HP, 4-6 enhancements, tier 6-8 cybernetics

**Level 30 player, nightmare settings (3.0x power, +30 level, 100% ultimate, 3.0x enhancements):**
- Ultimate: Level 60, ~6750 HP, 12-18 enhancements including 9 mental mutations, tier 8 cybernetics
- This is designed to challenge even late-game god builds
