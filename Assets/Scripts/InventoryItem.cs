using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class InventoryItem: ScriptableObject
{
    public enum slotType { none, hand, head, torso, neck, ring, leg, foot, bag }
    public enum equipType { misc, weapon, armor, light, potion, money, deleted }
    public string genericName, fullName, description, lore;
    public slotType slot;
    public equipType type;
    public bool identified, magical, fragile, twoHanded, active;
    public int minDamage, maxDamage, fullCharges, maxDuration, quality;
    public int currentCharges, currentDuration;
    public float defense, critMultiplier, value;
    public int itemIconIndex;

    public InventoryItem(string gn, string fn, string desc, string lor, slotType slt, equipType eqt, bool id, bool mag, bool frg, bool th, bool ac, int minD, int maxD, int fc, int maxd, int qlty, int cC, float d, float cM, float v, int index)
    {
        genericName = gn;
        fullName = fn;
        description = desc;
        lore = lor;
        slot = slt;
        type = eqt;
        identified = id;
        magical = mag;
        fragile = frg;
        twoHanded = th;
        active = ac;
        minDamage = minD;
        maxDamage = maxD;
        fullCharges = fc;
        maxDuration = maxd;
        quality = qlty;
        currentCharges = cC;
        defense = d;
        currentDuration = maxd;
        critMultiplier = cM;
        value = v;
        itemIconIndex = index;
    }
    public InventoryItem(SaveSlot.serialItem i)
    {
        {
            genericName = i.genericName;
            fullName = i.fullName;
            description = i.description;
            lore = i.lore;
            slot = i.slot;
            type = i.type;
            identified = i.identified;
            magical = i.magical;
            fragile = i.fragile;
            twoHanded = i.twoHanded;
            active = i.active;
            minDamage = i.minDamage;
            maxDamage = i.maxDamage;
            fullCharges = i.fullCharges;
            maxDuration = i.maxDuration;
            quality = i.quality;
            currentCharges = i.currentCharges;
            currentDuration = i.currentDuration;
            defense = i.defense;
            critMultiplier = i.critMultiplier;
            value = i.value;
            itemIconIndex = i.itemIconIndex;
        }
    }

    public void LoadItem(SaveSlot.serialItem i)
    {        
        genericName = i.genericName;
        fullName = i.fullName;
        description = i.description;
        lore = i.lore;
        slot = i.slot;
        type = i.type;
        identified = i.identified;
        magical = i.magical;
        fragile = i.fragile;
        twoHanded = i.twoHanded;
        active = i.active;
        minDamage = i.minDamage;
        maxDamage = i.maxDamage;
        fullCharges = i.fullCharges;
        maxDuration = i.maxDuration;
        quality = i.quality;
        currentCharges = i.currentCharges;
        currentDuration = i.currentDuration;
        defense = i.defense;
        critMultiplier = i.critMultiplier;
        value = i.value;
        itemIconIndex = i.itemIconIndex;
    }
}