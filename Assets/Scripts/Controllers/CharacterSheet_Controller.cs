using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheet_Controller : MonoBehaviour
{
    public Text ref_characterName, ref_characterClass, ref_XPtext, ref_strength, ref_dexterity, ref_intelligence, ref_wisdom, ref_charisma, ref_health, ref_wounds, ref_mana, ref_drain, ref_attack, ref_defense, ref_freePoints;
    public GameObject ref_portrait, ref_freePointPanel, ref_levelUpPanel, ref_StatusPanel;
    public Text ref_addStr, ref_addDex, ref_addIQ, ref_addWis, ref_addCha, ref_addHlth, ref_addMana;
    

    private int add_Str, add_Dex, add_IQ, add_Wis, add_Cha, add_Hlth, add_Mana;

    void Start()
    {
        UpdateCharacterSheet();
    }

    public void ModifyStats(string delta)
    {
        if (delta == "S+")
        {
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints > 0)
            {
                add_Str++;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints--;
            }
            UpdateCharacterSheet();
        }
        if (delta == "S-")
        {
            if (add_Str > 0)
            {
                add_Str--;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints++;
            }
            UpdateCharacterSheet();
        }
        if (delta == "D+")
        {
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints > 0)
            {
                add_Dex++;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints--;
            }
            UpdateCharacterSheet();
        }
        if (delta == "D-")
        {
            if (add_Dex > 0)
            {
                add_Dex--;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints++;
            }
            UpdateCharacterSheet();
        }
        if (delta == "I+")
        {
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints > 0)
            {
                add_IQ++;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints--;
            }
            UpdateCharacterSheet();
        }
        if (delta == "I-")
        {
            if (add_IQ > 0)
            {
                add_IQ--;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints++;
            }
            UpdateCharacterSheet();
        }
        if (delta == "W+")
        {
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints > 0)
            {
                add_Wis++;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints--;
            }
            UpdateCharacterSheet();
        }
        if (delta == "W-")
        {
            if (add_Wis > 0)
            {
                add_Wis--;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints++;
            }
            UpdateCharacterSheet();
        }
        if (delta == "C+")
        {
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints > 0)
            {
                add_Cha++;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints--;
            }
            UpdateCharacterSheet();
        }
        if (delta == "C-")
        {
            if (add_Cha > 0)
            {
                add_Cha--;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints++;
            }
            UpdateCharacterSheet();
        }
        if (delta == "H+")
        {
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints > 0)
            {
                add_Hlth++;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints--;
            }
            UpdateCharacterSheet();
        }
        if (delta == "H-")
        {
            if (add_Hlth > 0)
            {
                add_Hlth--;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints++;
            }
            UpdateCharacterSheet();
        }
        if (delta == "M+")
        {
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints > 0)
            {
                add_Mana++;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints--;
            }
            UpdateCharacterSheet();
        }
        if (delta == "M-")
        {
            if (add_Mana > 0)
            {
                add_Mana--;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints++;
            }
            UpdateCharacterSheet();
        }
    }

    public void UpdateCharacterSheet()
    {
        GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].UpdateHeroStats();
        ref_portrait.GetComponent<Image>().sprite = GameManager.GAME.PC_Portrait[GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].portraitIndex];
        ref_characterName.text = GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].characterName;
        ref_characterClass.text = GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].type.ToString();
        ref_XPtext.text = "Level " + GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].xpLevel + " XP: " 
            + GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].xpPoints + " of " + GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].xpNNL;
        ref_strength.text = "Strength: " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].strength + add_Str);
        ref_dexterity.text = "Dexterity: " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].dexterity + add_Dex);
        ref_intelligence.text = "Intelligence: " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].intelligence + add_IQ);
        ref_wisdom.text = "Wisdom: " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].wisdom + add_Wis);
        ref_charisma.text = "Charisma: " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].charisma + add_Cha);
        ref_health.text = "Max Health: " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].health + add_Hlth);
        ref_wounds.text = "Wounds: " + GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].wounds;
        ref_mana.text = "Mana: " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].mana + add_Mana);
        ref_drain.text = "Mana Drained: " + GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].drain;
        ref_attack.text = "Attack Bonus: " + GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].attack;
        ref_defense.text = "Defense: " + GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].defense;

        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].freePoints > 0) ref_freePointPanel.SetActive(true);

        if (ref_freePointPanel.activeSelf)
        {
            ref_addStr.text = add_Str.ToString();
            ref_addDex.text = add_Dex.ToString();
            ref_addIQ.text = add_IQ.ToString();
            ref_addWis.text = add_Wis.ToString();
            ref_addCha.text = add_Cha.ToString();
            ref_addHlth.text = add_Hlth.ToString();
            ref_addMana.text = add_Mana.ToString();
            ref_freePoints.text = "Points to be Assigned: " + GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].freePoints;
        }       

        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].xpPoints < GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].xpNNL)
        {
            ref_levelUpPanel.SetActive(false);
        }

        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].xpPoints >= GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].xpNNL)
        {
            ref_levelUpPanel.SetActive(true);
        }

        ref_StatusPanel.transform.Find("Blessed").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("Cursed").gameObject.SetActive(GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].cursed);
        ref_StatusPanel.transform.Find("Paralyzed").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("Poisoned").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("Regenerating").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("StrMod").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("DexMod").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("IntMod").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("WisMod").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("ChaMod").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("HealthMod").gameObject.SetActive(false);
        ref_StatusPanel.transform.Find("ManaMod").gameObject.SetActive(false);

        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].blessed != 0) ref_StatusPanel.transform.Find("Blessed").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].paralyzed != 0) ref_StatusPanel.transform.Find("Paralyzed").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].poisoned != 0) ref_StatusPanel.transform.Find("Poisoned").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].regen != 0) ref_StatusPanel.transform.Find("Regenerating").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].strMod != 0) ref_StatusPanel.transform.Find("StrMod").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].dexMod != 0) ref_StatusPanel.transform.Find("DexMod").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].intMod != 0) ref_StatusPanel.transform.Find("IntMod").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].wisMod != 0) ref_StatusPanel.transform.Find("WisMod").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].chaMod != 0) ref_StatusPanel.transform.Find("ChaMod").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].healthMod != 0) ref_StatusPanel.transform.Find("HealthMod").gameObject.SetActive(true);
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].manaMod != 0) ref_StatusPanel.transform.Find("ManaMod").gameObject.SetActive(true);
    }

    public void UpdateAddPoints()
    {
        GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_str += add_Str;
        GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_dex += add_Dex;
        GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_iq += add_IQ;
        GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_wis += add_Wis;
        GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_cha += add_Cha;
        GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_health += add_Hlth;
        GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_mana += add_Mana;
        add_Str = 0; add_Dex = 0; add_IQ = 0; add_Wis = 0; add_Cha = 0; add_Hlth = 0; add_Mana = 0;
        ref_freePointPanel.SetActive(false);
        GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].UpdateHeroStats();
        UpdateCharacterSheet();
    }

    public void LevelUpCharacter() //<--------------------------------------------------------------------------------------------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].xpPoints >= GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].xpNNL)
        {
            GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].xpLevel++; //increase level
            GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].xpPoints -= GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].xpNNL; //reduce xp by the amount needed to level
            GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].xpNNL += (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].xpLevel * GameManager.RULES.NNL_perLevel); //increase NNL by current XP_NNL+(XP_Level * NNL_perLevel)
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == Character.characterClass.Fighter)
            {
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_health += GameManager.RULES.Fighter_Health;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_mana += GameManager.RULES.Fighter_Mana;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].attack = GameManager.RULES.Fighter_Attack;
            }
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == Character.characterClass.Rogue)
            {
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_health += GameManager.RULES.Rogue_Health;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_mana += GameManager.RULES.Rogue_Mana;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].attack = GameManager.RULES.Rogue_Attack;
            }
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == Character.characterClass.Cleric)
            {
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_health += GameManager.RULES.Cleric_Health;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_mana += GameManager.RULES.Cleric_Mana + ((GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].wisdom / 2) - 4);
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].attack = GameManager.RULES.Cleric_Attack;
            }
            if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == Character.characterClass.Mage)
            {
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_health += GameManager.RULES.Mage_Health;
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].base_mana += GameManager.RULES.Mage_Mana + ((GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].intelligence / 2)-4);
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].attack = GameManager.RULES.Mage_Attack;
            }
            GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].freePoints += GameManager.RULES.FreePointsPerLevel;

            UpdateCharacterSheet();
        }
    }

    public void CloseCharacterSheet()
    {
        GameManager.EXPLORE.ClearAllScreens();
    }
    public void Navigate_Left()
    {
        int _c = GameManager.EXPLORE.selected_Character;
        GetComponentInParent<ExploreController>().ClearAllScreens();
        GetComponentInParent<ExploreController>().OpenInventoryScreen(_c);
    }
    public void Navigate_Right()
    {
        int _c = GameManager.EXPLORE.selected_Character;
        GetComponentInParent<ExploreController>().ClearAllScreens();
        GetComponentInParent<ExploreController>().OpenSpellCompendium(_c);
    }
    public void ShowCloseTooltip()
    {
        Tooltip.ShowToolTip_Static("Close the Character Sheet");
    }
    public void HideCloseTooltip()
    {
        Tooltip.HideToolTip_Static();
    }
    public void ShowLeftArrowToolTip()
    {
        Tooltip.ShowToolTip_Static("Go to Inventory");
    }
    public void HideLeftArrowToolTip()
    {
        Tooltip.HideToolTip_Static();
    }
    public void ShowRightArrowToolTip()
    {
        Tooltip.ShowToolTip_Static("Go to Spell Compendium");
    }
    public void HideRightArrowToolTip()
    {
        Tooltip.HideToolTip_Static();
    }    
    public void Status_Blessed() { Tooltip.ShowToolTip_Static("This Character is Blessed. /nThey get get hurt less and they hit harder."); }
    public void Status_Cursed() { Tooltip.ShowToolTip_Static("This Character is Cursed. /nThey get hurt more and are less able to deal damage."); }
    public void Status_Paralyzed() { Tooltip.ShowToolTip_Static("This Character is Paralyzed and unable to act."); }
    public void Status_Poisoned() { Tooltip.ShowToolTip_Static("This Character is Poisoned. /nTheir vitality will slowly drain away until healed."); }
    public void Status_Regen() { Tooltip.ShowToolTip_Static("This Character is Regenerating. The thrill of battle revitalizes them."); }
    public void Status_StrMod() { Tooltip.ShowToolTip_Static("This Character's Strength is modified. " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].strMod)); }
    public void Status_DexMod() { Tooltip.ShowToolTip_Static("This Character's Dexterity is modified. " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].dexMod)); }
    public void Status_IntMod() { Tooltip.ShowToolTip_Static("This Character's Intelligence is modified. " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].intMod)); }
    public void Status_WisMod() { Tooltip.ShowToolTip_Static("This Character's Wisdom is modified. " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].wisMod)); }
    public void Status_ChaMod() { Tooltip.ShowToolTip_Static("This Character's Charisma is modified. " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].chaMod)); }
    public void Status_HealthMod() { Tooltip.ShowToolTip_Static("This Character's Health is modified. " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].healthMod)); }
    public void Status_ManaMod() { Tooltip.ShowToolTip_Static("This Character's Mana is modified. " + (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].manaMod)); }
}
