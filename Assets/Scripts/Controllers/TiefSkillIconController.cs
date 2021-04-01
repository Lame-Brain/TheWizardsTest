using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TiefSkillIconController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        string _interactionType = GameManager.PARTY.interactContext;
        GameObject _interactionObj = GameManager.PARTY.Interact_Object;
        //MessageWindow.ShowMessage_Static(_interactionType);

        if (GameManager.PARTY.light > 0) //Check if there is light
        {
            if (_interactionType == "") //SEARCH
            {
                MessageWindow.ShowMessage_Static(GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].characterName + " looks around, but there is nothing to be found.");
                GameManager.EXPLORE.ClearAllScreens();
            }
            if (_interactionType == "LOCKED DOOR")
            {
                _interactionObj.GetComponent<Hello_I_am_a_door>().UnlockDoor();
            }
            if (_interactionType == "TRAP")
            {
                _interactionObj = GameManager.PARTY.FindMyNode(); //for some reason my previous call to Interact Object is not registering, so Fuck It. Here is the data direct from the source.
                float dex = (GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].dexterity / 2) - 5;
                float threshold = Random.Range(0, GameManager.RULES.RandomRange) + (_interactionObj.GetComponent<GridNode>().trapLevel * 10);
                float roll = Random.Range(0, GameManager.RULES.RandomRange) + dex;
                Debug.Log("Threshold: " + threshold + ", roll is: " + roll);
                if (roll >= threshold)
                {
                    MessageWindow.ShowMessage_Static(GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].characterName + " manages to disarm the trap!");
                    GameManager.EXPLORE.ClearAllScreens();
                    _interactionObj.GetComponent<GridNode>().trapLevel = 0;
                    _interactionObj.GetComponent<GridNode>().trapDamage = 0;
                    _interactionObj.GetComponent<GridNode>().trapDark = false;
                    GameManager.PARTY.trapdamage = 0;
                    transform.gameObject.SetActive(false);
                    GameManager.PARTY.interactContext = "";
                }
                else
                {
                    MessageWindow.ShowMessage_Static(GameManager.PARTY.PC[GameManager.EXPLORE.selected_Character].characterName + " fails to disarm the trap.");
                }
            }
        }
        if(GameManager.PARTY.light <= 0) //Check if there is no light
        {
            MessageWindow.ShowMessage_Static("Unfortunately, it is too dark to do anything.");
        }
        GameManager.PARTY.PassTurn(); //Everytime you use Thief Skills, pass a turn.
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowToolTip_Static("Thief Skills allow you to attempt to bypass locks and disarm traps.");        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideToolTip_Static();
    }
}
