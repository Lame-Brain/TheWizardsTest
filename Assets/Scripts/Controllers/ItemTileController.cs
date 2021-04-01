using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemTileController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public InventoryItem item = null;
    private static bool enableToolTips = true;

    public void OnDrag(PointerEventData eventData)
    {

        //If item is coins, transfer value to money
        if (item.type == InventoryItem.equipType.money)
        {
            GameManager.PARTY.money += (int)item.value;
            item = null;
            Tooltip.HideToolTip_Static();
            if (GameManager.EXPLORE.selected_Character == -1) gameObject.transform.parent.GetComponentInParent<ChestController>().ScreenToInventory();
            if (GameManager.EXPLORE.selected_Character > -1) GameManager.EXPLORE.current_InventoryScreen.GetComponent<Inventory_Controller>().ScreenToInventory();
        }else
        {
            enableToolTips = false;
            transform.GetComponent<Image>().raycastTarget = false;
            transform.position = eventData.position;
            transform.parent.transform.SetAsLastSibling();
            transform.parent.parent.transform.SetAsLastSibling();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        enableToolTips = true;
        transform.localPosition = Vector3.zero;
        transform.GetComponent<Image>().raycastTarget = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Right Click
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //If item is coins, transfer value to money
            if (item.type == InventoryItem.equipType.money)
            {
                GameManager.PARTY.money += (int)item.value;
                item = null;
                Tooltip.HideToolTip_Static();
                if (GameManager.EXPLORE.selected_Character == -1) gameObject.transform.parent.GetComponentInParent<ChestController>().ScreenToInventory();
                if (GameManager.EXPLORE.selected_Character > -1) GameManager.EXPLORE.current_InventoryScreen.GetComponent<Inventory_Controller>().ScreenToInventory();
            }
        }

        //Left Click
        if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 1 && GameManager.EXPLORE.selected_Character != -2)
        {
            //If item is coins, transfer value to money
            if (item.type == InventoryItem.equipType.money)
            {
                GameManager.PARTY.money += (int)item.value;
                item = null;
                Tooltip.HideToolTip_Static();
                if (GameManager.EXPLORE.selected_Character == -1) gameObject.transform.parent.GetComponentInParent<ChestController>().ScreenToInventory();
                if (GameManager.EXPLORE.selected_Character > -1) GameManager.EXPLORE.current_InventoryScreen.GetComponent<Inventory_Controller>().ScreenToInventory();
            }
        }

        //Left Double Click
        if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2 && GameManager.EXPLORE.selected_Character != -2)
        {
            Debug.Log("Left Mouse Button Double Clicked on: " + item.genericName);

            bool _done = false;

            //If item is coins, transfer value to money
            if(item.type == InventoryItem.equipType.money)
            {
                GameManager.PARTY.money += (int)item.value;
                item.type = InventoryItem.equipType.deleted;
                _done = true;
                Tooltip.HideToolTip_Static();
            }

            //if item is on ground, transfer it to inventory
            if(!_done && (transform.parent.parent.name == "Ground_Inventory" || transform.parent.parent.name == "Chest_Inventory"))
            {
                int _result = 20;
                for (int _i = 19; _i >= 0; _i--)
                {
                    if (transform.parent.parent.parent.Find("Bag_Inventory").GetChild(_i).childCount == 0) _result = _i; //Find the lowest empty slot
                }
                if (_result < 20) transform.SetParent(transform.parent.parent.parent.Find("Bag_Inventory").GetChild(_result).transform); //Set object to selected slot                
                _done = true;
            }

            //if clicked on light, toggle active.
            if (item.type == InventoryItem.equipType.light)
            {
                item.active = !item.active;                
                
                if (item.active)
                {
                    Tooltip.ShowToolTip_Static(" It is now lit ");
                    item.itemIconIndex--; //Shifts icon toward lit version
                }
                if (!item.active)
                {
                    Tooltip.ShowToolTip_Static(" It is now unlit ");
                    item.itemIconIndex++; //Shifts icon toward unlit version
                }
                _done = true;
            }

            //If clicked on potion, use it.
            if (GameManager.EXPLORE.selected_Character != -1 && item.type == InventoryItem.equipType.potion)
            {
                _done = true;
            }

            //If clicked on weapons or armor, look for an empty slot. If found, equip. If not, relocate to next available bag slot.
            if (item.type == InventoryItem.equipType.weapon || item.type == InventoryItem.equipType.armor)
            {
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.head && GameManager.EXPLORE.current_InventoryScreen.transform.Find("Head_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("Head_Slot").transform); _done = true; }
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.neck && GameManager.EXPLORE.current_InventoryScreen.transform.Find("Neck_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("Neck_Slot").transform); _done = true; }
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.ring && GameManager.EXPLORE.current_InventoryScreen.transform.Find("RightFinger_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("RightFinger_Slot").transform); _done = true; }
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.ring && GameManager.EXPLORE.current_InventoryScreen.transform.Find("LeftFinger_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("LeftFinger_Slot").transform); _done = true; }
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.hand && GameManager.EXPLORE.current_InventoryScreen.transform.Find("RightHand_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("RightHand_Slot").transform); _done = true; }
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.hand && GameManager.EXPLORE.current_InventoryScreen.transform.Find("LeftHand_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("LeftHand_Slot").transform); _done = true; }
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.torso && GameManager.EXPLORE.current_InventoryScreen.transform.Find("Torso_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("Torso_Slot").transform); _done = true; }
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.leg && GameManager.EXPLORE.current_InventoryScreen.transform.Find("Legs_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("Legs_Slot").transform); _done = true; }
                if (!_done && GameManager.EXPLORE.selected_Character != -1 && item.slot == InventoryItem.slotType.foot && GameManager.EXPLORE.current_InventoryScreen.transform.Find("Feet_Slot").childCount == 0) { transform.SetParent(GameManager.EXPLORE.current_InventoryScreen.transform.Find("Feet_Slot").transform); _done = true; }
            }

            //relocate to next empty bag slot.
            if(!_done)
            {
                int _result = 19;
                for (int _i = 19; _i >= 0; _i--)
                {
                    if (transform.parent.parent.parent.Find("Bag_Inventory").GetChild(_i).childCount == 0) _result = _i; //Find the lowest empty slot
                }
                if (_result < 20) transform.SetParent(transform.parent.parent.parent.Find("Bag_Inventory").GetChild(_result).transform); //Set object to selected slot       
            }

            Tooltip.HideToolTip_Static();
            if (item.type == InventoryItem.equipType.deleted) item = null;
            if (GameManager.EXPLORE.selected_Character == -1) gameObject.transform.parent.GetComponentInParent<ChestController>().ScreenToInventory();
            if (GameManager.EXPLORE.selected_Character > -1) GameManager.EXPLORE.current_InventoryScreen.GetComponent<Inventory_Controller>().ScreenToInventory();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string output = null;
        if (!item.identified) output = item.genericName + "\n" + item.description;
        if (item.identified) output = item.fullName + "\n" + item.lore;
        if (item.type == InventoryItem.equipType.weapon) output += "\nDamage: " + item.minDamage + " to " + item.maxDamage;
        if (item.type == InventoryItem.equipType.armor) output += "\nDefense bonus: " + item.defense;
        if (item.type == InventoryItem.equipType.potion) output += "\nDouble Click to use.";
        if (item.type == InventoryItem.equipType.light) output += "\nDuration: " + item.currentDuration + " of " + item.maxDuration;
        if (item.fragile) output += "\nThis item is fragile.";
        if (item.type == InventoryItem.equipType.light && item.active) output += "\nDouble Click to snuff.";
        if (item.type == InventoryItem.equipType.light && !item.active) output += "\nDouble Click to light.";
        if (item.type == InventoryItem.equipType.weapon || item.type == InventoryItem.equipType.weapon) output += "\nDouble Click to equip or unequip.";

        if (enableToolTips) Tooltip.ShowToolTip_Static(output);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideToolTip_Static();
    }

    public void UpdateItemTile()
    {
        GetComponentInParent<Image>().sprite = GameManager.GAME.item_Icons[item.itemIconIndex];
    }
}
