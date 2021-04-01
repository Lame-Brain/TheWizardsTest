using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Panel_Controller : MonoBehaviour
{
    public void PushMonsterButton()
    {
        transform.GetComponentInParent<BattleScreenController>().ButtonPushed("Monster" + (transform.GetSiblingIndex()+1));
    }
}
