using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatScreenFadeController : MonoBehaviour
{
    public Animator Anim;
    private string Command;

    public void Start()
    {
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
        if (Command == "Load") GameManager.EXPLORE.LoadGame();
    }
}
