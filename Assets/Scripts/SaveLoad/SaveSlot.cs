﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveSlot
{
    public serialParty thisParty;
    public List<SceneData> scene_List;
    public int CurrentScene;

    [System.Serializable]
    public struct serialItem
    {
        public string genericName, fullName, description, lore;
        public InventoryItem.slotType slot;
        public InventoryItem.equipType type;
        public bool identified, magical, fragile, twoHanded, active;
        public int minDamage, maxDamage, fullCharges, maxDuration, quality;
        public int currentCharges, currentDuration;
        public float defense, critMultiplier, value;
        public int itemIconIndex;

        public serialItem(InventoryItem i)
        {
            genericName = "";
            fullName = "";
            description = "";
            lore = "";
            slot = 0;
            type = 0;
            identified = false;
            magical = false;
            fragile = false;
            twoHanded = false;
            active = false;
            minDamage = 0;
            maxDamage = 0;
            fullCharges = 0;
            maxDuration = 0;
            quality = 0;
            currentCharges = 0;
            currentDuration = 0;
            defense = 0;
            critMultiplier = 0;
            value = 0;
            itemIconIndex = 0;
            if (i != null)
            {
                genericName = i.genericName;
                fullName = i.fullName;
                description = i.description;
                lore = i.lore;
                slot = i.slot;
                type = i.type;
                identified = i.identified;
                magical = i.magical;
                fragile = i.fragile;
                twoHanded = i.twoHanded;
                active = i.active;
                minDamage = i.minDamage;
                maxDamage = i.maxDamage;
                fullCharges = i.fullCharges;
                maxDuration = i.maxDuration;
                quality = i.quality;
                currentCharges = i.currentCharges;
                currentDuration = i.currentDuration;
                defense = i.defense;
                critMultiplier = i.critMultiplier;
                value = i.value;
                itemIconIndex = i.itemIconIndex;
            }
        }
    }
    [System.Serializable]
    public struct serialCharacter
    {
        public string characterName;
        public Character.characterClass type;
        public int xpLevel, xpPoints, xpNNL, freePoints;
        public int strength, dexterity, intelligence, wisdom, charisma;
        public float health, wounds, mana, drain, defense, attack;
        public int portraitIndex;
        public serialItem eq_Head, eq_Neck, eq_LeftFinger, eq_RightFinger, eq_LeftHand, eq_RightHand, eq_Torso, eq_Legs, eq_Feet;
        public int poisoned; //take INT damage every turn or round in combat. Does not wear off
        public int regen; //heal INT damage ever round in combat. Ends after combat or after INT turns
        public int paralyzed; //cannot move or act in battle. counts down every turn. does not count down in battle
        public bool cursed; //halves attack and doubles incoming damage. does not wear off
        public int blessed; //doubles attack and halves incoming damage. Counts down every turn, does not count down in battle
        public int strMod, dexMod, intMod, wisMod, chaMod, healthMod, manaMod; //the associated stat is modded by INT wears off after battle

        public serialCharacter(int n)
        {
            characterName = GameManager.PARTY.PC[n].characterName;
            type = GameManager.PARTY.PC[n].type;
            xpLevel = GameManager.PARTY.PC[n].xpLevel; xpPoints = GameManager.PARTY.PC[n].xpPoints; xpNNL = GameManager.PARTY.PC[n].xpNNL; freePoints = GameManager.PARTY.PC[n].freePoints;
            strength = GameManager.PARTY.PC[n].base_str;
            dexterity = GameManager.PARTY.PC[n].base_dex;
            intelligence = GameManager.PARTY.PC[n].base_iq;
            wisdom = GameManager.PARTY.PC[n].base_wis;
            charisma = GameManager.PARTY.PC[n].base_cha;
            health = GameManager.PARTY.PC[n].base_health;
            wounds = GameManager.PARTY.PC[n].wounds;
            mana = GameManager.PARTY.PC[n].base_mana;
            drain = GameManager.PARTY.PC[n].drain;
            defense = GameManager.PARTY.PC[n].defense;
            attack = GameManager.PARTY.PC[n].attack;
            portraitIndex = GameManager.PARTY.PC[n].portraitIndex;
            eq_Head = new serialItem(GameManager.PARTY.PC[n].eq_Head);
            eq_Neck = new serialItem(GameManager.PARTY.PC[n].eq_Neck);
            eq_LeftFinger = new serialItem(GameManager.PARTY.PC[n].eq_LeftFinger);
            eq_RightFinger = new serialItem(GameManager.PARTY.PC[n].eq_RightFinger);
            eq_LeftHand = new serialItem(GameManager.PARTY.PC[n].eq_LeftHand);
            eq_RightHand = new serialItem(GameManager.PARTY.PC[n].eq_RightHand);
            eq_Torso = new serialItem(GameManager.PARTY.PC[n].eq_Torso);
            eq_Legs = new serialItem(GameManager.PARTY.PC[n].eq_Legs);
            eq_Feet = new serialItem(GameManager.PARTY.PC[n].eq_Feet);
            poisoned = GameManager.PARTY.PC[n].poisoned;
            regen = GameManager.PARTY.PC[n].regen;
            paralyzed = GameManager.PARTY.PC[n].paralyzed;
            cursed = GameManager.PARTY.PC[n].cursed;
            blessed = GameManager.PARTY.PC[n].blessed;
            strMod = GameManager.PARTY.PC[n].strMod;
            dexMod = GameManager.PARTY.PC[n].dexMod;
            intMod = GameManager.PARTY.PC[n].intMod;
            wisMod = GameManager.PARTY.PC[n].wisMod;
            chaMod = GameManager.PARTY.PC[n].chaMod;
            healthMod = GameManager.PARTY.PC[n].healthMod;
            manaMod = GameManager.PARTY.PC[n].manaMod;
        }
    }
    [System.Serializable]
    public struct serialParty
    {
        public serialCharacter[] PC;
        public int money; 
        public int light;
        public int magical_light;
        public int x_coor, y_coor, face;
        public serialItem[] inventory;

        public serialParty(int n)
        {
            PC = new serialCharacter[4];
            for(int _i = 0; _i < 4; _i++)
            {
                PC[_i] = new serialCharacter(_i);
            }
            money = GameManager.PARTY.money;
            light = GameManager.PARTY.light;
            magical_light = GameManager.PARTY.magical_light;
            x_coor = GameManager.PARTY.x_coor; y_coor = GameManager.PARTY.y_coor; face = GameManager.PARTY.face;
            inventory = new serialItem[20];
            for(int _i = 0; _i < 20; _i++)
            {
                inventory[_i] = new serialItem(GameManager.PARTY.bagInventory[_i]);
            }
        }
    }
    [System.Serializable]
    public struct ChestData
    {
        public string chestName;
        public int x, y;
        public serialItem[] inventory;

        //Here is how to get the scene value = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        public ChestData(string _cn, int _x, int _y, InventoryItem[] _inv)
        {
            chestName = _cn;
            x = _x; y = _y;
            inventory = new serialItem[_inv.Length];
            for (int i = 0; i < _inv.Length; i++)
            {
                //Debug.Log("Data Dump: _inv.Length = " + _inv.Length + ", inventory.length = " + inventory.Length + " and current index is " + i);
                inventory[i] = new serialItem(_inv[i]);
            }
        }
    }
    [System.Serializable]
    public struct DoorData
    {
        public int x, y;
        public bool doorOpen, knownLocked;
        public float lockValue;

        public DoorData(int _x, int _y, bool _do, bool _kl, float _lv)
        {
            x = _x; y = _y;
            doorOpen = _do;
            knownLocked = _kl;
            lockValue = _lv;
        }
    }
    [System.Serializable]
    public struct NodeData
    {
        public int x, y;
        public serialItem[] inventory;

        public NodeData(int _x, int _y, InventoryItem[] _inv)
        {
            x = _x; y = _y;
            inventory = new serialItem[9];
            for (int i = 0; i < 9; i++) inventory[i] = new serialItem(_inv[i]);
        }
    }
    [System.Serializable]
    public struct MiniMapData
    {
        public int[] mapCenter;
        public int[] mapN;
        public int[] mapE;
        public int[] mapS;
        public int[] mapW;
        public bool[] mapNdoor;
        public bool[] mapEdoor;
        public bool[] mapSdoor;
        public bool[] mapWdoor;
        public bool[] mapNtrap;
        public bool[] mapEtrap;
        public bool[] mapStrap;
        public bool[] mapWtrap;
        public bool[] mapChest;
        public MiniMapData(int Filler)
        {         
            mapCenter = new int[325]; for (int _i = 0; _i < 325; _i++) mapCenter[_i] = 0;
            mapN = new int[325]; for (int _i = 0; _i < 325; _i++) mapN[_i] = 0;
            mapE = new int[325]; for (int _i = 0; _i < 325; _i++) mapE[_i] = 0;
            mapS = new int[325]; for (int _i = 0; _i < 325; _i++) mapS[_i] = 0;
            mapW = new int[325]; for (int _i = 0; _i < 325; _i++) mapW[_i] = 0;
            mapNdoor = new bool[325]; for (int _i = 0; _i < 325; _i++) mapNdoor[_i] = false;
            mapEdoor = new bool[325]; for (int _i = 0; _i < 325; _i++) mapEdoor[_i] = false;
            mapSdoor = new bool[325]; for (int _i = 0; _i < 325; _i++) mapSdoor[_i] = false;
            mapWdoor = new bool[325]; for (int _i = 0; _i < 325; _i++) mapWdoor[_i] = false;
            mapNtrap = new bool[325]; for (int _i = 0; _i < 325; _i++) mapNtrap[_i] = false;
            mapEtrap = new bool[325]; for (int _i = 0; _i < 325; _i++) mapEtrap[_i] = false;
            mapStrap = new bool[325]; for (int _i = 0; _i < 325; _i++) mapStrap[_i] = false;
            mapWtrap = new bool[325]; for (int _i = 0; _i < 325; _i++) mapWtrap[_i] = false;
            mapChest = new bool[325]; for (int _i = 0; _i < 325; _i++) mapChest[_i] = false;
        }
        public MiniMapData(int[,] map, int[,] mN, int[,] mE, int[,] mS, int[,] mW, bool[,] mND, bool[,] mED, bool[,] mSD, bool[,] mWD, bool[,] mNT, bool[,] mET, bool[,] mST, bool[,] mWT, bool[,] mapC)
        {
            mapCenter = new int[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapCenter[y * 18 + x] = map[x, y];
            mapN = new int[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapN[y * 18 + x] = mN[x, y];
            mapE = new int[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapE[y * 18 + x] = mE[x, y];
            mapS = new int[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapS[y * 18 + x] = mS[x, y];
            mapW = new int[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapW[y * 18 + x] = mW[x, y];
            mapNdoor = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapNdoor[y * 18 + x] = mND[x, y];
            mapEdoor = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapEdoor[y * 18 + x] = mED[x, y];
            mapSdoor = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapSdoor[y * 18 + x] = mSD[x, y];
            mapWdoor = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapWdoor[y * 18 + x] = mWD[x, y];
            mapNtrap = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapNtrap[y * 18 + x] = mNT[x, y];
            mapEtrap = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapEtrap[y * 18 + x] = mET[x, y];
            mapStrap = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapStrap[y * 18 + x] = mST[x, y];
            mapWtrap = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapWtrap[y * 18 + x] = mWT[x, y];
            mapChest = new bool[324];
            for (int y = 0; y < 18; y++) for (int x = 0; x < 18; x++) mapChest[y * 18 + x] = mapC[x, y];
        }
    }
    [System.Serializable]
    public struct WaypointData
    {
        public int Xcoor, Ycoor;
        public string UID;
        public WaypointData(int x, int y, string uid)
        {
            Xcoor = x; Ycoor = y;
            UID = uid;
        }
    }
    [System.Serializable]
    public struct MonsterData
    {
        public int XCoor, Ycoor;
        public float wounds;
        public string MonsterState, Orders;
        public WaypointData waypoint;
        public MonsterData(int x, int y, float w, string ms, string o, GameObject wp)
        {
            XCoor = x; Ycoor = y;
            wounds = w;
            MonsterState = ms; Orders = o;
            waypoint = new WaypointData((int)wp.transform.position.x, (int)wp.transform.position.z, "0");
            if (wp == null) { waypoint.Xcoor = 0; waypoint.Ycoor = 0; waypoint.UID = "0"; }
            if (wp.GetComponent<WaypointController>() != null) waypoint.UID = wp.GetComponent<WaypointController>().UID;
        }
    }
    [System.Serializable]
    public struct SpawnPointData
    {
        public int Xcoor, Ycoor;
        public MonsterData[] children;
        public SpawnPointData(int x, int y, GameObject[] childs)
        {
            Xcoor = x; Ycoor = y;
            children = new MonsterData[childs.Length];            
            for (int _i = 0; _i < childs.Length; _i++)
            {
                if(childs[_i].GetComponent<MonsterLogic>().wayPoint != null) children[_i] = new MonsterData((int)childs[_i].transform.position.x, (int)childs[_i].transform.position.z, childs[_i].GetComponent<MonsterLogic>().wounds, 
                    childs[_i].GetComponent<MonsterLogic>().monsterState, childs[_i].GetComponent<MonsterLogic>().orders, childs[_i].GetComponent<MonsterLogic>().wayPoint);
                if (childs[_i].GetComponent<MonsterLogic>().wayPoint == null) children[_i] = new MonsterData((int)childs[_i].transform.position.x, (int)childs[_i].transform.position.z, childs[_i].GetComponent<MonsterLogic>().wounds,
                     childs[_i].GetComponent<MonsterLogic>().monsterState, childs[_i].GetComponent<MonsterLogic>().orders, childs[_i]);
            }
        }
    }
    [System.Serializable]
    public struct SceneData
    {
        public List<ChestData> ChestData;
        public List<DoorData> DoorData;
        public List<NodeData> NodeData;
        public List<MiniMapData> MiniMapData;
        public List<SpawnPointData> SpawnPointData;

        public SceneData(int filler)
        {
            ChestData = new List<ChestData>();
            DoorData = new List<DoorData>();
            NodeData = new List<NodeData>();
            MiniMapData = new List<MiniMapData>();
            SpawnPointData = new List<SpawnPointData>();
        }
    }


    public void InitialSave()
    {
        List<GameObject> _results = new List<GameObject>();
        List<GameObject> _temp = new List<GameObject>();

        //Set Current Scene
        CurrentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        //build party data
        thisParty = new serialParty(0);
        thisParty.PC[0] = new serialCharacter(0);
        thisParty.PC[1] = new serialCharacter(1);
        thisParty.PC[2] = new serialCharacter(2);
        thisParty.PC[3] = new serialCharacter(3);
        thisParty.money = GameManager.PARTY.money;
        thisParty.x_coor = GameManager.PARTY.x_coor;
        thisParty.y_coor = GameManager.PARTY.y_coor;
        thisParty.face = GameManager.PARTY.face;        

        //build scenedata
        scene_List = new List<SceneData>(0);
        for (int i = 0; i < GameManager.GAME.Map.Length; i++)
        {
            scene_List.Add(new SceneData(0)); //scenes

            //Chests
            _results.Clear();
            RULES.FindAllChildrenWithTag(GameManager.GAME.Map[i].transform, "ChestParent", _results);
            foreach (GameObject go in _results)
                scene_List[i].ChestData.Add(new ChestData(go.name, (int)go.transform.position.x, (int)go.transform.position.z, go.GetComponentInChildren<Hello_I_am_a_Chest>().inventory));
            _results.Clear();

            //Doors
            RULES.FindAllChildrenWithTag(GameManager.GAME.Map[i].transform, "MapDoor", _temp);
            foreach (GameObject go in _temp) if (go.GetComponent<Hello_I_am_a_door>() != null) _results.Add(go);
            foreach (GameObject go in _results)
                scene_List[i].DoorData.Add(new DoorData((int)go.transform.position.x, (int)go.transform.position.z, go.GetComponent<Hello_I_am_a_door>().doorOpen, go.GetComponent<Hello_I_am_a_door>().knownLocked, go.GetComponent<Hello_I_am_a_door>().lockValue));
            _results.Clear(); _temp.Clear();

            //Nodes
            RULES.FindAllChildrenWithTag(GameManager.GAME.NodeHive[i].transform, "Node", _results);
            foreach (GameObject go in _results) scene_List[i].NodeData.Add(new NodeData((int)go.gameObject.transform.position.x, (int)go.gameObject.transform.position.z, go.GetComponent<GridNode>().inventory));

            //MiniMap
            if (scene_List[i].MiniMapData.Count == 0) { scene_List[i].MiniMapData.Add(new MiniMapData(0)); }
            else { scene_List[i].MiniMapData.Insert(0, new MiniMapData(0)); }

            //Monsters
            GameObject[] _spawners = GameObject.FindGameObjectsWithTag("MobSpawner");
            foreach (GameObject _go in _spawners)
            {
                GameObject[] _wpList = new GameObject[_go.transform.childCount - 1];
                for (int _i = 0; _i < _go.transform.childCount - 1; _i++) _wpList[_i] = _go.transform.GetChild(_i + 1).gameObject;
                scene_List[i].SpawnPointData.Add(new SpawnPointData((int)_go.transform.position.x, (int)_go.transform.position.z, _wpList));
            }
        }
    }

    public void SaveData(int s)
    {
        List<GameObject> _results = new List<GameObject>(); List<GameObject> _temp = new List<GameObject>();

        //Set Current Scene
        CurrentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        thisParty = new serialParty(0);
        thisParty.PC[0] = new serialCharacter(0);
        thisParty.PC[1] = new serialCharacter(1);
        thisParty.PC[2] = new serialCharacter(2);
        thisParty.PC[3] = new serialCharacter(3);
        thisParty.money = GameManager.PARTY.money;
        thisParty.x_coor = GameManager.PARTY.x_coor;
        thisParty.y_coor = GameManager.PARTY.y_coor;
        thisParty.face = GameManager.PARTY.face;
        
        //Chests
        RULES.FindAllChildrenWithTag(GameObject.FindGameObjectWithTag("Map").transform, "ChestParent", _results);
        foreach (GameObject go in _results)
            scene_List[s].ChestData.Add(new ChestData(go.name, (int)go.transform.position.x, (int)go.transform.position.z, go.GetComponentInChildren<Hello_I_am_a_Chest>().inventory));
        _results.Clear();

        //Doors
        RULES.FindAllChildrenWithTag(GameObject.FindGameObjectWithTag("Map").transform, "MapDoor", _temp);
        foreach (GameObject go in _temp) if (go.GetComponent<Hello_I_am_a_door>() != null) _results.Add(go);
        foreach (GameObject go in _results)
            scene_List[s].DoorData.Add(new DoorData((int)go.transform.position.x, (int)go.transform.position.z, go.GetComponent<Hello_I_am_a_door>().doorOpen, go.GetComponent<Hello_I_am_a_door>().knownLocked, go.GetComponent<Hello_I_am_a_door>().lockValue));
        _results.Clear(); _temp.Clear();

        //Nodes
        RULES.FindAllChildrenWithTag(GameObject.FindGameObjectWithTag("NodeHive").transform, "Node", _results);
        foreach (GameObject go in _results) scene_List[s].NodeData.Add(new NodeData((int)go.gameObject.transform.position.x, (int)go.gameObject.transform.position.z, go.GetComponent<GridNode>().inventory));

        //MiniMap
        PartyController p = GameManager.PARTY;
        scene_List[s].MiniMapData.Insert(0, new MiniMapData(p.map, p.mapN, p.mapE, p.mapS, p.mapW, p.mapND, p.mapED, p.mapSD, p.mapWD, p.mapNT, p.mapET, p.mapST, p.mapWT, p.mapC));

        //Monsters
        GameObject[] _spawners = GameObject.FindGameObjectsWithTag("MobSpawner");
        foreach (GameObject _go in _spawners)
        {
            GameObject[] _wpList = new GameObject[_go.transform.childCount - 1];
            for (int _i = 0; _i < _go.transform.childCount - 1; _i++) _wpList[_i] = _go.transform.GetChild(_i + 1).gameObject;
            scene_List[s].SpawnPointData.Add(new SpawnPointData((int)_go.transform.position.x, (int)_go.transform.position.z, _wpList));
        }
    }

    public void LoadData(SaveSlot s)
    {
        List<GameObject> _results = new List<GameObject>(); List<GameObject> _temp = new List<GameObject>();

        int c = s.CurrentScene;

        GameManager.PARTY.LoadParty(s.thisParty);

        //Chests
        RULES.FindAllChildrenWithTag(GameObject.FindGameObjectWithTag("Map").transform, "ChestParent", _results);
        foreach (GameObject go in _results)
            foreach (ChestData savedChest in s.scene_List[c].ChestData)
                if ((int)go.transform.position.x == savedChest.x && (int)go.transform.position.z == savedChest.y && go.name == savedChest.chestName) go.GetComponentInChildren<Hello_I_am_a_Chest>().LoadInventory(savedChest.inventory);
        _results.Clear();

        //Doors
        RULES.FindAllChildrenWithTag(GameObject.FindGameObjectWithTag("Map").transform, "MapDoor", _temp);
        foreach (GameObject go in _temp) if (go.GetComponent<Hello_I_am_a_door>() != null) _results.Add(go);
        foreach (GameObject go in _results)
            foreach (DoorData savedDoor in s.scene_List[c].DoorData)
                if ((int)go.transform.position.x == savedDoor.x && (int)go.transform.position.z == savedDoor.y) go.GetComponent<Hello_I_am_a_door>().LoadDoor(savedDoor.doorOpen, savedDoor.knownLocked, savedDoor.lockValue);
        _results.Clear(); _temp.Clear();

        //Nodes
        RULES.FindAllChildrenWithTag(GameObject.FindGameObjectWithTag("NodeHive").transform, "Node", _results);
        foreach (GameObject go in _results)
            foreach (NodeData savedNode in s.scene_List[c].NodeData)
                if ((int)go.transform.position.x == savedNode.x && (int)go.transform.position.z == savedNode.y) go.GetComponent<GridNode>().LoadInventory(savedNode.inventory);
        _results.Clear();

        //MiniMap
        GameManager.PARTY.LoadMiniMap(s.scene_List[c].MiniMapData[0].mapCenter,
            s.scene_List[c].MiniMapData[0].mapN, s.scene_List[c].MiniMapData[0].mapE, s.scene_List[c].MiniMapData[0].mapS, s.scene_List[c].MiniMapData[0].mapW, 
            s.scene_List[c].MiniMapData[0].mapNdoor, s.scene_List[c].MiniMapData[0].mapEdoor, s.scene_List[c].MiniMapData[0].mapSdoor, s.scene_List[c].MiniMapData[0].mapWdoor, 
            s.scene_List[c].MiniMapData[0].mapNtrap, s.scene_List[c].MiniMapData[0].mapEtrap, s.scene_List[c].MiniMapData[0].mapStrap, s.scene_List[c].MiniMapData[0].mapWtrap, 
            s.scene_List[c].MiniMapData[0].mapChest);

        //Monsters
        GameManager.EXPLORE.LoadMonsters(s.scene_List[c].SpawnPointData);

    }

    public void SetMiniMap(SaveSlot s, int c)
    {
        Debug.Log("Saving data for saveslot #" + GameManager.GAME.SelectedSaveSlot + ", map " + c);
        PartyController p = GameManager.PARTY;
        s.scene_List[c].MiniMapData.Insert(0, new MiniMapData(p.map, p.mapN, p.mapE, p.mapS, p.mapW, p.mapND, p.mapED, p.mapSD, p.mapWD, p.mapNT, p.mapET, p.mapST, p.mapWT, p.mapC));
    }

    public void GetMiniMap(SaveSlot s, int c)
    {
        //MiniMap
        GameManager.PARTY.LoadMiniMap(s.scene_List[c].MiniMapData[0].mapCenter,
            s.scene_List[c].MiniMapData[0].mapN, s.scene_List[c].MiniMapData[0].mapE, s.scene_List[c].MiniMapData[0].mapS, s.scene_List[c].MiniMapData[0].mapW,
            s.scene_List[c].MiniMapData[0].mapNdoor, s.scene_List[c].MiniMapData[0].mapEdoor, s.scene_List[c].MiniMapData[0].mapSdoor, s.scene_List[c].MiniMapData[0].mapWdoor,
            s.scene_List[c].MiniMapData[0].mapNtrap, s.scene_List[c].MiniMapData[0].mapEtrap, s.scene_List[c].MiniMapData[0].mapStrap, s.scene_List[c].MiniMapData[0].mapWtrap,
            s.scene_List[c].MiniMapData[0].mapChest);
    }
}