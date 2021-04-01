using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_I_am_a_ladder : MonoBehaviour
{
    public int DestinationIndex;
    public string Destination;

    public void InteractWithMe()
    {
        GameManager.GAME.LoadLevel(DestinationIndex, Destination);
    }
}
