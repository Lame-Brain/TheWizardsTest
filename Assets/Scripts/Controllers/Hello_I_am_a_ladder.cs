using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello_I_am_a_ladder : MonoBehaviour
{
    public int DestinationIndex;
    public string Destination;

    public void InteractWithMe()
    {
        if (transform.parent.name == "Door_out")
        {
            StartCoroutine(PlayDoorSound());
        } else { StartCoroutine(PlayLadderSound()); }                
    }

    IEnumerator PlayLadderSound()
    {
        GameManager.GAME.laddersound.Play();
        yield return new WaitForSeconds(GameManager.GAME.laddersound.clip.length);
        GameManager.GAME.LoadLevel(DestinationIndex, Destination);
    }
    IEnumerator PlayDoorSound()
    {
        GameManager.GAME.laddersound.Play();
        yield return new WaitForSeconds(GameManager.PARTY.WoodDoorSound.clip.length);
        GameManager.GAME.LoadLevel(DestinationIndex, Destination);
    }
}
