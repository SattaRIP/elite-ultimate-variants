# Elite Variants - Quick Smoke Test (5-10 minutes)

This is a minimal checklist to verify that nothing is fundamentally broken in the mod. If these tests pass, the mod is working correctly.

## Prerequisites

- Fresh game or test save
- Access to wish commands (press ~ or ` to open console)
- At least one test should be done in an open area with space for spawns

## Critical Tests

### 1. Basic Functionality (2 minutes)

**Test spawning basics:**
- [ ] Run `elitetest:spawn`
- [ ] Verify output shows:
  - 1 elite spawned (white/cyan appearance)
  - 1 ultimate spawned (golden/yellow appearance)
  - Both have enhanced stats displayed
- [ ] Examine both creatures (look at them)
  - Elite should have cyan/white glow
  - Ultimate should have golden/yellow glow
  - Both should show enhanced stats in their description

**Test debug mode:**
- [ ] Run `elitedebug:toggle` - Should see "Debug mode ENABLED"
- [ ] Run `elitedebug:toggle` again - Should see "Debug mode DISABLED"
- [ ] Run `elitedebug:status` - Should show current debug settings

**Expected result:** Both creatures spawn with visible color effects, debug mode toggles on/off successfully

---

### 2. Equipment System (1 minute)

**Test equipment counts:**
- [ ] Run `elitetest:equipment`
- [ ] Wait for test to complete (few seconds)
- [ ] Check message log for test results
- [ ] Verify all subtests show {{G|PASS}} (green PASS)

**Visual verification:**
- [ ] Examine any elite creature from test #1
- [ ] Look at their inventory/equipment
- [ ] Should see multiple items equipped

**Expected result:** Test shows all PASS, elites have visible equipment/items

---

### 3. Army System (1 minute)

**Test army spawning:**
- [ ] Run `spawnarmy`
- [ ] Select any army type from the menu (e.g., "Small Squad", "Elite Patrol")
- [ ] Verify multiple creatures spawn together as a group
- [ ] Check that they share the same faction (examine them)
- [ ] Wait a few turns and observe - goons should follow leaders

**Expected result:** Multiple elites spawn together, move as a coordinated group

---

### 4. Settings System (1 minute)

**Test settings changes:**
- [ ] Run `elitedebug:status` - Note current power multiplier
- [ ] Run `EliteVariantsPower1.5` - Should see "Power multiplier set to 1.5x"
- [ ] Run `elitedebug:status` again - Verify power multiplier changed to 1.5
- [ ] Run `EliteVariantsReset` - Should reset to defaults
- [ ] Run `elitedebug:status` - Verify power multiplier back to 1.0

**Expected result:** Settings change and persist, reset command works

---

### 5. Automated Tests (2-3 minutes)

**Run full test suite:**
- [ ] Run `elitetest:full`
- [ ] Wait for all tests to complete (10-20 seconds)
- [ ] Check message log for final summary
- [ ] Verify message shows "TEST SUMMARY: X/X PASSED" with all tests passing

**If you want to see details:**
- [ ] Scroll up in message log to see individual test results
- [ ] All tests should show {{G|PASS}} in green
- [ ] Any {{R|FAIL}} in red indicates a problem

**Expected result:** All automated tests pass (X/X PASSED where both numbers match)

---

## What Success Looks Like

- [ ] All `elitetest:full` tests show GREEN (PASS)
- [ ] Elites spawn with cyan/white glow and enhanced stats
- [ ] Ultimates spawn with golden/yellow glow and more enhancements
- [ ] Armies spawn as coordinated groups with leaders and goons
- [ ] Settings changes work and reflect in `elitedebug:status`
- [ ] No error messages in the message log

## If Something Fails

1. **Check the message log** for error details (press M or check the message panel)
2. **Run individual tests** to isolate the problem:
   - `elitetest:equipment` - Tests equipment system
   - `elitetest:mutations` - Tests mutation caps
   - `elitetest:multipliers` - Tests stat multipliers
   - `elitetest:armies` - Tests army composition
   - `elitetest:settings` - Tests settings system
3. **Check current state:**
   - `elitedebug:status` - Shows debug settings
   - `elitedebug:stats` - Shows spawn statistics
   - `elitespawn:status` - Shows natural spawning settings
4. **Reset everything:**
   - `EliteVariantsReset` - Reset mod settings
   - `elitespawn:reset` - Reset natural spawning
   - `elitearmy:reset` - Reset army tracking
5. **Try the failing test again** after reset

## Quick Command Reference

### Spawn Commands
- `elitetest:spawn` - Spawn 1 elite + 1 ultimate for visual inspection
- `spawnelite` - Spawn single elite variant
- `spawnultimate` - Spawn single ultimate variant
- `spawnarmy` - Spawn elite army (choose type from menu)

### Debug Commands
- `elitedebug:toggle` - Toggle debug mode on/off
- `elitedebug:status` - Show current debug settings
- `elitedebug:stats` - Show spawn statistics

### Test Commands
- `elitetest:full` - Run all automated tests (recommended)
- `elitetest:spawn` - Quick spawn test with stat display

### Settings Commands
- `elitedebug:status` - View current settings
- `EliteVariantsReset` - Reset all settings to defaults

---

## Time Estimate

- **Minimum:** 5 minutes (just run automated tests)
- **Recommended:** 8-10 minutes (manual verification + automated tests)
- **Thorough:** 15 minutes (includes troubleshooting if needed)

---

**That's it!** If these 5 test categories pass, the Elite Variants mod is working correctly and ready for gameplay.

For comprehensive testing of all features and edge cases, see **TESTING_FULL.md**.
