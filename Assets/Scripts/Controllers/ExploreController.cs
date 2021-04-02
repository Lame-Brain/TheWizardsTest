/* Added sounds for opening bags
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploreController : MonoBehaviour
{
    [Header("Party UI element Hookups")]
    public GameObject[] pcSlot = new GameObject[4];
    public GameObject ref_InventoryScreen;
    public GameObject ref_CharacterScreen;
    public GameObject ref_SpellCompendium;
    public GameObject ref_Map;
    public GameObject ref_MainMenu;
    public GameObject ref_BattleScreen;
    public GameObject ref_TownScreen;
    public Image ref_Interact;
    public Sprite ref_empty;
    public GameObject ref_Splash;
    public Sprite ref_bagSprite;
    public Text ref_darkwarningtext;
    public GameObject ref_SignPanel;
    public AudioSource OpenBagSound, goldJingle;    

    [Header("Other")]
    public bool movementPaused = false;
    public int selected_Character;

    public GameObject current_InventoryScreen;
    public GameObject current_CharacterSheetScreen;
    public GameObject current_SpellCompendium;
    public GameObject current_Map;
    public GameObject current_Chest_Panel;
    public GameObject current_Battle_Screen;
    public GameObject current_Sign_panel;
    public GameObject current_Town_Screen;

    private void Awake()
    {
        
    }

    void Start()
    {
        if (GameManager.EXPLORE == null) GameManager.EXPLORE = this;
        else if (GameManager.EXPLORE != this) Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);

        //DrawExplorerUI(); //First draw the UI.
    }

    public void DrawExplorerUI()
    {
        for (int _i = 0; _i < 4; _i++)
        {
            pcSlot[_i].transform.Find("Character_Key").GetChild(0).GetComponent<Text>().text = (_i+1).ToString(); //This draws the slot number to the portrait, so player can hit this key to bring up this PC's info
            pcSlot[_i].transform.Find("Character_Portrait").GetComponent<Image>().sprite = GameManager.GAME.PC_Portrait[GameManager.PARTY.PC[_i].portraitIndex]; //This draws the PC's portrait
            pcSlot[_i].transform.Find("Character_Name_Plate").transform.GetChild(0).GetComponent<Text>().text = GameManager.PARTY.PC[_i].characterName;//this draws the PC's name
            pcSlot[_i].transform.Find("HealthBarContainer").transform.GetChild(1).GetComponent<Image>().fillAmount = (GameManager.PARTY.PC[_i].health - GameManager.PARTY.PC[_i].wounds) / GameManager.PARTY.PC[_i].health; //this draws the PC's health bar
            if (GameManager.PARTY.PC[_i].wounds >= GameManager.PARTY.PC[_i].health) pcSlot[_i].transform.Find("Dead").gameObject.SetActive(true);
            if (GameManager.PARTY.PC[_i].wounds < GameManager.PARTY.PC[_i].health) pcSlot[_i].transform.Find("Dead").gameObject.SetActive(false);
            if (GameManager.PARTY.PC[_i].mana == 0) { pcSlot[_i].transform.Find("ManaBarContainer").gameObject.SetActive(false); } //If the PC has no mana, hide the mana bar.
            else
            {
                pcSlot[_i].transform.Find("ManaBarContainer").gameObject.SetActive(true); //IF the PC has Mana, show the mana bar
                pcSlot[_i].transform.Find("ManaBarContainer").transform.GetChild(1).GetComponent<Image>().fillAmount = (GameManager.PARTY.PC[_i].mana - GameManager.PARTY.PC[_i].drain) / GameManager.PARTY.PC[_i].mana; //draw the PC's Mana bar.
            }             
        }   
    }

    public void ClearAllScreens()
    {
        OpenBagSound.Play();
        if (current_InventoryScreen != null) Destroy(current_InventoryScreen);
        if (current_CharacterSheetScreen != null) Destroy(current_CharacterSheetScreen);
        if (current_SpellCompendium != null) Destroy(current_SpellCompendium);
        if (current_Map != null) Destroy(current_Map);
        if (current_Chest_Panel != null) Destroy(current_Chest_Panel);
        if (current_Sign_panel != null) Destroy(current_Sign_panel);
        if (current_Town_Screen != null) Destroy(current_Town_Screen);
        Tooltip.HideToolTip_Static();

        current_InventoryScreen = null;
        current_CharacterSheetScreen = null;
        current_SpellCompendium = null;
        current_Map = null;
        current_Chest_Panel = null;
        current_Sign_panel = null;
        current_Town_Screen = null;

        GameManager.PARTY.SetAllowedMovement(true); // Allow party to move again.

        selected_Character = 0;
    }

    public void OpenInventoryScreen(int _n)
    {
        if(_n == selected_Character && (current_InventoryScreen != null || current_CharacterSheetScreen != null || current_SpellCompendium != null || current_Map != null))
        {
            ClearAllScreens();
        }
        else
        {
            ClearAllScreens();
            selected_Character = _n;
            current_InventoryScreen = Instantiate(ref_InventoryScreen, this.transform);
            GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
        }
    }
    public void OpenInventoryScreen()
    {
        ClearAllScreens();
        current_InventoryScreen = Instantiate(ref_InventoryScreen, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void OpenCharacterSheetScreen()
    {
        ClearAllScreens();
        current_CharacterSheetScreen = Instantiate(ref_CharacterScreen, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void OpenCharacterSheetScreen(int _n)
    {
        ClearAllScreens();
        selected_Character = _n;
        current_CharacterSheetScreen = Instantiate(ref_CharacterScreen, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void OpenSpellCompendium()
    {
        ClearAllScreens();
        current_SpellCompendium = Instantiate(ref_SpellCompendium, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void OpenSpellCompendium(int _n)
    {
        ClearAllScreens();
        selected_Character = _n;
        current_SpellCompendium = Instantiate(ref_SpellCompendium, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void OpenMapSheet()
    {
        ClearAllScreens();
        current_Map = Instantiate(ref_Map, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void OpenMapSheet(int _n)
    {
        ClearAllScreens();
        selected_Character = _n;
        current_Map = Instantiate(ref_Map, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void OpenSign()
    {
        ClearAllScreens();
        current_Sign_panel = Instantiate(ref_SignPanel, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void OpenTownScreen()
    {
        ClearAllScreens();
        selected_Character = -2;
        Instantiate(GameManager.EXPLORE.ref_TownScreen, GameManager.EXPLORE.transform);
        GameManager.PARTY.SetAllowedMovement(false);
    }

    public void SaveGame()
    {
        Debug.Log("Game Saved by ExploreController");
        SaveLoadModule.SaveGame(GameManager.GAME.SelectedSaveSlot);
        ref_MainMenu.SetActive(false);
    }

    public void LoadGame()
    {
        Debug.Log("Game Loaded by ExploreController");
        SaveLoadModule.LoadGame(GameManager.GAME.SelectedSaveSlot);        
        ref_MainMenu.SetActive(false);
    }

    public void OpenBattleScreen()
    {
        ClearAllScreens();
        current_Battle_Screen = Instantiate(ref_BattleScreen, this.transform);
        GameManager.PARTY.SetAllowedMovement(false); // Disallow party movement
    }

    public void LoadMonsters(List<SaveSlot.SpawnPointData> spawns)
    {
        //1. find the spawnpoints, load the data
        GameObject[] _allSpawnpoints = GameObject.FindGameObjectsWithTag("MobSpawner");
        for (int _i = 0; _i < spawns.Count; _i++)
        {
            foreach(GameObject _go in _allSpawnpoints) if(spawns[_i].Xcoor == (int)_go.transform.position.x && spawns[_i].Ycoor == (int)_go.transform.position.z) //checks the coordinates to get a match
                {
                    _go.GetComponent<SpawnController>().RestoreChildren(spawns[_i].children);
                }
        }
    }
}
