using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreenController : MonoBehaviour
{
    public AudioSource hisssss;
    public GameObject ref_PlayScreen, ref_CreditScreen, ref_loading;

    private float timer;
    private bool fileOps;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GAME.ToggleUI(false);
        timer = Random.Range(100f, 1500f);
        fileOps = false;
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
        ref_loading.SetActive(fileOps);
    }
    
    public void KillSave(int s)
    {
        if (!fileOps)
        {
            StartCoroutine(FileOpsDelay(2f));
            if (File.Exists(Application.persistentDataPath + "/saveGame0" + s + ".sg")) File.Delete(Application.persistentDataPath + "/saveGame0" + s + ".sg");
        }
    }

    public void PlayGame(int s)
    {
        if (!fileOps)
        {
            StartCoroutine(FileOpsDelay(2f));
            GameManager.GAME.SelectedSaveSlot = s;
            if (File.Exists(Application.persistentDataPath + "/saveGame0" + s + ".sg"))
            {
                SaveLoadModule.LoadGame(s);
            }
            else
            {
                File.Create(Application.persistentDataPath + "/saveGame0" + s + ".sg");
                SceneManager.LoadScene("CreateCharacter");
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator FileOpsDelay(float n)
    {
        fileOps = true;
        yield return new WaitForSeconds(n);
        fileOps = false;
    }
}
