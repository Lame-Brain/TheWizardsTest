using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TempleController : MonoBehaviour
{
    public Text ref_StoreText, ref_money;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ref_money.text = GameManager.PARTY.money.ToString();
    }

    public void ToolTip(string message)
    {
        Tooltip.ShowToolTip_Static(message);
    }
    public void HideToolTip() { Tooltip.HideToolTip_Static(); }

    public void Heal(int h)
    {
        //TO DO: pick target
        Debug.Log("Heal " + h + " wounds");
    }
    public void Vivify(int v)
    {
        //TO DO: pick target
        Debug.Log("Revive " + v + " drain points");
    }
    public void Antidote()
    {
        //TO DO: pick target
        Debug.Log("Cure Poison");
    }
    public void RemoveCurse()
    {
        //TO DO: pick target
        Debug.Log("Remove Curse");
    }
    public void CureDisease()
    {
        //TO DO: pick target
        Debug.Log("Remove Disease");
    }
    public void Purify()
    {
        //TO DO: pick target
        Debug.Log("Cure Poison + Remove Curse + Remove Disease");
    }
    public void Bless()
    {
        GameManager.PARTY.PC[0].GetComponent<Character>().blessed += 25;
        GameManager.PARTY.PC[1].GetComponent<Character>().blessed += 25;
        GameManager.PARTY.PC[2].GetComponent<Character>().blessed += 25;
        GameManager.PARTY.PC[3].GetComponent<Character>().blessed += 25;
    }
    public void Resurrect()
    {
        //TO DO: pick target
        Debug.Log("Resurrect character to life");
    }
    public void Tithe()
    {
        Debug.Log("Deducting: " + (int)(GameManager.PARTY.money * 0.1f));
        GameManager.PARTY.money = GameManager.PARTY.money - (int)(GameManager.PARTY.money * 0.1f);
        GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
    }
    public void PayThePiper(int w)
    {
        if(GameManager.PARTY.money >= w) GameManager.PARTY.money = -w;
        GameManager.EXPLORE.goldJingle.PlayOneShot(GameManager.EXPLORE.goldJingle.clip);
    }
}
