using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TempleController : MonoBehaviour
{
    public GameObject[] pcSlot;
    public GameObject ref_whoScreen;
    public Text ref_StoreText, ref_money;

    public int SelectedCharacter, price;
    public string SpellCast = "";

    // Start is called before the first frame update
    void Start()
    {
        SelectCharacter(-1);
    }

    // Update is called once per frame
    void Update()
    {
        ref_money.text = GameManager.PARTY.money.ToString();
        for (int _i = 0; _i < 4; _i++)
        {
            pcSlot[_i].transform.Find("Character_Key").GetChild(0).GetComponent<Text>().text = (_i + 1).ToString(); //This draws the slot number to the portrait, so player can hit this key to bring up this PC's info
            pcSlot[_i].transform.Find("Character_Portrait").GetComponent<Image>().sprite = GameManager.GAME.PC_Portrait[GameManager.PARTY.PC[_i].portraitIndex]; //This draws the PC's portrait
            pcSlot[_i].transform.Find("Character_Name_Plate").transform.GetChild(0).GetComponent<Text>().text = GameManager.PARTY.PC[_i].characterName;//this draws the PC's name
            pcSlot[_i].transform.Find("HealthBarContainer").transform.GetChild(1).GetComponent<Image>().fillAmount = (GameManager.PARTY.PC[_i].health - GameManager.PARTY.PC[_i].wounds) / GameManager.PARTY.PC[_i].health; //this draws the PC's health bar
            if (GameManager.PARTY.PC[_i].wounds >= GameManager.PARTY.PC[_i].health) pcSlot[_i].transform.Find("Dead").gameObject.SetActive(true);
            if (GameManager.PARTY.PC[_i].wounds < GameManager.PARTY.PC[_i].health) pcSlot[_i].transform.Find("Dead").gameObject.SetActive(false);
            if (GameManager.PARTY.PC[_i].mana == 0) { pcSlot[_i].transform.Find("ManaBarContainer").gameObject.SetActive(false); } //If the PC has no mana, hide the mana bar.
            else
            {
                pcSlot[_i].transform.Find("ManaBarContainer").gameObject.SetActive(true); //IF the PC has Mana, show the mana bar
                pcSlot[_i].transform.Find("ManaBarContainer").transform.GetChild(1).GetComponent<Image>().fillAmount = (GameManager.PARTY.PC[_i].mana - GameManager.PARTY.PC[_i].drain) / GameManager.PARTY.PC[_i].mana; //draw the PC's Mana bar.
            }
        }
        if(SpellCast != "" && SpellCast != "Bless")
        {
            ref_whoScreen.SetActive(true);
            if(SelectedCharacter > -1)
            {
                ref_whoScreen.SetActive(false);
                if (SpellCast == "Heal1") Heal(15);
                if (SpellCast == "Heal2") Heal(30);
                if (SpellCast == "Heal3") Heal(45);
                if (SpellCast == "Vivify1") Vivify(5);
                if (SpellCast == "Vivify2") Vivify(10);
                if (SpellCast == "Vivify3") Vivify(15);
                if (SpellCast == "Anidote") Antidote();
                if (SpellCast == "RemoveCurse") RemoveCurse();
                if (SpellCast == "CureDisease") CureDisease();
                if (SpellCast == "Purify") Purify();
                if (SpellCast == "Resurrect") Resurrect();
            }
        }
        if (SpellCast == "Bless") Bless();
    }

    public void ToolTip(string message)
    {
        Tooltip.ShowToolTip_Static(message);
    }
    public void HideToolTip() { Tooltip.HideToolTip_Static(); }

    public void TriggerSpell(string s)
    {
        SpellCast = s;
    }

    public void Heal(int h)
    {
        if(GameManager.PARTY.money >= price)
        {
            GameManager.PARTY.money -= price;
            GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
            GameManager.PARTY.spellSound.PlayOneShot(GameManager.PARTY.spellSound.clip);
            GameManager.PARTY.PC[SelectedCharacter].wounds -= h;
            if (GameManager.PARTY.PC[SelectedCharacter].wounds < 0) GameManager.PARTY.PC[SelectedCharacter].wounds = 0;
            SpellCast = "";
            SelectedCharacter = -1;
        }
    }
    public void Vivify(int v)
    {
        if (GameManager.PARTY.money >= price)
        {
            GameManager.PARTY.money -= price;
            GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
            GameManager.PARTY.spellSound.PlayOneShot(GameManager.PARTY.spellSound.clip);
            GameManager.PARTY.PC[SelectedCharacter].drain -= v;
            if (GameManager.PARTY.PC[SelectedCharacter].drain < 0) GameManager.PARTY.PC[SelectedCharacter].drain = 0;
            SpellCast = "";
            SelectedCharacter = -1;
        }
    }
    public void Antidote()
    {
        if (GameManager.PARTY.money >= price)
        {
            GameManager.PARTY.money -= price;
            GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
            GameManager.PARTY.spellSound.PlayOneShot(GameManager.PARTY.spellSound.clip);
            GameManager.PARTY.PC[SelectedCharacter].poisoned = 0;
            SpellCast = "";
            SelectedCharacter = -1;
        }
    }
    public void RemoveCurse()
    {
        if (GameManager.PARTY.money >= price)
        {
            GameManager.PARTY.money -= price;
            GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
            GameManager.PARTY.spellSound.PlayOneShot(GameManager.PARTY.spellSound.clip);
            GameManager.PARTY.PC[SelectedCharacter].cursed = false;
            SpellCast = "";
            SelectedCharacter = -1;
        }
    }
    public void CureDisease()
    {
        if (GameManager.PARTY.money >= price)
        {
            GameManager.PARTY.money -= price;
            GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
            GameManager.PARTY.spellSound.PlayOneShot(GameManager.PARTY.spellSound.clip);
            if(GameManager.PARTY.PC[SelectedCharacter].strMod < 0) GameManager.PARTY.PC[SelectedCharacter].strMod = 0;
            if(GameManager.PARTY.PC[SelectedCharacter].dexMod < 0) GameManager.PARTY.PC[SelectedCharacter].dexMod = 0;
            if(GameManager.PARTY.PC[SelectedCharacter].intMod < 0) GameManager.PARTY.PC[SelectedCharacter].intMod = 0;
            if(GameManager.PARTY.PC[SelectedCharacter].wisMod < 0) GameManager.PARTY.PC[SelectedCharacter].wisMod = 0;
            if(GameManager.PARTY.PC[SelectedCharacter].chaMod < 0) GameManager.PARTY.PC[SelectedCharacter].chaMod = 0;
            if(GameManager.PARTY.PC[SelectedCharacter].healthMod < 0) GameManager.PARTY.PC[SelectedCharacter].healthMod = 0;
            if(GameManager.PARTY.PC[SelectedCharacter].manaMod < 0) GameManager.PARTY.PC[SelectedCharacter].manaMod = 0;
            SpellCast = "";
            SelectedCharacter = -1;
        }
    }
    public void Purify()
    {
        if (GameManager.PARTY.money >= price)
        {
            GameManager.PARTY.money -= price;
            GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
            GameManager.PARTY.spellSound.PlayOneShot(GameManager.PARTY.spellSound.clip);
            GameManager.PARTY.PC[SelectedCharacter].poisoned = 0;
            GameManager.PARTY.PC[SelectedCharacter].cursed = false;
            if (GameManager.PARTY.PC[SelectedCharacter].strMod < 0) GameManager.PARTY.PC[SelectedCharacter].strMod = 0;
            if (GameManager.PARTY.PC[SelectedCharacter].dexMod < 0) GameManager.PARTY.PC[SelectedCharacter].dexMod = 0;
            if (GameManager.PARTY.PC[SelectedCharacter].intMod < 0) GameManager.PARTY.PC[SelectedCharacter].intMod = 0;
            if (GameManager.PARTY.PC[SelectedCharacter].wisMod < 0) GameManager.PARTY.PC[SelectedCharacter].wisMod = 0;
            if (GameManager.PARTY.PC[SelectedCharacter].chaMod < 0) GameManager.PARTY.PC[SelectedCharacter].chaMod = 0;
            if (GameManager.PARTY.PC[SelectedCharacter].healthMod < 0) GameManager.PARTY.PC[SelectedCharacter].healthMod = 0;
            if (GameManager.PARTY.PC[SelectedCharacter].manaMod < 0) GameManager.PARTY.PC[SelectedCharacter].manaMod = 0;
            SpellCast = "";
            SelectedCharacter = -1;
        }
    }
    public void Bless()
    {
        if (GameManager.PARTY.money >= price)
        {
            GameManager.PARTY.money -= price;
            GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
            GameManager.PARTY.spellSound.PlayOneShot(GameManager.PARTY.spellSound.clip);
            GameManager.PARTY.PC[0].GetComponent<Character>().blessed += 25;
            GameManager.PARTY.PC[1].GetComponent<Character>().blessed += 25;
            GameManager.PARTY.PC[2].GetComponent<Character>().blessed += 25;
            GameManager.PARTY.PC[3].GetComponent<Character>().blessed += 25;
            SpellCast = "";
            SelectedCharacter = -1;
        }
    }
    public void Resurrect()
    {
        if (GameManager.PARTY.money >= price)
        {
            GameManager.PARTY.money -= price;
            GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
            GameManager.PARTY.spellSound.PlayOneShot(GameManager.PARTY.spellSound.clip);
            GameManager.PARTY.PC[SelectedCharacter].wounds = GameManager.PARTY.PC[SelectedCharacter].health - 1;
            pcSlot[SelectedCharacter].transform.Find("Dead").gameObject.SetActive(false);
            SpellCast = "";
            SelectedCharacter = -1;
        }
    }
    public void Tithe()
    {
        Debug.Log("Deducting: " + (int)(GameManager.PARTY.money * 0.1f));
        GameManager.PARTY.money = GameManager.PARTY.money - (int)(GameManager.PARTY.money * 0.1f);
        GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
    }
    public void PayThePiper(int w)
    {
        price = w;
    }
    public void SelectCharacter(int n)
    {
        SelectedCharacter = n;
    }
}
