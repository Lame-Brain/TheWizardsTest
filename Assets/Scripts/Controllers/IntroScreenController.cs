using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntroScreenController : MonoBehaviour
{
    public AudioSource hisssss;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GAME.ToggleUI(false);
        timer = Random.Range(100f, 1500f);        
    }

    // Update is called once per frame
    void Update()
    {
        timer--;
        if(timer < 0)
        {
            timer = Random.Range(1000f, 15000f);
            hisssss.PlayOneShot(hisssss.clip, Random.Range(0f, .5f));
        }
    }

    public void PlayGame()
    {
        GameManager.GAME.ToggleUI(true);
    }
}
