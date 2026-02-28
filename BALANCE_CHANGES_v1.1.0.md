# Balance Changes - Version 1.1.0

## Summary
This update addresses player feedback about excessive high-tier loot drops making the game economy trivial. Changes reduce loot quality, spawn frequency, and gate elites/ultimates behind more progression.

---

## 1. LOOT QUALITY REDUCTION

### Elite Tier Chances
| Setting | Before | After | Change |
|---------|--------|-------|--------|
| Tier 8 (Zetachrome) | 40% | 10% | -75% |
| Tier 7 (Flawless Crysteel) | 40% | 25% | -37.5% |
| Tier 6 (Remainder) | 20% | 65% | +225% |

**Impact:** Elites now primarily drop Tier 6 gear (65%), with occasional Tier 7 (25%), and rare Tier 8 (10%). Zetachrome drops reduced by 75%.

### Ultimate Tier Chances
| Setting | Before | After | Change |
|---------|--------|-------|--------|
| Tier 8 (Zetachrome) | 60% | 30% | -50% |
| Tier 7 (Flawless Crysteel) | 30% | 40% | +33% |
| Tier 6 (Remainder) | 10% | 30% | +200% |

**Impact:** Ultimates still favor high-tier loot but less overwhelmingly. Tier 7 becomes most common (40%), with Tier 8 reduced from majority to 30%.

---

## 2. SPAWN FREQUENCY REDUCTION

### Encounter Rates
| Setting | Before | After | Change |
|---------|--------|-------|--------|
| Elite Spawn Chance | 20% (1 in 5) | 10% (1 in 10) | -50% |
| Ultimate Upgrade Chance | 30% | 15% | -50% |

**Combined Elite Rate:** 20% → 10% (-50%)
**Combined Ultimate Rate:** 6% (20% × 30%) → 1.5% (10% × 15%) → (-75%)

**Impact:**
- Elites reduced from every 5th creature to every 10th creature
- Ultimates reduced from ~1 in 17 creatures to ~1 in 67 creatures
- Dramatically fewer high-tier loot sources in the world

---

## 3. PROGRESSION GATING

### Elite Requirements
| Setting | Before | After | Change |
|---------|--------|-------|--------|
| Minimum Player Level | 10 | 15 | +5 levels |
| Minimum Zone Tier | 4 | 5 | +1 tier |

**Impact:** Players must reach level 15 and explore Tier 5+ zones before encountering elites. Pushes first encounter ~5 levels deeper into progression.

### Ultimate Requirements
| Setting | Before | After | Change |
|---------|--------|-------|--------|
| Minimum Player Level | 20 | 25 | +5 levels |
| Minimum Zone Tier | 6 | 7 | +1 tier |

**Impact:** Ultimates delayed to late-game (level 25, Tier 7+ zones). Players must reach near-endgame content before seeing these encounters.

---

## Overall Impact Analysis

### Before (v1.0.0):
- **Loot Saturation:** 20% of creatures = elites with 80% Tier 7-8 loot
- **Early Access:** Tier 8 gear available at level 10
- **Economy Trivial:** Selling excess high-tier drops provided massive wealth
- **Progression Skipped:** Players could gear up quickly, skipping mid-game tiers

### After (v1.1.0):
- **Loot Scarcity:** 10% of creatures = elites with 35% Tier 7-8 loot
- **Late Access:** Tier 8 gear rare until level 25+
- **Economy Balanced:** High-tier items are rare rewards, not farmable
- **Progression Respected:** Players experience full tier progression curve

---

## Expected Player Experience

### Low Level (1-14):
- **Before:** Could encounter elites at level 10, get Tier 8 gear early
- **After:** No elites yet, must use normal progression

### Mid Level (15-24):
- **Before:** Farming elites for Tier 7-8 gear, trivializing zones
- **After:** Occasional elites drop mostly Tier 6, some Tier 7. Rare Tier 8 feels rewarding.

### High Level (25+):
- **Before:** Swimming in zetachrome weapons, selling extras
- **After:** Ultimates appear but drop balanced loot mix. Tier 8 still special.

---

## Migration Notes

**Existing players:** These are default changes. Custom settings are preserved. Use `elitequick:reset` or adjust manually in mod options.

**New players:** Will experience balanced defaults. Can still increase difficulty via Custom category or presets.

**Custom config users:** Not affected unless using "Normal" preset. Manually configured values unchanged.

---

## Future Considerations

Possible additions based on further feedback:

1. **Loot Scarcity Mode:** Optional 25-50% chance elites drop NO equipment
2. **Progressive Scaling:** Loot quality scales with zone depth (deeper = better)
3. **Boss-Only Tier 8:** Reserve highest tier for uniquely powerful spawns
4. **Equipment Degradation:** Dropped items start with reduced durability

---

## Version History

**v1.1.0** - Balance update: Reduced loot spam, gated progression
**v1.0.0** - Initial release

---

## GitHub
Changes pushed to: https://github.com/SattaRIP/elite-ultimate-variants
Commit: b9cab0c - "Balance Update v1.1.0: Reduce loot spam and gate progression"
