using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Spell : ScriptableObject
{    
    public enum TargetType { self, friend, party, enemy, foes }
    public string spellName;
    public int spellLevel;
    public float spellCost;
    public bool UseInExplore;
    public bool UseInBattle;
    public Character.characterClass spellType;
    public TargetType spellTarget;
    public bool asleep;
    public bool cureAsleep;
    public int bless;
    public int curse;
    public int heal;
    public int harm;
    public int paralyze;
    public bool cureParalyze;
    public int poison;
    public bool curePoison;
    public int regen;
    public int strMod, dexMod, intMod, wisMod, chaMos, healthMod, manaMod;
    public int enemy_init, enemy_toHit, enemy_minDamage, enemy_maxDamage, enemy_defense;
    public int createLight;
    public int unlock;
    public int disarmTrap;
    public string spellDescription;
}
