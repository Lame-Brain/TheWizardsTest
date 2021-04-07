using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGenController : MonoBehaviour
{
    public Image[] ref_portrait;
    public Text[] ref_name, ref_class;
    public GameObject[] ref_character;
    public GameObject ref_toonPortraitpickerslot, ref_PortraitPickerFrame;
    public Image ref_toonPortrait;
    public GameObject ref_MakeToonPanel, ref_toonName, ref_toonClass;
    public Text ref_toonSTR, ref_toonDEX, ref_toonIQ, ref_toonWIS, ref_toonCHA;

    private int selectedToon;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int _i = 0; _i < 4; _i++)
        {
            ref_portrait[_i].sprite = GameManager.GAME.PC_Portrait[ref_character[_i].GetComponent<Character>().portraitIndex];
            ref_name[_i].text = ref_character[_i].GetComponent<Character>().characterName;
            ref_class[_i].text = ref_character[_i].GetComponent<Character>().type.ToString();
        }
        if (ref_MakeToonPanel.activeSelf)
        {
            ref_toonPortrait.sprite = GameManager.GAME.PC_Portrait[ref_character[selectedToon].GetComponent<Character>().portraitIndex];
            ref_toonSTR.text = "Strength " + ref_character[selectedToon].GetComponent<Character>().base_str;
            ref_toonSTR.text = "Dexterity " + ref_character[selectedToon].GetComponent<Character>().base_dex;
            ref_toonSTR.text = "Intelligence " + ref_character[selectedToon].GetComponent<Character>().base_iq;
            ref_toonSTR.text = "Wisdom " + ref_character[selectedToon].GetComponent<Character>().base_wis;
            ref_toonSTR.text = "Charisma " + ref_character[selectedToon].GetComponent<Character>().base_cha;
        }
        if (ref_PortraitPickerFrame.activeSelf)
        {
        }
    }

    public void MakeToon(int n)
    {
        selectedToon = n;
        ref_MakeToonPanel.SetActive(true);
    }
    public void RollStats()
    {
        ref_character[selectedToon].GetComponent<Character>().base_str = Random.Range(3, 18);
        ref_character[selectedToon].GetComponent<Character>().base_dex = Random.Range(3, 18);
        ref_character[selectedToon].GetComponent<Character>().base_iq = Random.Range(3, 18);
        ref_character[selectedToon].GetComponent<Character>().base_wis = Random.Range(3, 18);
        ref_character[selectedToon].GetComponent<Character>().base_cha = Random.Range(3, 18);
    }
    public void SetName(string s)
    {
        ref_character[selectedToon].GetComponent<Character>().characterName = s;
    }
    public void SetClass(string s)
    {
        if (s == "Fighter") ref_character[selectedToon].GetComponent<Character>().type = Character.characterClass.Fighter;
        if (s == "Rogue") ref_character[selectedToon].GetComponent<Character>().type = Character.characterClass.Rogue;
        if (s == "Magic User") ref_character[selectedToon].GetComponent<Character>().type = Character.characterClass.Mage;
        if (s == "Cleric") ref_character[selectedToon].GetComponent<Character>().type = Character.characterClass.Cleric;
    }
    public void populatePortraitPicker()
    {
        GameObject _go;
        for (int _i = 0; _i < ref_PortraitPickerFrame.transform.childCount; _i++) Destroy(ref_PortraitPickerFrame.transform.GetChild(_i)); //kill the children
        for (int _i = 0; _i < GameManager.GAME.PC_Portrait.Length - 1; _i++)
        {
            _go = Instantiate(ref_toonPortraitpickerslot, ref_PortraitPickerFrame.transform);
            _go.GetComponent<Image>().sprite = GameManager.GAME.PC_Portrait[_i];
        }

    }
    public void ChangeFace()
    {
        
    }
    public void SaveToon()
    {

    }
    public void KillToon()
    {

    }
}
