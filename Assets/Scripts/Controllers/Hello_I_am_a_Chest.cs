using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_I_am_a_Chest : MonoBehaviour
{
    public GameObject ref_ChestPanel;
    public InventoryItem[] inventory = new InventoryItem[16];

    private GameObject chestInventoryScreen;
    private bool TimeIsPassing = false;

    private void Start()
    {
        for (int _i = 0; _i < 16; _i++)
        {
            if (inventory[_i] != null) inventory[_i] = new InventoryItem(inventory[_i].genericName, inventory[_i].fullName, inventory[_i].description, inventory[_i].lore, inventory[_i].slot, inventory[_i].type, inventory[_i].identified, inventory[_i].magical, inventory[_i].fragile, inventory[_i].twoHanded, inventory[_i].active, inventory[_i].minDamage, inventory[_i].maxDamage, inventory[_i].fullCharges, inventory[_i].maxDuration, inventory[_i].quality, inventory[_i].currentCharges, inventory[_i].defense, inventory[_i].critMultiplier, inventory[_i].value, inventory[_i].itemIconIndex);
            if (inventory[_i] != null) inventory[_i].name = inventory[_i].genericName;
        }
    }

    public void InteractWithMe()
    {
        //Bring up menu
        GameManager.EXPLORE.selected_Character = -1;
        chestInventoryScreen = Instantiate(ref_ChestPanel, GameManager.EXPLORE.transform);
        chestInventoryScreen.GetComponent<ChestController>().ref_MyChest = transform.gameObject;
        GameManager.EXPLORE.current_Chest_Panel = chestInventoryScreen;
        chestInventoryScreen.GetComponent<ChestController>().InventoryToScreen();
    }

    public void TurnPasses()
    {
        for (int _i = 0; _i < 16; _i++)
        {
            if (inventory[_i] != null && inventory[_i].type == InventoryItem.equipType.light && inventory[_i].active) { inventory[_i].active = false; inventory[_i].itemIconIndex++; }
        }
    }

    public void LoadInventory(SaveSlot.serialItem[] serialItem)
    {        
        for (int c = 0; c < 16; c++)
        {            
            inventory[c] = null;            
            if (serialItem[c].genericName != "") inventory[c] = new InventoryItem(serialItem[c]);
        }
    }
}
