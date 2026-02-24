# EliteVariants Mod - Wish Commands Reference

## Primary Wish Commands

### Elite Variant Spawner
```
EliteVariantSpawn
```
**Description**: Spawns an elite variant encounter with a Level 40 leader and 4-6 goons. The leader and goons are enhanced with a mix of physical mutations, mental mutations, cybernetics, and powerful equipment.

**Features**:
- Leader: Level 40, 500 HP, 12 AV, 6 DV, 2-4 random enhancements
- Goons: Level 30, 166 HP, 1 random enhancement each
- Golden/yellow visual theme with 2-radius glow
- Creatures selected randomly from entire game + all loaded mods
- Faction: EliteCollective (hostile to most)

**Enhancement Types** (weighted random):
- Physical Mutations (30% weight) - 33 mutations with exclusion rules
- Mental Mutations (30% weight) - 27 psychic powers
- Cybernetics (20% weight) - Tier 4-6 implants
- Equipment (20% weight) - Tier 7-8 weapons and armor

---

## Related Blueprint Names

### Base Spawner
```
BaseEliteVariantSpawn
```
Base class for all elite variant spawners (usually not wished directly)

---

## Faction Name

```
EliteCollective
```
The faction that all elite variants belong to. Can be used with faction-related console commands.

---

## Notes

- Wish commands in Caves of Qud are case-sensitive
- The final letter does NOT need to be capitalized
- You don't need to type "wish" when using the in-game wish interface
- Spawners are invisible trigger objects that create the actual encounter and then destroy themselves

---

## Mod Version Info

- **Mod ID**: EliteVariants
- **Mod Title**: Elite Variants
- **Description**: Adds elite/champion variants with mixed enhancements (mutations, cybernetics, equipment)
- **Location**: `/home/mythraps/.config/unity3d/Freehold Games/CavesOfQud/Mods/EliteVariants/`

---

## Technical Details

**Population Table Integration**:
- Tier 7 Caves: Weight 10
- Tier 8 Caves: Weight 12
- Tier 7 Ruins: Weight 8
- Tier 8 Ruins: Weight 10
- Tier X Cave Encounters: Weight 6

**Enhancement Configuration**:
- Leaders get 2-4 enhancements (random, weighted)
- Goons get exactly 1 enhancement
- Physical mutations: Level 8-10 (leaders), Level 4-6 (goons)
- Mental mutations: Level 8-10 (leaders), Level 4-6 (goons)
- Cybernetics: Tier 6 table (leaders), Tier 4 table (goons)
- Equipment: Tier 7-8 weapons/armor

**Creature Pool Exclusions**:
- BaseObject templates
- ExcludeFromDynamicEncounters tag
- Boss creatures
- Robot/mechanical creatures
- Immobile creatures
- Wall/furniture/plant types
- Warden creatures
- Named merchants

---

*Last Updated: 2026-02-13*
*Generated during EliteVariants mod development*
