using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlotController : MonoBehaviour
{
    public Spell spell;
    public Text spellName;

    public void ShowToolTip()
    {
        Tooltip.ShowToolTip_Static(spell.spellName + ", level " + spell.spellLevel + "\n" + spell.spellDescription + "\ncost: " + spell.spellCost + " mana");
    }
    public void ClearToolTip()
    {
        Tooltip.HideToolTip_Static();
    }
    public void CastSpell()
    {
        if(GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].drain + spell.spellCost <= GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].mana)
        {
            GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].drain += spell.spellCost;

            if (spell.spellTarget == Spell.TargetType.self) ApplyMagicalEffect(GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].gameObject);
        }
    }

    public void ApplyMagicalEffect(GameObject target)
    {
        if (spell.unlock > 0 && GameManager.PARTY.interactContext == "LOCKED DOOR") //Unlock
        {
            GameManager.PARTY.Interact_Object.GetComponent<Hello_I_am_a_door>().lockValue -= spell.unlock;
            if (GameManager.PARTY.Interact_Object.GetComponent<Hello_I_am_a_door>().lockValue < 0) GameManager.PARTY.Interact_Object.GetComponent<Hello_I_am_a_door>().lockValue = 0;
        }
        if (spell.disarmTrap > 0 && GameManager.PARTY.interactContext == "TRAP") //Disarm Trap
        {
            GameManager.PARTY.Interact_Object.GetComponent<GridNode>().trapLevel -= spell.disarmTrap;
            if (GameManager.PARTY.Interact_Object.GetComponent<GridNode>().trapLevel < 0) GameManager.PARTY.Interact_Object.GetComponent<GridNode>().trapLevel = 0;
        }
        if (spell.createLight > 0) GameManager.PARTY.magical_light += spell.createLight; //Create Light
        if (spell.heal > 0) //Heal
        {
            if (target.GetComponent<Character>() != null)
            {
                target.GetComponent<Character>().wounds -= spell.heal;
                if (target.GetComponent<Character>().wounds < 0) target.GetComponent<Character>().wounds = 0;
            }
        }
        if (spell.harm > 0) //Harm
        {
            if (target.GetComponent<MonsterLogic>() != null)
            {
                target.GetComponent<MonsterLogic>().wounds += spell.heal;
                if (target.GetComponent<MonsterLogic>().wounds < 0) target.GetComponent<MonsterLogic>().wounds = 0;
            }
        }
    }
}
