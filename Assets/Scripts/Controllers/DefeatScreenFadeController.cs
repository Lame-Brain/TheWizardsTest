using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatScreenFadeController : MonoBehaviour
{
    private Animator Anim;
    private string Command;

    public void Start()
    {
        Anim = this.GetComponent<Animator>();
    }
    public void FadeScreen()
    {
        Anim.SetBool("FadeAway", true);
    }
    public void SetCommand(string s)
    {
        Command = s;
    }
    public void WaitForFadeToFinish()
    {
        GameManager.GAME.ToggleUI(true);
        if (Command == "MainMenu") SceneManager.LoadScene("TitleScreen");
    }
}
