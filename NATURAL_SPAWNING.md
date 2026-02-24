# EliteVariants - Natural Spawning Implementation

## ✅ Implementation Complete!

Natural elite spawning has been integrated into the EliteVariants mod. Elites and ultimates will now spawn automatically as you explore zones, scaled to provide balanced challenge.

---

## How It Works

### ObjectCreatedEvent Hook
The mod intercepts **all creature creation** in the game and has a chance to transform them into elite variants based on:
- Zone tier (1-8)
- Player level
- Spawn rate settings
- Per-zone density limits
- Global cooldown

### Automatic Scaling

| Zone Tier | Elite % | Ultimate % | Max per Zone |
|-----------|---------|------------|--------------|
| 1-3       | 0%      | 0%         | 0            |
| 4         | 2%      | 0%         | 1            |
| 5         | 5%      | 1%         | 2            |
| 6         | 8%      | 2%         | 3            |
| 7         | 12%     | 4%         | 4            |
| 8         | 15%     | 6%         | 5            |

**Player Level Gates:**
- Elites won't spawn until player level 10+
- Ultimates won't spawn until player level 20+

**Cooldown:**
- Minimum 50 turns between elite spawns (prevents spam)

---

## Configuration Settings

Natural spawning is **enabled by default** with these settings:
- **Spawn Rate:** 1.0x (adjustable 0.5x - 2.0x)
- **Min Player Level (Elites):** 10
- **Min Player Level (Ultimates):** 20
- **Min Zone Tier:** 4

---

## Wish Commands for Testing

### Status Check
```
elitespawn:status
```
Shows:
- Total elites spawned
- Elites in current zone
- Turns since last spawn
- Natural spawning enabled/disabled

### Toggle On/Off
```
elitespawn:toggle
```
Quickly enable/disable natural spawning for testing.

### Reset Counters
```
elitespawn:reset
```
Resets zone counters and clears caches. Useful for testing.

### Show All Settings
```
EliteVariantsSettings
```
Displays all mod settings including natural spawning configuration.

---

## Files Added

### New C# Files
1. **EliteSpawnController.cs** (7.4 KB)
   - Singleton controller with ObjectCreatedEvent handler
   - Maintains zone elite counters and cooldown tracking
   - Prevents infinite loops during transformation

2. **EliteSpawnEvaluator.cs** (8.4 KB)
   - Decision logic for elite transformation
   - Eligibility checking (reuses CreaturePool exclusions)
   - Spawn chance calculation with tier/level scaling

3. **EliteSpawnSafety.cs** (1.8 KB)
   - Constants and safety limits
   - Spawn chance tables by tier
   - Max elites per zone by tier

4. **EliteSpawnWishCommands.cs** (1.8 KB)
   - Wish command implementations for testing
   - Status, toggle, and reset commands

### Modified Files
1. **EliteVariantSettings.cs**
   - Added 5 new settings properties
   - Added HandleWishCommand() for testing
   - Extended GetDebugInfo() output

2. **ObjectBlueprints.xml**
   - Added EliteSpawnController blueprint
   - Added wish command blueprints (status, toggle, reset)

---

## Safety Measures

### Infinite Loop Prevention
- `_isTransforming` flag prevents recursive transformations
- Skips if `E.ReplacementObject` already set
- Skips creatures with `IsEliteVariant` property

### Performance Optimization
- Blueprint eligibility cached in dictionary
- Caches cleared on game load
- Early exit checks before expensive operations

### Hard Safety Caps
- Absolute max: 10 elites per zone (failsafe)
- Max elite percentage: 25% of zone creatures
- Minimum global cooldown: 25 turns (even at 2.0x multiplier)

---

## Testing Instructions

### Phase 1: Basic Functionality
1. Start new game
2. Travel to tier 4+ zone (should be safe, player level 1)
3. **Verify:** No elites spawn (below level 10)
4. Use wish: `xp:100000` to level up to 10
5. **Verify:** Elites begin spawning (white color)
6. Level up to 20
7. **Verify:** Ultimates begin spawning (gold color)

### Phase 2: Density Limits
1. Travel to tier 5 zone
2. Use `elitespawn:status` to check zone count
3. **Verify:** Max 2 elites per zone
4. Travel to tier 8 zone
5. **Verify:** Max 5 elites per zone

### Phase 3: Toggle Feature
1. Use `elitespawn:toggle` to disable
2. Spawn creatures with wishes
3. **Verify:** No transformations occur
4. Use `elitespawn:toggle` to re-enable
5. **Verify:** Transformations resume

### Phase 4: Settings Integration
1. Check `EliteVariantsSettings`
2. **Verify:** Shows natural spawning info
3. Test spawn rate multiplier (if ModOptions works)

---

## Known Limitations

1. **ModOptions.xml Disabled**
   - Options menu integration crashes game
   - Settings must be configured via wish commands
   - See plan file for investigation details

2. **No Retroactive Transformation**
   - Only affects NEW creature spawns
   - Existing creatures in loaded zones won't transform

3. **Zone Counter Reset**
   - Counters reset on game load (intentional)
   - Prevents save/load exploits

---

## Troubleshooting

### No Elites Spawning?
1. Check player level: `The.Player.Statistics["Level"].Value`
2. Check zone tier: Use Look command on zone
3. Verify feature enabled: `elitespawn:status`
4. Check cooldown: Last spawn turn in status

### Too Many/Few Elites?
- Adjust spawn rate with settings
- Default is 1.0x, try 0.5x (casual) or 1.5x (challenge)

### Game Performance Issues?
- Natural spawning has minimal overhead
- Eligibility checks are cached
- Only processes creatures (not items/furniture)

---

## Architecture Overview

```
Game Engine
    ↓
ObjectCreatedEvent fired
    ↓
EliteSpawnController intercepts
    ↓
EliteSpawnEvaluator checks:
  ✓ Feature enabled?
  ✓ Creature eligible?
  ✓ Zone tier sufficient?
  ✓ Player level sufficient?
  ✓ Zone density OK?
  ✓ Cooldown expired?
  ✓ RNG roll success?
    ↓
EliteVariantGenerator creates elite
    ↓
E.ReplacementObject set
    ↓
Original creature replaced with elite
```

---

## Next Steps

1. **Test in-game** - Start new character, level up, observe spawning
2. **Adjust settings** - Fine-tune spawn rates based on preference
3. **Report bugs** - Note any unexpected behavior
4. **ModOptions integration** - Optional: investigate crash (see plan file)

---

**Implementation Date:** February 15, 2026
**Status:** ✅ Complete and ready for testing
**Files Added:** 4 new, 2 modified
**Total Lines:** ~450 lines of new code
