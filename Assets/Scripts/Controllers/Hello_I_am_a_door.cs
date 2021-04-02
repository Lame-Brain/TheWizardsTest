//Added sounds for opening and discovering that it is lcked. Need to be sure to add the right sound (metal or wood) to the appropriate door in Map Prefab

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_I_am_a_door : MonoBehaviour
{
    public bool doorOpen, knownLocked;
    public float lockValue;
    public int IconIndex;
    private bool metaldoor;

    private void Start()
    {
        if (doorOpen) transform.gameObject.SetActive(false);
        if (transform.name.Substring(transform.name.Length - 2) == "01") metaldoor = false;
        if (transform.name.Substring(transform.name.Length - 2) == "02") metaldoor = true;
    }

    private void Update()
    {
        if (!knownLocked) IconIndex = 27;
        if (knownLocked) IconIndex = 30;
    }

    public void InteractWithMe()
    {
        if (lockValue == 0)//Door is unlocked, open it.
        {
            doorOpen = true;
            if (metaldoor) GameManager.PARTY.MetalDoorSound.PlayOneShot(GameManager.PARTY.MetalDoorSound.clip, 1f);
            if (!metaldoor) GameManager.PARTY.MetalDoorSound.PlayOneShot(GameManager.PARTY.WoodDoorSound.clip, 1f);
            transform.gameObject.SetActive(false);
        }
        if (lockValue > 0 && !knownLocked)
        {
            GameManager.PARTY.LockDoorSound.PlayOneShot(GameManager.PARTY.LockDoorSound.clip, 1f);
            knownLocked = true;//Door is locked, but untried. Mark it as locked.
        }
    }

    public void UnlockDoor()
    {
        float dex = (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].dexterity / 2) - 5;
        float threshold = Random.Range(0, GameManager.RULES.RandomRange) + (lockValue * 10);
        float roll = Random.Range(0, GameManager.RULES.RandomRange) + dex;
        Debug.Log("Threshold: " + threshold + ", roll is: " + roll);
        if (roll >= threshold)
        {
            MessageWindow.ShowMessage_Static(GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].characterName + " manages to open the lock!");
            GameManager.EXPLORE.ClearAllScreens();
            doorOpen = true;
            transform.gameObject.SetActive(false);
            GameManager.PARTY.interactContext = "";
        }
        else
        {
            MessageWindow.ShowMessage_Static(GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].characterName + " fails to open the lock.");
        }
    }

    public void LoadDoor(bool _do, bool _kl, float _lv)
    {
        doorOpen = _do;
        knownLocked = _kl;
        lockValue = _lv;
        if (doorOpen) transform.gameObject.SetActive(false);
        if (!doorOpen) transform.gameObject.SetActive(true);
    }
}
