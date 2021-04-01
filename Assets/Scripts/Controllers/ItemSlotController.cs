using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotController : MonoBehaviour, IDropHandler
{
    public InventoryItem.slotType slotType;

    public void OnDrop(PointerEventData eventData)
    {
        if (slotType == InventoryItem.slotType.bag || eventData.pointerDrag.GetComponent<ItemTileController>().item.slot == slotType) //Check if dropping in bag, or if the item is valid for the slot you are dropping on
        {            
            if (gameObject.transform.childCount == 0) //if the slot is empty
            {
                //Debug.Log("Dragged from " + eventData.pointerDrag.transform.parent.name + ", a " + eventData.pointerDrag.transform.parent.GetComponent<ItemSlotController>().slotType + " slot, to " + gameObject.name + ", a " + slotType + " slot");
                //Debug.Log("Dropped " + eventData.pointerDrag.name+" on " + gameObject.name);
                Debug.Log("Dragged from " + eventData.pointerDrag.transform.parent.tag + " to " + gameObject.tag);
                if (GameManager.EXPLORE.selected_Character == -2) {
                    if (eventData.pointerDrag.transform.parent.tag == "StoreSlot" && gameObject.tag == "BagSlot") gameObject.transform.parent.transform.parent.GetComponentInParent<UI_StoreController>().StoreMoney += (int)eventData.pointerDrag.GetComponent<ItemTileController>().item.value;
                    if (eventData.pointerDrag.transform.parent.tag == "BagSlot" && gameObject.tag == "StoreSlot") gameObject.transform.parent.transform.parent.GetComponentInParent<UI_StoreController>().StoreMoney += (int)eventData.pointerDrag.GetComponent<ItemTileController>().item.value;
                }
                eventData.pointerDrag.transform.SetParent(gameObject.transform);
                if (GameManager.EXPLORE.selected_Character == -1) gameObject.transform.parent.GetComponentInParent<ChestController>().ScreenToInventory();
                if (GameManager.EXPLORE.selected_Character > -1) GameManager.EXPLORE.current_InventoryScreen.GetComponent<Inventory_Controller>().ScreenToInventory();
                eventData.pointerDrag.GetComponent<ItemTileController>().enabled = true;
            }
            if (gameObject.transform.childCount == 1 && GameManager.EXPLORE.selected_Character > -2) // if the slot is in use, swap
            {
                if (eventData.pointerDrag.transform.parent.GetComponent<ItemSlotController>().slotType == InventoryItem.slotType.bag)
                {
                    gameObject.transform.GetChild(0).transform.SetParent(eventData.pointerDrag.transform.parent);
                    eventData.pointerDrag.transform.SetParent(gameObject.transform);
                    if (GameManager.EXPLORE.selected_Character == -1) gameObject.transform.parent.GetComponentInParent<ChestController>().ScreenToInventory();
                    if (GameManager.EXPLORE.selected_Character > -1) GameManager.EXPLORE.current_InventoryScreen.GetComponent<Inventory_Controller>().ScreenToInventory();
                    eventData.pointerDrag.GetComponent<ItemTileController>().enabled = true;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
