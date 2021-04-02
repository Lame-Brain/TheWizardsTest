using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_I_am_a_ladder : MonoBehaviour
{
    public int DestinationIndex;
    public string Destination;

    public void InteractWithMe()
    {
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        GameManager.GAME.laddersound.Play();
        yield return new WaitForSeconds(GameManager.GAME.laddersound.clip.length);
        GameManager.GAME.LoadLevel(DestinationIndex, Destination);
    }
}
