using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TownPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseTown()
    {
        StartCoroutine(_CloseTown());
    }

    IEnumerator _CloseTown()
    {
        GameManager.GAME.laddersound.Play();
        yield return new WaitForSeconds(GameManager.GAME.laddersound.clip.length);
        GameManager.GAME.LoadLevel(1, "From Surface");
        GameManager.PARTY.SetAllowedMovement(true);
        Destroy(this.gameObject);
    }

}
