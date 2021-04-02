using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenController : MonoBehaviour
{
    public AudioSource winmusic;
    public AudioSource losemusic;
    public AudioSource music;
    public Sprite power, righteous, noble, loyal;
    public GameObject contText;

    private string winCondition;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource _music = winmusic;
        //winCondition = GameManager.PARTY.interactContext;
        winCondition = "LOYAL";
        if (winCondition == "POWER") _music = losemusic;

        if (winCondition == "POWER") transform.Find("PoterPanel").GetComponent<Image>().sprite = power;
        if (winCondition == "LOYAL") transform.Find("PoterPanel").GetComponent<Image>().sprite = loyal;
        if (winCondition == "RIGHTEOUS") transform.Find("PoterPanel").GetComponent<Image>().sprite = righteous;
        if (winCondition == "NOBLE") transform.Find("PoterPanel").GetComponent<Image>().sprite = noble;
        _music.PlayOneShot(_music.clip, 1f);
        StartCoroutine(PlayBackgroundMusic(_music.clip.length));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayBackgroundMusic(float n)
    {
        yield return new WaitForSeconds(n);
        music.Play();
        contText.SetActive(true);
    }
}
