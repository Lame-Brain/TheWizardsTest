/*Added sound for use in battle and when doing splats and messages
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public static PartyController PARTY;
    public static ExploreController EXPLORE;
    public static RULES RULES;

    public Sprite[] PC_Portrait, monster_Sprite, item_Icons, Icons;
    public Material[] DungeonColorTextures;
    public Spell[] spells;
    public InventoryItem[] items;
    public AudioSource PageSound, SplatSound, WhiffSound, VictorySound, LoseSound, BattleSound, laddersound;
    public GameObject partyWindow, UIwindow;

    public int SelectedSaveSlot;

    //Dynamic Level Controller data
    public GameObject[] Map, NodeHive; //, Spawner


    void Awake()
    {
        if (GAME == null) GAME = this;
        else if (GAME != this) Destroy(this.gameObject);
        
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {        
        //Debug settings
        SelectedSaveSlot = 0;
        SaveLoadModule.InitSave(SelectedSaveSlot);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab)) //DEBUG
        {
            //LoadLevel(1, "From Level 1");
            LoadLevel(0, "STORE");
            GameManager.PARTY.TeleportToDungeonStart("From Level 1");
            //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

            //PARTY.PassTurn();
            //Splash("-14 hp", Color.red, Color.white, EXPLORE.pcSlot[0]);
            //Splash("-12 hp", Color.red, Color.white, EXPLORE.pcSlot[1]);
            //Splash("-28 hp", Color.red, Color.white, EXPLORE.pcSlot[2]);
            //Splash("-5 hp", Color.red, Color.white, EXPLORE.pcSlot[3]);
            //MessageWindow.ShowMessage_Static("This is a test message!");

            //Debug.Log("Save");
            //SaveLoadModule.SaveGame(0);
            //Debug.Log("Load");
            //SaveLoadModule.LoadGame(0); 

            /*
             * foreach(Material _mat in DungeonColorTextures)
                {
                    _mat.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
                }
            */

            //            Debug.Log(PARTY.face + " = " + PARTY.transform.rotation.eulerAngles.y);
        }
    }

    public void LoadLevel(int DestinationIndex, string Destination)
    {
        SaveLoadModule.save_slot[SelectedSaveSlot].SetMiniMap(SaveLoadModule.save_slot[SelectedSaveSlot], SceneManager.GetActiveScene().buildIndex);

        if (Destination != "STORE")
        {
            SceneManager.LoadScene(DestinationIndex);
            StartCoroutine(waitForSceneLoad(DestinationIndex, Destination));            
        }
        if(Destination == "STORE")
        {
            EXPLORE.OpenTownScreen();
        }
    }

    public void LoadLevelandWaitUntilDone(int sceneNumber, string Destination)
    {
        SaveLoadModule.save_slot[SelectedSaveSlot].SetMiniMap(SaveLoadModule.save_slot[SelectedSaveSlot], SceneManager.GetActiveScene().buildIndex);

        SceneManager.LoadScene(sceneNumber);
        StartCoroutine(_LoadLevelandWaitUntilDone(sceneNumber, Destination));
    }

    IEnumerator waitForSceneLoad(int sceneNumber, string Destination)
    {
        while (SceneManager.GetActiveScene().buildIndex != sceneNumber)
        {
            yield return null;
        }

        GameManager.PARTY.TeleportToDungeonStart(Destination);
    }

    IEnumerator _LoadLevelandWaitUntilDone(int sceneNumber, string Destination)
    {
        while (SceneManager.GetActiveScene().buildIndex != sceneNumber)
        {
            yield return null;
        }
        Debug.Log("scene loaded");

        SaveLoadModule.FinishLoadingGame(SelectedSaveSlot);
    }

    public static void Splash(string text, Color bg, Color tc, GameObject target)
    {
        GameManager.GAME.SplatSound.Play();
        GameObject _go = Instantiate(EXPLORE.ref_Splash, target.transform);
        _go.GetComponent<Splash>().Show(text, bg, tc);
    }

    public void ToggleUI(bool state)
    {
        Debug.Log("Turning off UI");
        partyWindow.SetActive(state);
        UIwindow.SetActive(state);
    }
}