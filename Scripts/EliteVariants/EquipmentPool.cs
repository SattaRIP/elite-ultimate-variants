using System;
using System.Collections.Generic;
using XRL.World;

namespace XRL.World.Parts
{
    /// <summary>
    /// Pool of high-tier weapons and armor for elite variants
    /// </summary>
    public static class EquipmentPool
    {
        // Tier 7-8 Melee Weapons
        private static readonly string[] Tier78MeleeWeapons = new[]
        {
            "Cudgel8",                   // Zetachrome hammer
            "Cudgel8th",                 // Zetachrome warhammer (2H)
            "Long Sword8",               // Zetachrome long sword
            "Long Sword8th",             // Zetachrome great sword (2H)
            "Battle Axe8",               // Zetachrome battle axe
            "Battle Axe8th",             // Zetachrome halberd (2H)
            "Dagger8",                   // Zetachrome dagger
            "Cudgel7",                   // Flawless Crysteel mace
            "Long Sword7",               // Flawless Crysteel long sword
            "Battle Axe7",               // Flawless Crysteel battle axe
        };

        // Tier 7-8 Armor (Body)
        private static readonly string[] Tier78Armor = new[]
        {
            "Flawless Crysteel Shardmail",
            "Zetachrome Lune",
        };

        // Tier 7-8 Helmets/Headgear
        private static readonly string[] Tier78Headgear = new[]
        {
            "Crysteel Coronet",
            "Flawless Crysteel Coronet",
            "Zetachrome Apex",
            "Fullerite Armet",
        };

        // Tier 7-8 Footwear
        private static readonly string[] Tier78Footwear = new[]
        {
            "Crysteel Boots",
            "Flawless Crysteel Boots",
            "Zetachrome Pumps",
            "Fullerite Boots",
        };

        // Tier 6 Melee Weapons
        private static readonly string[] Tier6MeleeWeapons = new[]
        {
            "Cudgel6",                   // Folded carbide hammer
            "Cudgel6th",                 // Folded carbide warhammer (2H)
            "Long Sword6",               // Folded carbide long sword
            "Long Sword6th",             // Folded carbide great sword (2H)
            "Battle Axe6",               // Folded carbide battle axe
            "Battle Axe6th",             // Folded carbide halberd (2H)
            "Dagger6",                   // Folded carbide dagger
        };

        // Tier 6 Armor (Body)
        private static readonly string[] Tier6Armor = new[]
        {
            "Folded Carbide Masterwork",
            "Carbide Plate Mail",
        };

        // Tier 6 Helmets/Headgear
        private static readonly string[] Tier6Headgear = new[]
        {
            "Carbide Folded Helm",
            "Folded Carbide Visored Helm",
        };

        // Tier 6 Footwear
        private static readonly string[] Tier6Footwear = new[]
        {
            "Carbide Boots",
            "Folded Carbide Boots",
        };

        /// <summary>
        /// Equipment slot types
        /// </summary>
        private enum EquipmentSlot
        {
            Weapon,
            Armor,
            Helmet,
            Boots
        }

        /// <summary>
        /// Check if creature can carry an item without exceeding weight limit
        /// </summary>
        private static bool CanCarry(GameObject creature, GameObject item)
        {
            if (creature == null || item == null)
                return false;

            try
            {
                // Get creature's current weight and carrying capacity
                int currentWeight = creature.GetIntProperty("CarriedWeight");
                int maxWeight = creature.Statistics["CarryBonus"].Value;
                int itemWeight = (int)item.Weight;

                // Check if adding this item would exceed carrying capacity
                return (currentWeight + itemWeight) <= maxWeight;
            }
            catch (Exception)
            {
                // If we can't determine weight, allow the item
                return true;
            }
        }

        /// <summary>
        /// Grant random equipment to elite creature using new weighted system
        /// </summary>
        public static void GrantEquipment(GameObject creature, Random rng, EliteTier tier, bool isLeader)
        {
            if (creature == null || !isLeader)
                return; // Only leaders get equipment

            // Get settings for this tier
            int minItems = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateMinItemCount
                : EliteVariantSettings.EliteMinItemCount;
            int maxItems = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateMaxItemCount
                : EliteVariantSettings.EliteMaxItemCount;

            // Roll item count (capped at 4 max - one per slot)
            int itemCount = Math.Min(rng.Next(minItems, maxItems + 1), 4);

            if (itemCount <= 0)
                return;

            // Select random slots (weighted toward weapon and armor)
            var selectedSlots = SelectRandomSlots(rng, itemCount);

            // Grant each item
            foreach (var slot in selectedSlots)
            {
                int itemTier = GetItemTier(rng, tier);
                GrantItemBySlot(creature, slot, itemTier, rng);
            }
        }

        /// <summary>
        /// Select random equipment slots with weighted distribution
        /// </summary>
        private static List<EquipmentSlot> SelectRandomSlots(Random rng, int count)
        {
            var selected = new List<EquipmentSlot>();
            var available = new List<EquipmentSlot>
            {
                EquipmentSlot.Weapon,
                EquipmentSlot.Armor,
                EquipmentSlot.Helmet,
                EquipmentSlot.Boots
            };

            // Slot weights: Weapon and Armor prioritized
            var weights = new Dictionary<EquipmentSlot, int>
            {
                { EquipmentSlot.Weapon, 40 },
                { EquipmentSlot.Armor, 30 },
                { EquipmentSlot.Helmet, 15 },
                { EquipmentSlot.Boots, 15 }
            };

            for (int i = 0; i < count && available.Count > 0; i++)
            {
                // Weighted random selection
                int totalWeight = 0;
                foreach (var slot in available)
                    totalWeight += weights[slot];

                int roll = rng.Next(totalWeight);
                int cumulative = 0;

                EquipmentSlot selectedSlot = EquipmentSlot.Weapon;
                foreach (var slot in available)
                {
                    cumulative += weights[slot];
                    if (roll < cumulative)
                    {
                        selectedSlot = slot;
                        break;
                    }
                }

                selected.Add(selectedSlot);
                available.Remove(selectedSlot);
            }

            return selected;
        }

