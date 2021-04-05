/*Added sounds to battle Open. Battle close, Hit and Miss
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScreenController : MonoBehaviour
{
    public GameObject[] pcSlot;
    public List<GameObject> enemy, enemySlot;
    public GameObject ref_OutputPanel, ref_monsterPanelPF, ref_MPF, ref_InfoBox;
    public Text ref_OutputText;    
    public Sprite ref_AggStanceIcon, ref_DefStanceIcon;
    public bool battleStarted;

    private string buttonPushed;
    private GameObject go;
    private bool WaitforNextStep = false;
    private string BattleStep;
    public List<GameObject> iniativeOrder = new List<GameObject>();
            
    private void Start()
    {
        battleStarted = false;
        enemySlot = new List<GameObject>();
        StartCoroutine("DelayStart", 1f);
    }
    IEnumerator DelayStart(float n)
    {
        yield return new WaitForSeconds(n);

        battleStarted = true;
        for (int _i = 0; _i < enemy.Count; _i++)
        {
            go = Instantiate(ref_monsterPanelPF, ref_MPF.transform);
            enemySlot.Add(go);
        }
        GameManager.GAME.BattleSound.Play();
        UpdatePlayerGUI();
        UpdateEnemyGUI();
        BuildIniativeOrder();
        BattleStep = "Actor Turn";
    }

    private void UpdatePlayerGUI()
    {
        for (int _i = 0; _i < 4; _i++)
        {
            GameManager.PARTY.PC[_i].BS_Slot = _i;
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
            if (GameManager.PARTY.PC[_i].frontLine) pcSlot[_i].transform.Find("StanceIcon").GetComponent<Image>().sprite = ref_AggStanceIcon;
            if (!GameManager.PARTY.PC[_i].frontLine) pcSlot[_i].transform.Find("StanceIcon").GetComponent<Image>().sprite = ref_DefStanceIcon;
            pcSlot[_i].transform.Find("Hilight").gameObject.SetActive(false);            
        }        
    }

    private void UpdateEnemyGUI()
    {
        for (int _i = 0; _i < enemySlot.Count; _i++)
        {
            enemy[_i].GetComponent<MonsterLogic>().BS_Slot = _i;
            GameObject _e = enemySlot[_i];
            _e.transform.GetChild(0).Find("Portrait").GetComponent<Image>().sprite = GameManager.GAME.monster_Sprite[enemy[_i].GetComponent<MonsterLogic>().monsterFaceIndex]; //Draw NPC portrait
            _e.transform.GetChild(0).Find("Name").GetComponent<Text>().text = enemy[_i].GetComponent<MonsterLogic>().NPC_Name; //Draw NPC name
            _e.transform.GetChild(0).Find("ID Placard").GetComponentInChildren<Text>().text = (_i + 1).ToString();
            if (_i == 9) _e.transform.GetChild(0).Find("ID Placard").GetComponentInChildren<Text>().text = "0";
            _e.transform.GetChild(0).Find("Health").GetChild(0).transform.GetComponentInChildren<Image>().fillAmount = (enemy[_i].GetComponent<MonsterLogic>().health - enemy[_i].GetComponent<MonsterLogic>().wounds) / enemy[_i].GetComponent<MonsterLogic>().health;
            //Debug.Log("Health = " + (enemy[_i].GetComponent<MonsterLogic>().health - enemy[_i].GetComponent<MonsterLogic>().wounds) + " of " + enemy[_i].GetComponent<MonsterLogic>().health);
            if (enemy[_i].GetComponent<MonsterLogic>().wounds < enemy[_i].GetComponent<MonsterLogic>().health) enemySlot[_i].transform.GetChild(0).Find("Dead").gameObject.SetActive(false);
            if (enemy[_i].GetComponent<MonsterLogic>().wounds >= enemy[_i].GetComponent<MonsterLogic>().health) enemySlot[_i].transform.GetChild(0).Find("Dead").gameObject.SetActive(true);
            _e.transform.Find("Hilight").gameObject.SetActive(false);
        }        
    }

    public void ButtonPushed(string b)
    {
        buttonPushed = b;
        b = "";
    }

    private void EndBattle()
    {
        StartCoroutine(ResolveEndBattle());
    }

    /* COMBAT
     * Determine Initiative order
     * on their turn, an attacker picks a target, resolves the damage/spell/item/stance change, whatever
     * next attacker goes
     * when all attackers have gone, determine new iniative order.
     * If all the monsters are dead. delete their gameobjects.
     * if all the player are dead, show the defeat screen
     * if the player ran, delete gameobjects of dead enemies
     */

    private void BuildIniativeOrder()
    {
        //To determine Initiative Order, I have everyone roll RandomRange and add initBonus (for NPCs) or dexMod (PCs), whichever has the highest number goes first, then the next one and so on.

        //Put all the battle actors in a list.
        int _numActors = 4 + enemy.Count;
        float[] _allActors = new float[_numActors]; for (int _f = 0; _f < _numActors; _f++) _allActors[_f] = -1;
        GameObject[] _tempReftoActorGOs = new GameObject[_numActors];
        for (int _i = 0; _i < 4; _i++) { _allActors[_i] = (Random.Range(0, GameManager.RULES.RandomRange)) + (GameManager.PARTY.PC[_i].GetComponent<Character>().dexterity / 2) - 4; _tempReftoActorGOs[_i] = GameManager.PARTY.PC[_i].gameObject; }
        for (int _i = 4; _i < (enemy.Count + 4); _i++) { _allActors[_i] = (Random.Range(0, GameManager.RULES.RandomRange)) + (enemy[_i - 4].GetComponent<MonsterLogic>().initBonus); _tempReftoActorGOs[_i] = enemy[_i - 4].gameObject; }

        //now sort by _allActor values
        iniativeOrder.Clear();
        for (int _i = 0; _i < _numActors; _i++)
        {
            int _current = 0; float _biggest = 0;
            for (int _c = 0; _c < _allActors.Length; _c++)
                if (_allActors[_c] > _biggest)
                {
                    _biggest = _allActors[_c];
                    _current = _c;
                }
            iniativeOrder.Add(_tempReftoActorGOs[_current]);            
            _allActors[_current] = 0;
        }
    }

    private void Update()
    {
        //Check for end of battle
        if(GameManager.PARTY.PC[0].wounds >= GameManager.PARTY.PC[0].health && GameManager.PARTY.PC[1].wounds >= GameManager.PARTY.PC[1].health 
            && GameManager.PARTY.PC[2].wounds >= GameManager.PARTY.PC[2].health && GameManager.PARTY.PC[3].wounds >= GameManager.PARTY.PC[3].health) //Party is dead, Party loses game.
        {
            GameManager.GAME.LoseSound.Play();
            EndBattle();
        }

        bool _battleOver = true;
        for (int _i = 0; _i < enemy.Count; _i++) if (enemy[_i].GetComponent<MonsterLogic>().wounds < enemy[_i].GetComponent<MonsterLogic>().health) _battleOver = false;
        if(_battleOver) //All monsters deadm, battle over, party wins
        {
            GameManager.GAME.VictorySound.Play();
            EndBattle();
        }

        if (!WaitforNextStep) //Cannot proceed until the current step has finished
        {
            if(BattleStep == "Actor Turn")
            {
                //1. Determine who's turn it is
                if (iniativeOrder[0].GetComponent<Character>() == true) BattleStep = "Character Turn";
                if (iniativeOrder[0].GetComponent<MonsterLogic>() == true) BattleStep = "Enemy Turn";
            }

            if (BattleStep == "Enemy Turn")
            {
                WaitforNextStep = true;
                ref_InfoBox.SetActive(false);
                enemySlot[iniativeOrder[0].GetComponent<MonsterLogic>().BS_Slot].transform.Find("Hilight").gameObject.SetActive(true);
                if (iniativeOrder[0].GetComponent<MonsterLogic>().wounds < iniativeOrder[0].GetComponent<MonsterLogic>().health) //check if the enemy is not dead
                {
                    //2. determine who the target is
                    GameObject _target;
                    //     >>build list of potential targets
                    List<GameObject> _targetlist = new List<GameObject>();
                    for (int _i = 0; _i < 4; _i++)
                    {
                        if (GameManager.PARTY.PC[_i].GetComponent<Character>().wounds < GameManager.PARTY.PC[_i].GetComponent<Character>().health)
                        {
                            _targetlist.Add(GameManager.PARTY.PC[_i].gameObject);
                            if (GameManager.PARTY.PC[_i].frontLine) _targetlist.Add(GameManager.PARTY.PC[_i].gameObject); //Add the player twice if they are Frontline
                        }
                    }
                    //      >>Pick a random target from targetList
                    int _random = Random.Range(0, _targetlist.Count);
                    _target = _targetlist[_random];                    

                    //3. build attack values
                    int _attack, _damage;
                    _attack = Random.Range(0, (int)GameManager.RULES.RandomRange) + iniativeOrder[0].GetComponent<MonsterLogic>().bonusToHit;
                    _damage = Random.Range(iniativeOrder[0].GetComponent<MonsterLogic>().minDamage, iniativeOrder[0].GetComponent<MonsterLogic>().maxDamage);
                    if (_damage < 0) _damage = 0;

                    //4. Get target's defense values
                    int _defense = (int)(Random.Range(0, GameManager.RULES.RandomRange) + _target.GetComponent<Character>().defense);

                    //5. Resolve the turn with a messaging CoRoutine
                    iniativeOrder[0].GetComponent<MonsterLogic>().attack.Play();
                    StartCoroutine(ResolveTurn(iniativeOrder[0], _target, _attack, _damage, _defense));
                }
                if (iniativeOrder[0].GetComponent<MonsterLogic>().wounds >= iniativeOrder[0].GetComponent<MonsterLogic>().health) StartCoroutine(ResolveTurn(iniativeOrder[0], "cannot act, as they have died."));
            }

            if(BattleStep == "Character Turn")
            {
                WaitforNextStep = true;
                pcSlot[iniativeOrder[0].transform.GetSiblingIndex()].transform.Find("Hilight").gameObject.SetActive(true);

                if (iniativeOrder[0].GetComponent<Character>().wounds < iniativeOrder[0].GetComponent<Character>().health) //check if the Hero is dead
                {
                    ref_InfoBox.SetActive(true);
                    ref_OutputPanel.SetActive(false);
                    ref_InfoBox.GetComponentInChildren<Text>().text = iniativeOrder[0].GetComponent<Character>().characterName + "'s turn:";

                    //Update Attack Buttons
                    int _i = iniativeOrder[0].transform.GetSiblingIndex();
                    if (GameManager.PARTY.PC[_i].eq_RightHand == null || GameManager.PARTY.PC[_i].eq_RightHand.type == InventoryItem.equipType.armor) transform.Find("Input Panel").transform.Find("AWRH Button").gameObject.SetActive(false); //if the right or left hands are empty or holding shields
                    if (GameManager.PARTY.PC[_i].eq_LeftHand == null || GameManager.PARTY.PC[_i].eq_LeftHand.type == InventoryItem.equipType.armor) transform.Find("Input Panel").transform.Find("AWLH Button").gameObject.SetActive(false);// then you cannot attack with them
                    if (transform.Find("Input Panel").transform.Find("AWRH Button").gameObject.activeSelf && GameManager.PARTY.PC[_i].eq_RightHand.identified)
                        transform.Find("Input Panel").transform.Find("AWRH Button").GetComponentInChildren<Text>().text = GameManager.PARTY.PC[_i].eq_RightHand.fullName + "\n" + GameManager.PARTY.PC[_i].eq_RightHand.minDamage + " to " + GameManager.PARTY.PC[_i].eq_RightHand.maxDamage;
                    if (transform.Find("Input Panel").transform.Find("AWLH Button").gameObject.activeSelf && GameManager.PARTY.PC[_i].eq_LeftHand.identified)
                        transform.Find("Input Panel").transform.Find("AWLH Button").GetComponentInChildren<Text>().text = GameManager.PARTY.PC[_i].eq_LeftHand.fullName + "\n" + GameManager.PARTY.PC[_i].eq_LeftHand.minDamage + " to " + GameManager.PARTY.PC[_i].eq_LeftHand.maxDamage;
                    if (transform.Find("Input Panel").transform.Find("AWRH Button").gameObject.activeSelf && !GameManager.PARTY.PC[_i].eq_RightHand.identified)
                        transform.Find("Input Panel").transform.Find("AWRH Button").GetComponentInChildren<Text>().text = GameManager.PARTY.PC[_i].eq_RightHand.genericName + "\n" + GameManager.PARTY.PC[_i].eq_RightHand.minDamage + " to " + GameManager.PARTY.PC[_i].eq_RightHand.maxDamage;
                    if (transform.Find("Input Panel").transform.Find("AWLH Button").gameObject.activeSelf && !GameManager.PARTY.PC[_i].eq_LeftHand.identified)
                        transform.Find("Input Panel").transform.Find("AWLH Button").GetComponentInChildren<Text>().text = GameManager.PARTY.PC[_i].eq_LeftHand.genericName + "\n" + GameManager.PARTY.PC[_i].eq_LeftHand.minDamage + " to " + GameManager.PARTY.PC[_i].eq_LeftHand.maxDamage;

                    //Update Cast spell button, so it only shows up if the charcter has mana
                    if (GameManager.PARTY.PC[_i].mana > 0) transform.Find("Input Panel").transform.Find("SpellButton").gameObject.SetActive(true);
                    if (GameManager.PARTY.PC[_i].mana <= 0) transform.Find("Input Panel").transform.Find("SpellButton").gameObject.SetActive(false);

                    BattleStep = "Wait for Hero Command";
                }
                if (iniativeOrder[0].GetComponent<Character>().wounds >= iniativeOrder[0].GetComponent<Character>().health) StartCoroutine(ResolveTurn(iniativeOrder[0], "cannot act, as they have died."));
            }
        }
        if(BattleStep == "Wait for Hero Command") //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! INPUT
        {
            if (buttonPushed == "RunAway") EndBattle();
            if (buttonPushed == "AttackRight") BattleStep = "Wait for Right Target";
            if (buttonPushed == "AttackLeft") BattleStep = "Wait for Left Target";
            if (buttonPushed == "PassTurn") StartCoroutine(ResolveTurn(iniativeOrder[0], "Passes Turn"));
            if (buttonPushed == "StanceChange")
            {
                iniativeOrder[0].GetComponent<Character>().frontLine = !iniativeOrder[0].GetComponent<Character>().frontLine;
                UpdatePlayerGUI();
                pcSlot[iniativeOrder[0].transform.GetSiblingIndex() - 1].transform.Find("Hilight").gameObject.SetActive(true);
            }

            if (buttonPushed != "") buttonPushed = ""; //Reset which button has been pushed
        }
        if (BattleStep == "Wait for Right Target" || BattleStep == "Wait for Left Target" || BattleStep == "Wait for Spell Target")
        {
            ref_InfoBox.GetComponentInChildren<Text>().text = "Choose a target:";
            int _trgt = -1;
            if (buttonPushed.Contains("Monster"))
            {
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "1") _trgt = 0;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "2") _trgt = 1;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "3") _trgt = 2;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "4") _trgt = 3;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "5") _trgt = 4;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "6") _trgt = 5;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "7") _trgt = 6;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "8") _trgt = 7;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "9") _trgt = 8;
                if (buttonPushed.Substring(buttonPushed.Length - 1) == "0") _trgt = 9;
            }
            if (buttonPushed != "") buttonPushed = ""; //Reset which button has been pushed
            if(_trgt > -1) //RESOLVE PENDING ATTACK
            {
                GameObject _target = enemy[_trgt];
                int _damage = 0, _attack = (int)(iniativeOrder[0].GetComponent<Character>().attack + Random.Range(0, GameManager.RULES.RandomRange));
                if (BattleStep == "Wait for Right Target") _damage = Random.Range(iniativeOrder[0].GetComponent<Character>().eq_RightHand.minDamage, iniativeOrder[0].GetComponent<Character>().eq_RightHand.maxDamage) + (iniativeOrder[0].GetComponent<Character>().strength / 2) - 4;
                if (BattleStep == "Wait for Left Target") _damage = Random.Range(iniativeOrder[0].GetComponent<Character>().eq_LeftHand.minDamage, iniativeOrder[0].GetComponent<Character>().eq_LeftHand.maxDamage) + (iniativeOrder[0].GetComponent<Character>().strength / 2) - 4;
                if (_damage < 1) _damage = 1; //Always do at least 1 point of damage if you hit.
                int _defense = _target.GetComponent<MonsterLogic>().defenseValue;
                StartCoroutine(ResolveTurn(iniativeOrder[0], _target, _attack, _damage, _defense));
            }
        }
    }

    IEnumerator ResolveEndBattle()
    {
        ref_OutputText.text = "The battle is over!";

        //Pause
        for (float timer = GameManager.RULES.messageDelay; timer >= 0; timer -= Time.deltaTime)
        {
            if (Input.GetButtonUp("Submit")) timer = 0;
            yield return null;
        }

        foreach (GameObject _go in enemy)
            if (_go.GetComponent<MonsterLogic>().wounds >= _go.GetComponent<MonsterLogic>().health)
            {
                for (int _i = 0; _i < 4; _i++) GameManager.PARTY.PC[_i].xpPoints += _go.GetComponent<MonsterLogic>().xpValue;
                Destroy(_go);
            }
        Destroy(gameObject);
        GameManager.EXPLORE.DrawExplorerUI();
        GameManager.PARTY.SetAllowedMovement(true);

    }

    IEnumerator ResolveTurn(GameObject _actor, string message)
    {
        ref_OutputPanel.SetActive(true);

        if (_actor.GetComponent<Character>() != null) //the actor is a hero
        {
            ref_OutputText.text = _actor.GetComponent<Character>().characterName + " " + message;
        }
        if (_actor.GetComponent<MonsterLogic>() != null) //the actor is a monster
        {
            ref_OutputText.text = _actor.GetComponent<MonsterLogic>().NPC_Name + " " + message;
        }

        //Pause
        for (float timer = GameManager.RULES.messageDelay; timer >= 0; timer -= Time.deltaTime)
        {
            if (Input.GetButtonUp("Submit")) timer = 0;
            yield return null;
        }

        iniativeOrder.RemoveAt(0); //remove the actor who just went from the iniative order list
        if (iniativeOrder.Count <= 0) BuildIniativeOrder();
        UpdatePlayerGUI();
        UpdateEnemyGUI();
        WaitforNextStep = false; //tell the update loop to beging the next phase
        BattleStep = "Actor Turn"; //point the phase to getting the next person in the iniative order, or rebuilding the iniative order if it is empty
    }

    IEnumerator ResolveTurn(GameObject _attacker, GameObject _defender, int _attack, int _damage, int _defense)
    {
        ref_OutputPanel.SetActive(true);

        if (_defender.GetComponent<Character>() != null) //the defender is a hero
        {
            ref_OutputText.text = _attacker.GetComponent<MonsterLogic>().NPC_Name + " attacks " + _defender.GetComponent<Character>().characterName;
        }
        if (_defender.GetComponent<MonsterLogic>() != null) //the defender is a monster
        {
            ref_OutputText.text = _attacker.GetComponent<Character>().characterName + " attacks " + _defender.GetComponent<MonsterLogic>().NPC_Name;
        }        

        //Pause
        for (float timer = GameManager.RULES.messageDelay; timer >= 0; timer -= Time.deltaTime)
        {
            if (Input.GetButtonUp("Submit")) timer = 0;
            yield return null;
        }

        if (_attack > _defense)
        {            
            if (_defender.GetComponent<Character>() != null) //the defender is a hero
            {
                ref_OutputText.text = _defender.GetComponent<Character>().characterName + " takes " + _damage + " points of damage!";
                GameManager.Splash("-" + _damage + "hp", Color.red, Color.white, pcSlot[_defender.GetComponent<Character>().BS_Slot]); 
                _defender.GetComponent<Character>().wounds += _damage;
                UpdatePlayerGUI();
            }
            if (_defender.GetComponent<MonsterLogic>() != null) //the defender is a monster
            {
                _defender.GetComponent<MonsterLogic>().hit.Play();
                ref_OutputText.text = _defender.GetComponent<MonsterLogic>().NPC_Name + " takes " + _damage + " points of damage!";
                GameManager.Splash("-" + _damage + "hp", Color.red, Color.white, enemySlot[_defender.GetComponent<MonsterLogic>().BS_Slot]);
                _defender.GetComponent<MonsterLogic>().wounds += _damage;
                UpdateEnemyGUI();
            }
        }
        if (_attack <= _defense)
        {
            GameManager.GAME.WhiffSound.Play();
            if (_defender.GetComponent<Character>() != null) //the defender is a hero
            {
                ref_OutputText.text = "...but " + _attacker.GetComponent<MonsterLogic>().NPC_Name + " misses!";
            }
            if (_defender.GetComponent<MonsterLogic>() != null) //the defender is a monster
            {
                ref_OutputText.text = "...but " + _attacker.GetComponent<Character>().characterName + " misses!";
            }
        }

        //Pause
        for (float timer = GameManager.RULES.messageDelay; timer >= 0; timer -= Time.deltaTime)
        {
            if (Input.GetButtonUp("Submit")) timer = 0;
            yield return null;
        }

        //Check for death
        if (_defender.GetComponent<Character>() != null) //the defender is a hero
        {
            if(_defender.GetComponent<Character>().health <= _defender.GetComponent<Character>().wounds)
            {
                ref_OutputText.text = _defender.GetComponent<Character>().characterName + " Succumbs to their wounds!";
                //Pause
                for (float timer = GameManager.RULES.messageDelay; timer >= 0; timer -= Time.deltaTime)
                {
                    if (Input.GetButtonUp("Submit")) timer = 0;
                    yield return null;
                }
            }
        }
        if (_defender.GetComponent<MonsterLogic>() != null) //the defender is a monster
        {
            if (_defender.GetComponent<MonsterLogic>().health <= _defender.GetComponent<MonsterLogic>().wounds)
            {
                ref_OutputText.text = _defender.GetComponent<MonsterLogic>().NPC_Name + " Succumbs to their wounds!";
                //Pause
                for (float timer = GameManager.RULES.messageDelay; timer >= 0; timer -= Time.deltaTime)
                {
                    if (Input.GetButtonUp("Submit")) timer = 0;
                    yield return null;
                }
            }
        }


        iniativeOrder.RemoveAt(0); //remove the actor who just went from the iniative order list
        if (iniativeOrder.Count <= 0) BuildIniativeOrder();
        UpdatePlayerGUI();
        UpdateEnemyGUI();
        WaitforNextStep = false; //tell the update loop to beging the next phase
        BattleStep = "Actor Turn"; //point the phase to getting the next person in the iniative order, or rebuilding the iniative order if it is empty
    }
}
