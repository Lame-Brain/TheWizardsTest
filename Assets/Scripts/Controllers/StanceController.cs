using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StanceController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.transform.parent.GetComponentInParent<Inventory_Controller>().ChangeCharacterStance();
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].frontLine) Tooltip.ShowToolTip_Static("Characters set to Aggressive Stance will be attacked more frequently. \nClick to change stance to Defensive.");
        if (!GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].frontLine) Tooltip.ShowToolTip_Static("Characters set to Defensive Stance will be attacked less frequently. \nClick to change stance to Aggresive.");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].frontLine) Tooltip.ShowToolTip_Static("Characters set to Aggressive Stance will be attacked more frequently. \nClick to change stance to Defensive.");
        if (!GameManager.PARTY.PC[GetComponentInParent<ExploreController>().selected_Character].frontLine) Tooltip.ShowToolTip_Static("Characters set to Defensive Stance will be attacked less frequently. \nClick to change stance to Aggresive.");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideToolTip_Static();
    }
}