        /// <summary>
        /// Get item tier based on weighted distribution
        /// </summary>
        private static int GetItemTier(Random rng, EliteTier tier)
        {
            // Get tier chances from settings
            int tier8Chance = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateTier8Chance
                : EliteVariantSettings.EliteTier8Chance;
            int tier7Chance = (tier == EliteTier.Ultimate)
                ? EliteVariantSettings.UltimateTier7Chance
                : EliteVariantSettings.EliteTier7Chance;

            // Tier 6 gets remainder
            int tier6Chance = 100 - tier8Chance - tier7Chance;
            if (tier6Chance < 0) tier6Chance = 0;

            // Roll
            int roll = rng.Next(100);

            if (roll < tier8Chance)
                return 8;
            else if (roll < tier8Chance + tier7Chance)
                return 7;
            else
                return 6;
        }

        /// <summary>
        /// Grant item by slot and tier
        /// </summary>
        private static void GrantItemBySlot(GameObject creature, EquipmentSlot slot, int tier, Random rng)
        {
            switch (slot)
            {
                case EquipmentSlot.Weapon:
                    GrantWeapon(creature, tier, rng);
                    break;
                case EquipmentSlot.Armor:
                    GrantArmor(creature, tier, rng);
                    break;
                case EquipmentSlot.Helmet:
                    GrantHelmet(creature, tier, rng);
                    break;
                case EquipmentSlot.Boots:
                    GrantBoots(creature, tier, rng);
                    break;
            }
        }

        /// <summary>
        /// Grant a random weapon of specified tier
        /// </summary>
        private static void GrantWeapon(GameObject creature, int tier, Random rng)
        {
            try
            {
                // Select weapon pool based on tier
                string[] weaponPool = tier >= 7 ? Tier78MeleeWeapons : Tier6MeleeWeapons;
                string weaponBlueprint = weaponPool[rng.Next(weaponPool.Length)];
                GameObject weapon = GameObjectFactory.Factory.CreateObject(weaponBlueprint);

                if (weapon != null)
                {
                    // Check if creature can carry this weapon
                    if (CanCarry(creature, weapon))
                    {
                        // Force equip as weapon
                        weapon.SetIntProperty("AlwaysEquipAsWeapon", 1);
                        creature.ReceiveObject(weapon);
                    }
                    else
                    {
                        // Creature can't carry it - destroy the item
                        weapon.Obliterate();
                    }
                }
            }
            catch (Exception)
            {
                // Weapon blueprint might not exist - skip silently
            }
        }

        /// <summary>
        /// Grant random armor of specified tier
        /// </summary>
        private static void GrantArmor(GameObject creature, int tier, Random rng)
        {
            try
            {
                // Select armor pool based on tier
                string[] armorPool = tier >= 7 ? Tier78Armor : Tier6Armor;
                string armorBlueprint = armorPool[rng.Next(armorPool.Length)];
                GameObject armor = GameObjectFactory.Factory.CreateObject(armorBlueprint);

                if (armor != null)
                {
                    // Check if creature can carry this armor
                    if (CanCarry(creature, armor))
                    {
                        creature.ReceiveObject(armor);
                    }
                    else
                    {
                        // Creature can't carry it - destroy the item
                        armor.Obliterate();
                    }
                }
            }
            catch (Exception)
            {
                // Armor blueprint might not exist - skip silently
            }
        }

        /// <summary>
        /// Grant random helmet of specified tier
        /// </summary>
        private static void GrantHelmet(GameObject creature, int tier, Random rng)
        {
            try
            {
                // Select helmet pool based on tier
                string[] helmetPool = tier >= 7 ? Tier78Headgear : Tier6Headgear;
                string helmetBlueprint = helmetPool[rng.Next(helmetPool.Length)];
                GameObject helmet = GameObjectFactory.Factory.CreateObject(helmetBlueprint);

                if (helmet != null)
                {
                    // Check if creature can carry this helmet
                    if (CanCarry(creature, helmet))
                    {
                        creature.ReceiveObject(helmet);
                    }
                    else
                    {
                        // Creature can't carry it - destroy the item
                        helmet.Obliterate();
                    }
                }
            }
            catch (Exception)
            {
                // Helmet blueprint might not exist - skip silently
            }
        }

        /// <summary>
        /// Grant random boots of specified tier
        /// </summary>
        private static void GrantBoots(GameObject creature, int tier, Random rng)
        {
            try
            {
                // Select boots pool based on tier
                string[] bootsPool = tier >= 7 ? Tier78Footwear : Tier6Footwear;
                string bootsBlueprint = bootsPool[rng.Next(bootsPool.Length)];
                GameObject boots = GameObjectFactory.Factory.CreateObject(bootsBlueprint);

                if (boots != null)
                {
                    // Check if creature can carry these boots
                    if (CanCarry(creature, boots))
                    {
                        creature.ReceiveObject(boots);
                    }
                    else
                    {
                        // Creature can't carry them - destroy the item
                        boots.Obliterate();
                    }
                }
            }
            catch (Exception)
            {
                // Boots blueprint might not exist - skip silently
            }
        }
    }
}
