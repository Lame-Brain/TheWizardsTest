using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    public GameObject[] ref_BagSlot, ref_GroundSlot, ref_ChestSlot;
    public GameObject ref_itemTile;
    public Text ref_money;
    [HideInInspector] public GameObject ref_MyChest;

    private void Update()
    {
        GameManager.PARTY.SetAllowedMovement(false); //Keep party from moving
    }
    public void CloseChest()
    {
        Destroy(transform.gameObject);
        GameManager.EXPLORE.ClearAllScreens();
    }

    //Updates what is happening on the UI panel based on values in program
    public void InventoryToScreen()
    {
        //Clean up old Item_Tiles
        GameObject[] _killList = GameObject.FindGameObjectsWithTag("Item_Tile");
        foreach (GameObject _target in _killList) Destroy(_target);
        //Draws Party Money
        ref_money.text = GameManager.PARTY.money.ToString();
        //Draw Bag Items that are in inventory
        GameObject _go = null;
        for (int _i = 0; _i < 20; _i++)
        {
            if (GameManager.PARTY.bagInventory[_i] != null)
            {
                _go = Instantiate(ref_itemTile, ref_BagSlot[_i].transform);
                _go.GetComponent<ItemTileController>().item = GameManager.PARTY.bagInventory[_i];
                _go.GetComponent<ItemTileController>().UpdateItemTile();
            }
        }
        //Draw Ground Items in this panel
        for (int _i = 0; _i < 9; _i++)
        {
            if (GameManager.PARTY.FindMyNode().GetComponent<GridNode>().inventory[_i] != null)
            {
                _go = Instantiate(ref_itemTile, ref_GroundSlot[_i].transform);
                _go.GetComponent<ItemTileController>().item = GameManager.PARTY.FindMyNode().GetComponent<GridNode>().inventory[_i];
                _go.GetComponent<ItemTileController>().UpdateItemTile();
            }
        }
        //Draw Chest Items in that are in panel
        for (int _i = 0; _i < 16; _i++)
        {
            if (ref_MyChest.GetComponent<Hello_I_am_a_Chest>().inventory[_i] != null)
            {
                _go = Instantiate(ref_itemTile, ref_ChestSlot[_i].transform);
                _go.GetComponent<ItemTileController>().item = ref_MyChest.GetComponent<Hello_I_am_a_Chest>().inventory[_i];
                _go.GetComponent<ItemTileController>().UpdateItemTile();
            }
        }
    }
    //Updates what is in the program values based on what has happend in the UI
    public void ScreenToInventory()
    {
        //Update Party's bag slots
        for (int _i = 0; _i < 20; _i++)
        {
            if (ref_BagSlot[_i].transform.childCount == 0) GameManager.PARTY.bagInventory[_i] = null;
            if (ref_BagSlot[_i].transform.childCount > 0) GameManager.PARTY.bagInventory[_i] = ref_BagSlot[_i].GetComponentInChildren<ItemTileController>().item;
        }
        //Update Ground slots
        for (int _i = 0; _i < 9; _i++)
        {
            if (ref_GroundSlot[_i].transform.childCount == 0) GameManager.PARTY.FindMyNode().GetComponent<GridNode>().inventory[_i] = null;
            if (ref_GroundSlot[_i].transform.childCount > 0) GameManager.PARTY.FindMyNode().GetComponent<GridNode>().inventory[_i] = ref_GroundSlot[_i].GetComponentInChildren<ItemTileController>().item;
        }
        //Update Chest slots
        for (int _i = 0; _i < 16; _i++)
        {
            if (ref_ChestSlot[_i].transform.childCount == 0) ref_MyChest.GetComponent<Hello_I_am_a_Chest>().inventory[_i] = null;
            if (ref_ChestSlot[_i].transform.childCount > 0) ref_MyChest.GetComponent<Hello_I_am_a_Chest>().inventory[_i] = ref_ChestSlot[_i].GetComponentInChildren<ItemTileController>().item;
        }
        //Updated Dynamic Props on GridNode
        GameManager.PARTY.FindMyNode().GetComponent<GridNode>().DynamicProps();
        //Close the Loop! Call InventorytoScreen
        InventoryToScreen();
    }
}
