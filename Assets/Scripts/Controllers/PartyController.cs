﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class PartyController : MonoBehaviour
{
    public Character[] PC;
    public int money; //how much money does the party have?
    new public int light; //how many more turns of light does the party have?
    public int magical_light = 0;
    public int x_coor, y_coor, face;
    public InventoryItem[] bagInventory = new InventoryItem[20]; //What is the party carrying?

    //mapstuff
    public int[,] map = new int[18, 18];
    public int[,] mapN = new int[18, 18];
    public int[,] mapE = new int[18, 18];
    public int[,] mapS = new int[18, 18];
    public int[,] mapW = new int[18, 18];
    public bool[,] mapND = new bool[18, 18];
    public bool[,] mapED = new bool[18, 18];
    public bool[,] mapSD = new bool[18, 18];
    public bool[,] mapWD = new bool[18, 18];
    public bool[,] mapNT = new bool[18, 18];
    public bool[,] mapET = new bool[18, 18];
    public bool[,] mapST = new bool[18, 18];
    public bool[,] mapWT = new bool[18, 18];
    public bool[,] mapC= new bool[18, 18];

    private bool moving = false, AllowMovement = true;
    public Transform moveTarget, lookTarget;
    private string actionQueue;

    public string interactContext;
    public GameObject Interact_Object = null;

    private int trapcheckonX = -100, trapcheckonY = -100;
    public int trapdamage = 0;
    public bool trapdark = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < 18; y++) //Declare map is blank
            for (int x = 0; x < 18; x++)
            {
                map[x, y] = 0; mapN[x, y] = 0; mapE[x, y] = 0; mapS[x, y] = 0; mapW[x, y] = 0; mapND[x, y] = false; mapED[x, y] = false; mapWD[x, y] = false; mapSD[x, y] = false;
                mapNT[x, y] = false; mapET[x, y] = false; mapST[x, y] = false; mapWT[x, y] = false; mapC[x, y] = false;
            }

        if (GameManager.PARTY == null) GameManager.PARTY = this;
        else if (GameManager.PARTY != this) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        GameManager.EXPLORE.DrawExplorerUI();
        
        InventoryItem[] _temp = new InventoryItem[20]; //declare items into instances
        for (int _i = 0; _i < 20; _i++)
            if (bagInventory[_i] != null)
            {
                bagInventory[_i] = new InventoryItem(bagInventory[_i].genericName, bagInventory[_i].fullName, bagInventory[_i].description, bagInventory[_i].lore,
                    bagInventory[_i].slot, bagInventory[_i].type, bagInventory[_i].identified, bagInventory[_i].magical, bagInventory[_i].fragile, bagInventory[_i].twoHanded, bagInventory[_i].active, bagInventory[_i].minDamage, bagInventory[_i].maxDamage,
                    bagInventory[_i].fullCharges, bagInventory[_i].maxDuration, bagInventory[_i].quality, bagInventory[_i].currentCharges, bagInventory[_i].defense, bagInventory[_i].critMultiplier, bagInventory[_i].value, bagInventory[_i].itemIconIndex);
                bagInventory[_i].name = bagInventory[_i].fullName;
            }
        TeleportToDungeonStart("From Surface");
        //PassTurn();
    }

    // Update is called once per frame
    void Update() //<------------------------------------------------------------------------------------------------ Update
    {
        //Check for Lightsource Shenanigans
        light = 0;
        for (int _i = 0; _i < 20; _i++)
            if (bagInventory[_i] != null && bagInventory[_i].type == InventoryItem.equipType.light && bagInventory[_i].active &&
                bagInventory[_i].currentDuration > light) light = bagInventory[_i].currentDuration; //Sets light to the greatest duration that is active
        //Check for magical light
        light += magical_light;

        GameObject.FindGameObjectWithTag("LightSource").GetComponent<Light>().range = GameManager.RULES.BrightLight; //Set light to bright
        GameManager.EXPLORE.ref_darkwarningtext.gameObject.SetActive(false);
        if (light < 5) //If the light is low, set light to dim
        {
            GameObject.FindGameObjectWithTag("LightSource").GetComponent<Light>().range = GameManager.RULES.DimLight;
            GameManager.EXPLORE.ref_darkwarningtext.gameObject.SetActive(true);
        }
        if (light <= 0) GameObject.FindGameObjectWithTag("LightSource").GetComponent<Light>().range = 0;//If the light is expired, set light to 0;

        //keep x_coor and y_coor up-to-date
        x_coor = (int)transform.position.x; y_coor = (int)transform.position.z;

        if (AllowMovement) //I want to be able to pause movement and input while in menus
        {
            //Process button presses to build the action queue
            if (!moving && Input.GetAxisRaw("Vertical") > 0) StartCoroutine(DelayInput("UP", GameManager.RULES.MoveDelay));
            if (!moving && Input.GetAxisRaw("Vertical") < 0) StartCoroutine(DelayInput("DOWN", GameManager.RULES.MoveDelay));
            if (!moving && Input.GetAxisRaw("Horizontal2") < 0) StartCoroutine(DelayInput("LEFT", GameManager.RULES.MoveDelay));
            if (!moving && Input.GetAxisRaw("Horizontal2") > 0) StartCoroutine(DelayInput("RIGHT", GameManager.RULES.MoveDelay));
            if (!moving && Input.GetAxisRaw("Horizontal") > 0) StartCoroutine(DelayInput("SLIDE_RIGHT", GameManager.RULES.MoveDelay));
            if (!moving && Input.GetAxisRaw("Horizontal") < 0) StartCoroutine(DelayInput("SLIDE_LEFT", GameManager.RULES.MoveDelay));

            //convert party location to gridnode coordinates
            int x = (int)((transform.position.x + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize);
            int y = (int)((transform.position.z + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize);

            if (actionQueue == "UP")
            {
                //Debug.Log("Where am I going? Here: (" + (int)moveTarget.position.x + ", " + (int)moveTarget.position.z + ")");
                if (face == 0 && NodeIsValid(x, y - 1) && NotBlockedForMovement(face)) moveTarget = FindNode(x, y - 1).transform;
                if (face == 1 && NodeIsValid(x - 1, y) && NotBlockedForMovement(face)) moveTarget = FindNode(x - 1, y).transform;
                if (face == 2 && NodeIsValid(x, y + 1) && NotBlockedForMovement(face)) moveTarget = FindNode(x, y + 1).transform;
                if (face == 3 && NodeIsValid(x + 1, y) && NotBlockedForMovement(face)) moveTarget = FindNode(x + 1, y).transform;
                lookTarget = FaceMyTarget(moveTarget.gameObject, face);
            }
            if (actionQueue == "DOWN")
            {
                if (face == 0 && NodeIsValid(x, y + 1) && NotBlockedForMovement(2)) moveTarget = FindNode(x, y + 1).transform;
                if (face == 1 && NodeIsValid(x + 1, y) && NotBlockedForMovement(3)) moveTarget = FindNode(x + 1, y).transform;
                if (face == 2 && NodeIsValid(x, y - 1) && NotBlockedForMovement(0)) moveTarget = FindNode(x, y - 1).transform;
                if (face == 3 && NodeIsValid(x - 1, y) && NotBlockedForMovement(1)) moveTarget = FindNode(x - 1, y).transform;
                lookTarget = FaceMyTarget(moveTarget.gameObject, face);
            }
            if (actionQueue == "LEFT")
            {
                face--;
                if (face < 0) face = 3;
                lookTarget = FaceMyTarget(moveTarget.gameObject, face);
            }
            if (actionQueue == "RIGHT")
            {
                face++;
                if (face > 3) face = 0;
                lookTarget = FaceMyTarget(moveTarget.gameObject, face);
            }
            if (actionQueue == "SLIDE_LEFT")
            {
                if (face == 0 && NodeIsValid(x + 1, y) && NotBlockedForMovement(3)) moveTarget = FindNode(x + 1, y).transform;
                if (face == 1 && NodeIsValid(x, y - 1) && NotBlockedForMovement(0)) moveTarget = FindNode(x, y - 1).transform;
                if (face == 2 && NodeIsValid(x - 1, y) && NotBlockedForMovement(1)) moveTarget = FindNode(x - 1, y).transform;
                if (face == 3 && NodeIsValid(x, y + 1) && NotBlockedForMovement(2)) moveTarget = FindNode(x, y + 1).transform;
                lookTarget = FaceMyTarget(moveTarget.gameObject, face);
            }
            if (actionQueue == "SLIDE_RIGHT")
            {
                if (face == 0 && NodeIsValid(x - 1, y) && NotBlockedForMovement(1)) moveTarget = FindNode(x - 1, y).transform;
                if (face == 1 && NodeIsValid(x, y + 1) && NotBlockedForMovement(2)) moveTarget = FindNode(x, y + 1).transform;
                if (face == 2 && NodeIsValid(x + 1, y) && NotBlockedForMovement(3)) moveTarget = FindNode(x + 1, y).transform;
                if (face == 3 && NodeIsValid(x, y - 1) && NotBlockedForMovement(0)) moveTarget = FindNode(x, y - 1).transform;
                lookTarget = FaceMyTarget(moveTarget.gameObject, face);
            }
            actionQueue = "";

            //Interact Command
            if (Input.GetButtonUp("Submit") && Interact_Object != null)
            {
                if (Interact_Object.tag == "MapDoor") Interact_Object.GetComponent<Hello_I_am_a_door>().InteractWithMe();
                if (Interact_Object.tag == "Chest") Interact_Object.GetComponent<Hello_I_am_a_Chest>().InteractWithMe();
                if (Interact_Object.tag == "Signage" && light > 0) Interact_Object.GetComponent<Hello_I_am_a_sign>().InteractWithMe();
                if (Interact_Object.tag == "MapLadder") Interact_Object.GetComponent<Hello_I_am_a_ladder>().InteractWithMe();

                StartCoroutine(DelayInput("INTERACT", GameManager.RULES.MoveDelay));
                Interact_Object = null; //Reset Interact Object to Null. Prevents crashes.
            }
        }
        ShowInteract();

        //Controls outside of Allow Movement
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.F1)) GameManager.EXPLORE.OpenInventoryScreen(0);
        if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.F2)) GameManager.EXPLORE.OpenInventoryScreen(1);
        if (Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.F3)) GameManager.EXPLORE.OpenInventoryScreen(2);
        if (Input.GetKeyUp(KeyCode.Alpha4) || Input.GetKeyUp(KeyCode.F4)) GameManager.EXPLORE.OpenInventoryScreen(3);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.EXPLORE.current_CharacterSheetScreen != null || GameManager.EXPLORE.current_InventoryScreen != null || GameManager.EXPLORE.current_SpellCompendium != null || GameManager.EXPLORE.current_Map != null || GameManager.EXPLORE.current_Chest_Panel != null || GameManager.EXPLORE.current_Sign_panel != null)
            {
                GameManager.EXPLORE.ClearAllScreens();
            }
            else
            {
                GameManager.EXPLORE.ref_MainMenu.SetActive(!GameManager.EXPLORE.ref_MainMenu.activeSelf);
            }
        }

        CheckForTraps();

        //build map data
        if (light > 0)
        {
            int x_ = (int)((x_coor + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize), y_ = (int)((y_coor + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize);
            GameObject _go = FindMyNode(); //Debug.Log(x_ + ", " + y_);

            if (_go.GetComponent<GridNode>().northChest || _go.GetComponent<GridNode>().eastChest || _go.GetComponent<GridNode>().southChest || _go.GetComponent<GridNode>().westChest) mapC[x_, y_] = true;
            if (_go.GetComponent<GridNode>().northLink == null) //Wall to the north
            {
                mapN[x_, y_] = 2; //Interior Wall
                if (FindNode(x_, y_ - 1) == null) mapN[x_, y_] = 1; //Exterior Wall
            }
            if (_go.GetComponent<GridNode>().northDoor) mapND[x_, y_] = true; //North Door 
            if (_go.GetComponent<GridNode>().northTorch) mapNT[x_, y_] = true; //North Torch 

            if (_go.GetComponent<GridNode>().eastLink == null) //Wall to the east
            {
                mapE[x_, y_] = 2; //Interior Wall
                if (FindNode(x_ - 1, y_) == null) mapE[x_, y_] = 1; //Exterior Wall
            }
            if (_go.GetComponent<GridNode>().eastDoor) mapED[x_, y_] = true; //East Door 
            if (_go.GetComponent<GridNode>().eastTorch) mapET[x_, y_] = true; //East Torch

            if (_go.GetComponent<GridNode>().southLink == null) //Wall to the south
            {
                mapS[x_, y_] = 2; //Interior Wall
                if (FindNode(x_, y_ + 1) == null) mapS[x_, y_] = 1; //Exterior Wall
            }
            if (_go.GetComponent<GridNode>().southDoor) mapSD[x_, y_] = true; //South Door 
            if (_go.GetComponent<GridNode>().southTorch) mapST[x_, y_] = true; //South Torch

            if (_go.GetComponent<GridNode>().westLink == null) //Wall to the west
            {
                mapW[x_, y_] = 2; //Interior Wall
                if (FindNode(x_ + 1, y_) == null) mapW[x_, y_] = 1; //Exterior Wall
            }
            if (_go.GetComponent<GridNode>().westDoor) mapWD[x_, y_] = true; //West Door 
            if (_go.GetComponent<GridNode>().westTorch) mapWT[x_, y_] = true; //West Torch

            if (_go.GetComponent<GridNode>().trapDamage == 0) map[x_, y_] = 1;//Floor
            if (_go.GetComponent<GridNode>().trapDamage != 0) map[x_, y_] = 2;
            if (_go.GetComponent<GridNode>().trapDark) map[x_, y_] = 3;
        }





    }

    private void FixedUpdate() //<------------------------------------------------------------------------------------------------Fixed Update
    {
        float move = GameManager.RULES.TileSize / GameManager.RULES.moveSpeed;

        transform.position = Vector3.MoveTowards(transform.position, moveTarget.position, move * Time.deltaTime); //move transform toward moveTarget position

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position), GameManager.RULES.turnSpeed * Time.deltaTime); //Rotate party to look at LookTarget


        if (Vector3.Distance(transform.position, moveTarget.position) < .1 && Quaternion.Angle(transform.rotation, Quaternion.LookRotation(lookTarget.position - transform.position)) < 4) //If the transform is close to the moveTarget, then the party is not moving.
        {
            transform.position = moveTarget.position; //"Snap" party to target location
            transform.LookAt(lookTarget.position); //"Snap" party to rotation            
        }
    }

    IEnumerator DelayInput(string i, float n)
    {
        moving = true;
        actionQueue = i;

        if (i == "UP" || i == "DOWN" || i == "SLIDE_LEFT" || i == "SLIDE_RIGHT" || i == "INTERACT") PassTurn();

        yield return new WaitForSecondsRealtime(n);

        moving = false;
    }

    public void TeleportToDungeonStart(string destination)
    {
        //Debug.Log("Teleporting to the beginning in " + SceneManager.GetActiveScene().buildIndex);
        actionQueue = ""; //Clear the action queue

        GameObject[] _entrances = GameObject.FindGameObjectsWithTag("MazeEntrance"); //find the entrance
        foreach (GameObject _entrance in _entrances)
        {
            //Debug.Log("Does " + _entrance.name + " == " + destination + "?");
            if (_entrance.name == destination)
            {
                transform.position = _entrance.transform.position; //Set Party location
                transform.rotation = _entrance.transform.rotation; //Set Party rotation
            }
        }
        //set party facing based on rotation
        if (transform.rotation.eulerAngles.y < -135 || transform.rotation.eulerAngles.y > 135) face = 0; //facing North
        if (transform.rotation.eulerAngles.y >= 225 && transform.rotation.eulerAngles.y <= 315) face = 1; //facing East
        if (transform.rotation.eulerAngles.y > -45 && transform.rotation.eulerAngles.y < 45) face = 2; //face South
        if (transform.rotation.eulerAngles.y >= 45 && transform.rotation.eulerAngles.y <= 135) face = 3; //Face West
        //Reset moveTarget and lookTarget
        moveTarget = FindMyNode().transform;
        lookTarget = FaceMyTarget(FindMyNode(), face);
    }

    public void LoadParty(SaveSlot.serialParty p) //**************************************************************************************************************<<<<<
    {
        for (int _i = 0; _i < 4; _i++) PC[_i].LoadCharacter(p.PC[_i]);
        money = p.money;
        light = p.light;
        magical_light = p.magical_light;

        for (int i = 0; i < 20; i++)
        {
            bagInventory[i] = null;
            if(p.inventory[i].genericName != "") bagInventory[i] = new InventoryItem(p.inventory[i]);
        }
            

        x_coor = p.x_coor; y_coor = p.y_coor; face = p.face;
        transform.position = new Vector3(x_coor, 1, y_coor); Debug.Log("Party repositioned to: "+transform.position);
        transform.rotation = FaceMyTarget(FindMyNode(), face).rotation;
        moveTarget = FindMyNode().transform;
        lookTarget = FaceMyTarget(FindMyNode(), face);
    }
    
    public void LoadMiniMap(int[] mapCenter, int[] mapNorth, int[] mapEast, int[] mapSouth, int[]mapWest, bool[] doorNorth, bool[] doorEast, bool[] doorSouth, bool[] doorWest, bool[] trapNorth, bool[] trapEast, bool[] trapSouth, bool[] trapWest, bool[] chest)
    {

        for (int y = 0; y < 18; y++)
            for(int x = 0; x < 18; x++)
            {
                map[x, y] = mapCenter[y * 18 + x];
                mapN[x, y] = mapNorth[y * 18 + x];
                mapE[x, y] = mapEast[y * 18 + x];
                mapS[x, y] = mapSouth[y * 18 + x];
                mapW[x, y] = mapWest[y * 18 + x];
                mapND[x, y] = doorNorth[y * 18 + x];
                mapED[x, y] = doorEast[y * 18 + x];
                mapSD[x, y] = doorSouth[y * 18 + x];
                mapWD[x, y] = doorWest[y * 18 + x];
                mapNT[x, y] = trapNorth[y * 18 + x];
                mapET[x, y] = trapEast[y * 18 + x];
                mapST[x, y] = trapSouth[y * 18 + x];
                mapWT[x, y] = trapWest[y * 18 + x];
                mapC[x, y] = chest[y * 18 + x];
            }
    }

    public GameObject FindMyNode() //returns a reference to the gameobject of the node that is in the same tile as the party object
    {        
        GameObject[] _nodeList = GameObject.FindGameObjectsWithTag("Node");
        GameObject _result = null;
        int _nodeX, _nodeY;
        _nodeX = (int)((transform.position.x + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize);
        _nodeY = (int)((transform.position.z + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize);
        foreach (GameObject _go in _nodeList) if (_go.GetComponent<GridNode>().nodeX == _nodeX && _go.GetComponent<GridNode>().nodeY == _nodeY) _result = _go;
        return _result;
    }

    public GameObject FindNode(int x, int y) //Takes the grid coordinates of a node, not the transform, returns reference to node gameobject
    {
        GameObject[] _nodeList = GameObject.FindGameObjectsWithTag("Node");
        GameObject _result = null;
        foreach (GameObject _go in _nodeList) if (_go.GetComponent<GridNode>().nodeX == x && _go.GetComponent<GridNode>().nodeY == y) _result = _go;
        return _result;
    }
    
    public bool NodeIsValid(int x, int y)//Takes the grid coordinates of a node, not the transform, returns true if the node exists
    {
        bool _result = false;
        GameObject[] _nodeList = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject _go in _nodeList) if (_go.GetComponent<GridNode>().nodeX == x && _go.GetComponent<GridNode>().nodeY == y) _result = true;
        return _result;
    }

    public bool NotBlockedForMovement(int d)
    {
        bool _result = false; GameObject _node = FindMyNode();
        //Check if there is a link. If there is a link, is there a door? If there is a door, is it open?
        if (d == 0 && _node.GetComponent<GridNode>().northLink != null)
        {
            if (_node.GetComponent<GridNode>().northDoor == null) _result = true;
            if (_node.GetComponent<GridNode>().northDoor != null && _node.GetComponent<GridNode>().northDoor.GetComponent<Hello_I_am_a_door>().doorOpen) _result = true;
        }
        if (d == 1 && _node.GetComponent<GridNode>().eastLink != null)
        {
            if (_node.GetComponent<GridNode>().eastDoor == null) _result = true;
            if (_node.GetComponent<GridNode>().eastDoor != null && _node.GetComponent<GridNode>().eastDoor.GetComponent<Hello_I_am_a_door>().doorOpen) _result = true;
        }
        if (d == 2 && _node.GetComponent<GridNode>().southLink != null)
        {
            if (_node.GetComponent<GridNode>().southDoor == null) _result = true;
            if (_node.GetComponent<GridNode>().southDoor != null && _node.GetComponent<GridNode>().southDoor.GetComponent<Hello_I_am_a_door>().doorOpen) _result = true;
        }
        if (d == 3 && _node.GetComponent<GridNode>().westLink != null)
        {
            if (_node.GetComponent<GridNode>().westDoor == null) _result = true;
            if (_node.GetComponent<GridNode>().westDoor != null && _node.GetComponent<GridNode>().westDoor.GetComponent<Hello_I_am_a_door>().doorOpen) _result = true;
        }
        return _result;
    }

    public Transform FaceMyTarget(GameObject t, int f)
    {
        Transform _result = null;
        if (f == 0) _result = t.transform.Find("North").transform;
        if (f == 1) _result = t.transform.Find("East").transform;
        if (f == 2) _result = t.transform.Find("South").transform;
        if (f == 3) _result = t.transform.Find("West").transform;

        return _result;
    }

    //public access to AllowMovement bool
    public void ToggleAllowMovement() { AllowMovement = !AllowMovement; }
    public void SetAllowedMovement(bool b) { AllowMovement = b; }
    public bool GetAllowedMovement() { return AllowMovement; }

    public void ShowInteract()
    {
        //Raycast
        Sprite _result = GameManager.EXPLORE.ref_empty;
        RaycastHit rcHit; Interact_Object = null;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rcHit, GameManager.RULES.TileSize))
        {
            //Debug.Log(rcHit.transform.tag);
            Interact_Object = rcHit.transform.gameObject; interactContext = ""; 
            if (rcHit.transform.tag == "MapDoor")
            {
                _result = GameManager.GAME.Icons[rcHit.transform.GetComponent<Hello_I_am_a_door>().IconIndex];

                if (rcHit.transform.gameObject.GetComponent<Hello_I_am_a_door>().knownLocked) interactContext = "LOCKED DOOR";
                if (!rcHit.transform.gameObject.GetComponent<Hello_I_am_a_door>().knownLocked) interactContext = "CLOSED DOOR";
                if (FindMyNode().GetComponent<GridNode>().trapLevel > 0) interactContext = "TRAP"; //Make sure that traps get called out in context
            }
            if(rcHit.transform.tag == "Chest")
            {
                interactContext = "CHEST";
                Interact_Object = rcHit.transform.gameObject;
                _result = GameManager.GAME.Icons[28];
            }
            if (rcHit.transform.tag == "MapLadder")
            {
                interactContext = "LADDER";
                Interact_Object = rcHit.transform.gameObject;                
                if (Interact_Object.name == "Ladder_down") _result = GameManager.GAME.Icons[32];
                if (Interact_Object.name == "Ladder_up") _result = GameManager.GAME.Icons[33];

            }
            if (rcHit.transform.tag == "Signage")
            {
                interactContext = "SIGN";
                Interact_Object = rcHit.transform.gameObject;
                _result = GameManager.GAME.Icons[3];
            }

        }

        if (light > 0) GameManager.EXPLORE.ref_Interact.GetComponent<Image>().sprite = _result;
        if (light <= 0) GameManager.EXPLORE.ref_Interact.GetComponent<Image>().sprite = GameManager.EXPLORE.ref_empty;
    }
    
    public void PassTurn()
    {
        GameObject[] _all_GameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject _go in _all_GameObjects) _go.gameObject.BroadcastMessage("TurnPasses", 1, SendMessageOptions.DontRequireReceiver);

        if (magical_light > 0) magical_light--; //consume magical light
        for (int _i = 0; _i < 20; _i++)
        {
            if (bagInventory[_i] != null && bagInventory[_i].type == InventoryItem.equipType.light && bagInventory[_i].active)
            {
                bagInventory[_i].currentDuration--; //active lightsource reduces duration
                if (bagInventory[_i].currentDuration <= 0) bagInventory[_i] = null; //consume lightsource if the duration is exceeded.
            }            
        }
    }

    public void CheckForTraps()
    {
        if(transform.position == FindMyNode().transform.position) //Check that the party is on the node
        {

            if (transform.position.x != trapcheckonX || transform.position.z != trapcheckonY) //Check that a trap check has not happened on this node yet this turn
            {
                TriggerTraps();

                //Remove Trap context
                interactContext = "";
                Interact_Object = null;

                trapcheckonX = (int)transform.position.x; trapcheckonY = (int)transform.position.z; //set trapcheckonX & Y to curret position

                if (FindMyNode().GetComponent<GridNode>().trapLevel > 0) // check for traps
                {
                    interactContext = "TRAP";
                    Interact_Object = FindMyNode();
                    MessageWindow.ShowMessage_Static("CLICK! A plate below you shifts as your party steps on it...");
                }

                trapdamage = FindMyNode().GetComponent<GridNode>().trapDamage;
                trapdark = FindMyNode().GetComponent<GridNode>().trapDark;
                if (light <= 0) TriggerTraps();
            }
        }
    }

    public void TriggerTraps()
    {
        if (trapdamage > 0)
        {
            int damage = trapdamage;
            GameManager.Splash("-" + damage + "hp", Color.red, Color.white, GameManager.EXPLORE.pcSlot[0]);
            GameManager.Splash("-" + damage + "hp", Color.red, Color.white, GameManager.EXPLORE.pcSlot[1]);
            GameManager.Splash("-" + damage + "hp", Color.red, Color.white, GameManager.EXPLORE.pcSlot[2]);
            GameManager.Splash("-" + damage + "hp", Color.red, Color.white, GameManager.EXPLORE.pcSlot[3]);
            MessageWindow.ShowMessage_Static("The party takes " + damage + " damage!");
            GameManager.PARTY.PC[0].wounds += damage;
            GameManager.PARTY.PC[1].wounds += damage;
            GameManager.PARTY.PC[2].wounds += damage;
            GameManager.PARTY.PC[3].wounds += damage;
            GameManager.EXPLORE.DrawExplorerUI(); //update screen
        }
        if (trapdark) //Kills your light source
        {
            for (int _i = 0; _i < 20; _i++)
                if (bagInventory[_i] != null && bagInventory[_i].type == InventoryItem.equipType.light && bagInventory[_i].active) bagInventory[_i].currentDuration = 0;
            MessageWindow.ShowMessage_Static("You are plunged into darkness!");
        }
    }
}
