# Elite Variants - Comprehensive Testing Guide

This document provides detailed testing procedures for all Elite Variants mod features, organized by priority and category.

**Quick testing?** See **TESTING_QUICK.md** for a 5-10 minute smoke test.

---

## Table of Contents

1. [Automated Testing](#automated-testing) (Priority: CRITICAL)
2. [Basic Spawning](#basic-spawning) (Priority: CRITICAL)
3. [Visual Effects](#visual-effects) (Priority: HIGH)
4. [Elite Tiers](#elite-tiers) (Priority: HIGH)
5. [Enhancement Systems](#enhancement-systems) (Priority: HIGH)
6. [Army System](#army-system) (Priority: MEDIUM)
7. [Settings & Configuration](#settings--configuration) (Priority: MEDIUM)
8. [Natural Spawning](#natural-spawning) (Priority: MEDIUM)
9. [Edge Cases](#edge-cases) (Priority: LOW)
10. [Performance Testing](#performance-testing) (Priority: LOW)

---

## Test Environment Setup

### Prerequisites
- Fresh game save or dedicated test save
- Access to wish commands (press ~ or `)
- Character level 20+ recommended for comprehensive testing
- Open area with space for multiple spawns
- Ability to examine creatures (look at them)

### Recommended Test Zone
- Tier 6-8 zone for realistic spawning conditions
- Or use `EliteVariantsLevel:0` to match your current level

### Reset State Before Testing
```
EliteVariantsReset
elitespawn:reset
elitearmy:reset
```

---

## 1. Automated Testing
**Priority: CRITICAL** | **Time: 3-5 minutes**

The mod includes comprehensive automated tests that verify most functionality.

### 1.1 Full Test Suite

**Command:** `elitetest:full`

**What it tests:**
- Equipment count ranges (elite vs ultimate)
- Mutation cap limits (elite: 1-3, ultimate: 4-6)
- Stat multiplier scaling (power settings)
- Army composition (leader/goon ratios)
- Settings persistence

**Expected result:**
```
========================================
       ELITE VARIANTS TEST REPORT
========================================

PASS Equipment System
  PASS Elite item count range: Expected 2-5, Got 2-5
  PASS Ultimate item count range: Expected 3-8, Got 4-7
  PASS Elite/Ultimate item differential: Ultimate has more

PASS Mutation Caps
  PASS Elite mental mutation cap: Expected 1-3, Got 1-3
  PASS Ultimate mental mutation cap: Expected 4-6, Got 4-6

PASS Stat Multipliers
  PASS Power 1.0x baseline
  PASS Power 2.0x doubles stats
  PASS Power 3.0x triples stats

PASS Army Compositions
  PASS Small Squad: 2-3 members
  PASS Elite Patrol: 4-5 members
  PASS Elite Warband: 5-6 members (1 ultimate + goons)
  PASS Ultimate Host: 8-10 members (2 ultimates + goons)

PASS Settings System
  PASS Settings persist between commands
  PASS Reset command works

=== TEST SUMMARY: 5/5 PASSED ===
```

**If tests fail:**
1. Note which specific subtest failed
2. Run individual test for that category
3. Check error message in log
4. Reset settings and try again

### 1.2 Individual Test Categories

Run these if you want to focus on specific systems or if `elitetest:full` reports failures.

#### Equipment System
**Command:** `elitetest:equipment`

**Tests:**
- Elite item count: 2-5 items
- Ultimate item count: 3-8 items
- Ultimate should have more items than elite on average

**Manual verification:**
1. Run `elitetest:spawn`
2. Examine both creatures
3. Count equipped/inventory items
4. Ultimate should have noticeably more gear

#### Mutation Caps
**Command:** `elitetest:mutations`

**Tests:**
- Elite mental mutations: 1-3
- Ultimate mental mutations: 4-6
- Enhancement multiplier scaling

**Manual verification:**
1. Run `elitetest:spawn`
2. Examine both creatures
3. Count mental mutations in their description
4. Elite: 1-3 mutations, Ultimate: 4-6 mutations

#### Stat Multipliers
**Command:** `elitetest:multipliers`

**Tests:**
- Power 1.0x = baseline stats
- Power 2.0x = 2x HP, AV, DV
- Power 3.0x = 3x HP, AV, DV

**Manual verification:**
1. Run `elitedebug:status` (note power setting)
2. Run `elitetest:spawn` (note elite HP/stats)
3. Run `EliteVariantsPower2.0`
4. Run `elitetest:spawn` (HP should be ~2x higher)

#### Army Compositions
**Command:** `elitetest:armies`

**Tests:**
- Small Squad: 2-3 members
- Elite Patrol: 4-5 members
- Elite Warband: 1 ultimate leader + 4-5 goons
- Ultimate Host: 2 ultimate leaders + 6-8 goons
- Titan Pair: 2-3 ultimates, no goons

#### Settings Persistence
**Command:** `elitetest:settings`

**Tests:**
- Settings save between commands
- Settings reset properly
- Multiple settings can be changed

---

## 2. Basic Spawning
**Priority: CRITICAL** | **Time: 5 minutes**

### 2.1 Quick Spawn Test

**Command:** `elitetest:spawn`

**Expected result:**
- Spawns 1 elite and 1 ultimate
- Displays stats comparison in message log
- Both creatures have enhanced stats vs base creature

**Verify:**
- [ ] Elite appears with cyan/white glow
- [ ] Ultimate appears with golden/yellow glow
- [ ] Stats shown in message log
- [ ] No error messages

### 2.2 Individual Spawn Commands

#### Spawn Elite
**Command:** `spawnelite`

**Verify:**
- [ ] Single elite variant spawns
- [ ] Has cyan/white visual effect
- [ ] Has 1-3 mental mutations
- [ ] Has enhanced stats (examine creature)

#### Spawn Ultimate
**Command:** `spawnultimate`

**Verify:**
- [ ] Single ultimate variant spawns
- [ ] Has golden/yellow visual effect
- [ ] Has 4-6 mental mutations
- [ ] Has significantly enhanced stats
- [ ] Has tier 6-8 cybernetics

### 2.3 Legacy Spawn Commands

#### EliteVariantSpawn
**Command:** `EliteVariantSpawn`

**Verify:**
- [ ] Spawns elite with random tier (based on zone)
- [ ] Tier 1-4 zones: likely elite tier
- [ ] Tier 7-8 zones: likely ultimate tier

#### UltimateVariantSpawn
**Command:** `UltimateVariantSpawn`

**Verify:**
- [ ] Always spawns ultimate tier
- [ ] Golden appearance guaranteed
- [ ] 4-6 mental mutations

### 2.4 Spawn Consistency

**Test procedure:**
1. Run `spawnelite` 10 times
2. Verify all have cyan/white effect
3. Verify all have 1-3 mental mutations
4. Verify variety in creature types

**Expected:**
- [ ] Consistent visual effects
- [ ] Consistent tier (all elite)
- [ ] Variety in base creatures
- [ ] No duplicate creatures (usually)

---

## 3. Visual Effects
**Priority: HIGH** | **Time: 3 minutes**

### 3.1 Elite Tier Appearance

**Spawn:** `spawnelite`

**Visual checks:**
- [ ] Cyan/white glow (2-cell radius)
- [ ] Creature has "&amp;C" or "&amp;c" color code (cyan)
- [ ] Visible shimmer/aura effect
- [ ] Glow visible in darkness
- [ ] Glow doesn't obscure creature's base appearance

### 3.2 Ultimate Tier Appearance

**Spawn:** `spawnultimate`

**Visual checks:**
- [ ] Golden/yellow glow (2-cell radius)
- [ ] Creature has "&amp;Y" or "&amp;y" color code (yellow)
- [ ] More intense shimmer than elite
- [ ] Glow visible in darkness
- [ ] Distinctive from elite tier

### 3.3 Color Consistency

**Test procedure:**
1. Spawn 5 elites with `spawnelite`
2. Spawn 5 ultimates with `spawnultimate`
3. Verify all elites have same color family (cyan/white)
4. Verify all ultimates have same color family (golden/yellow)

**Expected:**
- [ ] Consistent coloring within each tier
- [ ] Clear visual distinction between tiers
- [ ] Colors don't change over time
- [ ] Colors persist after save/load

### 3.4 Glow Effects

**Enable debug mode:** `elitedebug:toggle`

**Spawn:** `spawnelite`

**Check debug output for:**
- [ ] Glow color applied
- [ ] Glow radius = 2
- [ ] Glow visible to player
- [ ] No glow rendering errors

---

## 4. Elite Tiers
**Priority: HIGH** | **Time: 5 minutes**

### 4.1 Elite Tier Characteristics

**Spawn:** `spawnelite` (multiple times)

**Verify for each spawn:**
- [ ] 1-3 mental mutations (count them)
- [ ] Cyan/white appearance
- [ ] Enhanced stats (check HP, AV, DV)
- [ ] 2-5 equipment items
- [ ] Tier 4-6 cybernetics (if any)

**Expected ranges (default 1.0x power):**
- HP: 500-1000 (varies by base creature)
- AV: 8-12
- DV: 6-10
- Mental mutations: 1-3
- Equipment: 2-5 items

### 4.2 Ultimate Tier Characteristics

**Spawn:** `spawnultimate` (multiple times)

**Verify for each spawn:**
- [ ] 4-6 mental mutations (count them)
- [ ] Golden/yellow appearance
- [ ] Significantly enhanced stats
- [ ] 3-8 equipment items
- [ ] Tier 6-8 cybernetics (better than elite)

**Expected ranges (default 1.0x power):**
- HP: 750-1500 (varies by base creature)
- AV: 10-15
- DV: 8-12
- Mental mutations: 4-6
- Equipment: 3-8 items

### 4.3 Tier Progression

**Test procedure:**
1. Spawn 1 elite: `spawnelite`
2. Note its stats (HP, mutations, equipment count)
3. Spawn 1 ultimate: `spawnultimate`
4. Note its stats
5. Compare

**Expected:**
- [ ] Ultimate has more HP than elite (50-100% more)
- [ ] Ultimate has more mental mutations (at least +2)
- [ ] Ultimate has better cybernetics (higher tier)
- [ ] Ultimate has more equipment items
- [ ] Ultimate is clearly more threatening

### 4.4 Tier Forcing

**Test ultimate forcing:**
1. Run `EliteVariantsUltimate100` (100% ultimate chance)
2. Run `EliteVariantSpawn` 5 times
3. Verify all spawns are ultimate tier (golden)

**Test elite forcing:**
1. Run `EliteVariantsUltimate0` (0% ultimate chance)
2. Run `EliteVariantSpawn` 5 times
3. Verify all spawns are elite tier (cyan)

**Reset:** `EliteVariantsReset`

---

## 5. Enhancement Systems
**Priority: HIGH** | **Time: 10 minutes**

### 5.1 Mental Mutations

**Enable debug:** `elitedebug:toggle`

**Spawn:** `spawnultimate`

**Verify mental mutations:**
- [ ] Count total mental mutations (should be 4-6)
- [ ] Check mutation levels (should be 8-10 for ultimates)
- [ ] Verify mutations are functional (check creature abilities)
- [ ] No duplicate mutations on same creature

**Common mental mutations to look for:**
- Teleportation, Force Wall, Sunder Mind
- Ego Projection, Temporal Fugue
- Psychometry, Precognition
- Light Manipulation, Cryokinesis

**Test mutation levels:**
1. Examine ultimate
2. Check each mutation's level in description
3. Should be level 8-10 for ultimate tier
4. Should be level 4-6 for elite tier

### 5.2 Physical Mutations

**Spawn several elites/ultimates and look for:**
- [ ] Multiple Legs (extra movement speed)
- [ ] Multiple Arms (extra attacks)
- [ ] Quills (defensive damage)
- [ ] Heightened Speed (extra quickness)
- [ ] Carapace (extra AV)
- [ ] Horns (charge attack)

**Verify:**
- [ ] Mutations are visible in creature description
- [ ] Mutations are functional (watch combat)
- [ ] Mutation levels scale with tier
- [ ] No game-breaking combinations

### 5.3 Cybernetics

**Spawn ultimate:** `spawnultimate`

**Look for high-tier cybernetics:**
- [ ] Tier 6-8 implants (check description)
- [ ] Dermal Insulation (temp resist)
- [ ] Optical Bioscanner (detects life)
- [ ] Nocturnal Apex (night vision)
- [ ] Parabolic Muscular Subroutine (strength boost)

**Verify:**
- [ ] Cybernetics appear in examination
- [ ] Tier appropriate (6-8 for ultimate, 4-6 for elite)
- [ ] Functional effects (check stats)
- [ ] No license requirements preventing installation

### 5.4 Equipment

**Run test:** `elitetest:equipment`

**Manual verification:**
1. Spawn several ultimates: `spawnultimate` (5 times)
2. Examine each one
3. Count equipped items
4. Check item tiers

**Verify:**
- [ ] Ultimate has 3-8 items
- [ ] Elite has 2-5 items
- [ ] Tier 7-8 weapons (vibro weapons, phase weapons)
- [ ] Tier 7-8 armor (plastifer, zetachrome)
- [ ] Items are equipped, not just in inventory
- [ ] Weapons are appropriate to creature anatomy

### 5.5 Enhancement Multiplier

**Test 1.0x (default):**
1. `EliteVariantsReset`
2. `spawnultimate`
3. Count enhancements (should be 4-8 total)

**Test 2.0x:**
1. `EliteVariantsEnhancements2.0`
2. `spawnultimate`
3. Count enhancements (should be ~8-16 total)

**Test 3.0x (max):**
1. `EliteVariantsEnhancements3.0`
2. `spawnultimate`
3. Count enhancements (should be ~12-24 total)
4. Verify ultimate has 9 mental mutations (3x cap of 3)

**Reset:** `EliteVariantsReset`

---

## 6. Army System
**Priority: MEDIUM** | **Time: 10 minutes**

### 6.1 Army Spawning

**Command:** `spawnarmy`

**Test each army type:**

#### Small Squad (2-3 elites)
1. Select "Small Squad" from menu
2. Verify 2-3 elite variants spawn
3. Verify all are elite tier (cyan)
4. Verify they spawn near each other (within 5-10 cells)
5. Check they're all from the same base creature type

#### Elite Patrol (4-5 elites)
1. Select "Elite Patrol" from menu
2. Verify 4-5 elite variants spawn
3. All should be elite tier
4. Should spawn in formation

#### Elite Warband (1 ultimate + 4-5 goons)
1. Select "Elite Warband" from menu
2. Verify 1 ultimate (golden) spawns
3. Verify 4-5 elite goons (cyan) spawn
4. Goons should be lower level than leader
5. All should be same creature type

#### Ultimate Host (2 ultimates + 6-8 goons)
1. Select "Ultimate Host" from menu
2. Verify 2 ultimates (golden) spawn
3. Verify 6-8 elite goons (cyan) spawn
4. Goons should be lower level
5. This is a large, dangerous group

#### Titan Pair (2-3 ultimates, no goons)
1. Select "Titan Pair" from menu
2. Verify 2-3 ultimates spawn
3. Verify NO goons spawn
4. All should be ultimate tier (golden)
5. All should be high threat level

### 6.2 Army Cohesion

**Test army behavior:**
1. Spawn any army with `spawnarmy`
2. Wait several turns (press 5 to wait)
3. Observe creature movement

**Verify:**
- [ ] Goons stay near leaders (within ~10 cells)
- [ ] Army moves together as a unit
- [ ] If one member enters combat, others join
- [ ] Leaders don't abandon goons
- [ ] Goons follow leader's movement

### 6.3 Army Faction Assignment

**Test faction:**
1. Spawn army: `spawnarmy` (select Elite Warband)
2. Examine leader (ultimate)
3. Examine several goons
4. Check faction membership

**Verify:**
- [ ] All army members share same faction
- [ ] Faction should be creature's original faction OR EliteCollective
- [ ] All army members hostile/friendly to same targets
- [ ] No infighting within army

### 6.4 Army Status Tracking

**Command:** `elitearmy:status`

**Verify output shows:**
- [ ] Total armies spawned (lifetime counter)
- [ ] Active armies (currently alive)
- [ ] Army breakdown by type
- [ ] Leader/goon ratios

**Test tracking:**
1. Run `elitearmy:reset` (reset counters)
2. Run `elitearmy:status` (should show 0)
3. Spawn 3 armies: `spawnarmy` (3 times, different types)
4. Run `elitearmy:status` (should show 3 total)
5. Kill one army
6. Run `elitearmy:status` (should show 2 active)

### 6.5 Army Composition Edge Cases

**Test leader-only armies (traders/wardens):**
1. Keep spawning armies until you get a trader or warden base creature
2. Verify it spawns solo OR with appropriate goons
3. Traders should not have combat goons
4. Wardens should have faction-appropriate escorts

**Test mixed tiers:**
1. Spawn Elite Warband (1 ultimate + goons)
2. Verify leader is higher level than goons
3. Verify leader has more enhancements
4. Verify visual distinction (leader golden, goons cyan)

---

## 7. Settings & Configuration
**Priority: MEDIUM** | **Time: 10 minutes**

### 7.1 Power Multiplier

**Test available values:** 1.0x, 1.25x, 1.5x, 2.0x, 2.5x, 3.0x

**For each value:**
1. Set power: `EliteVariantsPower[value]` (e.g., `EliteVariantsPower1.5`)
2. Check status: `elitedebug:status`
3. Spawn creature: `spawnelite`
4. Note HP value

**Expected scaling:**
```
1.0x power: Elite HP ~500
1.5x power: Elite HP ~750 (1.5x)
2.0x power: Elite HP ~1000 (2x)
3.0x power: Elite HP ~1500 (3x)
```

**Verify:**
- [ ] HP scales proportionally
- [ ] AV/DV scale proportionally
- [ ] Stats scale proportionally
- [ ] Setting persists between spawns

**Custom values:**
```
EliteVariantsPower1.0
EliteVariantsPower1.25
EliteVariantsPower1.5
EliteVariantsPower2.0
EliteVariantsPower2.5
EliteVariantsPower3.0
```

### 7.2 Level Offset

**Test available values:** 0, +5, +10, +15, +20, +30

**For each value:**
1. Note your current level
2. Set offset: `EliteVariantsLevel[value]` (e.g., `EliteVariantsLevel+10`)
3. Check status: `elitedebug:status`
4. Spawn creature: `spawnelite`
5. Examine creature's level

**Expected:**
```
Your level: 20
+0 offset: Elite level 20
+10 offset: Elite level 30
+20 offset: Elite level 40
```

**Verify:**
- [ ] Level scales correctly
- [ ] Higher level = more HP, better stats
- [ ] Mutations scale with level
- [ ] Setting persists

**Custom values:**
```
EliteVariantsLevel0
EliteVariantsLevel+5
EliteVariantsLevel+10
EliteVariantsLevel+15
EliteVariantsLevel+20
EliteVariantsLevel+30
```

### 7.3 Ultimate Chance

**Test available values:** 0%, 30% (default), 50%, 75%, 100%

**Test 0% (never ultimate):**
1. `EliteVariantsUltimate0`
2. Spawn 10 times: `EliteVariantSpawn`
3. Verify all are elite tier (cyan)

**Test 100% (always ultimate):**
1. `EliteVariantsUltimate100`
2. Spawn 10 times: `EliteVariantSpawn`
3. Verify all are ultimate tier (golden)

**Test 50% (roughly even):**
1. `EliteVariantsUltimate50`
2. Spawn 20 times: `EliteVariantSpawn`
3. Count elites vs ultimates
4. Should be roughly 10/10 split (allow variance)

**Verify:**
- [ ] 0% = no ultimates
- [ ] 100% = all ultimates
- [ ] Intermediate values show probability
- [ ] Setting persists

**Custom values:**
```
EliteVariantsUltimate0
EliteVariantsUltimate30
EliteVariantsUltimate50
EliteVariantsUltimate75
EliteVariantsUltimate100
```

### 7.4 Enhancement Multiplier

**Test available values:** 1.0x, 1.25x, 1.5x, 2.0x, 2.5x, 3.0x

**For each value:**
1. Set multiplier: `EliteVariantsEnhancements[value]`
2. Spawn ultimate: `spawnultimate`
3. Count total enhancements (mutations + cybernetics + equipment)

**Expected:**
```
1.0x: Ultimate has ~6-10 enhancements
2.0x: Ultimate has ~12-20 enhancements
3.0x: Ultimate has ~18-30 enhancements
```

**Special case - Mental mutation cap:**
- 1.0x: Ultimate has max 6 mental mutations
- 2.0x: Ultimate has max 12 mental mutations
- 3.0x: Ultimate has max 18 mental mutations

**Verify:**
- [ ] Enhancement count scales
- [ ] Mental mutation cap increases
- [ ] More equipment at higher multipliers
- [ ] More cybernetics at higher multipliers

### 7.5 Settings Persistence

**Test persistence:**
1. Set multiple settings:
   ```
   EliteVariantsPower2.0
   EliteVariantsLevel+10
   EliteVariantsUltimate75
   EliteVariantsEnhancements1.5
   ```
2. Check status: `elitedebug:status`
3. Spawn creature: `spawnelite`
4. Save and quit game
5. Load game
6. Check status: `elitedebug:status`

**Verify:**
- [ ] All settings persist after save/load
- [ ] Settings don't reset between sessions
- [ ] Multiple settings can coexist
- [ ] Reset command clears all settings

### 7.6 Settings Reset

**Test reset:**
1. Set extreme settings:
   ```
   EliteVariantsPower3.0
   EliteVariantsLevel+30
   EliteVariantsUltimate100
   EliteVariantsEnhancements3.0
   ```
2. Check status: `elitedebug:status` (should show extreme values)
3. Reset: `EliteVariantsReset`
4. Check status again

**Verify reset values:**
- [ ] Power: 1.0x
- [ ] Level offset: +0
- [ ] Ultimate chance: 30%
- [ ] Enhancements: 1.0x

### 7.7 Recommended Configurations

**Test preset configurations from SETTINGS.md:**

#### Standard (Default)
```
EliteVariantsReset
spawnelite
```
Verify: Balanced, fair challenge

#### Hard Mode
```
EliteVariantsPower1.5
EliteVariantsLevel+10
EliteVariantsUltimate60
EliteVariantsEnhancements1.5
spawnultimate
```
Verify: Noticeably harder than default

#### Brutal Mode
```
EliteVariantsPower2.0
EliteVariantsLevel+20
EliteVariantsUltimate100
EliteVariantsEnhancements2.0
spawnultimate
```
Verify: Extremely challenging, golden ultimates only

#### Nightmare Mode
```
EliteVariantsPower3.0
EliteVariantsLevel+30
EliteVariantsUltimate100
EliteVariantsEnhancements3.0
spawnultimate
```
Verify: Nearly impossible, massive stats, 9+ mental mutations

---

## 8. Natural Spawning
**Priority: MEDIUM** | **Time: 15 minutes**

### 8.1 Natural Spawn Toggle

**Test enable/disable:**
1. Check status: `elitespawn:status`
2. Note whether natural spawning is enabled
3. Toggle: `elitespawn:toggle`
4. Check status again
5. Toggle again
6. Check status again

**Verify:**
- [ ] Status shows ON/OFF state
- [ ] Toggle changes state
- [ ] State persists between checks
- [ ] Can toggle multiple times

### 8.2 Natural Spawn Rates

**Enable spawning:** `elitespawn:toggle` (if disabled)

**Test in-game spawning:**
1. Travel to tier 7-8 zone (high spawn rate)
2. Explore for 10-15 minutes
3. Count elite/ultimate encounters

**Expected spawn rate (tier 7-8):**
- Approximately 1 elite/ultimate per 5-10 encounters
- Higher chance in tier 8 vs tier 7
- Mix of elites and ultimates (~60% ultimate in tier 8)

**Test in different tiers:**
- Tier 1-4: Rare, mostly elite tier
- Tier 5-6: Uncommon, balanced elite/ultimate
- Tier 7-8: Common, mostly ultimate tier

### 8.3 Natural Spawn Debug

**Enable debug mode:** `elitedebug:toggle`

**Test spawn detection:**
1. Enable debug: `elitedebug:toggle`
2. Enable spawning: `elitespawn:toggle`
3. Travel and trigger encounters
4. Watch message log for spawn notifications

**Verify debug output:**
- [ ] "Elite spawn triggered" messages
- [ ] Creature type and tier shown
- [ ] Zone tier shown
- [ ] Ultimate chance calculation shown

### 8.4 Zone Tier Scaling

**Test spawn scaling by zone:**

**Tier 4 zone (Rust Wells):**
1. Travel to Rust Wells
2. Trigger encounters
3. Note elite/ultimate ratio
4. Expected: Mostly elite tier

**Tier 6 zone (Bethesda Susa):**
1. Travel to Bethesda Susa
2. Trigger encounters
3. Expected: Balanced elite/ultimate

**Tier 8 zone (Tomb of the Eaters):**
1. Travel to Tomb of the Eaters
2. Trigger encounters
3. Expected: Mostly ultimate tier

### 8.5 Spawn Statistics

**Track spawns:**
1. Reset stats: `elitespawn:reset`
2. Enable spawning: `elitespawn:toggle`
3. Play for 30 minutes
4. Check stats: `elitedebug:stats`

**Verify stats show:**
- [ ] Total spawns (lifetime)
- [ ] Elite vs ultimate count
- [ ] Spawn rate by zone tier
- [ ] Most common creature types

### 8.6 Natural Spawn Safety

**Test bypass conditions:**
1. Enable debug: `elitedebug:toggle`
2. Run bypass help: `elitedebug:bypass`
3. Read conditions that prevent spawning

**Verify spawns are bypassed for:**
- [ ] Legendary encounters
- [ ] Quest-critical encounters
- [ ] Village/town spawns
- [ ] Historical sites
- [ ] Unique creatures

**Test bypass:**
1. Travel to Joppa (starting village)
2. Trigger encounters
3. Verify NO elite spawns in village proper
4. Verify elites CAN spawn in wilderness nearby

---

## 9. Edge Cases
**Priority: LOW** | **Time: 10 minutes**

### 9.1 Excluded Creatures

**Test that these NEVER spawn as elites:**

**Liquid creatures:**
- Check: Weeps, Liquid creatures
- Verify: Never become elites

**Immobile creatures:**
- Check: Turrets, Plants (most)
- Verify: Never become elites

**Boss creatures:**
- Check: Legendary creatures
- Verify: Never become elites (they're already unique)

**Mechanical creatures:**
- Check: Robots, Drones
- Verify: Do NOT become elites (excluded)

**Test procedure:**
1. Enable debug: `elitedebug:toggle`
2. Enable spawning: `elitespawn:toggle`
3. Play for extended period
4. Check that excluded types never spawn as elites

### 9.2 Extreme Settings

**Test maximum settings:**
```
EliteVariantsPower3.0
EliteVariantsLevel+30
EliteVariantsUltimate100
EliteVariantsEnhancements3.0
```

**Spawn ultimate:** `spawnultimate`

**Verify creature is valid:**
- [ ] No stat overflow errors
- [ ] HP is reasonable (not negative or billions)
- [ ] AV/DV are reasonable (<100)
- [ ] Has 9+ mental mutations (3x cap)
- [ ] Creature is functional (can move, attack)
- [ ] No crash or errors

**Verify gameplay:**
- [ ] Creature is extremely dangerous
- [ ] Combat is possible (not instant death)
- [ ] Creature AI works normally
- [ ] No performance issues

### 9.3 Minimum Settings

**Test minimum settings:**
```
EliteVariantsPower1.0
EliteVariantsLevel0
EliteVariantsUltimate0
EliteVariantsEnhancements1.0
```

**Spawn elite:** `spawnelite`

**Verify:**
- [ ] Creature spawns successfully
- [ ] Stats are enhanced but reasonable
- [ ] Always elite tier (never ultimate)
- [ ] Has 1-3 mental mutations
- [ ] Creature is still stronger than base version

### 9.4 Rapid Spawning

**Test spawn spam:**
1. Run `spawnultimate` 20 times rapidly
2. Verify all spawns succeed
3. Check for lag or performance issues
4. Verify creatures are unique (different types)

**Verify:**
- [ ] All spawns succeed
- [ ] No duplicate blueprints
- [ ] No memory leaks
- [ ] Game remains responsive

### 9.5 Mixed Faction Armies

**Test faction diversity:**
1. Spawn multiple armies: `spawnarmy` (10 times)
2. Note the base creature factions
3. Verify variety (not all same faction)

**Expected:**
- [ ] Mix of factions (snapjaw, fish, robots, etc.)
- [ ] Each army maintains internal faction consistency
- [ ] Different armies can be hostile to each other
- [ ] Player can manipulate faction relations

### 9.6 Save/Load Stability

**Test persistence:**
1. Configure settings:
   ```
   EliteVariantsPower2.0
   EliteVariantsLevel+10
   ```
2. Spawn several elites/ultimates
3. Save game
4. Quit completely
5. Reload game
6. Check creatures still exist with enhancements
7. Check settings: `elitedebug:status`

**Verify:**
- [ ] Spawned creatures persist
- [ ] Enhancements remain intact
- [ ] Settings persist
- [ ] No corruption or errors

---

## 10. Performance Testing
**Priority: LOW** | **Time: 5 minutes**

### 10.1 Large Army Performance

**Spawn massive army:**
1. Spawn 5x Ultimate Host armies: `spawnarmy` (select Ultimate Host, 5 times)
2. This creates 40-50 elite creatures
3. Move near them to trigger AI
4. Observe performance

**Verify:**
- [ ] Game remains playable (FPS acceptable)
- [ ] No excessive lag
- [ ] AI behaves normally
- [ ] No crashes

### 10.2 Extreme Enhancement Performance

**Spawn extreme creature:**
```
EliteVariantsEnhancements3.0
spawnultimate
```

**Verify:**
- [ ] Creature loads without delay
- [ ] Description displays without lag
- [ ] Combat calculations work normally
- [ ] No performance degradation

### 10.3 Extended Play Session

**Long-term test:**
1. Enable natural spawning: `elitespawn:toggle`
2. Play normally for 1-2 hours
3. Monitor for issues

**Verify:**
- [ ] No memory leaks
- [ ] No increasing lag over time
- [ ] Spawns remain consistent
- [ ] No save file corruption

---

## Troubleshooting

### Common Issues

**Issue: Automated tests fail**
- Solution: Run `EliteVariantsReset`, then `elitetest:full` again
- Check: Mod is enabled in mod manager
- Check: No conflicting mods

**Issue: Creatures don't spawn**
- Solution: Verify wish command spelling (case-sensitive)
- Check: You're in a valid spawn location (open area)
- Check: Natural spawning is enabled (`elitespawn:status`)

**Issue: Visual effects not visible**
- Check: Lighting in current zone (go outside)
- Check: You're examining the right creature
- Spawn fresh creature to verify

**Issue: Settings don't persist**
- Check: You're using correct syntax (e.g., `EliteVariantsPower2.0` not `EliteVariantsPower:2.0`)
- Run `elitedebug:status` to verify current settings
- Try `EliteVariantsReset` and set again

**Issue: Armies don't stay together**
- This is normal if combat separates them
- Goons have limited follow range
- Leaders may chase targets independently

### Reset Everything

If things are broken, reset all systems:
```
EliteVariantsReset
elitespawn:reset
elitearmy:reset
elitedebug:toggle
```
(Toggle debug off if it was on)

Then run `elitetest:full` to verify core functionality.

---

## Test Checklist Summary

Use this checklist to track your testing progress:

### Critical Tests (Must Pass)
- [ ] `elitetest:full` - All automated tests pass
- [ ] `elitetest:spawn` - Basic spawn test works
- [ ] `spawnelite` - Elite spawns with cyan glow
- [ ] `spawnultimate` - Ultimate spawns with golden glow
- [ ] Visual effects visible and correct

### High Priority Tests
- [ ] Elite tier: 1-3 mental mutations, cyan appearance
- [ ] Ultimate tier: 4-6 mental mutations, golden appearance
- [ ] Equipment system: Correct item counts for each tier
- [ ] Enhancement scaling works properly

### Medium Priority Tests
- [ ] All army types spawn correctly
- [ ] Army cohesion (members stay together)
- [ ] Power multiplier scaling (1.0x to 3.0x)
- [ ] Level offset scaling (0 to +30)
- [ ] Ultimate chance settings (0% to 100%)
- [ ] Settings persistence across sessions
- [ ] Natural spawning enable/disable

### Low Priority Tests
- [ ] Excluded creatures never spawn as elites
- [ ] Extreme settings don't crash game
- [ ] Rapid spawning works without issues
- [ ] Save/load stability
- [ ] Performance with large armies

---

## Reporting Issues

If you find bugs during testing:

1. **Note the exact wish command used**
2. **Note your settings** (`elitedebug:status` output)
3. **Describe expected vs actual behavior**
4. **Include any error messages**
5. **Note which test failed** (from automated suite)

Example bug report:
```
Test: elitetest:mutations
Command: elitetest:mutations
Settings: Default (1.0x all)
Expected: Elite has 1-3 mental mutations
Actual: Elite has 0 mental mutations
Error: None visible
```

---

## Quick Reference: All Commands

### Spawn Commands
```
spawnelite                  - Spawn single elite variant
spawnultimate               - Spawn single ultimate variant
EliteVariantSpawn           - Spawn variant (random tier by zone)
UltimateVariantSpawn        - Spawn variant (force ultimate)
spawnarmy                   - Spawn elite army (menu)
```

### Test Commands
```
elitetest:full              - Run all automated tests
elitetest:spawn             - Quick spawn test with stats
elitetest:equipment         - Test equipment system
elitetest:mutations         - Test mutation caps
elitetest:multipliers       - Test stat scaling
elitetest:armies            - Test army composition
elitetest:settings          - Test settings persistence
```

### Debug Commands
```
elitedebug:toggle           - Toggle debug mode
elitedebug:status           - Show current settings
elitedebug:stats            - Show spawn statistics
elitedebug:bypass           - Show bypass conditions
```

### Settings Commands
```
EliteVariantsPower[X]       - Set power (1.0-3.0)
EliteVariantsLevel[+X]      - Set level offset (0-30)
EliteVariantsUltimate[X]    - Set ultimate % (0-100)
EliteVariantsEnhancements[X] - Set enhancement mult (1.0-3.0)
EliteVariantsReset          - Reset all to defaults
```

### Natural Spawn Commands
```
elitespawn:status           - Show spawn settings
elitespawn:toggle           - Enable/disable spawning
elitespawn:reset            - Reset spawn tracking
```

### Army Commands
```
elitearmy:status            - Show army statistics
elitearmy:reset             - Reset army tracking
```

---

## Conclusion

This comprehensive guide covers all testable aspects of the Elite Variants mod. For quick verification that nothing is broken, use **TESTING_QUICK.md** instead.

**Estimated total time for full testing:** 60-90 minutes

**Recommended testing frequency:**
- Quick smoke test: Before each play session
- Full comprehensive test: After mod updates
- Spot tests: When changing settings or investigating issues

Happy hunting!
