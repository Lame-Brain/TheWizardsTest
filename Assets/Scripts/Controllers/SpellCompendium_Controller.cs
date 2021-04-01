using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCompendium_Controller : MonoBehaviour
{
    public GameObject ref_Portrait, ref_manaBar, ref_spellSlotPF, ref_spellSlotPanel;
    public Text ref_NameText, ref_spellTypeText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSpellCompendiumScreen();
    }

    public void UpdateSpellCompendiumScreen()
    {
        GameObject _slot = null;
        ref_Portrait.GetComponent<Image>().sprite = GameManager.GAME.PC_Portrait[GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].portraitIndex];
        ref_NameText.text = GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].characterName;

        if(GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == Character.characterClass.Cleric || GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == Character.characterClass.Mage)
        {
            ref_manaBar.transform.parent.gameObject.SetActive(true);
            ref_manaBar.GetComponent<Image>().fillAmount = (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].mana - GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].wounds) / GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].mana;
        }
        else
        {
            ref_manaBar.transform.parent.gameObject.SetActive(false);
        }

        ref_spellTypeText.text = "Superstition";
        if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == Character.characterClass.Cleric) ref_spellTypeText.text = "Cleric Spells";
        if (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == Character.characterClass.Mage) ref_spellTypeText.text = "Mage Spells";

        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("SpellSlot")) Destroy(_go);
        for(int _i = 0; _i < GameManager.GAME.spells.Length; _i++)
            if (GameManager.GAME.spells[_i].UseInExplore && GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].type == GameManager.GAME.spells[_i].spellType && 
                GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].xpLevel >= GameManager.GAME.spells[_i].spellLevel)                       
            {
                _slot = Instantiate(ref_spellSlotPF, ref_spellSlotPanel.transform);
                _slot.GetComponent<SpellSlotController>().spell = GameManager.GAME.spells[_i];
                _slot.GetComponent<SpellSlotController>().spellName.text = GameManager.GAME.spells[_i].spellName;
            }
    }

    public void CloseSpellCompendium()
    {
        GetComponentInParent<ExploreController>().ClearAllScreens();
    }
    public void Navigate_Left()
    {
        int _c = GameManager.EXPLORE.selected_Character;
        GetComponentInParent<ExploreController>().ClearAllScreens();
        GetComponentInParent<ExploreController>().OpenCharacterSheetScreen(_c);
    }
    public void Navigate_Right()
    {
        int _c = GameManager.EXPLORE.selected_Character;
        GetComponentInParent<ExploreController>().ClearAllScreens();
        GetComponentInParent<ExploreController>().OpenMapSheet(_c);
    }
    public void ShowCloseTooltip()
    {
        Tooltip.ShowToolTip_Static("Close the Spell Compendium");
    }
    public void ShowLeftArrowToolTip()
    {
        Tooltip.ShowToolTip_Static("Go to Character Sheet");
    }
    public void ShowRightArrowToolTip()
    {
        Tooltip.ShowToolTip_Static("Go to Map");
    }
    public void HideToolTip()
    {
        Tooltip.HideToolTip_Static();
    }
}
