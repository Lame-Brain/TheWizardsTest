using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public enum characterClass { monster, Fighter, Rogue, Mage, Cleric}
    public string characterName;
    public characterClass type;
    public int xpLevel, xpPoints, xpNNL, freePoints;
    [Header("Base Attributes")]
    public int base_str;
    public int base_dex, base_iq, base_wis, base_cha;
    [Header("Derived Stats")]
    public float base_health;
    public float wounds, base_mana, drain, defense, attack;
    public int BS_Slot;
    [HideInInspector]
    public int strength, dexterity, intelligence, wisdom, charisma;
    [HideInInspector]
    public float health, mana;
    [Header("Portrait")]
    public int portraitIndex;
    [Header("Equipped Inventory")]
    public InventoryItem eq_Head;
    public InventoryItem eq_Neck, eq_LeftFinger, eq_RightFinger, eq_LeftHand, eq_RightHand, eq_Torso, eq_Legs, eq_Feet;
    [Header("Stance")]
    public bool frontLine;
    [Header("Conditons")]
    public bool asleep; //simple toggle. Asleep and cannot act, awaken when taking damage or after battle is over
    public int poisoned; //take INT damage every turn or round in combat. Does not wear off
    public int regen; //heal INT damage ever round in combat. Ends after combat or after INT turns
    public int paralyzed; //cannot move or act in battle. counts down every turn. does not count down in battle
    public bool cursed; //halves attack and doubles incoming damage. does not wear off
    public int blessed; //doubles attack and halves incoming damage. Counts down every turn, does not count down in battle
    public int strMod, dexMod, intMod, wisMod, chaMod, healthMod, manaMod; //the associated stat is modded by INT wears off after battle


    private void Start()
    {
        if (eq_Head != null) eq_Head = new InventoryItem(eq_Head.genericName, eq_Head.fullName, eq_Head.description, eq_Head.lore,
            eq_Head.slot, eq_Head.type, eq_Head.identified, eq_Head.magical, eq_Head.fragile, eq_Head.twoHanded, eq_Head.active, eq_Head.minDamage, eq_Head.maxDamage,
            eq_Head.fullCharges, eq_Head.maxDuration, eq_Head.quality, eq_Head.currentCharges, eq_Head.defense, eq_Head.critMultiplier, eq_Head.value, eq_Head.itemIconIndex);

        if (eq_Neck != null) eq_Neck = new InventoryItem(eq_Neck.genericName, eq_Neck.fullName, eq_Neck.description, eq_Neck.lore,
            eq_Neck.slot, eq_Neck.type, eq_Neck.identified, eq_Neck.magical, eq_Neck.fragile, eq_Neck.twoHanded, eq_Neck.active, eq_Neck.minDamage, eq_Neck.maxDamage,
            eq_Neck.fullCharges, eq_Neck.maxDuration, eq_Neck.quality, eq_Neck.currentCharges, eq_Neck.defense, eq_Neck.critMultiplier, eq_Neck.value, eq_Neck.itemIconIndex);

        if (eq_LeftFinger != null) eq_LeftFinger = new InventoryItem(eq_LeftFinger.genericName, eq_LeftFinger.fullName, eq_LeftFinger.description, eq_LeftFinger.lore,
            eq_LeftFinger.slot, eq_LeftFinger.type, eq_LeftFinger.identified, eq_LeftFinger.magical, eq_LeftFinger.fragile, eq_LeftFinger.twoHanded, eq_LeftFinger.active, eq_LeftFinger.minDamage, eq_LeftFinger.maxDamage,
            eq_LeftFinger.fullCharges, eq_LeftFinger.maxDuration, eq_LeftFinger.quality, eq_LeftFinger.currentCharges, eq_LeftFinger.defense, eq_LeftFinger.critMultiplier, eq_LeftFinger.value, eq_LeftFinger.itemIconIndex); 

        if (eq_RightFinger != null) eq_RightFinger = new InventoryItem(eq_RightFinger.genericName, eq_RightFinger.fullName, eq_RightFinger.description, eq_RightFinger.lore,
            eq_RightFinger.slot, eq_RightFinger.type, eq_RightFinger.identified, eq_RightFinger.magical, eq_RightFinger.fragile, eq_RightFinger.twoHanded, eq_RightFinger.active, eq_RightFinger.minDamage, eq_RightFinger.maxDamage,
            eq_RightFinger.fullCharges, eq_RightFinger.maxDuration, eq_RightFinger.quality, eq_RightFinger.currentCharges, eq_RightFinger.defense, eq_RightFinger.critMultiplier, eq_RightFinger.value, eq_RightFinger.itemIconIndex);

        if (eq_LeftHand != null) eq_LeftHand = new InventoryItem(eq_LeftHand.genericName, eq_LeftHand.fullName, eq_LeftHand.description, eq_LeftHand.lore,
            eq_LeftHand.slot, eq_LeftHand.type, eq_LeftHand.identified, eq_LeftHand.magical, eq_LeftHand.fragile, eq_LeftHand.twoHanded, eq_LeftHand.active, eq_LeftHand.minDamage, eq_LeftHand.maxDamage,
            eq_LeftHand.fullCharges, eq_LeftHand.maxDuration, eq_LeftHand.quality, eq_LeftHand.currentCharges, eq_LeftHand.defense, eq_LeftHand.critMultiplier, eq_LeftHand.value, eq_LeftHand.itemIconIndex);

        if (eq_RightHand != null) eq_RightHand = new InventoryItem(eq_RightHand.genericName, eq_RightHand.fullName, eq_RightHand.description, eq_RightHand.lore,
            eq_RightHand.slot, eq_RightHand.type, eq_RightHand.identified, eq_RightHand.magical, eq_RightHand.fragile, eq_RightHand.twoHanded, eq_RightHand.active, eq_RightHand.minDamage, eq_RightHand.maxDamage,
            eq_RightHand.fullCharges, eq_RightHand.maxDuration, eq_RightHand.quality, eq_RightHand.currentCharges, eq_RightHand.defense, eq_RightHand.critMultiplier, eq_RightHand.value, eq_RightHand.itemIconIndex);

        if (eq_Torso != null) eq_Torso = new InventoryItem(eq_Torso.genericName, eq_Torso.fullName, eq_Torso.description, eq_Torso.lore,
            eq_Torso.slot, eq_Torso.type, eq_Torso.identified, eq_Torso.magical, eq_Torso.fragile, eq_Torso.twoHanded, eq_Torso.active, eq_Torso.minDamage, eq_Torso.maxDamage,
            eq_Torso.fullCharges, eq_Torso.maxDuration, eq_Torso.quality, eq_Torso.currentCharges, eq_Torso.defense, eq_Torso.critMultiplier, eq_Torso.value, eq_Torso.itemIconIndex);

        if (eq_Legs != null) eq_Legs = new InventoryItem(eq_Legs.genericName, eq_Legs.fullName, eq_Legs.description, eq_Legs.lore,
            eq_Legs.slot, eq_Legs.type, eq_Legs.identified, eq_Legs.magical, eq_Legs.fragile, eq_Legs.twoHanded, eq_Legs.active, eq_Legs.minDamage, eq_Legs.maxDamage,
            eq_Legs.fullCharges, eq_Legs.maxDuration, eq_Legs.quality, eq_Legs.currentCharges, eq_Legs.defense, eq_Legs.critMultiplier, eq_Legs.value, eq_Legs.itemIconIndex);

        if (eq_Feet != null) eq_Feet = new InventoryItem(eq_Feet.genericName, eq_Feet.fullName, eq_Feet.description, eq_Feet.lore,
            eq_Feet.slot, eq_Feet.type, eq_Feet.identified, eq_Feet.magical, eq_Feet.fragile, eq_Feet.twoHanded, eq_Feet.active, eq_Feet.minDamage, eq_Feet.maxDamage,
            eq_Feet.fullCharges, eq_Feet.maxDuration, eq_Feet.quality, eq_Feet.currentCharges, eq_Feet.defense, eq_Feet.critMultiplier, eq_Feet.value, eq_Feet.itemIconIndex);

    }

    public void LoadCharacter(SaveSlot.serialCharacter c)
    {
        characterName = c.characterName;
        type = c.type;
        xpLevel = c.xpLevel; xpPoints = c.xpPoints; xpNNL = c.xpNNL; freePoints = c.freePoints;
        base_str = c.strength; base_dex = c.dexterity;
        base_iq = c.intelligence; base_wis = c.wisdom;
        base_cha = c.charisma;
        base_health = c.health; wounds = c.wounds;
        base_mana = c.mana; drain = c.drain;
        defense = c.defense; attack = c.attack;
        portraitIndex = c.portraitIndex;
        eq_Head = null;
        eq_Neck = null;
        eq_LeftFinger = null;
        eq_RightFinger = null;
        eq_LeftHand = null;
        eq_RightHand = null;
        eq_Torso = null;
        eq_Legs = null;
        eq_Feet = null;
        if (c.eq_Head.genericName != "") eq_Head = new InventoryItem(c.eq_Head);
        if (c.eq_Neck.genericName != "") eq_Neck = new InventoryItem(c.eq_Neck);
        if (c.eq_LeftFinger.genericName != "") eq_LeftFinger = new InventoryItem(c.eq_LeftFinger);
        if (c.eq_RightFinger.genericName != "") eq_RightFinger = new InventoryItem(c.eq_RightFinger);
        if (c.eq_LeftHand.genericName != "") eq_LeftHand = new InventoryItem(c.eq_LeftHand);
        if (c.eq_RightHand.genericName != "") eq_RightHand = new InventoryItem(c.eq_RightHand);
        if (c.eq_Torso.genericName != "") eq_Torso = new InventoryItem(c.eq_Torso);
        if (c.eq_Legs.genericName != "") eq_Legs = new InventoryItem(c.eq_Legs);
        if (c.eq_Feet.genericName != "") eq_Feet = new InventoryItem(c.eq_Feet);
        asleep = false;
        poisoned = c.poisoned;
        regen = c.regen;
        paralyzed = c.paralyzed;
        cursed = c.cursed;
        blessed = c.blessed;
        strMod = c.strMod;
        dexMod = c.dexMod;
        intMod = c.intMod;
        wisMod = c.wisMod;
        chaMod = c.chaMod;
    }

    public void UpdateHeroStats()
    {
        strength = base_str + strMod;
        dexterity = base_dex + dexMod;
        intelligence = base_iq + intMod;
        wisdom = base_wis + wisMod;
        charisma = base_cha + chaMod;
        health = base_health + healthMod;
        mana = base_mana + manaMod;
    }

    public void TurnPasses()
    {
        if (blessed > 0) blessed--;
        if (poisoned > 0) wounds += poisoned;
        if (regen > 0)
        {
            wounds -= regen;
            if (wounds < 0) wounds = 0;
            regen--;
        }
        if (paralyzed > 0) paralyzed--;
        if (asleep) asleep = false;
        if (strMod > 0) strMod = 0;
        if (dexMod > 0) dexMod = 0;
        if (intMod > 0) intMod = 0;
        if (wisMod > 0) wisMod = 0;
        if (chaMod > 0) chaMod = 0;
        if (healthMod > 0) healthMod = 0;
        if (manaMod > 0) manaMod = 0;
    }
}   

 
